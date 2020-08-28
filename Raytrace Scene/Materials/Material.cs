using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public abstract class Material
	{
		public abstract Ray? GetNewRay(Shape shape, Ray ray);
	}
}