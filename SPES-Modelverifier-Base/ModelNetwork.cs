using MoreLinq;
using NetOffice.VisioApi;
using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.ModelChecker;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Items;
using Utility.Testing;

namespace SPES_Modelverifier_Base
{
    public abstract class ModelNetwork
    {
        /// <summary>
        /// the shape template files located in the MyShapes folder, e.g. "HMSC.vssx"
        /// </summary>
        protected abstract List<String> ShapeTemplateFiles { get; }

        /// <summary>
        /// the derived type implementation of MappingType
        /// </summary>
        protected abstract Type MappingListType { get; }

        /// <summary>
        /// defines checkers to run
        /// </summary>
        protected virtual List<Type> CheckersToRun => new List<Type>() {  };

        private readonly Application _visioApplication;
        private readonly MappingList _mapping;
        protected List<Model> ModelList;
        protected List<ValidationFailedMessage> CollectedValidationMessages;


        /// <summary>
        /// event handler for error messages
        /// </summary>
        /// <param name="error">exception</param>
        public delegate void OnErrorReceived(Exception error);
        public event OnErrorReceived OnErrorReceivedEvent;

        /// <summary>
        /// event handler for log messages
        /// </summary>
        /// <param name="message">log message</param>
        public delegate void OnLogMessageReceived(String message);
        public event OnLogMessageReceived OnLogMessageReceivedEvent;

        /// <summary>
        /// creates a new instance of the model verifier for a specific model type
        /// </summary>
        /// <param name="pApplication">the visio application with the open document</param>
        public ModelNetwork(Application pApplication)
        {
            CollectedValidationMessages = new List<ValidationFailedMessage>();

            if(pApplication == null)
                throw new ArgumentNullException(nameof(pApplication));

            _visioApplication = pApplication;

            //gets called when document is loaded
            if (!UnitTestDetector.IsRunningUnittest)
            {
                //do not bind to UI events during tests, otherwise visio acts weird
                _visioApplication.DocumentCreatedEvent += VisioApplication_DocumentCreatedOrLoadedEvent;
                _visioApplication.DocumentOpenedEvent += VisioApplication_DocumentCreatedOrLoadedEvent;
            }

            // ReSharper disable once VirtualMemberCallInConstructor
            _mapping = Activator.CreateInstance(MappingListType) as MappingList;
        }

        /// <summary>
        /// define extra operations to be set during document initialization, e.g. settings
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void VisioApplication_DocumentCreatedOrLoadedEvent(IVDocument doc) { }

        /// <summary>
        /// verification method for general verification purposes. Overwrite for additional model-specific checks and call base.Verify() to do base checks. Throws exception if verification fails
        /// </summary>
        public virtual List<ValidationFailedMessage> VerifyModels()
        {
            //create empty list (empty = no errors)
            CollectedValidationMessages = new List<ValidationFailedMessage>();

            //step 1: create entities
            ModelList = GenerateModels();
            if (CollectedValidationMessages.Any())
                return CollectedValidationMessages;

            //step 2: validate connections between entities
            ModelList.ForEach(t => t.Verify());
            if (CollectedValidationMessages.Any())
                return CollectedValidationMessages;

            //step 3: validate cross model references
            foreach (var model in ModelList)
                foreach (var modelref in model.ObjectList.Where(t => t is ModelReference))
                {
                    var correspondingmodel = ModelList.FirstOrDefault(t => t.PageName == modelref.Text);
                    if (correspondingmodel == null)
                        NotifyVerificationFailed(modelref, 3, "Could not locate matching submodel.");
                    else
                    {
                        ((ModelReference) modelref).LinkedModel = correspondingmodel;
                        correspondingmodel.ParentModel = model;
                    }
                }

            if (CollectedValidationMessages.Any())
                return CollectedValidationMessages;

            //step 4: other stuff
            //run checkers if any specified
            foreach (var checkertype in CheckersToRun)
            {
                //check checker
                Debug.Assert(checkertype.IsSubclassOf(typeof(IModelNetworkChecker)));

                //create defined checker
                var checker = (IModelNetworkChecker)Activator.CreateInstance(checkertype);
                checker.ValidationFailedEvent += NotifyVerificationFailed;

                //run initialize method
                checker.Initialize(this);
            }

            return CollectedValidationMessages;
        }

