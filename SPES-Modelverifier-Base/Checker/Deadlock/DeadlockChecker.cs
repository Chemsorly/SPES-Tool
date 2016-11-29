using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Checker.Deadlock
{
    public class DeadlockChecker
    {
        List<Tree> TreeList { get; }

        /// <summary>
        /// init pattern for events
        /// </summary>
        public DeadlockChecker()
        {
            TreeList = new List<Tree>();
        }

        public void Initialize(List<Model> pModels)
        {
            //create a tree for every model
            pModels.ForEach(t => TreeList.Add(new Tree(t)));

            //call validate function for each tree
            TreeList.ForEach(t => t.Validate());
        }
    }
}
