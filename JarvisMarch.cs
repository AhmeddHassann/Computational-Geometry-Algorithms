using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> temp = new List<Point>();
            int n = points.Count;
            for (int i = 0; i < n; i++) if (!temp.Contains(points[i])) temp.Add(points[i]);
            points = temp;
            n = points.Count;
            if (n <= 3)
            {
                outPoints = points;
                return;
            }
            int minYIdx = 0;
            for (int i = 0; i < n; i++) if (points[i].Y < points[minYIdx].Y) minYIdx = i;
            int st = minYIdx, en;
            while (true)
            {
                outPoints.Add(points[st]);
                en = (st + 1) % n;
                for (int i = 0; i < n; i++)
                {
                    if (i == st || i == en) continue;
                    if (HelperMethods.CheckTurn(new Line(points[st], points[i]), points[en]) == Enums.TurnType.Colinear)
                        if (HelperMethods.GetDistance(points[st], points[i]) > HelperMethods.GetDistance(points[st], points[en]))
                            en = i;
                    if (HelperMethods.CheckTurn(new Line(points[st], points[i]), points[en]) == Enums.TurnType.Right)
                        en = i;
                }
                st = en;
                if (st == minYIdx) break;
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
