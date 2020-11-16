using System;
using System.Numerics;
using Raytrace_Scene.Extension;
using Raytrace_Scene.Maths;

namespace Raytrace_Scene.Shapes
{
	public class LineSegment
	{
		public Vector2 Start { get; set; }
		public Vector2 End { get; set; }

		public Vector2 Normal { get; set; }

		//Calculated properties
		public float Angle => Mathf.AngleTo(Start, End);

		public float Length => (Start - End).Length();
		public Vector2 Midpoint => Start + (End - Start) / 2;

		public float LengthSquared => (Start - End).LengthSquared();

		public LineSegment(Vector2 start, Vector2 end, Vector2 normal = default)
		{
			Start = start;
			End = end;
			Normal = normal;
		}

		public bool RayIntersects(Ray ray)
		{
			return Intersect.RayToLineSegmentIntersection(ray, this) != null;
		}

		public Vector2 RayIntersectionPoint(Ray ray)
		{
			Vector2? intersect = Intersect.RayToLineSegmentIntersection(ray, this);
			if (intersect == null)
			{
				return default;
			}

			return (Vector2)intersect;
		}
	}
}