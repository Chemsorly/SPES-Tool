using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Funktionsnetz.Items;
using SPES_Funktionsnetz.Models;

namespace SPES_Funktionsnetz
{
    internal class FunktionsnetzMapping : MappingList
    {
        protected override Dictionary<string, Type> Mapping => new Dictionary<String, Type>()
        {
            //Funktionsnetz
            { "Function", typeof(Function) },
            { "external Function", typeof(ContextFunction) },
            { "Interaction", typeof(Interaction) },
            { "Dependency", typeof(Dependency) },
            { "Dependency Connector", typeof(DependencyConnection) },

            //Automata            
            { "State", typeof(Step) },
            { "Initial State", typeof(Step) },
            { "Connection", typeof(NodeConnection) },
            //automata legacy, TODO lösung finden
            { "Step", typeof(Step) }
        };

        protected override List<Type> TargetModels => new List<Type>() { typeof(FunktionsnetzModel), typeof(AutomataModel) };
    }
}
