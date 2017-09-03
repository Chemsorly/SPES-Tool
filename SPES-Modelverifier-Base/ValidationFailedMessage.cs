using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Items;

namespace SPES_Modelverifier_Base
{
    public class ValidationFailedMessage
    {
        public int ProcessLevel { get; }
        public String Message { get; }
        public BaseObject ExceptionObject { get; }
        public String Sheet => ExceptionObject?.Visiopage;

        public ValidationFailedMessage(int pProcessLevel, String pMessage, BaseObject pTargetobject = null)
        {
            ProcessLevel = pProcessLevel;
            Message = pMessage;
            ExceptionObject = pTargetobject;
        }

        public ValidationFailedMessage(int pProcessLevel, ValidationFailedException pException) : this(pProcessLevel, pException.Message, pException.ExceptionObject)
        {

        }

        public bool ValuesEquals(object obj)
        {
            //check if proper object type, if not false
            if (!(obj is ValidationFailedMessage))
                return false;

            var message = (ValidationFailedMessage) obj;

            //compare fields
            return (this.ProcessLevel == message.ProcessLevel) &&
                   (this.Message == message.Message) &&
                   (this.ExceptionObject == message.ExceptionObject);
        }
    }

    public delegate void ValidationFailedDelegate(ValidationFailedMessage pArgs);
}
