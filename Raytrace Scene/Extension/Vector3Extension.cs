using System;
using System.Drawing;
using System.Numerics;
using Raytrace_Scene.Maths;

namespace Raytrace_Scene.Extension
{
	public static class Vector3Extension
	{
		public static Color ToColor(this Vector3 v)
		{
			return Color.FromArgb(255, (byte) (Mathf.Clamp01(v.X) * 255), (byte) (Mathf.Clamp01(v.Y) * 255),
				(byte) (Mathf.Clamp01(v.Z) * 255));
		}

		public static Vector3 Multiply(this Vector3 a, Vector3 b)
		{
			return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		}
	}
}