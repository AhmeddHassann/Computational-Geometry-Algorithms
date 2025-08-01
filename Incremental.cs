using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities.DataStructures;


namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        #region calculate angles
        //calculating angles between two vectors 
        public static double Calc_angle(Point a, Point b)
        {
            double  cross_product, dot_product, angle, degree_angle;
            Point new_point, FirstVec, SecVec;
            new_point = new Point(a.X + 1000, a.Y);
            FirstVec = a.Vector(new_point);
            SecVec = a.Vector(b);
            dot_product = HelperMethods.DotProduct(FirstVec, SecVec);
            cross_product = HelperMethods.CrossProduct(FirstVec, SecVec);
            
            angle = Math.Atan2(cross_product, dot_product);
            degree_angle = angle * (180 / Math.PI);
            if (degree_angle < 0)
                degree_angle = degree_angle + 360;
            return degree_angle;
        } 
        #endregion

        #region Calculate center of triangle 
        public static Point Calc_Center(Point a, Point b, Point c)
        {
            double x, y;
            x = (a.X + b.X + c.X) / 3.0f;
            y = (a.Y + b.Y + c.Y) / 3.0f;
            return new Point(x, y);

        }
        #endregion

        #region Algorithm
        Line line;
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            OrderedSet<Point> set = new OrderedSet<Point>(sort_angles);
            Point Center_Point, new_point, Previous, Next;
            int NumberOfPoints = points.Count();
            if (points.Count < 4)
            {
                outPoints = points;
                return;
            }
            Center_Point = Calc_Center(points[0], points[1], points[2]);
            new_point = new Point(Center_Point.X + 1000, Center_Point.Y);
            line = new Line(Center_Point, new_point);

            set.Add(points[0]);
            set.Add(points[1]);
            set.Add(points[2]);
            for (int i = 3; i < NumberOfPoints; i++)
            { 
                Previous = set.PreviousAndNext(points[i]).Value;
                Next = set.PreviousAndNext(points[i]).Key;
                // the point lies outside the polygon
                if (HelperMethods.CheckTurn(new Line(Previous, Next), points[i]) == Enums.TurnType.Right)
                {

                    Point NewPrevious = set.PreviousAndNext(Previous).Value;
                    Point NewNext = set.PreviousAndNext(Next).Key;
                    // clockwise
                    while (HelperMethods.CheckTurn(new Line(points[i], Next), NewNext) == Enums.TurnType.Right || HelperMethods.CheckTurn(new Line(points[i], Next), NewNext) == Enums.TurnType.Colinear)
                    { 
                        set.Remove(Next);
                        Next = NewNext;
                        NewNext = set.PreviousAndNext(Next).Key;
                    }
                    //anti clockwise
                    while (HelperMethods.CheckTurn(new Line(points[i], Previous), NewPrevious) == Enums.TurnType.Left || HelperMethods.CheckTurn(new Line(points[i], Previous), NewPrevious) == Enums.TurnType.Colinear)
                    {
                        set.Remove(Previous);
                        Previous = NewPrevious;
                        NewPrevious = set.PreviousAndNext(Previous).Value;
                    }
                    set.Add(points[i]);
                }
            }
            List<Point> Convex_hull = new List<Point>();
            Convex_hull = set.ToList();
            outPoints = Convex_hull;

        }
        #endregion

        #region sorting angles 
        //sort the angles between two points 
        public int sort_angles(Point a, Point b)
        {
            double First_Angle, Second_Angle;
            First_Angle = Calc_angle(line.Start, a);
            Second_Angle = Calc_angle(line.Start, b);
            if (First_Angle > Second_Angle)
                return 1;
            else if (First_Angle < Second_Angle)
                return -1;
            else
                return 0;
        } 
        #endregion
        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}