using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Funktionsnetz
{
    class Dependency : Item
    {
        public override bool CanHaveDuplicateText => true;
    }
}
