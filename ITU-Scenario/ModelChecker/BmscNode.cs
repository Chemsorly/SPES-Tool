using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.ModelChecker
{
    class BmscNode
    {
        public Item Current { get; }
        public List<BmscNode> NextNodes { get; }
        public List<Message> NextMessages { get; }
        public int CurrentDepth { get; }

        private double incomingHeight { get; }

        public BmscNode(Item pCurrent, int pDepth, double pIncomingHeight)
        {
            incomingHeight = pIncomingHeight;
            Current = pCurrent;
            CurrentDepth = pDepth;
            NextNodes = new List<BmscNode>();
            NextMessages = new List<Message>();

            //case depth > 100 (TODO proper abort function)
            if (CurrentDepth > 100)
            {
                throw new ValidationFailedException(Current, "Path length > 100 found");
            }

            //expand tree
            SetNextNodes();

            //case no next nodes: check if current node is an EndItem, if not throw exception
            //if (NextNodes.Count == 0)
            //    if (!(Current is StartEndItem) || (Current as StartEndItem).IsStart)
            //        throw new ValidationFailedException(Current, "Path does not reach an EndItem");
        }

        private void SetNextNodes()
        {
            //get all outgoing connections
            var outgoing = this.Current.Connections.Where(t => t.FromObject == this.Current);

            //TODO: alternative, so it returns an actual tree
            //get next connection with locationy <= maxheight and max locationy
            var nextconnections = outgoing.Where(t => t.Locationy <= incomingHeight);
            if(nextconnections.Any())
            {
                var next = nextconnections.MaxBy(t => t.Locationy);
                NextNodes.Add(new BmscNode((Item)next.ToObject, this.CurrentDepth + 1, next.Locationy));
                NextMessages.Add((Message)next);
            }
        }
    }
}
