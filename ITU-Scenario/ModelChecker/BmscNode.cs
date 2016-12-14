using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
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
        public Message IncomingMessage { get; }
        public int CurrentDepth { get; }
        private double incomingHeight { get; }

        public BmscNode(Item pCurrent, Message pIncomingMessage, int pDepth, double pIncomingHeight)
        {
            incomingHeight = pIncomingHeight;
            Current = pCurrent;
            CurrentDepth = pDepth;
            NextNodes = new List<BmscNode>();
            NextMessages = new List<Message>();
            IncomingMessage = pIncomingMessage;

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

            //TODO: fix only one container
            //get next connection with locationy <= maxheight and max locationy
            var nextpossibleconnections = outgoing.Where(t => t.Locationy <= incomingHeight);
            if(nextpossibleconnections.Any())
            {
                //get next outgoing connection
                var nextmessage = nextpossibleconnections.MaxBy(t => t.Locationy);

                //case: next is in container
                if (nextmessage.Containers.Any(t => t is InlineExpressionAltPar))
                {
                    //get bottom container from message
                    InlineExpressionAltPar bottomcontainer = (InlineExpressionAltPar)nextmessage.Containers.First(t => t is InlineExpressionAltPar);
                    while (bottomcontainer.FirstLevelChildContainers.Any())
                        bottomcontainer = (InlineExpressionAltPar)bottomcontainer.FirstLevelChildContainers.First();
                   

                    //if previous message was not in container: entering container
                    if (!this.IncomingMessage?.Containers.Any(t => t is InlineExpressionAltPar) ?? true)
                    {
                        //find corresponding split message in lower container body by condition: message == outgoing && max(locy)
                        var splitmessage =(Message)bottomcontainer.ObjectsBelowLine.Where(t =>
                                t is Message && 
                                ((Message) t).FromObject == this.Current)
                                ?.MaxBy(t => t.Locationy);
                        if (splitmessage == null)
                            throw new ValidationFailedException(bottomcontainer,$"container {bottomcontainer.Text} does not contain an outgoing message in lower body");

                        //add top message to nodes list and continue
                        NextNodes.Add(new BmscNode((Item) nextmessage.ToObject, (Message) nextmessage,
                            this.CurrentDepth + 1, nextmessage.Locationy));
                        NextMessages.Add((Message) nextmessage);

                        //add bottom message to nodes list and continue
                        NextNodes.Add(new BmscNode((Item) splitmessage.ToObject, (Message) splitmessage,
                            this.CurrentDepth + 1, splitmessage.Locationy));
                        NextMessages.Add((Message) splitmessage);
                    }
                    //if message was in previous container, continue
                    else
                    {
                        //check if next message inside container body exist
                        if (nextmessage.Containers.Contains(bottomcontainer))
                        {
                            //check if next message DOES swap from upper body to lower body
                            if (bottomcontainer.ObjectsAboveLine.Contains(IncomingMessage) &&
                                ((InlineExpressionAltPar) nextmessage.Containers.First()).ObjectsBelowLine
                                    .Contains(nextmessage))
                            {
                                //message does swap, move outside of container
                                nextmessage = nextpossibleconnections.Where(t => !t.Containers.Any()).MaxBy(t => t.Locationy);
                            }
                        }
                        //no message in body container exists
                        else
                        {
                            //get next message outside of container
                            nextmessage = nextpossibleconnections.Where(t => !t.Containers.Any()).MaxBy(t => t.Locationy);
                        }

                        //add top message to nodes list and continue
                        NextNodes.Add(new BmscNode((Item)nextmessage.ToObject, (Message)nextmessage,
                            this.CurrentDepth + 1, nextmessage.Locationy));
                        NextMessages.Add((Message)nextmessage);
                    }
                }
                else
                {
                    NextNodes.Add(new BmscNode((Item)nextmessage.ToObject,(Message)nextmessage, this.CurrentDepth + 1, nextmessage.Locationy));
                    NextMessages.Add((Message)nextmessage);
                }
            }
        }
    }
}
