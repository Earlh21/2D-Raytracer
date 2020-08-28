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
	}
}