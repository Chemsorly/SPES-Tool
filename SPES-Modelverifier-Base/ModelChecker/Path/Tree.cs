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
    /// <summary>
    /// simple tree with n child nodes (with n >= 0). does path checking for validation
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// event to throw in case of validation exception
        /// </summary>
        public event ValidationFailedDelegate ValidationFailedEvent;

        /// <summary>
        /// startnode in tree to traverse from
        /// </summary>
        private Node StartNode { get; set; }

        public void Initialize(Model pModel)
        {
            //check if model contains start item, if not abort
            var startitem = pModel.ObjectList.Find(t => t is StartEndItem && (t as StartEndItem).IsStart);
            if (startitem == null)
                throw new ValidationFailedException(pModel.ObjectList.First(t => t is StartEndItem), "StartItem not found");

            //create startnode and recursively create tree
            StartNode = new Node(startitem as Item, 1);
        }

        public void Validate()
        {
            //check all valid paths if they include all items
            ValidateAllNodesInValidPaths(StartNode);

        }

        private void ValidateAllNodesInValidPaths(Node pRoot)
        {
            //http://stackoverflow.com/questions/5691926/traverse-every-unique-path-from-root-to-leaf-in-an-arbitrary-tree-structure
            //create list and traverse tree. only return paths with enditem as leaf
            List<List<Node>> validpaths = new List<List<Node>>();
            Traverse(pRoot, new List<Node>(), validpaths);

            //validpths should now contain all paths where .Last() == StartEndItem with !IsStart
            //take all items from valid paths and check if they are equal with all items in model
            var allitems = StartNode.Current.ParentModel.ObjectList.Where(t => t is Item);
            var validpathitems = validpaths.SelectMany(t => t).Select(t => t.Current).Distinct();
            var missingitems = allitems.Where(t => !validpathitems.Contains(t)).ToList();
            if (missingitems.Any())
                missingitems.ForEach(t => ValidationFailedEvent?.Invoke(new ValidationFailedMessage(4, "Item has no valid path.", t)));
        }

        private static void Traverse(Node pRoot, List<Node> pPath, List<List<Node>> pValidpaths)
        {
            pPath.Add(pRoot);

            //check if leaf
            if (!pRoot.NextNodes.Any())
                if (pRoot.Current is StartEndItem && !(pRoot.Current as StartEndItem).IsStart)
                    pValidpaths.Add(pPath);
                else
                    return; //invalid path. end is not an end item

            //if no leaf, continue
            pRoot.NextNodes.ForEach(t => Traverse(t, new List<Node>(pPath), pValidpaths));            
        }
    }
}
