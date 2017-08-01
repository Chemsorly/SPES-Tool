using System.Collections.Generic;
using System.Linq;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.Items
{
    /// <summary>
    /// expression that is split horizontally to divide a path
    /// </summary>
    public class BMSCInlineExpression : Container
    {
        public enum BMSCInlineExpressionType
        {
            loop,   //handle stuff once
            par,    //handle stuff serial (left first, then right)
            opt,    //split here, in and out
            alt,    //split here, left and right
            exc     //merge with opt
        };


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