        /// <summary>
        /// does a pre check if export can be done. currently requires to be a valid model TODO
        /// </summary>
        /// <returns></returns>
        public bool CanExport()
        {
            return !this.VerifyModels().Any();
        }

        /// <summary>
        /// exports the model to a given XML file. the model has to be verified prior for the export to work
        /// </summary>
        /// <param name="pFile"></param>
        public void Export(String pFile)
        {
            try
            {
                //gets all objects from items namespace: all classes defined in Items and Models namespace. 
                //Sorts out compiler classes, check https://stackoverflow.com/questions/43068213/getting-all-types-under-a-userdefined-assembly
                List<Type> classes = Assembly.GetAssembly(this.GetType()).GetTypes().Where(t => 
                t.IsClass && 
                !t.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute)) &&
                (t.Namespace.EndsWith("Items") || t.Namespace.EndsWith("Models")))
                .ToList();
                //add NRO because missing
                classes.Add(typeof(NRO));

                XmlSerializer serializer = new XmlSerializer(typeof(List<Model>), classes.ToArray());
                
                using (FileStream stream = new FileStream(pFile, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(stream, ModelList);
                }

            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                throw ex;
            }
        }

        /// <summary>
        /// imports a given model from an XML file and tries to reconstruct it with the current loaded stencils
        /// </summary>
        /// <param name="pFile"></param>
        public void Import(String pFile)
        {
            try
            {
                //gets all objects from items namespace: all classes defined in Items and Models namespace. 
                //Sorts out compiler classes, check https://stackoverflow.com/questions/43068213/getting-all-types-under-a-userdefined-assembly
                Type[] classes = Assembly.GetAssembly(this.GetType()).GetTypes().Where(t =>
                        t.IsClass &&
                        !t.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute)) &&
                        (t.Namespace.EndsWith("Items") || t.Namespace.EndsWith("Models")))
                    .ToArray();

                XmlSerializer deserializer = new XmlSerializer(typeof(List<Model>), classes);
                using (FileStream stream = new FileStream(pFile, FileMode.Open))
                {
                    //get models
                    this.ModelList = (List<Model>)deserializer.Deserialize(stream);
                }

