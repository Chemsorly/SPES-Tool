using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.ModelChecker.Path;

namespace ITU_Scenario.ModelChecker
{
    internal class HmscNode : Node
    {
        public HmscNode(Item pCurrent, int pDepth) : base(pCurrent, pDepth)
        {
            //no changes to basic behaviour
        }

        public override List<Node> GetAllNextNodes()
        {
            //TODO: feedback notwendig: wie sieht hmsc mit spaltung aus? falls direkte verbindung zu jeder node: nicht notwendig 
            var outgoing = this.Current.Connections.Where(t => t.FromObject == this.Current);
            List<Node> nodes = new List<Node>();
            outgoing.ForEach(t => nodes.Add(new Node((t.ToObject as Item), this.CurrentDepth + 1)));
            return nodes;
        }
    }
}
