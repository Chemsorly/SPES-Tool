using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class Connection : BaseObject
    {
        /// <summary>
        /// a connection item has to define what it can be connected to
        /// </summary>
        public abstract List<Type> AllowedConnectionTypes { get; }


        public BaseObject FromObject { get; private set; }
        public BaseObject ToObject { get; private set; }

        public void SetConnections(List<BaseObject> pAllObjects)
        {
            //set from and to functions
            BaseObject xFromObject = this.GetObjectConnectingFrom(pAllObjects);
            BaseObject xToObject = this.GetObjectConnectingTo(pAllObjects);



            if (xFromObject != null && xToObject != null && AllowedConnectionTypes.Contains(xFromObject.GetType()) && AllowedConnectionTypes.Contains(xToObject.GetType()))
            {
                //this.FromObject = xFromObject;
                //this.ToObject = xToObject;

                ////flip from and to because visio
                //BaseObject stor = this.FromObject;
                //this.FromObject = this.ToObject;
                //this.ToObject = stor;

                //if (this is ConnectionArrow)
                //{
                //    if (this.FromObject is Item && this.ToObject is Item)
                //    {
                //        ((Item)FromObject).Connections.Add(this);
                //        ((Item)ToObject).Connections.Add(this);
                //    }
                //    else
                //    {
                //        throw new ValidationFailedException(this, this.type + " " + this.uniquename + " doesn't connect two items");
                //    }
                //}
                //else if (this is Message)
                //{
                //    //flip from and to because visio
                //    //BaseObject stor = this.FromObject;
                //    //this.FromObject = this.ToObject;
                //    //this.ToObject = stor;

                //    //connecting objects can be either function or dependency, but not both at the same time
                //    if (this.FromObject is Instance && this.ToObject is Instance)
                //    {
                //        ((Instance)FromObject).Connections.Add(this);
                //        ((Instance)ToObject).Connections.Add(this);
                //    }
                //    else
                //        throw new ValidationFailedException(this, this.type + " " + this.uniquename + " doesn't connect a function and a dependency");
                //}
                //else
                    throw new ValidationFailedException(this, "Undefined connecting object.");
            }
            else
                throw new ValidationFailedException(this, this.GetType().Name + " " + this.uniquename + " doesn't connect to two allowed types");
        }

        BaseObject GetObjectConnectingTo(List<BaseObject> pAllObjects)
        {
            try
            {
                return pAllObjects.Find(t => t.visioshape == this.visioshape.Connects[1].ToSheet);
            }
            catch { return null; }
        }

        BaseObject GetObjectConnectingFrom(List<BaseObject> pAllObjects)
        {
            try
            {
                return pAllObjects.Find(t => t.visioshape == this.visioshape.Connects[2].ToSheet);
            }
            catch { return null; }
        }
    }
}
