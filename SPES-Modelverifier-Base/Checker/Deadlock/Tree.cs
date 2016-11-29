using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Checker.Deadlock
{
    class Tree
    {
        Node StartNode { get; }

        public Tree (Model pModel)
        {
            //check if model contains start item, if not abort
            var startitem = pModel.ObjectList.Find(t => t is StartEndItem && (t as StartEndItem).IsStart);
            if (startitem == null)
                return;

            //create startnode and recursively create tree
            StartNode = new Node(startitem as Item, 1);
        }

        public void Validate()
        {
            //check 
        }

    }
}
