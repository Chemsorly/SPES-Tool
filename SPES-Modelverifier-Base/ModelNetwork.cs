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
        Application visioApplication;
        MappingList Mapping;

        /// <summary>
        /// creates a new instance of the model verifier for a specific model type
        /// </summary>
        /// <param name="pApplication">the visio application with the open document</param>
        /// <param name="pMappingList">the type of the visio-object mapping</param>
        public ModelNetwork(Application pApplication, Type pMappingList)
        {
            visioApplication = pApplication;
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
                models.Add(Activator.CreateInstance(Mapping.TargetModel.GetType(), page, Mapping) as Model);       

            return models;
        }
    }
}
