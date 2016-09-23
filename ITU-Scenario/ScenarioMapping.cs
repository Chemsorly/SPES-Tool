using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario
{
    public class ScenarioMapping : MappingList
    {
        public override Dictionary<String,Type> Mapping => new Dictionary<String, Type>()
        {
            //HMSC
            { "Connection Point", typeof(ConnectionPoint) },
            { "Start Symbol", typeof(StartSymbol) },
            { "End Symbol", typeof(EndSymbol) },
            { "MSC Reference", typeof(BMSCReference) },
            { "Connection Arrow", typeof(ConnectionArrow) },

            //BMSC            
            { "Line Instance", typeof(Instance) },
            { "Headless Instance", typeof(Instance) },
            { "Message (Left)", typeof(Message) },
            { "Message (Right)", typeof(Message) }
        };

        public override List<Type>TargetModels => new List<Type>() { typeof(HMSCModel), typeof(BMSCModel) };
    }
}
