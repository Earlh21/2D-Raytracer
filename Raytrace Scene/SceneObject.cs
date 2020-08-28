using System.Numerics;

namespace Raytrace_Scene
{
	public abstract class SceneObject
	{
		public Shape Shape { get; set; }
		public Material Material { get; set; }

		protected SceneObject(Shape shape)
		{
			Shape = shape;
		}

		public bool ContainsPoint(Vector2 point)
		{
			return Shape.ContainsPoint(point);
		}
		
		public float ReflectRay(Vector2 origin, float angle)
		{
			return Shape.ReflectRay(origin, angle);
		}
	}
}