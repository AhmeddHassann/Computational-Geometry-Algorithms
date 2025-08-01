using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        #region Get Min And Max 
        public static int MinX(List<Point> points)
        {
            double minX = points[0].X;
            double maxX = points[0].X;
            int index = 0;
            int index2 = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < minX)
                {
                    minX = points[i].X;
                    index = i;
                    return index;
                }
            }
            return 0;
        }
        public static int MaxX(List<Point> points)
        {
            double minX = points[0].X;
            double maxX = points[0].X;
            int index = 0;
            int index2 = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X > maxX)
                {
                    maxX = points[i].X;
                    index2 = i;
                    return index2;
                }
            }
            return 0;
        }
        public static int MinAndMaxY(List<Point> points)
        {
            double minY = points[0].Y;
            double maxY = points[0].Y;
            int index = 0;
            int index2 = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < minY)
                {
                    minY = points[i].Y;
                    index = i;
                    return index;
                }
            }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y > maxY)
                {
                    maxY = points[i].Y;
                    index2 = i;
                    return index2;
                }
            }
            return 0;
        }

        #endregion

        #region sort Anti_colckwise
        List<Point> Sorting(List<Point> points)
        {
            int Min_y, NumberOfPoints;
            Min_y = MinAndMaxY(points);
            NumberOfPoints = points.Count;
            Point new_point = new Point(points[Min_y].X + 1000, points[Min_y].Y);
            Line line = new Line(points[Min_y], new_point);
            List<Tuple<double, int>> My_list = new List<Tuple<double, int>>();
            List<Point> Sort = new List<Point>();
            for (int i = 0; i < NumberOfPoints; i++)
            {
                Point vector1, vector2;
                double dot_product, cross_product, angle, degree_angle;
                if (i == Min_y)
                    continue;

                vector1 = points[Min_y].Vector(new_point);
                vector2 = points[Min_y].Vector(points[i]);
                cross_product = HelperMethods.CrossProduct(vector1, vector2);
                dot_product = HelperMethods.DotProduct(vector1, vector2);
                angle = Math.Atan2(cross_product, dot_product);
                degree_angle = angle * (180 / Math.PI);
                if (degree_angle < 0)
                    degree_angle = degree_angle + 360;
                My_list.Add(Tuple.Create(degree_angle, i));
            }
            My_list.Sort((first, second) =>
            {
                if (first.Item1 == second.Item1)
                    return first.Item2.CompareTo(second.Item2);
                return first.Item1.CompareTo(second.Item1);
            });
            Sort.Add(points[Min_y]);

            foreach (var p in My_list)
            {
                Sort.Add(points[p.Item2]);
            }
            return Sort;
           
        }
        #endregion

        #region Get convex hull

        List<Point> GetConvexHull(List<Point> points)
        {
            int NumberOfPoints = points.Count;
            double midpoint;
            double sum = 0;
            if (NumberOfPoints < 4)
                return Sorting(points);
            List<Point> Left = new List<Point>();
            List<Point> Right = new List<Point>();  
            List<Point> Left_Hull = new List<Point>();
            List<Point> Right_Hull = new List<Point>();
            foreach (Point s in points)
                sum = sum + s.X;

            midpoint = sum / NumberOfPoints;
            foreach (Point point in points)
            {
                if (point.X < midpoint)
                    Left.Add(point);
                else
                    Right.Add(point);
            }
            //List<Point> RightPoints = new List<Point> { Right[0], Right[Right.Count - 1] };
            //List<Point> Leftpoints = new List<Point> { Left[0], Left[Left.Count - 1] };
            if (Left.Count == 0)
                return new List<Point> { Right[0], Right[Right.Count - 1] };
            if (Right.Count == 0)
                return new List<Point> { Left[0], Left[Left.Count - 1] };

             Left_Hull = GetConvexHull(Left);
             Right_Hull = GetConvexHull(Right);

            return Merge(Right_Hull, Left_Hull);
        }
        #endregion

        #region Get tangent


        Tuple<int, int> DrawTangent(List<Point> LeftHull, List<Point> RightHull, int Left, int Right)
        {
            Line Tangent_line = new Line(LeftHull[Left], RightHull[Right]);
            while (true)
            {
                while (HelperMethods.CheckTurn(Tangent_line, LeftHull[(Left + 1) % LeftHull.Count]) == Enums.TurnType.Right ||
                       HelperMethods.CheckTurn(Tangent_line, LeftHull[(Left - 1 + LeftHull.Count) % LeftHull.Count]) == Enums.TurnType.Right)
                {
                    Left = (Left - 1 + LeftHull.Count) % LeftHull.Count;
                    Tangent_line = new Line(LeftHull[Left], RightHull[Right]);
                }
                while (HelperMethods.CheckTurn(Tangent_line, RightHull[(Right + 1) % RightHull.Count]) == Enums.TurnType.Right ||
                       HelperMethods.CheckTurn(Tangent_line, RightHull[(Right - 1 + RightHull.Count) % RightHull.Count]) == Enums.TurnType.Right)
                {
                    Right = (Right + 1) % RightHull.Count;
                    Tangent_line = new Line(LeftHull[Left], RightHull[Right]);
                }
                if (HelperMethods.CheckTurn(Tangent_line, LeftHull[(Left + 1) % LeftHull.Count]) != Enums.TurnType.Right &&
                    HelperMethods.CheckTurn(Tangent_line, LeftHull[(Left - 1 + LeftHull.Count) % LeftHull.Count]) != Enums.TurnType.Right &&
                    HelperMethods.CheckTurn(Tangent_line, RightHull[(Right + 1) % RightHull.Count]) != Enums.TurnType.Right &&
                    HelperMethods.CheckTurn(Tangent_line, RightHull[(Right - 1 + RightHull.Count) % RightHull.Count]) != Enums.TurnType.Right)
                    break;
            }
            return Tuple.Create(Left, Right);
        }
        #endregion

        #region Merge two convex hull
        List<Point> Merge(List<Point> LeftHull, List<Point> RightHull)
        {
            // get max x of left hull
            int Left = MaxX(LeftHull);
            // get min x right hull
            int Right = MinX(RightHull);
       
            Tuple<int, int> LowerTangent = DrawTangent(LeftHull, RightHull, Left, Right);
            Tuple<int, int> UpperTangent = DrawTangent(RightHull, LeftHull, Right, Left);
            int MinX_lower = LowerTangent.Item2;
            int MaxX_lower = LowerTangent.Item1;
            int MaxX_upper = UpperTangent.Item2;
            int MinX_upper = UpperTangent.Item1;
            int Npoints_Rhull = RightHull.Count;
            int Npoints_lhull = LeftHull.Count;
            List<Point> Merged_points = new List<Point>();
            // loop on the right hull 
            for (int i = MinX_lower; i != MinX_upper; i = (i + 1) % Npoints_Rhull)
            {
                Merged_points.Add(RightHull[i]);
            }
            Merged_points.Add(RightHull[MinX_upper]);
            // loop on left hull 
            for (int i = MaxX_upper; i != MaxX_lower; i = (i + 1) % Npoints_lhull)
            {
                Merged_points.Add(LeftHull[i]);
            }
            Merged_points.Add(LeftHull[MaxX_lower]);
            
            for (int i = 0; i < Merged_points.Count; i++)
                if (HelperMethods.CheckTurn(new Line(Merged_points[(i - 1 + Merged_points.Count) % Merged_points.Count], Merged_points[i]), Merged_points[(i + 1) % Merged_points.Count]) == Enums.TurnType.Colinear)
                    Merged_points.RemoveAt(i);

            return Merged_points;
        }
        #endregion

       

        #region Algorithm
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points.Sort((a, b) => {
                if (a.X == b.X) 
                    return a.Y.CompareTo(b.Y);
                return a.X.CompareTo(b.X);
            });
            outPoints = GetConvexHull(points);
        }

        #endregion

     

       
        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }
    }
}