using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
using ITU_Scenario.Models;
using MoreLinq;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.ModelChecker;
using SPES_Modelverifier_Base.ModelChecker.Path;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.ModelChecker
{
    internal class HmscValidPathChecker : ValidPathChecker
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
                //only get startenditems outside of containers since they'll be handled differently and get their own tree each
                var startenditems = pModel.ObjectList.Where(t => t is StartEndItem && !t.Containers.Any()).Cast<StartEndItem>().ToList();
                if (startenditems.Count(t => t.IsStart) > 1)
                {
                    NotifyValidationFailed(new ValidationFailedMessage(4, "Model contains more than one start item.",startenditems.First(t => t.IsStart)));
                    return;
                }
                if (startenditems.Count(t => !t.IsStart) == 0)
                {
                    NotifyValidationFailed(new ValidationFailedMessage(4, "Model contains no enditems"));
                    return;
                }

                //add startitems from containers
                foreach (var container in pModel.ObjectList.Where(t => t is Container))
                {
                    //case altpar container
                    if (container is HMSCInlineExpressionAltPar)
                    {
                        var seleft = (container as HMSCInlineExpressionAltPar).ObjectsLeftOfLine.Where(t => t is StartEndItem).Cast<StartEndItem>().ToList();
                        var seright = (container as HMSCInlineExpressionAltPar).ObjectsRightOfLine.Where(t => t is StartEndItem).Cast<StartEndItem>().ToList();

                        //checks: left
                        if (seleft.Count(t => t.IsStart) > 1)
                        {
                            NotifyValidationFailed(new ValidationFailedMessage(4,"Container contains more than one start item on left side.", container));
                            return;
                        }
                        if (seleft.Count(t => !t.IsStart) == 0)
                        {
                            NotifyValidationFailed(new ValidationFailedMessage(4,"Container contains no enditems on left side", container));
                            return;
                        }

                        //checks: right
                        if (seright.Count(t => t.IsStart) > 1)
                        {
                            NotifyValidationFailed(new ValidationFailedMessage(4,"Container contains more than one start item on right side.", container));
                            return;
                        }
                        if (seright.Count(t => !t.IsStart) == 0)
                        {
                            NotifyValidationFailed(new ValidationFailedMessage(4,"Container contains no enditems on right side", container));
                            return;
                        }

                        //merge
                        startenditems.AddRange(seleft);
                        startenditems.AddRange(seright);
                    }
                    //case else no split
                    else
                    {
                        var seitems = (container as Container).ContainingItems.Where(t => t is StartEndItem).Cast<StartEndItem>().ToList();

                        //checks
                        if (seitems.Count(t => t.IsStart) > 1)
                        {
                            NotifyValidationFailed(new ValidationFailedMessage(4,"Container contains more than one start item.", container));
                            return;
                        }
                        if (seitems.Count(t => !t.IsStart) == 0)
                        {
                            NotifyValidationFailed(new ValidationFailedMessage(4, "Container contains no enditems", container));
                            return;
                        }

                        //merge
                        startenditems.AddRange(seitems);
                    }
                }

                //tree validation
                try
                {
                    var treelist = new List<HmscTree>();
                    foreach (var seitem in startenditems)
                    {
                        var tree = new HmscTree();
                        treelist.Add(tree);
                        tree.ValidationFailedEvent += NotifyValidationFailed;
                        tree.Initialize(new HmscNode(seitem, 0));
                        tree.Validate();
                    }

                    //cross validate the trees: check if all items on the model exist in the valid path items
                    HashSet<Item> validitems = new HashSet<Item>();
                    foreach (var tree in treelist)
                    {
                        //add all items to hashset; hashset distincts items because hashset
                        tree.GetValidPathItems().ForEach(t => validitems.Add(t));
                    }
                    var missing = pModel.ObjectList.Where(t => t is Item).Except(validitems);
                    missing.ForEach(t => NotifyValidationFailed(new ValidationFailedMessage(4,$"Item {t.Uniquename} is not in a valid path.",t)));
                }
                catch (ValidationFailedException ex)
                {
                    NotifyValidationFailed(new ValidationFailedMessage(4, ex));
                }
            }
        }
    }
}
