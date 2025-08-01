using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public List<Point> LowestPoint(Point p1, Point p2)
        {
            List<Point> sortedPoints = new List<Point>();
            Point swaped = p1;
            p1 = p2;
            p2 = swaped;
            sortedPoints.Add(p1);
            sortedPoints.Add(p2);
            return sortedPoints;
        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            int lowestby_y = 0;

            
            

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < points[lowestby_y].Y)
                    lowestby_y = i;

            }
            List<Point> sortedPoints = new List<Point>();
            sortedPoints = LowestPoint(points[0], points[lowestby_y]);

            //correct places in List of points
            points[0] = sortedPoints[0];
            points[lowestby_y] = sortedPoints[1];

            //Sorting points by least angle 
            points = points.OrderBy(p => Math.Atan2(p.Y - points[0].Y, p.X - points[0].X)).ToList();

            List<Point> extremequeue = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                Point First, Middle, Last;
                if (i == 0 || i == 1 || i == 2)
                    extremequeue.Add(points[i]);

                else
                {
                    extremequeue.Add(points[i]);

                    while (i <= points.Count)
                    {
                        First = extremequeue[extremequeue.Count - 1];
                        Middle = extremequeue[extremequeue.Count - 2];
                        Last = extremequeue[extremequeue.Count - 3];
                        Line testLine = new Line(Last, Middle);

                        if (HelperMethods.CheckTurn(testLine, First) == Enums.TurnType.Left)
                            break;

                        else
                            extremequeue.Remove(Middle);

                    }

                }

            }
            outPoints = extremequeue;
            
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}