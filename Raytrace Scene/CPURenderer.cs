using System;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Raytrace_Scene.Extension;
using Raytrace_Scene.Utility;

namespace Raytrace_Scene
{
	public static class CPURenderer
	{
		private const int SAMPLES = 5000;

		private const int MAX_BOUNCES = 2;

		//TODO: Weird stuff happens with this at 1
		private const float STEP_SIZE = 2.0f;
		private const float MAX_STEPS = 1024;
		private static readonly Random R;

		static CPURenderer()
		{
			R = new Random(100);
		}

		private static Vector3 Sample(Vector2 origin, float angle, Scene scene, SceneObject[,] pixel_map, TransparentWall medium)
		{
			float multiplier = 1;
			
			if (medium != null)
			{
				multiplier *= 1 - medium.Opacity;
			}

			int bounces = 0;
			Vector2 pos = origin;
			Vector2 dir = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * STEP_SIZE;

			for (int j = 0; j < MAX_STEPS; j++)
			{
				pos += dir;

				if (pos.X < 0 || pos.Y < 0 || pos.X >= scene.Width || pos.Y >= scene.Height)
				{
					break;
				}

				var pos_object = pixel_map[(int) pos.X, (int) pos.Y];

				if (pos_object == null)
				{
					if (medium != null && !medium.ContainsPoint(pos))
					{
						
						
						dir = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * STEP_SIZE;
						pos += dir;
						origin = pos;

						medium = null;
					}
					
					continue;
				}

				if (pos_object is Light light)
				{
					if (light.ContainsPoint(pos))
					{
						return multiplier * light.Color / SAMPLES;
					}
				}

				//TODO: Bounces
				//TODO: Support for medium-to-medium changes
				if (pos_object is TransparentWall transparent_wall)
				{
					if (medium != null)
					{
						continue;
					}
					
					if (!pos_object.ContainsPoint(pos))
					{
						continue;
					}

					

					continue;
				}

				if (pos_object is Wall wall)
				{
					if (!pos_object.ContainsPoint(pos))
					{
						continue;
					}

					bounces++;

					if (bounces > MAX_BOUNCES)
					{
						break;
					}

					float normal = wall.Shape.GetNormal(origin, angle);
					angle = Mathf.NormalizeAngle(normal + ((float) R.NextDouble() - 0.5f) * Mathf.PI2 * 0.99f);
						
					multiplier *= 1 - wall.Roughness;

					dir = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * STEP_SIZE;
					pos += dir;
					origin = pos;
				}
			}

			return Vector3.Zero;
		}

		private static Vector3 RenderPixel(int x, int y, Scene scene, SceneObject[,] pixel_map)
		{
			if (x == 0 && y == 19)
			{
				;
			}
			
			Vector2 pixel_pos = new Vector2(x + 0.5f, y + 0.5f);

			TransparentWall medium = null;
			
			var pixel_object = pixel_map[x, y];
			if (pixel_object is Light light)
			{
				return light.Color;
			}

			if (pixel_object is Wall)
			{
				return Vector3.Zero;
			}

			if (pixel_object is TransparentWall transparent_wall)
			{
				medium = transparent_wall;
			}

			Vector3 color = Vector3.Zero;
			for (int i = 0; i < SAMPLES; i++)
			{
				float angle = (float) R.NextDouble() * Mathf.PI2;

				color += Sample(pixel_pos, angle, scene, pixel_map, medium);
			}

			return color;
		}

		private static SceneObject[,] GeneratePixelMap(Scene scene)
		{
			var map = new SceneObject[scene.Width, scene.Height];
			for (int i = 0; i < scene.Width; i++)
			{
				for (int j = 0; j < scene.Height; j++)
				{
					Vector2[] points =
					{
						new Vector2(i + 0.1f, j + 0.1f),
						new Vector2(i + 0.1f, j + 0.9f),
						new Vector2(i + 0.9f, j + 0.1f),
						new Vector2(i + 0.9f, j + 0.9f),
					};

					foreach (Vector2 pos in points)
					{
						foreach (Wall wall in scene.Walls)
						{
							if (wall.ContainsPoint(pos))
							{
								map[i, j] = wall;
								goto NextPixel;
							}
						}

						foreach (Light light in scene.Lights)
						{
							if (light.ContainsPoint(pos))
							{
								map[i, j] = light;
								goto NextPixel;
							}
						}

						foreach (TransparentWall medium in scene.TransparentWalls)
						{
							if (medium.ContainsPoint(pos))
							{
								map[i, j] = medium;
								goto NextPixel;
							}
						}
					}

					map[i, j] = null;

					NextPixel: ;
				}
			}

			return map;
		}

		private static void RenderSection(Vector3[,] image, int y_min, int y_max, Scene scene,
			SceneObject[,] pixel_map)
		{
			int width = image.GetLength(0);
			for (int i = 0; i < width; i++)
			{
				for (int j = y_min; j <= y_max; j++)
				{
					image[i, j] = RenderPixel(i, j, scene, pixel_map);
				}
			}
		}

		public static Bitmap Render(Scene scene)
		{
			var pixel_map = GeneratePixelMap(scene);
			Vector3[,] render = new Vector3[scene.Width, scene.Height];
			DirectBitmap result = new DirectBitmap(scene.Width, scene.Height);

			int line = 0;
			var ranges = new Tuple<int, int>[Environment.ProcessorCount];

			int height = scene.Height / Environment.ProcessorCount;
			for (int i = 0; i < Environment.ProcessorCount - 1; i++)
			{
				ranges[i] = new Tuple<int, int>(line, line + height);
				line += height;
			}

			ranges[Environment.ProcessorCount - 1] = new Tuple<int, int>(line, scene.Height - 1);

			//Array.ForEach(ranges, range => { RenderSection(render, range.Item1, range.Item2, scene, pixel_map); });

			for (int i = 0; i < scene.Width; i++)
			{
				for (int j = 0; j < scene.Height; j++)
				{
					result.SetPixel(i, scene.Height - j - 1, RenderPixel(i, j, scene, pixel_map).ToColor());
				}

				Console.WriteLine($"{(int) ((float) i / (float) scene.Width * 100)}% done...");
			}

			return result.Bitmap;
		}
	}
}