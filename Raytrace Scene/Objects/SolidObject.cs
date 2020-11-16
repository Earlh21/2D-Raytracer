using System.Numerics;
using Raytrace_Scene.Materials;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Objects
{
	public class SolidObject : SceneObject
	{
		public SolidMaterial Material { get; set; }

		public SolidObject(Shape shape, SolidMaterial material) : base(shape)
		{
			Material = material;
		}
		
		public bool BounceRay(Ray ray)
		{
			return Material.BounceRay(Shape, ray);
		}

		public Vector3 GetColor(SolidObject solid_object, Vector2 point)
		{
			return Material.GetColor(solid_object, point);
		}
	}
}