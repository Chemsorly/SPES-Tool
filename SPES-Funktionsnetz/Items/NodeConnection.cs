using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Funktionsnetz
{
    class NodeConnection : Connection
    {
        public override List<Type> AllowedConnectionTypes => new List<Type>() { typeof(Step) };

        public override bool Inverted => false;
    }
}
