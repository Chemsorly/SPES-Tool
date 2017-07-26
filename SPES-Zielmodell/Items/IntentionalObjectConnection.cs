using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace SPES_Zielmodell.Items
{
    public class IntentionalObjectConnection : Connection
    {
        [XmlIgnore]
        public override List<Type> AllowedConnectedTypes => new List<Type>() { typeof(Actor), typeof(IntentionalObject)};

        public override bool Inverted => false;

        public override void Verify()
        {
            //call base verification
            base.Verify();

            //perform specific verification
            //can not connect two items which belong to the same actor
            if (this.ToObject is IntentionalObject && this.FromObject is IntentionalObject)
            {
                if (((IntentionalObject)this.ToObject).BelongingActorBoundary != null &&
                    ((IntentionalObject)this.FromObject).BelongingActorBoundary != null &&
                    ((IntentionalObject)this.ToObject).BelongingActorBoundary !=
                    ((IntentionalObject)this.FromObject).BelongingActorBoundary)
                {
                    throw new ValidationFailedException(this, "IntentionalObjectConnection cannot connect two IntentionalObject which belong to different actors");
                }
            }
        }
    }
}
