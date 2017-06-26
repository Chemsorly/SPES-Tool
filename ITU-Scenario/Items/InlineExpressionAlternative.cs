using System.Collections.Generic;
using System.Linq;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.Items
{
    /// <summary>
    /// expression that is split horizontally to divide a path
    /// </summary>
    public class InlineExpressionAltPar : Container
    {
        /// <summary>
        /// contains all items above the split line
        /// </summary>
        public List<BaseObject> ObjectsAboveLine => this.ContainingItems.Where(t => t.Locationy > this.Locationy).ToList();

        /// <summary>
        /// contains all items below the split line
        /// </summary>
        public List<BaseObject> ObjectsBelowLine => this.ContainingItems.Where(t => t.Locationy < this.Locationy).ToList();
    }
}