                //reconstruct by placing objects with proper parameters and setting connections
                Reconstruct();

            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                throw ex;
            }
        }

        /// <summary>
        /// takes the current ModelList and invokes a reonstruct action on Visio to load a model from an XML file
        /// </summary>
        private void Reconstruct()
        {
            //get current document
            IVDocument doc = _visioApplication.ActiveDocument;

            //clear
            foreach (var page in this._visioApplication.ActiveDocument.Pages)
                page.Delete(0);

            //create temp sheet (because visio can not have 0 sheets)
            String deletename = "deletemelater" + new Random(1337).Next(0, 1000);
            this._visioApplication.ActiveDocument.Pages.First().Name = deletename;

            //reconstruct each model one by one
            foreach (Model model in ModelList)
            {
                //create visio page
                var page = this._visioApplication.ActiveDocument.Pages.Add();
                page.Name = model.PageName;

                //iterate through all elements and set fields
                foreach (var item in model.ObjectList)
                {
                    //create new visio shape 
                    var master = GetMasters().FirstOrDefault(t => t.Name == item.TypeName);
                    if (master != null)
                    {
                        try
                        {
                            //drop shape at position
                            var shape = page.Drop(master, item.Locationx, item.Locationy);

                            //set text if applicable
                            if (!String.IsNullOrEmpty(item.Text))
                                shape.Text = item.Text;

                            //set height and width; does not work for connection types
                            if (!(item is Connection))
                            {
                                shape.Cells("Height").set_Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric, item.Height);
                                shape.Cells("Width").set_Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric, item.Width);
                            }
                            item.Visioshape = shape;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            throw;
                        }
                    }
                    else
                    {
                       throw new Exception($"Master for {item.TypeName} not found");
                    }
                }

                //iterate through all connectors and set connections
                foreach (var item in model.ObjectList)
                {
                    if (item is Connection)
                    {
                        try
                        {
                            //find to and from shapes
                            var connection = (Connection) item;
                            var toshape = model.ObjectList.First(t => t.Uniquename == connection.ToObject.Uniquename)
                                .Visioshape;
                            var fromshape = model.ObjectList
                                .First(t => t.Uniquename == connection.FromObject.Uniquename).Visioshape;

                            //set connection and glue together
                            var beginxcell = connection.Visioshape.CellsSRC((short)NetOffice.VisioApi.Enums.VisSectionIndices.visSectionObject,
                                (short)NetOffice.VisioApi.Enums.VisRowIndices.visRowXForm1D,
                                (short)NetOffice.VisioApi.Enums.VisCellIndices.vis1DBeginX);
                            beginxcell.GlueTo(fromshape.CellsSRC(
                                (short)NetOffice.VisioApi.Enums.VisSectionIndices.visSectionObject,
                                (short)NetOffice.VisioApi.Enums.VisRowIndices.visRowXFormOut,
                                (short)NetOffice.VisioApi.Enums.VisCellIndices.visXFormPinX));

                            var beginycell = connection.Visioshape.CellsSRC((short)NetOffice.VisioApi.Enums.VisSectionIndices.visSectionObject,
                                (short)NetOffice.VisioApi.Enums.VisRowIndices.visRowXForm1D,
                                (short)NetOffice.VisioApi.Enums.VisCellIndices.vis1DEndX);
                            beginycell.GlueTo(toshape.CellsSRC(
                                (short)NetOffice.VisioApi.Enums.VisSectionIndices.visSectionObject,
                                (short)NetOffice.VisioApi.Enums.VisRowIndices.visRowXFormOut,
                                (short)NetOffice.VisioApi.Enums.VisCellIndices.visXFormPinX));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            throw;
                        }
                    }
                }

                //delete stub page
                this._visioApplication.ActiveDocument.Pages.First(t => t.Name == deletename).Delete(0);
            }

        }

        /// <summary>
        /// checks if stencil files exist and downloads them if not
        /// </summary>
        public void CheckStencils()
        {
            try
            {
                foreach (var file in ShapeTemplateFiles)
                {
                    //check if file exists, if not, download from server
                    if (!System.IO.File.Exists(System.IO.Path.Combine(this._visioApplication.MyShapesPath, file)))
                        DownloadStencils(file);

                    else
                    {
                        //file exists, check if remote is newer
                        var stencilfile =
                            new FileInfo(System.IO.Path.Combine(this._visioApplication.MyShapesPath, file));

                        var webrequest = (HttpWebRequest) WebRequest.Create($"https://releases.chemsorly.com/SPES-Modelverifier/visiostencils/{file}");
                        webrequest.Method = "HEAD";
                        webrequest.Timeout = 5000;
                        HttpWebResponse webresponse = null;
                        try
                        {
                            webresponse = (HttpWebResponse) webrequest.GetResponse();
                            if (webresponse.LastModified > stencilfile.LastWriteTime)
                                DownloadStencils(file);
                        }
                        catch(Exception){ }
                        finally { webresponse?.Close(); }
                    }
                }
            }
            catch(Exception pEx)
            {
                NotifyErrorReceived(pEx);
            }
        }

        /// <summary>
        /// downloads the stencil into the MyShapes directory
        /// </summary>
        /// <param name="pStencilfile">target stencil</param>
        private void DownloadStencils(String pStencilfile)
        {
            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile($"https://releases.chemsorly.com/SPES-Modelverifier/visiostencils/{pStencilfile}",
                    System.IO.Path.Combine(this._visioApplication.MyShapesPath, pStencilfile));
            }
        }

        /// <summary>
        /// loads the stencils for the model network
        /// </summary>
        public void LoadShapes()
        {
            //load in shapes
            try
            {
                //check if current opening document is not on the shape list
                if (!ShapeTemplateFiles.Any(t => this._visioApplication.Documents.Any(c => c.Name == t)) && !_visioApplication.ActiveDocument.Name.Contains(".vsx") && !_visioApplication.ActiveDocument.Name.Contains(".vssx"))
                {
                    //cycle all files that have to be opened
                    foreach (var file in ShapeTemplateFiles)
                    {
                        //check if already opened, if not -> open
                        if (!this._visioApplication.Documents.Any(t => t.Name == file))
                        {
                             this._visioApplication.Documents.OpenEx(file, (short)NetOffice.VisioApi.Enums.VisOpenSaveArgs.visOpenDocked | (short)NetOffice.VisioApi.Enums.VisOpenSaveArgs.visOpenRO);
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                NotifyErrorReceived(ex);
            }
        }

        /// <summary>
        /// unloads the model network specific stencils
        /// </summary>
        public void UnloadShapes()
        {
            try
            {
                List<IVDocument> documents = this._visioApplication.Documents.Where(t => ShapeTemplateFiles.Any(c => c == t.Name)).ToList();
                for (int i = 0; i < documents.Count; i++)
                    documents[i].Close();
            }
            catch(Exception ex)
            {
                NotifyErrorReceived(ex);
            }
        }

        /// <summary>
        /// Creates the target models based on supplied mapping list and the target model. If more than one model type needs to be created, overwrite and implement own logic. 
        /// </summary>
        /// <returns></returns>
        protected virtual List<Model> GenerateModels()
        {
            //generate empty list
            var models = new List<Model>();

            //go through all pages and add model elements
            foreach (Page page in this._visioApplication.ActiveDocument.Pages)
            {
                var model = (Model)Activator.CreateInstance(GetTargetModelType(page));
                model.ValidationFailedEvent += NotifyVerificationFailed;
                model.Initialize(page, _mapping);
                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// returns the model type for the target visio page. if more than one exists, the most likely one will be returned (based on the amount of matching shapes)
        /// </summary>
        /// <param name="pPage">the visio page</param>
        /// <returns></returns>
        private Type GetTargetModelType(Page pPage)
        {
            //check how many model types exist, if one return that one
            if (_mapping.TargetModels.Count == 1)
                return _mapping.TargetModels.First();

            //create a model for each model type
            List<Model> models = new List<Model>();
            foreach (Type type in _mapping.TargetModels)
            {
                var model = (Model)Activator.CreateInstance(type);
                model.Initialize(pPage, _mapping);
                models.Add(model);
            }

            //calculate rating
            Dictionary<Type, int> ratings = new Dictionary<Type, int>();
            foreach (var model in models)
                ratings.Add(model.GetType(), model.CalculateRating());

            //return type with highest probability rating
            return ratings.MaxBy(t => t.Value).Key;
        }

        /// <summary>
        /// returns a list of masters from the active visio application
        /// </summary>
        /// <returns>masters list</returns>
        private List<IVMaster> GetMasters()
        {
            if(_visioApplication==null)
                throw new Exception("no visio application detected");

            List<IVMaster> masters = new List<IVMaster>();
            foreach(Document doc in _visioApplication.Documents)
                foreach(IVMaster master in doc.Masters)
                    masters.Add(master);
            return masters;
        }

        /// <summary>
        /// notify when exception appeared
        /// </summary>
        /// <param name="error">exception</param>
        private void NotifyErrorReceived(Exception error)
        {
            OnErrorReceivedEvent?.Invoke(error);
        }

        /// <summary>
        /// notify when a log message appeared
        /// </summary>
        /// <param name="message">message</param>
        private void NotifyLogMessageReceived(String message)
        {
            OnLogMessageReceivedEvent?.Invoke(message);
        }

        /// <summary>
        /// creates a new validationfailedmessage and notifies the proper event
        /// </summary>
        /// <param name="pObject">the object</param>
        /// <param name="pLevel">the level in the verification rule hierarchy</param>
        /// <param name="pMessage">the detailed error message</param>
        private void NotifyVerificationFailed(BaseObject pObject, int pLevel, String pMessage)
        {
            NotifyVerificationFailed(new ValidationFailedMessage(pLevel, pMessage, pObject));
        }

        /// <summary>
        /// adds a verification failed message to the message stack. checks for duplicates first
        /// </summary>
        /// <param name="pFailedMessage">the message to add</param>
        private void NotifyVerificationFailed(ValidationFailedMessage pFailedMessage)
        {
            //check if message exists in it's current form already
            if (CollectedValidationMessages.Any(t => t.ValuesEquals(pFailedMessage)))
                return;

            CollectedValidationMessages.Add(pFailedMessage);
        }

        /// <summary>
        /// returns the name (not full name)
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.GetType().Name;
    }
}
