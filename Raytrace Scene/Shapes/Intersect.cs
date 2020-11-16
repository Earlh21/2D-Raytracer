using System;
using System.Numerics;
using Raytrace_Scene.Maths;

namespace Raytrace_Scene.Shapes
{
	public static class Intersect
	{
		private static bool LineToLineIntersect(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4,
			out float r, out float s)
		{
			float d;
			Vector2 v1_to_v2 = v2 - v1;
			Vector2 v3_to_v4 = v4 - v3;

			if(v1_to_v2.Y / v1_to_v2.X != v3_to_v4.Y / v3_to_v4.X)
			{
				d = v1_to_v2.X * v3_to_v4.Y - v1_to_v2.Y * v3_to_v4.X;
				if (d != 0)
				{
					Vector2 v3_to_v1 = v1 - v3;
					r = (v3_to_v1.Y * v3_to_v4.X - v3_to_v1.X * v3_to_v4.Y) / d;
					s = (v3_to_v1.Y * v1_to_v2.X - v3_to_v1.X * v1_to_v2.Y) / d;
					return true;
				}
			}

			r = 0;
			s = 0;
			return false;
		}
		
		private static int LineToCircleIntersect(Vector2 v1, Vector2 v2,
			Vector2 circle_position, float radius,
			out Vector2? solution1, out Vector2? solution2)
		{
			// Vector from point 1 to point 2
			Vector2 v1_to_v2 = v2 - v1;
			// Vector from point 1 to the circle's center
			Vector2 circle_to_v1 = circle_position - v1;

			float dot = Vector2.Dot(v1_to_v2, circle_to_v1);
			Vector2 proj1 = v1_to_v2 * (dot / v1_to_v2.LengthSquared());
 
			Vector2 midpt = v1 + proj1;
			Vector2 circle_to_midpt = midpt - circle_position;
			
			float sqdist_to_center = circle_to_midpt.LengthSquared();
			if (sqdist_to_center > radius * radius)
			{
				solution1 = null;
				solution2 = null;
				return 0;
			}
			
			if (sqdist_to_center == radius * radius)
			{
				solution1 = midpt;
				solution2 = null;
				return 1;
			}
			
			float dist_to_intersect;
			
			if (sqdist_to_center == 0)
			{
				dist_to_intersect = radius;
			}
			else
			{
				dist_to_intersect = Mathf.Sqrt(radius * radius - sqdist_to_center);
			}

			v1_to_v2 = Vector2.Normalize(v1_to_v2);
			v1_to_v2 *= dist_to_intersect;
 
			solution1 = midpt + v1_to_v2;
			solution2 = midpt - v1_to_v2;
			return 2;
		}

		public static Vector2? LineSegmentToLineSegmentIntersect(LineSegment segment_a, LineSegment segment_b)
		{
			if(LineToLineIntersect(segment_a.Start, segment_a.End, segment_b.Start, segment_b.End, out var r, out var s))
			{
				if (r >= 0 && r <= 1)
				{
					if (s >= 0 && s <= 1)
					{
						return segment_a.Start + (segment_a.End - segment_a.Start) * r;
					}
				}
			}

			return null;
		}
		
		

		public static Vector2? RayToLineSegmentIntersection(Ray ray, LineSegment segment)
		{
			Vector2 v2 = ray.Origin + ray.Direction;
			if(LineToLineIntersect(ray.Origin, v2, segment.Start, segment.End, out var r, out var s))
			{
				if (r >= 0)
				{
					if (s >= 0 && s <= 1)
					{
						return ray.Origin + (v2 - ray.Origin) * r;
					}
				}
			}

			return null;
		}

		public static Vector2? RayToCircleIntersection(Ray ray, Vector2 position, float radius)
		{
			Vector2 v2 = ray.Origin + ray.Direction;
			Vector2 v_1_2 = v2 - ray.Origin;
			Vector2 v1_to_solution1, v2_to_solution2;
			LineToCircleIntersect(ray.Origin, v2, position, radius, out var solution1, out var solution2);

			if (solution2 != null)
			{
				v2_to_solution2 = (Vector2)solution2 - ray.Origin;
				if (Vector2.Dot(v2_to_solution2, v_1_2) > 0)
				{
					return solution2;
				}
			}

			if (solution1 != null)
			{
				v1_to_solution1 = (Vector2)solution1 - ray.Origin;
				if (Vector2.Dot(v1_to_solution1, v_1_2) > 0)
				{
					return solution1;
				}
			}

			return null;
		}
	}
}