using MoreLinq;
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
        List<Tree> TreeList { get; set; }

        /// <summary>
        /// event to throw in case of validation exception
        /// </summary>
        public event ValidationFailedDelegate ValidationFailedEvent;

        public void Initialize(List<Model> pModels)
        {
            //create treelist
            TreeList = new List<Tree>();

            //create a tree for every model
            pModels.Where(t => t.ObjectList.Any(u => u is StartEndItem)).ForEach(t => TreeList.Add(new Tree(t)));

            //call validate function for each tree
            TreeList.ForEach(t =>
            {
                try
                {
                    t.Validate();
                }
                catch(ValidationFailedException ex)
                {
                    ValidationFailedEvent?.Invoke(new ValidationFailedMessage(4, ex));
                }
            });

        }
    }
}
