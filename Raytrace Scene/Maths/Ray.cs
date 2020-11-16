using System.Numerics;
using Raytrace_Scene.Objects;

namespace Raytrace_Scene.Maths
{
	public class Ray
	{
		public Vector2 Origin { get; set; }
		public Vector2 Direction { get; set; }
		public TransparentObject Medium { get; set; }
		public Vector3 Color { get; set; }

		public Ray(Vector2 origin, Vector2 direction, TransparentObject medium, Vector3 color)
		{
			Origin = origin;
			Direction = direction / direction.Length();
			Medium = medium;
			Color = color;
		}
	}
}