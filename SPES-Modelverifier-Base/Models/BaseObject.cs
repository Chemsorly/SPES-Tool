using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Models.Helper;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class BaseObject
    {
        /// <summary>
        /// contains the text that is written on the shape
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// the shape name
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// the uniquename of the shape (uid); usually Type-UID
        /// </summary>
        public String Uniquename { get; set; }

        /// <summary>
        /// the associated visio page (uid)
        /// </summary>
        public String Visiopage { get; set; }

        /// <summary>
        /// the corresponding visio shape (netoffice)
        /// </summary>
        public NetOffice.VisioApi.IVShape Visioshape { get; internal set; }

        /// <summary>
        /// the corresponding model the object belongs to
        /// </summary>
        public Model ParentModel { get; set; }

        /// <summary>
        /// containers this object belongs to
        /// </summary>
        public List<Container> Containers { get; } = new List<Container>();

        /// <summary>
        /// returns the x value for the center of the shape
        /// </summary>
        public double Locationx
        {
            get
            {
                return Visioshape != null
                    ? Visioshape.Cells("PinX").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric)
                    : locationx;
            }
            set { locationx = value; }
        }
        private double locationx;

        /// <summary>
        /// returns the y value for the center of the shape
        /// </summary>
        public double Locationy
        {
            get
            {
                return Visioshape != null
                    ? Visioshape.Cells("PinY").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric)
                    : locationy;
            }
            set { locationy = value; }
        }
        private double locationy;


        public double Height
        {
            get
            {
                return Visioshape != null
                    ? Visioshape.Cells("Height").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric)
                    : height;
            }
            set { height = value; }
        }
        private double height;

        public double Width
        {
            get
            {
                return Visioshape != null
                    ? Visioshape.Cells("Width").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric)
                    : width;
            }
            set { width = value; }
        }
        private double width;

        public Coordinate Locationtopleft => new Coordinate() {X = Locationx - Width * 0.5, Y = Locationy + Height * 0.5 } ;
        public Coordinate Locationtopright => new Coordinate() { X = Locationx + Width * 0.5, Y = Locationy + Height * 0.5 };
        public Coordinate Locationbottomleft => new Coordinate() { X = Locationx - Width * 0.5, Y = Locationy - Height * 0.5 };
        public Coordinate Locationbottomright => new Coordinate() { X = Locationx + Width * 0.5, Y = Locationy - Height * 0.5 };

        public virtual void Validate()
        {

        }

        public virtual void Initialize()
        {
            
        }
    }
}
