using System.Numerics;
using Raytrace_Scene.Maths;

namespace Raytrace_Scene.Shapes
{
	public abstract class Shape
	{
		public abstract bool ContainsPoint(Vector2 point);

		public abstract float GetNormal(Ray ray);
		
		public float ReflectRay(Ray ray)
		{
			return Mathf.Reflect(ray.Angle, GetNormal(ray));
		}

		public abstract bool RayIntersects(Ray ray);

		public abstract Vector2 GetIntersectPoint(Ray ray);
	}
}