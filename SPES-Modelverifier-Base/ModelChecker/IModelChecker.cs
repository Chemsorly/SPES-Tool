using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.ModelChecker
{
    /// <summary>
    /// interface description for checkers to run on a model. checkers are meant to be used for complex validation operations
    /// </summary>
    public abstract class IModelChecker
    {
        /// <summary>
        /// event to throw in case of validation exception
        /// </summary>
        public event ValidationFailedDelegate ValidationFailedEvent;

        /// <summary>
        /// runs the checker against the target model. throws a ValidationFailedEvent if error is found
        /// </summary>
        /// <param name="pModel"></param>
        public abstract void Initialize(Model pModel);

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
