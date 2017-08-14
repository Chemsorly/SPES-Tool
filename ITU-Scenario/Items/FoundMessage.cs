using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base;

namespace ITU_Scenario.Items
{
    public class FoundMessage : BaseLostFoundMessage
    {
        public override void Verify()
        {
            base.Verify();

            //check if corresponding lost message exist
            var lostmessages = this.ParentModel.ObjectList.Where(t => t is LostMessage).Cast<LostMessage>();
            if(lostmessages.All(t => t.Text != Text))
                throw new ValidationFailedException(this,$"No corresponding LostMessage found for Message{this.Text}");
        }
    }
}
