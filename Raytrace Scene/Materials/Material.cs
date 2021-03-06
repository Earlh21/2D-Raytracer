using System.Numerics;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public abstract class Material
	{
		public abstract bool BounceRay(Shape shape, Ray ray);
	}
}