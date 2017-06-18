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
            if (CurrentDepth > 100)
            {
                throw new ValidationFailedException(Current, "Path length > 100 found");
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
            var outgoing = this.Current.Connections.Where(t => t is Message && t.FromObject == this.Current).Where(t => t.Locationy <= IncomingHeight);
            var messages = new HashSet<Connection>();

            //check if next messages exist, return if not
            if (outgoing.Any())
            {
                //get next outgoing connection
                var nextmessage = outgoing.MaxBy(t => t.Locationy);

                //check if nextmessage enters a new container
                HashSet<InlineExpressionAltPar> newcontainers = nextmessage.Containers.Where(t => t is InlineExpressionAltPar && !EnteredContainers.Contains(t))
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
                        this.CurrentDepth + 1, newmessage.Locationy, EnteredContainers));
                    NextMessages.Add((Message)newmessage);
                }
            }
        }
    }
}
