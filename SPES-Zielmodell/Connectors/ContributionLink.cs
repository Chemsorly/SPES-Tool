﻿using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Zielmodell.Connectors
{
    class ContributionLink : Connection
    {
        public override List<Type> AllowedConnectionTypes => throw new NotImplementedException();

        public override bool Inverted => false;
    }
}
