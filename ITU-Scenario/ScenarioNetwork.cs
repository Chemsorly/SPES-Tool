using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU_Scenario
{
    public class ScenarioNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<String> { "SMT_bMSC.vssx", "SMT_hMSC.vssx" };
        protected override Type MappingListType => typeof(ScenarioMapping);
        public override string ModelName => "Scenario-MSC";

        public ScenarioNetwork(Application pApplication) : base(pApplication)
        {
            
        }

        /// <summary>
        /// extra glueing settings are necessary for the shapes
        /// </summary>
        /// <param name="doc"></param>
        protected override void VisioApplication_DocumentCreatedOrLoadedEvent(IVDocument doc)
        {
            //set glue to geometry to true, to allow connectors to connect the instance shapes
            if (!doc.GlueSettings.HasFlag(NetOffice.VisioApi.Enums.VisGlueSettings.visGlueToGeometry))
                doc.GlueSettings = doc.GlueSettings | NetOffice.VisioApi.Enums.VisGlueSettings.visGlueToGeometry;
        }

        //public override string ToString()
        //{
        //    return "Scenario-MSC";
        //}
    }
}
