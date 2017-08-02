using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
using ITU_Scenario.Models;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.ModelChecker
{
    class BmscTree
    {
        /// <summary>
        /// event to throw in case of validation exception
        /// </summary>
        public event ValidationFailedDelegate ValidationFailedEvent;

        /// <summary>
        /// startnode in tree to traverse from
        /// </summary>
        private  BmscNode StartNode { get; set; }

        /// <summary>
        /// initializes the tree model
        /// </summary>
        /// <param name="pModel">the bmsc to create a tree from</param>
        public void Initialize(BMSCModel pModel)
        {
            //find start item
            //TODO assumption: all messages are perfectly horizontally aligned
            //check how many possible start messages exist
            var maxvalue = pModel.ObjectList.Where(t => t is Message).Max(t => t.Locationy);
            var startmessages = pModel.ObjectList.Where(t => t is Message && t.Locationy == maxvalue);

            //checks
            //case more than 1 possible starts found
            if(startmessages.Count() > 1)
                throw new ValidationFailedException(startmessages.First(), "More than one possible starting message found.");
            //case no start found
            else if (!startmessages.Any())
                throw new ValidationFailedException(null, "Start message not found");
            
            //start first node with first message
            var firstmessage = (Message)startmessages.First();
            var node = firstmessage.FromObject;
            StartNode = new BmscNode((Item)node,null,1, firstmessage.Locationy, new HashSet<Container>());
        }

        /// <summary>
        /// calls the validate function
        /// </summary>
        public void Validate()
        {
            //check all valid paths if they include all items
            ValidateAllNodesInValidPaths(StartNode);
        }

        /// <summary>
        /// creates a n-tree based from a root node
        /// </summary>
        /// <param name="pRoot">the root node to start the tree from</param>
        private void ValidateAllNodesInValidPaths(BmscNode pRoot)
        {
            //http://stackoverflow.com/questions/5691926/traverse-every-unique-path-from-root-to-leaf-in-an-arbitrary-tree-structure
            //create list and traverse tree. only return paths with enditem as leaf
            List<List<BmscNode>> validpaths = new List<List<BmscNode>>();
            TraverseNodes(pRoot, new List<BmscNode>(), validpaths);

            //validpths should now contain all paths where .Last() == StartEndItem with !IsStart
            //take all items from valid paths and check if they are equal with all items in model
            //for bmsc: only instances are relevant as placeableobjects are not part of the tree
            var allitems = StartNode.Current.ParentModel.ObjectList.Where(t => t is Instance);

            var validpathitems = validpaths.SelectMany(t => t).Select(t => t.Current).Distinct();
            var missingitems = allitems.Where(t => !validpathitems.Contains(t)).ToList();
            if (missingitems.Any())
                missingitems.ForEach(t => ValidationFailedEvent?.Invoke(new ValidationFailedMessage(4, "Item has no valid path.", t)));

            //check if all messages are traversed with valid paths
            var allmessages = StartNode.Current.ParentModel.ObjectList.Where(t => t is Message);
            var validpathmessages = validpaths.SelectMany(t => t).SelectMany(t => t.NextMessages).Distinct();
            var missingmessages = allmessages.Where(t => !validpathmessages.Contains(t)).ToList();
            if(missingmessages.Any())
                missingmessages.ForEach(t => ValidationFailedEvent?.Invoke(new ValidationFailedMessage(4, $"Message({t.Text}) has no valid path.", t)));
        }

        /// <summary>
        /// traverses the tree recursively
        /// </summary>
        /// <param name="pRoot">the node to start from</param>
        /// <param name="pPath">the current path</param>
        /// <param name="pValidpaths">all valid paths. return value</param>
        private static void TraverseNodes(BmscNode pRoot, List<BmscNode> pPath, List<List<BmscNode>> pValidpaths)
        {
            pPath.Add(pRoot);

            //check if leaf
            if (!pRoot.NextNodes.Any())
                pValidpaths.Add(pPath);

            //if no leaf, continue
            pRoot.NextNodes.ForEach(t => TraverseNodes(t, new List<BmscNode>(pPath), pValidpaths));
        }
    }
}
