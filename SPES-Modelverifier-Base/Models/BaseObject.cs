using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class BaseObject
    {
        public String Text { get; set; }
        public String Uniquename { get; set; }
        public String Visiopage { get; set; }
        public NetOffice.VisioApi.IVShape Visioshape { get; internal set; }
        public Model ParentModel { get; set; }
        public double Locationx { get; set; }
        public double Locationy { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public Coordinate Locationtopleft => new Coordinate() {X = Locationx - Width * 0.5, Y = Locationy + Height * 0.5 } ;
        public Coordinate Locationtopright => new Coordinate() { X = Locationx + Width * 0.5, Y = Locationy - Height * 0.5 };
        public Coordinate Locationbottomleft => new Coordinate() { X = Locationx - Width * 0.5, Y = Locationy + Height * 0.5 };
        public Coordinate Locationbottomright => new Coordinate() { X = Locationx + Width * 0.5, Y = Locationy - Height * 0.5 };

        public virtual void Validate()
        {

        }

        public virtual void Initialize()
        {
            
        }

        public struct Coordinate
        {
            public double X;
            public double Y;
        }
    }
}
