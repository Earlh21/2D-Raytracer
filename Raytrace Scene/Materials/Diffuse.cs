using System;
using System.Drawing;
using System.Numerics;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Objects;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public class Diffuse : SolidMaterial
	{
		public float Roughness { get; set; }
		public override Vector3 GetColor(SolidObject solid_object, Vector2 point)
		{
			return Vector3.Zero;
		}

		public Diffuse(float roughness)
		{
			Roughness = roughness;
		}

		public override bool BounceRay(Shape shape, Ray ray)
		{
			Vector2 normal = shape.GetNormal(ray);
			float new_angle = Mathf.AngleTo(Vector2.Zero, normal) + (Mathf.RandomFloat() - 0.5f) * Mathf.PI2 * 0.99f;
			ray.Origin = ray.Origin + normal * 0.01f;
			ray.Direction = Mathf.FromAngle(new_angle);
			return true;
		}
	}
}