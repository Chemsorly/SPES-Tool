using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU_Scenario
{
    internal class StartSymbol : StartEndItem
    {
        public override bool IsStart => true;
    }
}
