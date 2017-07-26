using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Items.Helper;
using SPES_Modelverifier_Base.Models;

namespace SPES_Modelverifier_Base.Items
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
        public String TypeName { get; set; }

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
        [XmlIgnore]
        public NetOffice.VisioApi.IVShape Visioshape { get; internal set; }

        /// <summary>
        /// the corresponding model the object belongs to
        /// </summary>
        [XmlIgnore]
        public Model ParentModel { get; set; }

        /// <summary>
        /// containers this object belongs to
        /// </summary>
        [XmlIgnore]
        public List<Container> Containers { get; } = new List<Container>();

        /// <summary>
        /// returns the x value for the center of the shape
        /// </summary>
        public double Locationx { get; set; }

        /// <summary>
        /// returns the y value for the center of the shape
        /// </summary>
        public double Locationy { get; set; }

        /// <summary>
        /// returns the height of the object
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// returns the width of the object
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// returns the coordinates for the topleft edge
        /// </summary>
        public Coordinate Locationtopleft => new Coordinate() {X = Locationx - Width * 0.5, Y = Locationy + Height * 0.5 } ;

        /// <summary>
        /// returns the coordinates for the topright edge
        /// </summary>
        public Coordinate Locationtopright => new Coordinate() { X = Locationx + Width * 0.5, Y = Locationy + Height * 0.5 };

        /// <summary>
        /// returns the coordinates for the bottomleft edge
        /// </summary>
        public Coordinate Locationbottomleft => new Coordinate() { X = Locationx - Width * 0.5, Y = Locationy - Height * 0.5 };

        /// <summary>
        /// returns the coordinates for the bottomright edge
        /// </summary>
        public Coordinate Locationbottomright => new Coordinate() { X = Locationx + Width * 0.5, Y = Locationy - Height * 0.5 };

        /// <summary>
        /// stub for validate function
        /// </summary>
        public virtual void Verify() { }

        /// <summary>
        /// stub for initialize function
        /// </summary>
        public virtual void Initialize() { }
    }
}
