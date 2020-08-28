using System;
using System.Numerics;
using Raytrace_Scene.Extension;
using Raytrace_Scene.Maths;

namespace Raytrace_Scene.Shapes
{
	public class Line
	{
		private float? normal;

		public Vector2 Start { get; set; }
		public Vector2 End { get; set; }

		public float? Normal
		{
			get => normal;
			set { normal = value == null ? (float?) null : Mathf.NormalizeAngle((float) value); }
		}

		//Calculated properties
		public float Angle => Mathf.AngleTo(Start, End);

		public float Length => (Start - End).Length();
		public Vector2 Midpoint => Start + (End - Start) / 2;

		public float LengthSquared => (Start - End).LengthSquared();

		public Line(Vector2 start, Vector2 end, float? normal = null)
		{
			Start = start;
			End = end;
			Normal = normal;
		}

		public Vector2 GetClosestPoint(Vector2 position)
		{
			Vector2 start_to_position = position - Start;
			Vector2 start_to_end = End - Start;

			float dot = start_to_position.Dot(start_to_end);
			float distance = dot / LengthSquared;

			if (distance < 0)
			{
				return Start;
			}

			if (distance > 1)
			{
				return End;
			}

			return Start + start_to_end * distance;
		}

		//TODO: Take reference floats instead of returning tuple
		private Tuple<float, float> Intersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
		{
			Vector2 v_1_2 = p2 - p1;
			Vector2 v_3_4 = End - Start;

			if (v_1_2.X / v_1_2.X != v_3_4.Y / v_3_4.X)
			{
				float d = v_1_2.X * v_3_4.Y - v_1_2.Y * v_3_4.X;
				if (d == 0)
				{
					return null;
				}


				Vector2 vertex3to1 = p1 - Start;
				float r = (vertex3to1.Y * v_3_4.X - vertex3to1.X * v_3_4.Y) / d;
				float s = (vertex3to1.Y * v_1_2.X - vertex3to1.X * v_1_2.Y) / d;

				return new Tuple<float, float>(r, s);
			}

			return null;
		}

		//TODO: Rewrite this
		public bool RayIntersects(Ray ray)
		{
			Vector2 ray_point = ray.Origin + new Vector2((float) Math.Cos(ray.Angle), (float) Math.Sin(ray.Angle));
			var intersect_data = Intersection(ray.Origin, ray_point, Start, End);

			if (intersect_data == null)
			{
				return false;
			}

			return intersect_data.Item1 >= 0 && intersect_data.Item2 >= 0 && intersect_data.Item2 <= 1;
		}

		public bool LineIntersects(Line line)
		{
			return LineIntersects(line.Start, line.End);
		}

		public bool LineIntersects(Vector2 start, Vector2 end)
		{
			var intersect_data = Intersection(Start, End, start, end);

			if (intersect_data == null)
			{
				return false;
			}

			return intersect_data.Item1 >= 0 && intersect_data.Item1 <= 1 && intersect_data.Item2 >= 0 &&
			       intersect_data.Item2 <= 1;
		}

		public Vector2 RayIntersectionPoint(Ray ray)
		{
			Vector2 ray_point = ray.Origin + new Vector2((float) Math.Cos(ray.Angle), (float) Math.Sin(ray.Angle));
			var intersect_data = Intersection(Start, End, ray.Origin, ray_point);

			if (intersect_data == null)
			{
				return default;
			}

			if (intersect_data.Item1 >= 0)
			{
				if (intersect_data.Item2 >= 0 && intersect_data.Item2 <= 1)
				{
					return Start + (End - Start) * intersect_data.Item1;
				}
			}

			return default;
		}

		public float ReflectRay(float ray_angle)
		{
			if (Normal == null)
			{
				return ray_angle;
			}

			return Mathf.Reflect(ray_angle, (float) Normal);
		}
	}
}