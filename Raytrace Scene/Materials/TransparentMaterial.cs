using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public abstract class TransparentMaterial : Material
	{
		public float IOR { get; set; }

		public TransparentMaterial(float ior)
		{
			IOR = ior;
		}

		public abstract void AffectRay(Ray ray, float distance);
	}
}