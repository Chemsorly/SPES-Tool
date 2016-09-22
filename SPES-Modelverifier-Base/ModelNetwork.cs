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
        public abstract Type MappingListType { get; }

        Application visioApplication;
        MappingList Mapping;

        public delegate void OnErrorReceived(Exception error);
        public event OnErrorReceived OnErrorReceivedEvent;

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
            this.visioApplication.DocumentOpenedEvent += DocumentLoadedHandler;
            this.visioApplication.DocumentCreatedEvent += DocumentLoadedHandler;
        }

        /// <summary>
        /// verification method for general verification purposes. Overwrite for additional model-specific checks and call base.Verify() to do base checks.
        /// </summary>
        public virtual void Verify()
        {
            //step 1: create entities
            var models = GenerateModels();

            //step 2: validate connections between entities
            models.ForEach(t => t.Validate());

            //step 3: validate cross model references
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
                models.Add(Activator.CreateInstance(Mapping.TargetModel, page, Mapping) as Model);       

            return models;
        }

        /// <summary>
        /// gets fired when a document gets loaded into the visio application. tries to load the shapes
        /// </summary>
        /// <param name="doc">newly opened document</param>
        void DocumentLoadedHandler(IVDocument doc)
        {
            //load in shapes
            try
            {
                //check if current opening document is not on the shape list
                if(!ShapeTemplateFiles.Any(t => t == doc.Name))
                {
                    //cycle all files that have to be opened
                    foreach(var file in ShapeTemplateFiles)
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
