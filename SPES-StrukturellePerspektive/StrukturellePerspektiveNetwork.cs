using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_StrukturellePerspektive
{
    public class StrukturellePerspektiveNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<String> { "SMT_Class.vssx" };

        protected override Type MappingListType => null;

        public StrukturellePerspektiveNetwork(Application pApplication) : base(pApplication)
        {
        }

        public override string ModelName => "Structural Perspective";
        //public override string ToString()
        //{
        //    return "Structural Perspective";
        //}
    }
}
