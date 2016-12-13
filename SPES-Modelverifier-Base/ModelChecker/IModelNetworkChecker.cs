using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.ModelChecker
{
    public abstract class IModelNetworkChecker
    {
        /// <summary>
        /// event to throw in case of validation exception
        /// </summary>
        public event ValidationFailedDelegate ValidationFailedEvent;

        /// <summary>
        /// runs the checker against the target model. throws a ValidationFailedEvent if error is found
        /// </summary>
        /// <param name="pModelNetwork"></param>
        public abstract void Initialize(ModelNetwork pModelNetwork);

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
