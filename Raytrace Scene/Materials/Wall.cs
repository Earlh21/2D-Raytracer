using System.Numerics;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public class Wall : Material
	{
		public float Roughness { get; set; }

		public Wall(float roughness)
		{
			Roughness = roughness;
		}

		public override Ray? GetNewRay(Shape shape, Ray ray)
		{
			float normal = shape.GetNormal(ray);
			float new_angle = Mathf.NormalizeAngle(normal + ((float) Program.R.NextDouble() - 0.5f) * Mathf.PI2 * 0.99f);
			Vector2 new_origin =
				shape.GetIntersectPoint(ray) + 0.01f * new Vector2(Mathf.Cos(normal), Mathf.Sin(normal));
			return new Ray(new_origin, new_angle);
		}
	}
}