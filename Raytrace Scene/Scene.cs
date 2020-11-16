using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Raytrace_Scene.Materials;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Objects;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene
{
	public class Scene
	{
		public List<SceneObject> SceneObjects { get; private set; }

		public int Width { get; private set; }
		public int Height { get; private set; }

		public SceneObject GetCollider(Ray ray)
		{
			SceneObject collider = ray.Medium;
			float min_distance = float.MaxValue;

			if (ray.Medium != null)
			{
				min_distance = (ray.Medium.GetIntersectPoint(ray) - ray.Origin).LengthSquared();
			}

			foreach (SceneObject scene_object in SceneObjects)
			{
				if (scene_object == ray.Medium)
				{
					continue;
				}
					
				if (scene_object.RayIntersects(ray))
				{
					Vector2 intersect_point = scene_object.GetIntersectPoint(ray);
					float distance = (intersect_point - ray.Origin).LengthSquared();

					if (distance < min_distance)
					{
						collider = scene_object;
						min_distance = distance;
					}
				}
			}

			return collider;
		}

		public TransparentObject GetMedium(Vector2 point)
		{
			foreach (SceneObject scene_object in SceneObjects)
			{
				if (scene_object is TransparentObject medium)
				{
					if (medium.ContainsPoint(point))
					{
						return medium;
					}
				}
			}

			return null;
		}

		public Scene(string filename)
		{
			SceneObjects = new List<SceneObject>();

			using (StreamReader reader = new StreamReader(filename))
			{
				string top_line = reader.ReadLine();
				string[] size_data = top_line.Split(',');

				Width = Convert.ToInt32(size_data[0]);
				Height = Convert.ToInt32(size_data[1]);

				while (reader.Peek() != -1)
				{
					string[] object_data = reader.ReadLine().Split('|');

					if (object_data[0][0] == '#')
					{
						continue;
					}
					
					Shape shape;
					Material material;

					string[] shape_data = object_data[0].Split(',');
					string[] material_data = object_data[1].Split(',');
					
					switch (shape_data[0])
					{
						case "circle":
							float x = (float) Convert.ToDouble(shape_data[1]);
							float y = (float) Convert.ToDouble(shape_data[2]);
							float radius = (float) Convert.ToDouble(shape_data[3]);
							
							shape = new Circle(x, y, radius);
							break;
						case "box":
							float x_left = (float) Convert.ToDouble(shape_data[1]);
							float y_bottom = (float) Convert.ToDouble(shape_data[2]);
							float x_right = (float) Convert.ToDouble(shape_data[3]);
							float y_top = (float) Convert.ToDouble(shape_data[4]);
							
							shape = Polygon.CreateRectangle(x_left, y_bottom, x_right, y_top);
							break;
						case "torus":
							float x2 = (float) Convert.ToDouble(shape_data[1]);
							float y2 = (float) Convert.ToDouble(shape_data[2]);
							float inner_radius = (float) Convert.ToDouble(shape_data[3]);
							float outer_radius = (float) Convert.ToDouble(shape_data[4]);

							shape = new Torus(new Vector2(x2, y2), inner_radius, outer_radius);
							break;
						default:
							throw new ArgumentException("Invalid shape type.");
					}

					switch (material_data[0])
					{
						case "light":
							float r = (float) Convert.ToDouble(material_data[1]);
							float g = (float) Convert.ToDouble(material_data[2]);
							float b = (float) Convert.ToDouble(material_data[3]);

							material = new Light(new Vector3(r, g, b));
							break;
						case "diffuse":
							float roughness = (float) Convert.ToDouble(material_data[1]);

							material = new Diffuse(roughness);
							break;
						case "glass":
							float ior = (float) Convert.ToDouble(material_data[1]);
							float r2 = (float) Convert.ToDouble(material_data[2]);
							float g2 = (float) Convert.ToDouble(material_data[3]);
							float b2 = (float) Convert.ToDouble(material_data[4]);

							material = new Glass(ior, new Vector3(r2,g2,b2));
							break;
						case "rainbow":
							float intensity = (float) Convert.ToDouble(material_data[1]);
							float band_length = (float) Convert.ToDouble(material_data[2]);
							
							material = new RainbowLight(intensity, band_length);
							break;
						default:
							throw new ArgumentException("Invalid material.");
					}

					if (material is SolidMaterial solid_mat)
					{
						SceneObjects.Add(new SolidObject(shape, solid_mat));
					}

					if (material is TransparentMaterial trans_mat)
					{
						SceneObjects.Add(new TransparentObject(shape, trans_mat));
					}
				}
			}
		}
	}
}