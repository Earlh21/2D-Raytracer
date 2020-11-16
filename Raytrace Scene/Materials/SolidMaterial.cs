using System.Numerics;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Objects;

namespace Raytrace_Scene.Materials
{
	public abstract class SolidMaterial : Material
	{
		public abstract Vector3 GetColor(SolidObject solid_object, Vector2 point);
	}
}