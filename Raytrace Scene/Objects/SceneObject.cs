using System.Numerics;
using Raytrace_Scene.Materials;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Objects
{
	public abstract class SceneObject
	{
		public Shape Shape { get; set; }

		public SceneObject(Shape shape)
		{
			Shape = shape;
		}

		public bool ContainsPoint(Vector2 point)
		{
			return Shape.ContainsPoint(point);
		}

		public Vector2 GetIntersectPoint(Ray ray)
		{
			return Shape.GetIntersectPoint(ray);
		}

		public bool RayIntersects(Ray ray)
		{
			return Shape.RayIntersects(ray);
		}
	}
}