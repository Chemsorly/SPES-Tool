using System;
using SPES_Modelverifier_Base.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Items.Helper;

namespace SPES_Zielmodell.Items
{
    public class ActorBoundary : Container
    {
        /// <summary>
        /// actor boundary is an ellipse and not a standard block-shape
        /// </summary>
        /// <returns></returns>
        protected override List<BaseObject> GetContainingItems()
        {
            return ParentModel.ObjectList.Where(t => t != this && ObjectIsInEllipse(t)).ToList();
        }

        /// <summary>
        /// checks if all corner points of a shape are inside the ellipse
        /// </summary>
        /// <param name="pObject">object to check</param>
        /// <returns>true if all four points are inside ellipse boundaries. returns false if 1 or more are not</returns>
        private bool ObjectIsInEllipse(BaseObject pObject)
        {
            //from https://stackoverflow.com/questions/13285007/how-to-determine-if-a-point-is-within-an-ellipse

            //check if all four edge points are inside the ellipse
            List<Coordinate> coordinatesToCheck = new List<Coordinate>
                {
                    pObject.Locationbottomleft,
                    pObject.Locationbottomright,
                    pObject.Locationtopleft,
                    pObject.Locationtopright
                };

            foreach (Coordinate cord in coordinatesToCheck)
            {
                Coordinate center = new Coordinate()
                {
                    X = this.Locationx,
                    Y = this.Locationy
                };
                double xRadius = this.Width / 2;
                double yRadius = this.Height / 2;

                Coordinate normalized = new Coordinate()
                {
                    X = cord.X - center.X,
                    Y = cord.Y - center.Y
                };

                //return false if any of the edge coordinates is outside the ellipse boundaries
                if (!(((double) (normalized.X * normalized.X) / (xRadius * xRadius)) +
                    ((double) (normalized.Y * normalized.Y) / (yRadius * yRadius)) <= 1.0))
                    return false;
            }

            return true;
        }
    }
}
