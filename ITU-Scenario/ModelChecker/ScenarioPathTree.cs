﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
using ITU_Scenario.Models;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.ModelChecker.Path;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.ModelChecker
{
    class ScenarioPathTree
    {
        /// <summary>
        /// event to throw in case of validation exception
        /// </summary>
        public event ValidationFailedDelegate ValidationFailedEvent;

        private ScenarioPathNode StartNode { get; set; }

        public void Initialize(Model pModel)
        {
            BaseObject startitem = null;
            //determine what model type the incoming model is and then find the starting item
            if (pModel is BMSCModel)
            {
                //find bmsc starting point: message with highest y value
                //check how many possible start messages exist
                var maxvalue = pModel.ObjectList.Where(t => t is Message).Max(t => t.Locationy);
                var startmessages = pModel.ObjectList.Where(t => t is Message && t.Locationy == maxvalue).ToList();

                //checks
                //case more than 1 possible starts found
                if (startmessages.Count() > 1)
                    NotifyValidationFailed(new ValidationFailedMessage(4, "More than one possible starting message found.", startmessages.First()));
                //case no start found
                else if (!startmessages.Any())
                    NotifyValidationFailed(new ValidationFailedMessage(4, $"Start message not found for model {pModel.PageName}"));
                else
                    startitem = startmessages.FirstOrDefault();
            }
            else if (pModel is HMSCModel)
            {
                //find hmsc starting point: first start-stop item
                //check if start item is unique; check if minimum one end item exists;
                var startenditems = pModel.ObjectList.Where(t => t is StartEndItem).Cast<StartEndItem>().ToList();
                if (startenditems.Count(t => t.IsStart) > 1)
                    NotifyValidationFailed(new ValidationFailedMessage(4, "Model contains more than one start item.", startenditems.First(t => t.IsStart)));
                if (startenditems.Count(t => !t.IsStart) == 0)
                    NotifyValidationFailed(new ValidationFailedMessage(4, "Model contains no enditems", startenditems.First()));
            }
            else
            {
                NotifyValidationFailed(new ValidationFailedMessage(4, $"unknown model detected: {pModel.PageName}"));
            }
        }

        public void Validate()
        {
            
        }

        /// <summary>
        /// notifies when a validation error occured
        /// </summary>
        /// <param name="pArgs">validation failed message</param>
        public void NotifyValidationFailed(ValidationFailedMessage pArgs)
        {
            ValidationFailedEvent?.Invoke(pArgs);
        }
    }
}
