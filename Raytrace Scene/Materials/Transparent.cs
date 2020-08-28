using System;
using System.Numerics;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public class Transparent : Material
	{
		public float IOR { get; set; }
		public float Opacity { get; set; }
		public float Roughness { get; set; }

		public Transparent(float ior, float opacity, float roughness)
		{
			IOR = ior;
			Opacity = opacity;
			Roughness = roughness;
		}

		private float GetEnterRefraction(Shape shape, Ray ray)
		{
			//TODO: Fix difference and direction
			float reversed_angle = Mathf.Reverse(ray.Angle);
			float normal = shape.GetNormal(ray);
			float incidence = Mathf.AngleDifference(normal, reversed_angle);
			int direction = Math.Sign(reversed_angle - normal);

			return Mathf.NormalizeAngle(Mathf.Reverse(normal) + direction * (float) Mathf.Refract(incidence, 1, IOR));
		}

		private float? GetExitRefraction(Shape shape, Ray ray)
		{
			float normal = shape.GetNormal(ray);
			float reversed_angle = Mathf.Reverse(ray.Angle);
			float reversed_normal = Mathf.Reverse(normal);
			float incidence = Math.Abs(reversed_normal - reversed_angle);
			float? refract = Mathf.Refract(incidence, IOR, 1);

			if (refract == null)
			{
				return null;
			}

			int direction = Math.Sign(reversed_angle - reversed_normal);
			return Mathf.NormalizeAngle(normal + direction * (float) refract);
		}

		public override Ray? GetNewRay(Shape shape, Ray ray)
		{
			float enter_angle = GetEnterRefraction(shape, ray);
			Vector2 enter_point = shape.GetIntersectPoint(ray) +
			                      new Vector2(Mathf.Cos(enter_angle), Mathf.Sin(enter_angle));

			Ray inside_ray = new Ray(enter_point, enter_angle);

			float? exit_angle = GetExitRefraction(shape, inside_ray);

			if (exit_angle == null)
			{
				return null;
			}

			Vector2 exit_point = shape.GetIntersectPoint(inside_ray) +
			                     new Vector2(Mathf.Cos((float) exit_angle), Mathf.Sin((float) exit_angle));

			return new Ray(exit_point, (float) exit_angle);
		}
	}
}