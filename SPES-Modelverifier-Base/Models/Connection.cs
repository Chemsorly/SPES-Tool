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
        /// a connection item has to define what it can be connected to. Empty list means it can connect any object derived from BaseObject
        /// </summary>
        public abstract List<Type> AllowedConnectionTypes { get; }

        /// <summary>
        /// define if the connection needs to be inverted (in case shape is directional and aligned wrong).
        /// </summary>
        public abstract bool Inverted { get; }


        public BaseObject FromObject { get; private set; }
        public BaseObject ToObject { get; private set; }

        public void SetConnections(List<BaseObject> pAllObjects)
        {
            //set from and to functions
            BaseObject xFromObject = this.GetObjectConnectingFrom(pAllObjects);
            BaseObject xToObject = this.GetObjectConnectingTo(pAllObjects);

            if (xFromObject != null && xToObject != null &&
                (AllowedConnectionTypes.Any() ? AllowedConnectionTypes.Contains(xFromObject.GetType()) : true) &&
                (AllowedConnectionTypes.Any() ? AllowedConnectionTypes.Contains(xToObject.GetType()) : true))
            {
                //flip from and to because visio
                if (Inverted)
                {
                    this.FromObject = xToObject;
                    this.ToObject = xFromObject;
                }
                else
                {
                    this.FromObject = xFromObject;
                    this.ToObject = xToObject;
                }

                //set connection items at target objects
                if (FromObject is Item)
                    (FromObject as Item).Connections.Add(this);
                if (ToObject is Item)
                    (ToObject as Item).Connections.Add(this);
            }
            else
                throw new ValidationFailedException(this, this.GetType().Name + " " + this.Uniquename + " doesn't connect to two allowed types and/or connected items are null.");
        }

        private BaseObject GetObjectConnectingTo(List<BaseObject> pAllObjects)
        {
            try
            {
                return pAllObjects.Find(t => t.Visioshape == this.Visioshape.Connects[1].ToSheet);
            }
            catch { return null; }
        }

        private BaseObject GetObjectConnectingFrom(List<BaseObject> pAllObjects)
        {
            try
            {
                return pAllObjects.Find(t => t.Visioshape == this.Visioshape.Connects[2].ToSheet);
            }
            catch { return null; }
        }
    }
}
