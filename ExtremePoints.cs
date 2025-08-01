using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGAlgorithms;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point>temp = new List<Point>();
            for (int i = 0; i < points.Count; i++) if (!temp.Contains(points[i])) temp.Add(points[i]);
            points = temp;
            List<Point> Eliminated = new List<Point>();
            for (int i = 0; i < points.Count; i++)
                for (int j = 0; j < points.Count && i < points.Count; j++)
                    for (int k = 0; k < points.Count && i<points.Count; k++)
                        for (int l = 0; l < points.Count && i < points.Count; l++)
                            if (points[i] != points[j] && points[i] != points[k] && points[i] != points[l])
                                if (((HelperMethods.PointInTriangle(points[i], points[j], points[k], points[l]) == Enums.PointInPolygon.Inside)) || ((HelperMethods.PointInTriangle(points[i], points[j], points[k], points[l]) == Enums.PointInPolygon.OnEdge)) && !Eliminated.Contains(points[i]))
                                    Eliminated.Add(points[i++]);

            for (int i = 0; i < points.Count; i++)
                if (!Eliminated.Contains(points[i]) && !outPoints.Contains(points[i]))
                    outPoints.Add(points[i]);
        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}