using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    internal abstract class BaseObject
    {
        public String text { get; set; }
        public String uniquename { get; set; }
        public String visiopage { get; set; }
        public NetOffice.VisioApi.IVShape visioshape { get; internal set; }
        public double locationx { get; set; }
        public double locationy { get; set; }
        public double height { get; set; }
        public double width { get; set; }
    }
}
