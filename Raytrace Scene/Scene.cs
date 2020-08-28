using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Raytrace_Scene
{
	public class Scene
	{
		public List<SceneObject> SceneObjects { get; private set; }

		public int Width { get; private set; }
		public int Height { get; private set; }

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
					string line = reader.ReadLine().Replace(" ", "");

					string[] line_data = line.Split(',');
					if (line_data[0].Equals("wall"))
					{
						float x_left = (float) Convert.ToDouble(line_data[1]);
						float y_bottom = (float) Convert.ToDouble(line_data[2]);
						float x_right = (float) Convert.ToDouble(line_data[3]);
						float y_top = (float) Convert.ToDouble(line_data[4]);
						float roughness = (float) Convert.ToDouble(line_data[5]);

						Wall wall = new Wall(Polygon.CreateRectangle(x_left, y_bottom, x_right, y_top), roughness);
						SceneObjects.Add(wall);
					}
					else if (line_data[0].Equals("light"))
					{
						float x_left = (float) Convert.ToDouble(line_data[1]);
						float y_bottom = (float) Convert.ToDouble(line_data[2]);
						float x_right = (float) Convert.ToDouble(line_data[3]);
						float y_top = (float) Convert.ToDouble(line_data[4]);

						float r = (float) Convert.ToDouble(line_data[5]);
						float g = (float) Convert.ToDouble(line_data[6]);
						float b = (float) Convert.ToDouble(line_data[7]);

						SceneObjects.Add(new Light(Polygon.CreateRectangle(x_left, y_bottom, x_right, y_top),
							new Vector3(r, g, b)));
					}
					else if (line_data[0].Equals("circle"))
					{
						float x = (float) Convert.ToDouble(line_data[1]);
						float y = (float) Convert.ToDouble(line_data[2]);
						float radius = (float) Convert.ToDouble(line_data[3]);
						float r = (float) Convert.ToDouble(line_data[4]);
						float g = (float) Convert.ToDouble(line_data[5]);
						float b = (float) Convert.ToDouble(line_data[6]);

						SceneObjects.Add(new Light(new Circle(new Vector2(x, y), radius), new Vector3(r, g, b)));
					}
					else if (line_data[0].Equals("circlewall"))
					{
						float x = (float) Convert.ToDouble(line_data[1]);
						float y = (float) Convert.ToDouble(line_data[2]);
						float radius = (float) Convert.ToDouble(line_data[3]);
						float roughness = (float) Convert.ToDouble(line_data[4]);
						
						SceneObjects.Add(new Wall(new Circle(new Vector2(x, y) ,radius), roughness));
					}
					else if (line_data[0].Equals("transparent"))
					{
						float x_left = (float) Convert.ToDouble(line_data[1]);
						float y_bottom = (float) Convert.ToDouble(line_data[2]);
						float x_right = (float) Convert.ToDouble(line_data[3]);
						float y_top = (float) Convert.ToDouble(line_data[4]);
						float ior = (float) Convert.ToDouble(line_data[5]);
						float opacity = (float) Convert.ToDouble(line_data[6]);
						float roughness = (float) Convert.ToDouble(line_data[7]);

						Shape shape = Polygon.CreateRectangle(x_left, y_bottom, x_right, y_top);
						TransparentWall wall = new TransparentWall(shape, ior, opacity, roughness);
						SceneObjects.Add(wall);
					}
					else if (line_data[0].Equals("transparentcircle"))
					{
						float x = (float) Convert.ToDouble(line_data[1]);
						float y = (float) Convert.ToDouble(line_data[2]);
						float radius = (float) Convert.ToDouble(line_data[3]);
						float ior = (float) Convert.ToDouble(line_data[4]);
						float opacity = (float) Convert.ToDouble(line_data[5]);
						float roughness = (float) Convert.ToDouble(line_data[6]);
						
						Shape shape = new Circle(new Vector2(x, y), radius);
						var wall = new TransparentWall(shape, ior, opacity, roughness);
						SceneObjects.Add(wall);
					}
					else
					{
						throw new ArgumentException("Invalid object type found");
					}
				}
			}
		}
	}
}