using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Funktionsnetz
{
    internal class Function : ModelReference
    {
        public override List<Type> AllowedReferenceTypes => new List<Type>() { typeof(FunktionsnetzModel), typeof(AutomataModel) };

        /// <summary>
        /// checks if the connected model has all messages from the connected connection items. also distuinguishes between incoming and outgoing messages
        /// </summary>
        public void CheckIfSubModelHasAllMessages()
        {
            //check if linked model exists
            if (LinkedModel == null)
                throw new ValidationFailedException(this, "Function " + this.text + " has no linked model.");

            //collect list of messages in connectors
            List<String> incomingConnectedMessages = this.Connections.Where(t => t.ToObject == this && t.GetType() == typeof(Interaction)).Select(t => t.text).ToList();
            List<String> outgoingConnectedMessages = this.Connections.Where(t => t.FromObject == this && t.GetType() == typeof(Interaction)).Select(t => t.text).ToList();
            List<String> incomingMessagesInModel, outgoingMessagesInModel;

            //case automata
            if (this.LinkedModel is AutomataModel)
            {
                incomingMessagesInModel = this.LinkedModel.ObjectList.Where(t => t is NodeConnection && t.text.Contains('?')).Select(t => t.text).ToList();
                outgoingMessagesInModel = this.LinkedModel.ObjectList.Where(t => t is NodeConnection && t.text.Contains('!')).Select(t => t.text).ToList();
            }
            //case other function model
            else if (this.LinkedModel is FunktionsnetzModel)
            {
                //TODO wat
                incomingMessagesInModel = this.LinkedModel.ObjectList
                    .Where(t => t is Interaction)
                    .Where(t => (t as Interaction).FromObject is ContextFunction) //&& (t as Interaction).connectors.All(p => p.fromObject == t))
                    .Select(t => t.text).ToList();

                outgoingMessagesInModel = this.LinkedModel.ObjectList
                    .Where(t => t is Interaction)
                    .Where(t => (t as Interaction).ToObject is ContextFunction) //&& (t as Interaction).connectors.All(p => p.fromObject == t))
                    .Select(t => t.text).ToList();
            }
            else
                throw new ValidationFailedException(this, "Function " + this.text + " linked model is neither function network nor an automat.");

            //check if collections are equal
            if (incomingConnectedMessages.Count != incomingMessagesInModel.Count)
                throw new ValidationFailedException(this, "Function " + this.text + " incoming message amounts are not equal.");
            if (outgoingConnectedMessages.Count != outgoingMessagesInModel.Count)
                throw new ValidationFailedException(this, "Function " + this.text + " outgoing message amounts are not equal.");
            if (!incomingConnectedMessages.All(t => incomingMessagesInModel.Any(p => p.Contains(t))) || !outgoingConnectedMessages.All(t => outgoingMessagesInModel.Any(p => p.Contains(t))))
                throw new ValidationFailedException(this, "Function " + this.text + " incoming and outgoing messages are not equal to the parent model");
        }
    }
}
