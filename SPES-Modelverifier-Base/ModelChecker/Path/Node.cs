using MoreLinq;
using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Items;

namespace SPES_Modelverifier_Base.ModelChecker.Path
{
    internal class Node
    {
        public Item Current { get; }
        public List<Node> NextNodes { get; }
        public int CurrentDepth { get; }

        public Node(Item pCurrent, int pDepth)
        {
            Current = pCurrent;
            CurrentDepth = pDepth;
            NextNodes = new List<Node>();

            //case depth > 100 (TODO proper abort function)
            if (CurrentDepth > 100)
                //throw new ValidationFailedException(Current, "Path length > 100 found");
                return;
            //expand tree
            NextNodes = GetAllNextNodes(this);

            //case no next nodes: check if current node is an EndItem, if not throw exception
            if (NextNodes.Count == 0)
                if(!(Current is StartEndItem) || (Current as StartEndItem).IsStart)
                    throw new ValidationFailedException(Current, "Path does not reach an EndItem");
        }

        private static List<Node> GetAllNextNodes(Node pNode)
        {
            var outgoing = pNode.Current.Connections.Where(t => t.FromObject == pNode.Current);
            List<Node> nodes = new List<Node>();
            outgoing.ForEach(t => nodes.Add(new Node((t.ToObject as Item), pNode.CurrentDepth + 1)));
            return nodes;
        }
    }
}
