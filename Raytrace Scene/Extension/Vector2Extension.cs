using System.Numerics;

namespace Raytrace_Scene.Extension
{
	public static class Vector2Extension
	{
		public static float Dot(this Vector2 a, Vector2 b)
		{
			return a.X * b.X + a.Y * b.Y;
		}
	}
}