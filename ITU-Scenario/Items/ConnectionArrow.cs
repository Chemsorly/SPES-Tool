﻿using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU_Scenario
{
    internal class ConnectionArrow : Connection
    {
        public override List<Type> AllowedConnectionTypes => new List<Type>() {
            typeof(ConnectionPoint),
            typeof(StartSymbol),
            typeof(EndSymbol),
            typeof(BMSCReference)};

        public override bool Inverted => false;
    }
}