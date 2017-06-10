﻿using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Funktionsnetz.Items;

namespace SPES_Funktionsnetz
{
    public class FunktionsnetzNetwork : ModelNetwork
    {
        public FunktionsnetzNetwork(NetOffice.VisioApi.Application pApplication) : base(pApplication)
        {
        }

        protected override Type MappingListType => typeof(FunktionsnetzMapping);

        protected override List<string> ShapeTemplateFiles => new List<string>() { "FunctionalDesign_Function.vssx", "FunctionalDesign_Funktionsnetz.vssx" };

        /// <summary>
        /// overriden verify method to also include model specific checks (e.g. cross model checking)
        /// </summary>
        public override List<ValidationFailedMessage> Validate()
        {
            //step 1-3; if parent function detected errors, return them
            base.Validate();
            if (CollectedValidationMessages.Any())
                return CollectedValidationMessages;

            //step 4
            //cross model message validation: check if all messages exist
            this.ModelList.ForEach(model =>
            {
                var functions = model.ObjectList.Where(obj => obj is Function);
                foreach (var function in functions)
                {
                    try
                    {
                        (function as Function).CheckIfSubModelHasAllMessages();
                    }
                    catch(ValidationFailedException ex)
                    {
                        CollectedValidationMessages.Add(new ValidationFailedMessage(4, ex));
                    }
                }
            });

            return CollectedValidationMessages;
        }
    }
}
