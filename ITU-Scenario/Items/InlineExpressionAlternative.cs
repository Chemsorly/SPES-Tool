using System.Collections.Generic;
using System.Linq;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.Items
{
    internal class InlineExpressionAltPar : Container
    {
        public List<BaseObject> ObjectsAboveLine => this.ContainingItems.Where(t => t.Locationy > this.Locationy).ToList();

        public List<BaseObject> ObjectsBelowLine => this.ContainingItems.Where(t => t.Locationy < this.Locationy).ToList();
    }
}
