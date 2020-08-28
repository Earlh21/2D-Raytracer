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

		public override float GetNormal(Ray ray)
		{
			//TODO: Fix difference and direction
			float center_angle = Mathf.AngleTo(ray.Origin, Position);
			float theta = Math.Abs(center_angle - ray.Angle);
			float distance = (ray.Origin - Position).Length();
			float phi = Mathf.NormalizeAngle((float) Math.Asin(distance * (float) Math.Sin(theta) / Radius) - theta);
			int direction = Math.Sign(ray.Angle - center_angle);
			float result = Mathf.NormalizeAngle(Mathf.NormalizeAngle(center_angle + Mathf.PI) - direction * phi);

			if (float.IsNaN(result))
			{
				return 0;
			}

			if (ContainsPoint(ray.Origin))
			{
				return Mathf.Reverse(result);
			}

			return result;
		}

		public override bool RayIntersects(Ray ray)
		{
			float angle_to_center = Mathf.AngleTo(Position, ray.Origin);
			float left_angle = angle_to_center - Mathf.PI / 2;
			Vector2 offset = Radius * new Vector2(Mathf.Cos(left_angle), Mathf.Sin(left_angle));
			Vector2 left_orthog = Position + offset;
			Vector2 right_orthog = Position - offset;
		}

		public override Vector2 GetIntersectPoint(Ray ray)
		{
			float normal = GetNormal(ray);
			return Position + Radius * new Vector2(Mathf.Cos(normal), Mathf.Sin(normal));
		}
	}
}