using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Funktionsnetz
{
    class FunktionsnetzMapping : MappingList
    {
        public override Dictionary<string, Type> Mapping => new Dictionary<String, Type>()
        {
            //Funktionsnetz
            { "Function", typeof(Function) },
            { "external Function", typeof(ContextFunction) },
            { "Interaction", typeof(Interaction) },
            { "Dependency", typeof(Dependency) },
            { "Dependency Connector", typeof(DependencyConnection) },

            //Automata            
            { "Step", typeof(Step) },
            { "Connection", typeof(NodeConnection) }
        };

        public override List<Type> TargetModels => new List<Type>() { typeof(FunktionsnetzModel), typeof(AutomataModel) };
    }
}
