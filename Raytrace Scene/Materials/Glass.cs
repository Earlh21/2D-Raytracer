using System.Numerics;
using Raytrace_Scene.Materials;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public class Glass : TransparentMaterial
	{
		public Vector3 Color { get; set; }
		
		public Glass(float ior, Vector3 color) : base(ior)
		{
			Color = color;
		}

		public override bool BounceRay(Shape shape, Ray ray)
		{
			throw new System.NotImplementedException();
		}

		public override void AffectRay(Ray ray, float distance)
		{
			ray.Color += Color * distance / 100;
		}
	}
}