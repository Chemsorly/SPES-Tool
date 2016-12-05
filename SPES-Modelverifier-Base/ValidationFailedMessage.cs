using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base
{
    public class ValidationFailedMessage
    {
        public int ProcessLevel { get; }
        public String Message { get; }
        public BaseObject ExceptionObject { get; }

        public ValidationFailedMessage(int pProcessLevel, String pMessage, BaseObject pTargetobject)
        {
            ProcessLevel = pProcessLevel;
            Message = pMessage;
            ExceptionObject = pTargetobject;
        }

        public ValidationFailedMessage(int pProcessLevel, ValidationFailedException pException) : this(pProcessLevel, pException.Message, pException.ExceptionObject)
        {

        }
    }

    public delegate void ValidationFailedDelegate(ValidationFailedMessage pArgs);
}
