using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> temp = new List<Point>();
            List<Point> eliminated = new List<Point>();
            for (int i = 0; i < points.Count; i++)
                if (!temp.Contains(points[i]))temp.Add(points[i]);
            points = temp;
            
            if (points.Count == 1 || points.Count == 2)
            {
                outPoints = points;
                return;
            }

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    bool left = false, right = false;
                    if (i == j) continue;

                    Line line = new Line(points[i], points[j]);
                    for (int k = 0; k < points.Count; k++)
                    {
                        if (k == i || k == j) continue;

                        if (HelperMethods.CheckTurn(line, points[k]) == Enums.TurnType.Right)
                            right = true;
                        else if (HelperMethods.CheckTurn(line, points[k]) == Enums.TurnType.Left)
                            left = true;

                    }
                    if (!left || !right)
                    {
                        outPoints.Add(points[i]);
                        outPoints.Add(points[j]);
                    }
                }
            }
            temp.Clear();
            for (int i = 0; i < outPoints.Count; i++)
                if (!temp.Contains(outPoints[i]))temp.Add(outPoints[i]);
            outPoints = temp;

            for (int i = 0; i < outPoints.Count; i++)
                for (int j = 0; j < outPoints.Count && i < outPoints.Count; j++)
                    for (int k = 0; k < outPoints.Count && i < outPoints.Count; k++)
                        if (outPoints[i] != outPoints[j] && outPoints[i] != outPoints[k])
                            if (HelperMethods.PointOnSegment(outPoints[i], outPoints[j], outPoints[k]))
                                eliminated.Add(outPoints[i++]);

            for (int i = 0; i < eliminated.Count; i++) outPoints.Remove(eliminated[i]);
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
