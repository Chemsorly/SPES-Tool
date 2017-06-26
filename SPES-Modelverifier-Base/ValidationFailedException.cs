using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Items;

namespace SPES_Modelverifier_Base
{
    public class ValidationFailedException : Exception
    {
        /// <summary>
        /// the model object the exception is referencing to
        /// </summary>
        public BaseObject ExceptionObject { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ExceptionObject">the exception object</param>
        /// <param name="message">the exception message</param>
        public ValidationFailedException(BaseObject ExceptionObject, String message) : base(message)
        {
            this.ExceptionObject = ExceptionObject;
        }
    }
}
