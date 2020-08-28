using System.Collections.Generic;
using System.Numerics;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public class Light : Material
	{
		public Vector3 Color { get; set; }

		public Light(Vector3 color)
		{
			Color = color;
		}

		public override Ray? GetNewRay(Shape shape, Ray ray)
		{
			return null;
		}
	}
}