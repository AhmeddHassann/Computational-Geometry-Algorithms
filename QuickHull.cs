using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public List<Point> Solve(List<Point> temp, Point min_x, Point max_x, char c)
        {
            var turnType = Enums.TurnType.Right;
            if (c == 'L') turnType = Enums.TurnType.Left;
            int idx = -1;
            double mx = -1;
            List<Point> ans = new List<Point>();
            if (temp.Count == 0) return temp;
            for (int i = 0; i < temp.Count; i++)
            {
                double dist = HelperMethods.GetDistance(min_x, temp[i]) + HelperMethods.GetDistance(max_x, temp[i]);
                if (HelperMethods.CheckTurn(new Line(min_x, max_x), temp[i]) == turnType && dist > mx)
                {
                    idx = i;
                    mx = dist;
                }
            }
            if (idx == -1)
            {
                ans.Add(min_x);
                ans.Add(max_x);
                return ans;
            }
            List<Point> solve1, solve2;
            char cc = HelperMethods.CheckTurn(new Line(temp[idx], min_x), max_x) == Enums.TurnType.Right ? 'L' : 'R';
            solve1 = Solve(temp, temp[idx], min_x, cc);
            cc = HelperMethods.CheckTurn(new Line(temp[idx], max_x), min_x) == Enums.TurnType.Right ? 'L' : 'R';
            solve2 = Solve(temp, temp[idx], max_x, cc);
            foreach (Point point in solve1) ans.Add(point);
            foreach (Point point in solve2) if (!ans.Contains(point)) ans.Add(point);
            return ans;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> temp = new List<Point>();
            for (int i = 0; i < points.Count; i++) if (!temp.Contains(points[i])) temp.Add(points[i]);
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            Point max_x = new Point(int.MinValue, 0);
            Point min_x = new Point(int.MaxValue, 0);
            foreach (Point point in points)
            {
                if (point.X < min_x.X) min_x = point;
                if (point.X > max_x.X) max_x = point;
            }
            List<Point> solve1 = Solve(points, min_x, max_x, 'L');
            List<Point> solve2 = Solve(points, min_x, max_x, 'R');
            foreach (Point point in solve1) outPoints.Add(point);
            foreach (Point point in solve2) if (!outPoints.Contains(point)) outPoints.Add(point);
        }
        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
