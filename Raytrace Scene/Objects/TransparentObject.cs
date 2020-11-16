using System.Numerics;
using Raytrace_Scene.Materials;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Objects
{
	public class TransparentObject : SceneObject
	{
		public TransparentMaterial Material { get; set; }
		public float IOR => Material.IOR;

		public TransparentObject(Shape shape, TransparentMaterial material) : base(shape)
		{
			Material = material;
		}
		
		public bool RefractRayIn(Ray ray)
		{
			float enter_ior = 1;

			if (ray.Medium != null)
			{
				enter_ior = ray.Medium.IOR;
			}
			
			Vector2 normal = Shape.GetNormal(ray);
			Vector2? direction = Mathf.RefractNormal(ray.Direction, normal, enter_ior, IOR);

			if (direction == null)
			{
				return false;
			}
			
			Vector2 position = Shape.GetIntersectPoint(ray) + ray.Direction * 0.1f;
			float distance = (position - ray.Origin).Length();
			
			ray.Origin = position;
			ray.Direction = (Vector2) direction;
			ray.Medium = this;
			
			Material.AffectRay(ray, distance);
			return true;
		}

		public bool RefractRayOut(Ray ray, TransparentObject new_medium)
		{
			float exit_ior = 1;

			if (new_medium != null)
			{
				exit_ior = new_medium.IOR;
			}
			
			Vector2 normal = Shape.GetNormal(ray);
			Vector2? direction = Mathf.RefractNormal(ray.Direction, -normal, IOR, exit_ior);

			if (direction == null)
			{
				return false;
			}
			
			Vector2 position = Shape.GetIntersectPoint(ray) + (Vector2)direction * 0.1f;
			float distance = (position - ray.Origin).Length();
			
			ray.Origin = position;
			ray.Direction = (Vector2) direction;
			ray.Medium = new_medium;
			
			Material.AffectRay(ray, distance);
			return true;
		}

		public bool GetBounceRay(Ray ray)
		{
			return Material.BounceRay(Shape, ray);
		}
	}
}