using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Verhaltensperspektive
{
    public class VerhaltensperspektiveNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<String> { "SMT_SM.vssx" };

        protected override Type MappingListType => null;

        public VerhaltensperspektiveNetwork(Application pApplication) : base(pApplication)
        {
        }

        public override string ModelName => "Behavioral Perspective";
        //public override string ToString()
        //{
        //    return "Behavioral Perspective";
        //}
    }
}
