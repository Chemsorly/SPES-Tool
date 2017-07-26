using SPES_Modelverifier_Base;
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

        protected override List<string> ShapeTemplateFiles => new List<string>() { "SMT_FN_Funktionsnetz.vssx", "SMT_FN_Automat.vssx" };

        /// <summary>
        /// overriden verify method to also include model specific checks (e.g. cross model checking)
        /// </summary>
        public override List<ValidationFailedMessage> VerifyModels()
        {
            //step 1-3; if parent function detected errors, return them
            base.VerifyModels();
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
