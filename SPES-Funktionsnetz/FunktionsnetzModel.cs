﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetOffice.VisioApi;
using SPES_Modelverifier_Base;

namespace SPES_Funktionsnetz
{
    class FunktionsnetzModel : SPES_Modelverifier_Base.Models.Model
    {
        public FunktionsnetzModel(Page pPage, MappingList pMapping) : base(pPage, pMapping)
        {
        }

        public override List<Type> AllowedItems => null;
    }
}
