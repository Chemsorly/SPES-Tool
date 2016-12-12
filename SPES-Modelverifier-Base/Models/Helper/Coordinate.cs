namespace SPES_Modelverifier_Base.Models.Helper
{
    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public bool IsContainedIn(Coordinate pTopleft, Coordinate pTopright, Coordinate pBottomleft,
            Coordinate pBottomright)
        {
            return this.X > pTopleft.X &&
                   this.X < pTopright.X &&
                   this.X > pBottomleft.X &&
                   this.X < pBottomright.X &&
                   this.Y < pTopleft.Y &&
                   this.Y < pTopright.Y &&
                   this.Y > pBottomleft.Y &&
                   this.Y > pBottomright.Y;
        }
    }
}
