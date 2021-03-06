﻿using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bowerbird
{
    public class Pathfinder
    {
        public List<Point3d> Points { get; private set; }

        public Pathfinder(List<Point3d> points)
        {
            Points = points;
        }

        public static Pathfinder Create(Path path, Surface surface, Vector3d uv, bool type, double stepSize)
        {
            var points = new List<Point3d>();

            var u = uv.X;
            var v = uv.Y;

            points.Add(surface.PointAt(u, v));

            Vector3d initialDirection = path.InitialDirection(surface, new Vector2d(u, v), type);

            foreach (var initDir in new[] { initialDirection, -initialDirection })
            {
                u = uv.X;
                v = uv.Y;

                points.Reverse();

                var direction = initDir;

                while (true)
                {
                    var delta = RK4(o => path.Direction(surface, o, direction, stepSize), u, v);

                    if (!delta.IsValid)
                        break;

                    var t = new Vector2d(1, 1);

                    var alpha = 1.0;

                    if (u + delta.X > 1 || u + delta.X < 0 || v + delta.Y > 1 || v + delta.Y < 0)
                    {
                        if (delta.X > 0.0)
                            t.X = (1.0 - u) / delta.X;
                        else if (delta.X < 0.0)
                            t.X = (0.0 - u) / delta.X;
                        else
                            t.X = double.PositiveInfinity;

                        if (delta.Y > 0.0)
                            t.Y = (1.0 - v) / delta.Y;
                        else if (delta.Y < 0.0)
                            t.Y = (0.0 - v) / delta.Y;
                        else
                            t.Y = double.PositiveInfinity;

                        alpha = Math.Min(Math.Min(t.X, t.Y), 1.0);

                        delta *= alpha;
                    }

                    u += delta.X;
                    v += delta.Y;

                    var curvature = Curvature.SurfaceCurvature.Create(surface, u, v);

                    direction = delta.X * curvature.A1 + delta.Y * curvature.A2;

                    if (points.Last().DistanceTo(curvature.X) < 1e-5)
                        break;

                    var normal = surface.NormalAt(u, v);

                    points.Add(curvature.X);

                    if (alpha < 1.0)
                        break;

                    if (points.Count > 100000)  // FIXME: Find a better solution
                        break;
                }
            }

            return new Pathfinder(points);
        }

        static Vector2d RK4(Func<Vector2d, Vector2d> func, double u, double v)
        {
            var uv0 = new Vector2d(u, v);
            var d0 = func(uv0);

            var uv1 = uv0 + d0 * 0.5;
            var d1 = func(uv1);

            var uv2 = uv0 + d1 * 0.5;
            var d2 = func(uv2);

            var uv3 = uv0 + d2;
            var d3 = func(uv3);

            var d = (d0 + 2 * d1 + 2 * d2 + d3) / 6;

            if (!d.IsValid)
                return Vector2d.Unset;

            return d0;
        }
    }
}
