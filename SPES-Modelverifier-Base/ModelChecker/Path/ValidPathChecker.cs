using MoreLinq;
using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.ModelChecker.Path
{
    public class ValidPathChecker : IModelChecker
    {
        /// <summary>
        /// runs the check
        /// </summary>
        /// <param name="pModel">target model</param>
        public override void Initialize(Model pModel)
        {
            //nullcheck
            Debug.Assert(pModel != null);

            //check model if start or enditem exist
            if (pModel.ObjectList.Any(t => t is StartEndItem))
            {
                //check if start item is unique; check if minimum one end item exists;
                var startenditems = pModel.ObjectList.Where(t => t is StartEndItem).Cast<StartEndItem>().ToList();
                if (startenditems.Count(t => t.IsStart) > 1)
                    NotifyValidationFailed(new ValidationFailedMessage(4, "Model contains more than one start item.", startenditems.First(t => t.IsStart)));
                if (startenditems.Count(t => !t.IsStart) == 0)
                    NotifyValidationFailed(new ValidationFailedMessage(4, "Model contains no enditems", startenditems.First()));

                //tree validation
                //create tree
                var tree = new Tree();
                tree.ValidationFailedEvent += NotifyValidationFailed;                

                //call validate function
                try
                {
                    tree.Initialize(pModel);
                    tree.Validate();
                }
                catch (ValidationFailedException ex)
                {
                    NotifyValidationFailed(new ValidationFailedMessage(4, ex));
                }
            }
        }
    }
}
