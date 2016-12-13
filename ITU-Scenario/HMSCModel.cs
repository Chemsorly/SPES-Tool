using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetOffice.VisioApi;
using SPES_Modelverifier_Base;

namespace ITU_Scenario
{
    public class HMSCModel : SPES_Modelverifier_Base.Models.Model
    {
        public override List<Type> AllowedItems => new List<Type>()
        {
            typeof(ConnectionPoint) ,
            typeof(StartSymbol) ,
            typeof(EndSymbol) ,
            typeof(BMSCReference) ,
            typeof(ConnectionArrow) ,
            typeof(InlineExpressionLoop)
        };
    }
}
