using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU_Scenario
{
    internal class Message : Connection
    {
        public override List<Type> AllowedConnectedTypes => new List<Type>() { typeof(Instance) };        

        public override bool Inverted => true;
        
    }
}
