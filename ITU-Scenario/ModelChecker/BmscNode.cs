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
        private HashSet<Container> KnownContainers { get; }
        //private Container IncomingMessageContainer { get; set; }

        public BmscNode(Item pCurrent, Message pIncomingMessage, int pDepth, double pIncomingHeight, HashSet<Container> pKnownContainers)
        {
            IncomingHeight = pIncomingHeight;
            Current = pCurrent;
            CurrentDepth = pDepth;
            NextNodes = new List<BmscNode>();
            NextMessages = new List<Message>();
            KnownContainers = pKnownContainers.ToHashSet();
            IncomingMessage = pIncomingMessage;
            //IncomingMessageContainer = pIncomingMessageContainer;

            //case depth > 100 (TODO proper abort function for bmsc)
            if (CurrentDepth > 100)
            {
                throw new ValidationFailedException(Current, "Path length > 100 found");
            }

            //expand tree
            SetNextNodesNew();

            //case no next nodes: check if current node is an EndItem, if not throw exception
            //if (NextNodes.Count == 0)
            //    if (!(Current is StartEndItem) || (Current as StartEndItem).IsStart)
            //        throw new ValidationFailedException(Current, "Path does not reach an EndItem");
        }

        private void SetNextNodesNew()
        {
            //get all outgoing messages
            var outgoing = this.Current.Connections.Where(t => t is Message && t.FromObject == this.Current).Where(t => t.Locationy <= IncomingHeight);
            var messages = new HashSet<Connection>();

            //check if next messages exist, return if not
            if (outgoing.Any())
            {
                //get next outgoing connection
                var nextmessage = outgoing.MaxBy(t => t.Locationy);

                //check if nextmessage enters a new container
                HashSet<InlineExpressionAltPar> newcontainers = nextmessage.Containers.Where(t => t is InlineExpressionAltPar && !KnownContainers.Contains(t))
                    .Cast<InlineExpressionAltPar>()
                    .ToHashSet<InlineExpressionAltPar>();
                if (newcontainers.Any())
                {
                    //add containers from split message as well
                    var newsplitcontainers = newcontainers.First().ObjectsBelowLine.Where(t => t is Connection)
                        .Cast<Connection>().MaxBy(t => t.Locationy).Containers.Cast<InlineExpressionAltPar>();
                    foreach (InlineExpressionAltPar nc in newsplitcontainers)
                        newcontainers.Add(nc);

                    //iterate through all new containers. pick each top and bottom message and add to list. no duplicates
                    foreach (InlineExpressionAltPar newcontainer in newcontainers)
                    {
                        //add to known containers
                        KnownContainers.Add(newcontainer);

                        //get top top message
                        messages.Add(newcontainer.ObjectsAboveLine.Where(t => t is Connection).Cast<Connection>().MaxBy(t => t.Locationy));

                        //get top bottom mesage
                        messages.Add(newcontainer.ObjectsBelowLine.Where(t => t is Connection).Cast<Connection>().MaxBy(t => t.Locationy));
                    }
                }
                //nextmessage does not enter a new container
                else
                {
                    //check if swaps, meaning next message in lower container and previous in upper
                    var swappingContainers =
                        nextmessage.Containers.Where(
                            t => ((InlineExpressionAltPar) t).ObjectsAboveLine.Contains(IncomingMessage) &&
                                 ((InlineExpressionAltPar) t).ObjectsBelowLine.Contains(nextmessage));
                    if (swappingContainers.Any())
                    {
                        //if swap, pick next message NOT in the swapping container   
                        if(swappingContainers.Count() > 1)
                            throw new Exception("unexcepted result. more than one swapping container found.");

                        //check if another outgoing message exists
                        var nextoutgoing = outgoing.Where(t => !swappingContainers.First().ContainingItems.Contains(t));
                        if(nextoutgoing.Any())
                            messages.Add(nextoutgoing.MaxBy(t => t.Locationy));
                    }
                    //no swap, pick next message
                    else
                    {
                        messages.Add(nextmessage);
                    }
                }

                //create new nodes for each message
                foreach (var newmessage in messages)
                {
                    NextNodes.Add(new BmscNode((Item)newmessage.ToObject, (Message)newmessage,
                        this.CurrentDepth + 1, newmessage.Locationy, KnownContainers));
                    NextMessages.Add((Message)newmessage);
                }
            }


            //next message enters new container
            //get most outer container
            //get all outgoing messages where message = first message of a container in upper area || first message of a container in lower area
            //create new nodes for each message

            //next message does not enter new container
            //find next message in line
            //check if message swaps from top to bottom container
            //find next message NOT in container
            //create new node for found mesage
            //message not swapping
            //create new node for next mesage
        }

        //private void SetNextNodes()
        //{
        //    //get all outgoing messages
        //    var outgoing = this.Current.Connections.Where(t => t is Message && t.FromObject == this.Current);

        //    //TODO: fix only one container
        //    //get next connection with locationy <= maxheight and max locationy
        //    var nextpossiblemessages = outgoing.Where(t => t.Locationy <= IncomingHeight);
        //    if(nextpossiblemessages.Any())
        //    {
        //        //get next outgoing connection
        //        var nextmessage = nextpossiblemessages.MaxBy(t => t.Locationy);

        //        //case: next is in container
        //        //get bottom container from next message and add to known list
        //        InlineExpressionAltPar nextbottomcontainer = null;
        //        if (nextmessage.Containers.Any(t => t is InlineExpressionAltPar))
        //        {
        //            nextbottomcontainer = (InlineExpressionAltPar) nextmessage.Containers.First(t => t is InlineExpressionAltPar);
        //            while (nextbottomcontainer.FirstLevelChildContainers.Any())
        //                nextbottomcontainer = (InlineExpressionAltPar) nextbottomcontainer.FirstLevelChildContainers.First();
        //        }

        //        //case: nextmessage is in NEW (aka unknown) container
        //        if (nextbottomcontainer != null)
        //        {
        //            //enter container, is now known
        //            KnownContainers.Add(nextbottomcontainer);

        //            //if previous message was not in already known container
        //            if (!this.KnownContainers.Contains(IncomingMessageContainer))
        //            {
        //                //find corresponding split message in lower container body by condition: message == outgoing && max(locy)
        //                var splitmessage =(Message)nextbottomcontainer.ObjectsBelowLine.Where(t =>
        //                        t is Message && 
        //                        ((Message) t).FromObject == this.Current)
        //                        ?.MaxBy(t => t.Locationy);
        //                if (splitmessage == null)
        //                    throw new ValidationFailedException(nextbottomcontainer,$"container {nextbottomcontainer.Text} does not contain an outgoing message in lower body");

        //                //add top message to nodes list and continue
        //                NextNodes.Add(new BmscNode((Item) nextmessage.ToObject, (Message) nextmessage,
        //                    this.CurrentDepth + 1, nextmessage.Locationy, KnownContainers, nextbottomcontainer));
        //                NextMessages.Add((Message) nextmessage);

        //                //add bottom message to nodes list and continue
        //                //get bottom message most child container
        //                InlineExpressionAltPar nextsplitbottomcontainer = null;
        //                if (splitmessage.Containers.Any(t => t is InlineExpressionAltPar))
        //                {
        //                    nextsplitbottomcontainer = (InlineExpressionAltPar)splitmessage.Containers.First(t => t is InlineExpressionAltPar);
        //                    while (nextsplitbottomcontainer.FirstLevelChildContainers.Any())
        //                        nextsplitbottomcontainer = (InlineExpressionAltPar)nextsplitbottomcontainer.FirstLevelChildContainers.First();
        //                }

        //                NextNodes.Add(new BmscNode((Item) splitmessage.ToObject, (Message) splitmessage,
        //                    this.CurrentDepth + 1, splitmessage.Locationy, KnownContainers, nextsplitbottomcontainer));
        //                NextMessages.Add((Message) splitmessage);
        //            }
        //            //if message was in previous container, continue
        //            else
        //            {
        //                //check if next message inside container body exist
        //                if (IncomingMessageContainer.ContainingItems.Contains(nextmessage))
        //                {
        //                    //check if next message DOES swap from upper body to lower body; exit container
        //                    if (((InlineExpressionAltPar)IncomingMessageContainer).ObjectsAboveLine.Contains(IncomingMessage) &&
        //                        nextbottomcontainer.ObjectsBelowLine
        //                            .Contains(nextmessage))
        //                    {
        //                        //message does swap, move outside of container
        //                        nextmessage = nextpossiblemessages.Where(t => !t.Containers.Contains(IncomingMessageContainer)).MaxBy(t => t.Locationy);
        //                    }
        //                }
        //                //no message in body container exists; container ends
        //                else
        //                {
        //                    //get next message outside of container
        //                    nextmessage = nextpossiblemessages.Where(t => !t.Containers.Contains(IncomingMessageContainer)).MaxBy(t => t.Locationy);
        //                }

        //                //add top message to nodes list and continue
        //                NextNodes.Add(new BmscNode((Item)nextmessage.ToObject, (Message)nextmessage,
        //                    this.CurrentDepth + 1, nextmessage.Locationy, KnownContainers, nextbottomcontainer));
        //                NextMessages.Add((Message)nextmessage);
        //            }
        //        }
        //        else
        //        {
        //            NextNodes.Add(new BmscNode((Item)nextmessage.ToObject,(Message)nextmessage, this.CurrentDepth + 1, nextmessage.Locationy, KnownContainers, nextbottomcontainer));
        //            NextMessages.Add((Message)nextmessage);
        //        }
        //    }
        //}
    }
}
