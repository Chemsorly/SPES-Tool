using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.Items
{
    public class EndSymbol : StartEndItem
    {
        public override bool IsStart => false;
    }
}
