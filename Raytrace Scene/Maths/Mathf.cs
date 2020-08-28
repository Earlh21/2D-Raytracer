using System;
using System.Numerics;

namespace Raytrace_Scene.Maths
{
	public static class Mathf
	{
		public const float PI = (float) Math.PI;
		public const float PI2 = (float) Math.PI * 2;
		
		public static float NormalizeAngle(float angle)
		{
			while (angle < 0)
			{
				angle += PI2;
			}

			while (angle > PI2)
			{
				angle -= PI2;
			}

			return angle;
		}

		public static float Min(float a, float b)
		{
			return a >= b ? b : a;
		}

		public static float Max(float a, float b)
		{
			return a >= b ? a : b;
		}
		
		public static float AngleDifference(float a, float b)
		{
			float c = Math.Abs(a - b);
			float d = Math.Abs(a + Mathf.PI2 - b);
			return Min(c, d);
		}

		public static float Reflect(float ray_angle, float normal)
		{
			float ray_reversed = ray_angle + (float) Math.PI;
			ray_reversed = NormalizeAngle(ray_reversed);

			float diff = Math.Abs(ray_reversed - normal);
			int sign = Math.Sign(normal - ray_reversed);

			return NormalizeAngle(normal + sign * diff);
		}

		public static float AngleTo(Vector2 start, Vector2 end)
		{
			return NormalizeAngle((float) Math.Atan2(end.Y - start.Y, end.X - start.X));
		}

		public static float Reverse(float angle)
		{
			return NormalizeAngle(PI + angle);
		}

		public static float Clamp01(float value)
		{
			if (value < 0)
			{
				return 0;
			}

			if (value > 1)
			{
				return 1;
			}

			return value;
		}

		public static float? Refract(float incidence, float n1, float n2)
		{
			float inside = n1 * Sin(incidence) / n2;
			
			if (inside > 1 || inside < -1)
			{
				return null;
			}
			
			return Asin(inside);
		}

		public static float Sin(float value)
		{
			return (float) Math.Sin(value);
		}

		public static float Cos(float value)
		{
			return (float) Math.Cos(value);
		}

		public static float Asin(float value)
		{
			return (float) Math.Asin(value);
		}
	}
}