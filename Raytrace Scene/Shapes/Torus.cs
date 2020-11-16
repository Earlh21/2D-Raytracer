using System.Numerics;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene
{
	public class Torus : Shape
	{
		public Vector2 Position { get; set; }
		public float InnerRadius { get; set; }
		public float OuterRadius { get; set; }
		
		public Torus(Vector2 position, float inner_radius, float outer_radius)
		{
			Position = position;
			InnerRadius = inner_radius;
			OuterRadius = outer_radius;
		}
		
		public override bool ContainsPoint(Vector2 point)
		{
			float distance = (point - Position).Length();
			return distance < OuterRadius && distance > InnerRadius;
		}

		public override Vector2 GetNormal(Ray ray)
		{
			Vector2 intersect = GetIntersectPoint(ray);
			Vector2 normal = intersect - Position;
			float length = normal.Length();
			float intersect_distance = (intersect - Position).Length();

			if (length != 0)
			{
				normal /= normal.Length();
			}

			if (intersect_distance < OuterRadius)
			{
				return -normal;
			}

			return normal;
		}

		public override bool RayIntersects(Ray ray)
		{
			return Intersect.RayToCircleIntersection(ray, Position, OuterRadius) != null;
		}

		public override Vector2 GetIntersectPoint(Ray ray)
		{
			float distance = (ray.Origin - Position).Length();

			if (distance > OuterRadius)
			{
				Vector2? intersect = Intersect.RayToCircleIntersection(ray, Position, OuterRadius);

				if (intersect == null)
				{
					return default;
				}

				return (Vector2) intersect;
			}
			
			Vector2? inner_intersect = Intersect.RayToCircleIntersection(ray, Position, InnerRadius);

			if (inner_intersect == null)
			{
				Vector2? intersect = Intersect.RayToCircleIntersection(ray, Position, OuterRadius);

				if (intersect == null)
				{
					return default;
				}

				return (Vector2) intersect;
			}

			return (Vector2) inner_intersect;
		}
	}
}