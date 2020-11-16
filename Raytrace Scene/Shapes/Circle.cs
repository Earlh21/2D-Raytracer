using System;
using System.Numerics;
using Raytrace_Scene.Maths;

namespace Raytrace_Scene.Shapes
{
	public class Circle : Shape
	{		
		public Vector2 Position { get; set; }
		public float Radius { get; set; }
		
		public Circle(Vector2 position, float radius)
		{
			Position = position;
			Radius = radius;
		}

		public Circle(float x, float y, float radius) : this(new Vector2(x, y), radius)
		{
			
		}

		public override bool ContainsPoint(Vector2 point)
		{
			return (point - Position).Length() < Radius;
		}

		public override Vector2 GetNormal(Ray ray)
		{
			Vector2 intersect = GetIntersectPoint(ray);
			return (intersect - Position) / Radius;
		}

		public override bool RayIntersects(Ray ray)
		{
			return Intersect.RayToCircleIntersection(ray, Position, Radius) != null;
		}

		public override Vector2 GetIntersectPoint(Ray ray)
		{
			Vector2? intersect = Intersect.RayToCircleIntersection(ray, Position, Radius);
			if (intersect == null)
			{
				return default;
			}

			return (Vector2) intersect;
		}
	}
}