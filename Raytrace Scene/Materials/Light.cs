using System.Collections.Generic;
using System.Numerics;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Objects;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public class Light : EmissiveMaterial
	{
		public Vector3 Color { get; set; }

		public Light(Vector3 color)
		{
			Color = color;
		}

		public override Vector3 GetColor(SolidObject solid_object, Vector2 point)
		{
			return Color;
		}

		public override bool BounceRay(Shape shape, Ray ray)
		{
			return false;
		}
	}
}