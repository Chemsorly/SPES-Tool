using MoreLinq;
using NetOffice.VisioApi;
using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base
{
    public abstract class ModelNetwork
    {
        /// <summary>
        /// the shape template files located in the MyShapes folder, e.g. "HMSC.vssx"
        /// </summary>
        public abstract List<String> ShapeTemplateFiles { get; }

        /// <summary>
        /// the derived type implementation of MappingType
        /// </summary>
        public abstract Type MappingListType { get; }

        protected Application visioApplication;
        MappingList Mapping;
        protected List<Model> ModelList;

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
        /// <param name="pMappingList">the type of the visio-object mapping</param>
        public ModelNetwork(Application pApplication)
        {
            //nullchecks
            if (pApplication == null)
                throw new ArgumentNullException("application");

            visioApplication = pApplication;
            Mapping = Activator.CreateInstance(MappingListType) as MappingList;
        }

        /// <summary>
        /// verification method for general verification purposes. Overwrite for additional model-specific checks and call base.Verify() to do base checks.
        /// </summary>
        public virtual void Verify()
        {
            //step 1: create entities
            ModelList = GenerateModels();

            //step 2: validate connections between entities
            ModelList.ForEach(t => t.Validate());

            //step 3: validate cross model references
            foreach (var model in ModelList)
                foreach (var modelref in model.ObjectList.Where(t => t is ModelReference))
                {
                    var correspondingmodel = ModelList.FirstOrDefault(t => t.PageName == modelref.text);
                    if (correspondingmodel == null)
                        throw new ValidationFailedException(modelref, "Could not locate matching submodel.");
                    else
                        (modelref as ModelReference).LinkedModel = correspondingmodel;
                }                      
        }

        public void LoadShapes()
        {
            //load in shapes
            try
            {
                //check if current opening document is not on the shape list
                if (!ShapeTemplateFiles.Any(t => this.visioApplication.Documents.Any(c => c.Name == t)))
                {
                    //cycle all files that have to be opened
                    foreach (var file in ShapeTemplateFiles)
                    {
                        //check if already opened, if not -> open
                        if (!this.visioApplication.Documents.Any(t => t.Name == file))
                            this.visioApplication.Documents.OpenEx(file, (short)NetOffice.VisioApi.Enums.VisOpenSaveArgs.visOpenDocked);
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyErrorReceived(ex);
            }
        }

        public void UnloadShapes()
        {
            try
            {
                List<IVDocument> documents = this.visioApplication.Documents.Where(t => ShapeTemplateFiles.Any(c => c == t.Name)).ToList();
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
        internal virtual List<Model> GenerateModels()
        {
            //generate empty list
            var models = new List<Model>();

            //go through all pages and add model elements
            foreach (Page page in this.visioApplication.ActiveDocument.Pages)
                models.Add(Activator.CreateInstance(GetTargetModelType(page), page, Mapping) as Model);       

            return models;
        }

        Type GetTargetModelType(Page pPage)
        {
            //check how many model types exist, if one return that one
            if (Mapping.TargetModels.Count == 1)
                return Mapping.TargetModels.First();

            //create a model for each model type
            List<Model> models = new List<Model>();
            foreach (Type type in Mapping.TargetModels)
                models.Add(Activator.CreateInstance(type, pPage, Mapping) as Model);

            //calculate rating
            Dictionary<Type, int> ratings = new Dictionary<Type, int>();
            foreach (var model in models)
                ratings.Add(model.GetType(), model.CalculateRating());

            //return type with highest probability rating
            return ratings.MaxBy(t => t.Value).Key;
        }

        /// <summary>
        /// notify when exception appeared
        /// </summary>
        /// <param name="error">exception</param>
        void NotifyErrorReceived(Exception error)
        {
            OnErrorReceivedEvent?.Invoke(error);
        }

        /// <summary>
        /// notify when a log message appeared
        /// </summary>
        /// <param name="message">message</param>
        void NotifyLogMessageReceived(String message)
        {
            OnLogMessageReceivedEvent?.Invoke(message);
        }

        public override string ToString() => this.GetType().Name;
    }
}
