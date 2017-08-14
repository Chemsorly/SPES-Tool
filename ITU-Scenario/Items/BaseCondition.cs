using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Items;

namespace ITU_Scenario.Items
{
    public abstract class BaseCondition : Item
    {
        public String Key => this.Text.Split('=')[0];
        public String Value => this.Text.Contains('=') ? this.Text.Split('=')[1] : String.Empty;

        public override void Verify()
        {
            //check for empty text
            if (String.IsNullOrWhiteSpace(this.Text))
                throw new ValidationFailedException(this, "Text for guarding condition cannot be empty.");

            //check for more than 1 '='
            if (this.Text.Count(t => t == '=') > 1)
                throw new ValidationFailedException(this, "Text for guarding condition contains more than 1 equalizer");

            base.Verify();
        }
    }
}
