using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
using ITU_Scenario.Models;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.ModelChecker.Path;

namespace ITU_Scenario.ModelChecker
{
    class ScenarioPathNode
    {
        //Note: not deriving from Node as this checker uses a special implementation. Candidate for general implementation
        public BaseObject Current { get; }
        public List<ScenarioPathNode> NextNodes { get; }
        public int CurrentDepth { get; }
        public Dictionary<String,String> ConditionsDictionary { get; }

        public ScenarioPathNode(BaseObject pCurrent, int pDepth, Dictionary<String, String> pConditionDictionary = null )
        {
            Current = pCurrent;
            CurrentDepth = pDepth;
            NextNodes = new List<ScenarioPathNode>();

            //case depth > 500 (TODO proper abort function)
            if (CurrentDepth > 500)
                //throw new ValidationFailedException(Current, "Path length > 500 found");
                return;

            //expand tree
            NextNodes = GetAllNextNodes();

            //case no next nodes: check if current node is an EndItem, if not throw exception
            if (NextNodes.Count == 0)
            {
                //extra abort checks
            };
        }

        private List<ScenarioPathNode> GetAllNextNodes()
        {
            List<ScenarioPathNode> nodes = new List<ScenarioPathNode>();

            //differentiate by model type
            //hmsc: next node connected to connector
            if (this.Current.ParentModel is HMSCModel)
            {
                if (this.Current is Connection)
                {
                    //if connector, next node is the item pointed at
                    var pointedItem = ((Connection) this.Current).ToObject;

                    //special cases:
                    //condition
                    if (pointedItem is Condition)
                    {
                        //condition sets a value
                        Condition cond = pointedItem as Condition;
                        this.ConditionsDictionary[cond.Key] = cond.Value;
                    }
                    //guarding condition
                    else if (pointedItem is GuardingCondition)
                    {
                        //guarding condition checks if a value has been set by a condition before
                        //if it does not exist, do not return next nodes
                        GuardingCondition cond = pointedItem as GuardingCondition;
                        if (this.ConditionsDictionary.ContainsKey(cond.Key))
                        {
                            //key exists, check if value is the same
                            if (this.ConditionsDictionary[cond.Key] == cond.Value)
                            {
                                //both key and value exist, continue
                                //do nothing
                            }
                            else
                            {
                                //key does exist, but wrong value is assigned, guarding condition is blocking access, return empty nodes list
                                return nodes;
                            }
                        }
                        else
                        {
                            //key does not exist, guarding condition is blocking access, return empty nodes list
                            return nodes;
                        }
                        
                    }
                    //containers logic
                    else if (pointedItem is Container)
                    {
                        
                    }
                    //container (loop)
                    //container (parallel)
                    //container (optional)
                    //container (alternative)
                    //container (exl (??))

                    nodes.Add(new ScenarioPathNode(((Connection)this.Current).ToObject, this.CurrentDepth +1, new Dictionary<string, string>(ConditionsDictionary)));
                }
                else
                {
                    //if not an connector, next node are all outgoing connections
                }

            }
            //bmsc: sequence chart logic
            else if (this.Current.ParentModel is BMSCModel)
            {
                
            }
            else
            {
                throw new ValidationFailedException(Current, "Path length > 500 found");
            }

            //var outgoing = this.Current.Connections.Where(t => t.FromObject == this.Current);
            //List<Node> nodes = new List<Node>();
            //outgoing.ForEach(t => nodes.Add(new Node((t.ToObject as Item), this.CurrentDepth + 1)));
            //return nodes;

            return nodes;
        }
    }
}
