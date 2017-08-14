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
    /// <summary>
    /// a node in the bmsctree evaluation tree
    /// </summary>
    class BmscNode
    {
        /// <summary>
        /// reference to the current item (instance)
        /// </summary>
        public Item Current { get; }

        /// <summary>
        /// a list of next nodes. necessary for traversal
        /// </summary>
        public List<BmscNode> NextNodes { get; }

        /// <summary>
        /// a list of next messages
        /// </summary>
        public List<Message> NextMessages { get; }

        /// <summary>
        /// the previous message that connected to the current item
        /// </summary>
        public Message IncomingMessage { get; }

        /// <summary>
        /// the current level in the tree
        /// </summary>
        public int CurrentDepth { get; }

        /// <summary>
        /// the starting height to pick the next message
        /// </summary>
        private double IncomingHeight { get; }

        /// <summary>
        /// a list of previously entered containers
        /// </summary>
        private HashSet<Container> EnteredContainers { get; }
        //private Container IncomingMessageContainer { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pCurrent">the item to create a tree node on</param>
        /// <param name="pIncomingMessage">the message that connects to the node</param>
        /// <param name="pDepth">current tree depth</param>
        /// <param name="pIncomingHeight">starting height</param>
        /// <param name="pKnownContainers">already entered containers</param>
        public BmscNode(Item pCurrent, Message pIncomingMessage, int pDepth, double pIncomingHeight, HashSet<Container> pKnownContainers)
        {
            IncomingHeight = pIncomingHeight;
            Current = pCurrent;
            CurrentDepth = pDepth;
            NextNodes = new List<BmscNode>();
            NextMessages = new List<Message>();
            EnteredContainers = pKnownContainers.ToHashSet();
            IncomingMessage = pIncomingMessage;
            //IncomingMessageContainer = pIncomingMessageContainer;

            //case depth > 100 (TODO proper abort function for bmsc)
            if (CurrentDepth > 200)
            {
                throw new ValidationFailedException(Current, "Path length > 200 found");
            }

            //expand tree
            SetNextNodesNew();
        }

        /// <summary>
        /// creates the next nodes in the tree
        /// </summary>
        private void SetNextNodesNew()
        {
            //get all outgoing messages
            var outgoing = this.Current.Connections.Where(t => (t is Message || t is LostMessage) && t.FromObject != null && t.FromObject == this.Current).Where(t => t.Locationy <= IncomingHeight);
            var coregions = this.Current.ParentModel.ObjectList.Where(t => t is CoregionBox && Math.Abs((t as CoregionBox).Locationx - this.Current.Locationx) < 1);
            var messages = new HashSet<Connection>();

            //check if next messages (or coregions) exist, return if not
            if (outgoing.Any() || coregions.Any())
            {
                //get next outgoing connection
                List<Connection> nextmessagesList = new List<Connection>();
                if (outgoing.Any())
                {
                    //order
                    var outgoingordered = outgoing.OrderByDescending(t => t.Locationy).ToList();

                    //handle lost messages as if they were async
                    //add lostmessages to list until normal message found
                    for (int i = 0; i < outgoingordered.Count; i++)
                    {
                        nextmessagesList.Add(outgoingordered[i]);
                        if (!(outgoingordered[i] is LostMessage))
                            break;
                    }
                }

                //check if a coregion exists before. if so, use those connections instead
                if (coregions.Any())
                {
                    var fittingCoregions = coregions.Where(t => t.Locationtopleft.Y < this.IncomingHeight);
                    if(nextmessagesList.Any() && fittingCoregions.Any())
                        fittingCoregions = fittingCoregions.Where(t => t.Locationtopleft.Y > nextmessagesList.First().Locationy).ToList();

                    if (fittingCoregions.Any())
                    {
                        //coregion found, replace nextmessages with coregion outgoing messages
                        nextmessagesList.Clear();
                        var coregionmax = fittingCoregions.MaxBy(t => t.Locationy) as Item;
                        var coregionOutgoingMessages = coregionmax.Connections.Where(t => t.FromObject == fittingCoregions.MaxBy(r => r.Locationy));
                        nextmessagesList.AddRange(coregionOutgoingMessages);

                        //special case: lost messages between coregion and incoming message
                        var lostmessagesbetweencoregion = outgoing.Where(t => t is LostMessage && t.Locationy < coregionmax.Locationy);
                        if(lostmessagesbetweencoregion.Any())
                            nextmessagesList.AddRange(lostmessagesbetweencoregion);
                    }
                }

                //check if a nextmessage still exists
                if (!nextmessagesList.Any())
                    return;

                foreach (var nextmessage in nextmessagesList)
                {
                    //check if nextmessage enters a new container
                    HashSet<BMSCInlineExpressionAltPar> newcontainers = nextmessage.Containers.Where(t => t is BMSCInlineExpressionAltPar && !EnteredContainers.Contains(t))
                        .Cast<BMSCInlineExpressionAltPar>()
                        .ToHashSet<BMSCInlineExpressionAltPar>();
                    if (newcontainers.Any())
                    {
                        //add containers from split message as well
                        var newsplitcontainers = newcontainers.First().ObjectsBelowLine.Where(t => t is Connection)
                            .Cast<Connection>().MaxBy(t => t.Locationy).Containers.Cast<BMSCInlineExpressionAltPar>();
                        foreach (BMSCInlineExpressionAltPar nc in newsplitcontainers)
                            newcontainers.Add(nc);

                        //iterate through all new containers. pick each top and bottom message and add to list. no duplicates
                        foreach (BMSCInlineExpressionAltPar newcontainer in newcontainers)
                        {
                            //add to known containers
                            EnteredContainers.Add(newcontainer);

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
                                t => ((BMSCInlineExpressionAltPar)t).ObjectsAboveLine.Contains(IncomingMessage) &&
                                     ((BMSCInlineExpressionAltPar)t).ObjectsBelowLine.Contains(nextmessage));
                        if (swappingContainers.Any())
                        {
                            //if swap, pick next message NOT in the swapping container   
                            if (swappingContainers.Count() > 1)
                                throw new Exception("unexcepted result. more than one swapping container found.");

                            //check if another outgoing message exists
                            var nextoutgoing = outgoing.Where(t => !swappingContainers.First().ContainingItems.Contains(t));
                            if (nextoutgoing.Any())
                                messages.Add(nextoutgoing.MaxBy(t => t.Locationy));
                        }
                        //no swap, pick next message
                        else
                        {
                            messages.Add(nextmessage);
                        }
                    }
                }

                //create new nodes for each message
                foreach (var newmessage in messages)
                {
                    //case: newmessage is LostMessage: find corresponding found message as next node if exists
                    if (newmessage is LostMessage)
                    {
                        //check for foundmessage
                        var foundmessage = (FoundMessage) this.Current.ParentModel.ObjectList.FirstOrDefault(t => t is FoundMessage && t.Locationy < newmessage.Locationy &&((FoundMessage) t).Text == newmessage.Text);
                        if (foundmessage != null)
                        {
                            NextNodes.Add(new BmscNode((Item)foundmessage.ToObject, (Message)foundmessage, this.CurrentDepth + 1, foundmessage.Locationy, EnteredContainers));
                            NextMessages.Add((Message)newmessage);
                        }
                    }
                    else
                    {
                        NextNodes.Add(new BmscNode((Item)newmessage.ToObject, (Message)newmessage, this.CurrentDepth + 1, newmessage.Locationy, EnteredContainers));
                        NextMessages.Add((Message)newmessage);
                    }
                }
            }
        }
    }
}
