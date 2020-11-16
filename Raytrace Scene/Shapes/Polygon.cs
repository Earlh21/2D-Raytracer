using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Raytrace_Scene.Maths;

namespace Raytrace_Scene.Shapes
{
    public class Polygon : Shape
    {
        private List<Vector2> points;
        private List<LineSegment> lines;
        private Rectangle AABB;
        public float Roughness { get; set; }

        public List<Vector2> Points
        {
            get => points;
            set
            {
                points = value;
                lines = new List<LineSegment>();
                for (int i = 0, w = points.Count; i < w; i++)
                {
                    lines.Add(GenerateLine(i));
                }

                LineSegment test_line_segment = lines[0];
                float test_normal = test_line_segment.Angle + (float) Math.PI / 2;
                Vector2 test_point = test_line_segment.Midpoint -
                                     new Vector2((float) Math.Cos(test_normal), (float) Math.Sin(test_normal)) * 0.001f;
                bool clockwise = !ContainsPoint(test_point);
                
                foreach (LineSegment line in lines)
                {
                    Vector2 direction = line.End - line.Start;
                    line.Normal = clockwise
                        ? Mathf.Rotate(direction, -Mathf.PI / 2)
                        : Mathf.Rotate(direction, Mathf.PI / 2);
                }
            }
        }
        
        private LineSegment GenerateLine(int index)
        {
            if (index > Points.Count - 1 || index < 0)
            {
                throw new ArgumentException("Line index out of bounds");
            }

            if (index == Points.Count - 1)
            {
                return new LineSegment(points[points.Count - 1], points[0]);
            }
            
            return new LineSegment(Points[index], Points[index + 1]);
        }

        public Polygon(List<Vector2> points)
        {
            Points = points;
        }

        public static Polygon CreateRectangle(float x_left, float y_bottom, float x_right, float y_top)
        {
            List<Vector2> points= new List<Vector2>();
            points.Add(new Vector2(x_left, y_bottom));
            points.Add(new Vector2(x_left, y_top));
            points.Add(new Vector2(x_right, y_top));
            points.Add(new Vector2(x_right, y_bottom));

            return new Polygon(points);
        }

        public override bool ContainsPoint(Vector2 point)
        {
            int intersect_count = 0;
            Ray test_ray = new Ray(point, new Vector2(1, 0), null, default);

            foreach (var line in lines)
            {
                if (line.RayIntersects(test_ray))
                {
                    intersect_count++;
                }
            }

            return intersect_count % 2 != 0;
        }

        public override Vector2 GetNormal(Ray ray)
        {
            foreach (LineSegment line in lines)
            {
                if (line.RayIntersects(ray))
                {
                    return line.Normal;
                }
            }

            return default;
        }

        public override bool RayIntersects(Ray ray)
        {
            foreach (var line in lines)
            {
                if (line.RayIntersects(ray))
                {
                    return true;
                }
            }

            return false;
        }

        public override Vector2 GetIntersectPoint(Ray ray)
        {
            foreach (var line in lines)
            {
                if (line.RayIntersects(ray))
                {
                    return line.RayIntersectionPoint(ray);
                }
            }

            return default;
        }
    }
}