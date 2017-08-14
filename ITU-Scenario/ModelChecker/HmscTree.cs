using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.ModelChecker.Path;

namespace ITU_Scenario.ModelChecker
{
    class HmscTree : Tree
    {
        protected override void ValidateAllNodesInValidPaths(Node pRoot)
        {
            //hmsctree has to account for items in containers, therefor do not check if ALL items are in valid paths, but rather just get those
        }

        public List<Item> GetValidPathItems()
        {
            //create list and traverse tree. only return paths with enditem as leaf
            List<List<Node>> validpaths = new List<List<Node>>();
            Traverse(this.StartNode, new List<Node>(), validpaths);

            //validpths should now contain all paths where .Last() == StartEndItem with !IsStart
            //take all items from valid paths and check if they are equal with all items in model
            var allitems = StartNode.Current.ParentModel.ObjectList.Where(t => t is Item && ((Item)t).IsPathItem);
            var validpathitems = validpaths.SelectMany(t => t).Select(t => t.Current).Distinct();
            return validpathitems.ToList();
        }
    }
}
