using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Raytrace_Scene.Maths
{
	public static class Mathf
	{
		public const float PI = (float) Math.PI;
		public const float PI2 = (float) Math.PI * 2;

		#region Math Wrappers
		public static float Min(float a, float b)
		{
			return a >= b ? b : a;
		}

		public static float Max(float a, float b)
		{
			return a >= b ? a : b;
		}

		public static float Atan2(float y, float x)
		{
			return (float) Math.Atan2(y, x);
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

		public static float Sqrt(float value)
		{
			return (float) Math.Sqrt(value);
		}

		public static float Acos(float value)
		{
			return (float) Math.Acos(value);
		}
		#endregion

		public static float AngleTo(Vector2 start, Vector2 end)
		{
			return Atan2(end.Y - start.Y, end.X - start.X);
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

		public static Vector2? InverseRefract(Vector2 refracted_ray, Vector2 normal, float medium_ior, float collider_ior)
		{
			float m = collider_ior / medium_ior;
			float refract_incidence = AngleBetween(refracted_ray, normal);
			float? incoming_incidence = Refract(refract_incidence, medium_ior, collider_ior);

			if (Refract(refract_incidence, collider_ior, medium_ior) == null)
			{
				return null;
			}

			if (incoming_incidence == null)
			{
				return null;
			}

			return (refracted_ray + normal * (m * Cos((float) incoming_incidence) - Cos(refract_incidence))) / m;
		}

		public static Vector2? RefractNormal(Vector2 incoming, Vector2 normal, float n1, float n2)
		{
			float r = n1 / n2;
			float incidence = AngleBetween(-incoming, normal);
			float? refracted_incidence = Refract(incidence, n1, n2);

			if (refracted_incidence == null)
			{
				return null;
			}

			return r * incoming + normal * (r * Cos(incidence) - Cos((float) refracted_incidence));
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

		public static float AngleBetween(Vector2 a, Vector2 b)
		{
			return Acos(Vector2.Dot(a, b) / a.Length() / b.Length());
		}

		public static Vector2 Rotate(Vector2 vector, float angle)
		{
			return new Vector2(Cos(angle) * vector.X - Sin(angle) * vector.Y,
				Sin(angle) * vector.X + Cos(angle) * vector.Y);
		}

		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Cos(angle), Sin(angle));
		}

		public static float RandomFloat()
		{
			return (float) Program.R.NextDouble();
		}
	}
}