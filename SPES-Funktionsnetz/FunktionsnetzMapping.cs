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
        public override Dictionary<string, Type> Mapping
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override List<Type> TargetModels => new List<Type>() { typeof(FunktionsnetzModel), typeof(AutomataModel) };
    }
}
