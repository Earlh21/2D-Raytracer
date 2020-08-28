using System.Numerics;

namespace Raytrace_Scene.Maths
{
	public struct Ray
	{
		public Vector2 Origin { get; }
		public float Angle { get; }

		public Ray(Vector2 origin, float angle)
		{
			Origin = origin;
			Angle = angle;
		}
	}
}