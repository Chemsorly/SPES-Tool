using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
using MoreLinq;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;
using Container = SPES_Modelverifier_Base.Items.Container;

namespace ITU_Scenario.ModelChecker
{
    class BmscNode
    {
        public Item Current { get; }
        public List<BmscNode> NextNodes { get; }
        public List<Message> NextMessages { get; }
        public Message IncomingMessage { get; }
        public int CurrentDepth { get; }
        private double IncomingHeight { get; }
        private List<Container> KnownContainers { get; }
        private Container IncomingMessageContainer { get; set; }

        public BmscNode(Item pCurrent, Message pIncomingMessage, int pDepth, double pIncomingHeight, List<Container> pKnownContainers, Container pIncomingMessageContainer)
        {
            IncomingHeight = pIncomingHeight;
            Current = pCurrent;
            CurrentDepth = pDepth;
            NextNodes = new List<BmscNode>();
            NextMessages = new List<Message>();
            KnownContainers = pKnownContainers.ToList();
            IncomingMessage = pIncomingMessage;
            IncomingMessageContainer = pIncomingMessageContainer;

            //case depth > 100 (TODO proper abort function for bmsc)
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
            //get all outgoing messages
            var outgoing = this.Current.Connections.Where(t => t is Message && t.FromObject == this.Current);

            //TODO: fix only one container
            //get next connection with locationy <= maxheight and max locationy
            var nextpossiblemessages = outgoing.Where(t => t.Locationy <= IncomingHeight);
            if(nextpossiblemessages.Any())
            {
                //get next outgoing connection
                var nextmessage = nextpossiblemessages.MaxBy(t => t.Locationy);

                //case: next is in container
                //get bottom container from next message and add to known list
                InlineExpressionAltPar nextbottomcontainer = null;
                if (nextmessage.Containers.Any(t => t is InlineExpressionAltPar))
                {
                    nextbottomcontainer = (InlineExpressionAltPar) nextmessage.Containers.First(t => t is InlineExpressionAltPar);
                    while (nextbottomcontainer.FirstLevelChildContainers.Any())
                        nextbottomcontainer = (InlineExpressionAltPar) nextbottomcontainer.FirstLevelChildContainers.First();
                }

                //case: nextmessage is in NEW (aka unknown) container
                if (nextbottomcontainer != null)
                {
                    //enter container, is now known
                    KnownContainers.Add(nextbottomcontainer);

                    //if previous message was not in already known container
                    if (!this.KnownContainers.Contains(IncomingMessageContainer))
                    {
                        //find corresponding split message in lower container body by condition: message == outgoing && max(locy)
                        var splitmessage =(Message)nextbottomcontainer.ObjectsBelowLine.Where(t =>
                                t is Message && 
                                ((Message) t).FromObject == this.Current)
                                ?.MaxBy(t => t.Locationy);
                        if (splitmessage == null)
                            throw new ValidationFailedException(nextbottomcontainer,$"container {nextbottomcontainer.Text} does not contain an outgoing message in lower body");

                        //add top message to nodes list and continue
                        NextNodes.Add(new BmscNode((Item) nextmessage.ToObject, (Message) nextmessage,
                            this.CurrentDepth + 1, nextmessage.Locationy, KnownContainers, nextbottomcontainer));
                        NextMessages.Add((Message) nextmessage);

                        //add bottom message to nodes list and continue
                        NextNodes.Add(new BmscNode((Item) splitmessage.ToObject, (Message) splitmessage,
                            this.CurrentDepth + 1, splitmessage.Locationy, KnownContainers, nextbottomcontainer));
                        NextMessages.Add((Message) splitmessage);
                    }
                    //if message was in previous container, continue
                    else
                    {
                        //check if next message inside container body exist
                        if (IncomingMessageContainer.ContainingItems.Contains(nextmessage))
                        {
                            //check if next message DOES swap from upper body to lower body; exit container
                            if (nextbottomcontainer.ObjectsAboveLine.Contains(IncomingMessage) &&
                                ((InlineExpressionAltPar) nextmessage.Containers.First()).ObjectsBelowLine
                                    .Contains(nextmessage))
                            {
                                //message does swap, move outside of container
                                nextmessage = nextpossiblemessages.Where(t => !t.Containers.Contains(IncomingMessageContainer)).MaxBy(t => t.Locationy);
                            }
                        }
                        //no message in body container exists; container ends
                        else
                        {
                            //get next message outside of container
                            nextmessage = nextpossiblemessages.Where(t => !t.Containers.Contains(IncomingMessageContainer)).MaxBy(t => t.Locationy);
                        }

                        //add top message to nodes list and continue
                        NextNodes.Add(new BmscNode((Item)nextmessage.ToObject, (Message)nextmessage,
                            this.CurrentDepth + 1, nextmessage.Locationy, KnownContainers, nextbottomcontainer));
                        NextMessages.Add((Message)nextmessage);
                    }
                }
                else
                {
                    NextNodes.Add(new BmscNode((Item)nextmessage.ToObject,(Message)nextmessage, this.CurrentDepth + 1, nextmessage.Locationy, KnownContainers, nextbottomcontainer));
                    NextMessages.Add((Message)nextmessage);
                }
            }
        }
    }
}
