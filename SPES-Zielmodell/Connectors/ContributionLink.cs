using SPES_Modelverifier_Base.Models;
using SPES_Zielmodell.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPES_Zielmodell.Connectors
{
    class ContributionLink : Connection
    {
        public override List<Type> AllowedConnectedTypes => new List<Type>() { typeof(Actor), typeof(Belief), typeof(Goal), typeof(Indicator), typeof(Resource), typeof(Task) };

        public override bool Inverted => true;
    }
}
