using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Raytrace_Scene.Extension;
using Raytrace_Scene.Materials;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Objects;

namespace Raytrace_Scene
{
	public static class CPURenderer
	{
		public static int Samples = 100;
		private const int MAX_BOUNCES = 0;

		private static Vector3 Sample(Ray ray, Scene scene)
		{
			int bounces = 0;

			while (true)
			{
				SceneObject collider = scene.GetCollider(ray);

				if (collider == null)
				{
					return Vector3.Zero;
				}

				if (collider == ray.Medium)
				{
					Vector2 new_point = collider.GetIntersectPoint(ray) + ray.Direction * 0.01f;
					TransparentObject new_medium = scene.GetMedium(new_point);

					bool ray_exists = ray.Medium.RefractRayOut(ray, new_medium);

					if (!ray_exists)
					{
						return Vector3.Zero;
					}

					continue;
				}

				if (collider is TransparentObject transparent_collider)
				{
					bool ray_exists = transparent_collider.RefractRayIn(ray);

					if (!ray_exists)
					{
						return Vector3.Zero;
					}

					continue;
				}

				if (collider is SolidObject solid_collider)
				{
					if (solid_collider.Material is EmissiveMaterial light)
					{
						Vector3 light_color = light.GetColor(solid_collider, solid_collider.GetIntersectPoint(ray));
						
						float ray_color_mag = ray.Color.Length();

						if (ray_color_mag == 0)
						{
							return light_color;
						}

						Vector3 ray_color = ray.Color / ray_color_mag;

						if (ray_color_mag > 1)
						{
							return light_color.Multiply(ray_color);
						}

						return light_color * (1 - ray_color_mag) + light_color.Multiply(ray_color) * ray_color_mag;
					}

					bounces++;

					if (bounces > MAX_BOUNCES)
					{
						return Vector3.Zero;
					}

					bool ray_exists = solid_collider.BounceRay(ray);

					if (!ray_exists)
					{
						return Vector3.Zero;
					}
				}
			}
		}

		private static Vector3 RenderPixel(int x, int y, Scene scene, Random R, float scale)
		{
			Vector2 pixel_pos = new Vector2(x + 0.5f, y + 0.5f) / scale;

			TransparentObject medium = null;
			foreach (SceneObject scene_object in scene.SceneObjects)
			{
				if (scene_object.ContainsPoint(pixel_pos))
				{
					if (scene_object is SolidObject solid_object)
					{
						return solid_object.Material.GetColor(solid_object,
							pixel_pos);
					}

					if (scene_object is TransparentObject transparent_object)
					{
						medium = transparent_object;
						break;
					}
				}
			}

			Ray ray = new Ray(Vector2.Zero, Vector2.Zero, null, Vector3.Zero);
			Vector3 color = Vector3.Zero;
			for (int i = 0; i < Samples; i++)
			{
				float angle = (float) R.NextDouble() * Mathf.PI2;
				ray.Origin = pixel_pos;
				ray.Direction = Mathf.FromAngle(angle);
				ray.Medium = medium;
				ray.Color = Vector3.Zero;
				color += Sample(ray, scene) / Samples;
			}

			return color;
		}

		public static void RenderPass(Vector3[,] pass, Scene scene, Random R, float scale , RenderProgress progress)
		{
			for (int i = 0; i < pass.GetLength(0); i++)
			{
				for (int j = 0; j < pass.GetLength(1); j++)
				{
					pass[i, j] = RenderPixel(i, j, scene, R, scale);
				}
				
				progress.FinishLine();
			}
		}

		private static Vector3[,] AveragePasses(Vector3[][,] passes)
		{
			Vector3[,] average = new Vector3[passes[0].GetLength(0), passes[0].GetLength(1)];

			foreach (Vector3[,] pass in passes)
			{
				for (int i = 0; i < average.GetLength(0); i++)
				{
					for (int j = 0; j < average.GetLength(1); j++)
					{
						average[i, j] += pass[i, j] / Environment.ProcessorCount;
					}
				}
			}

			return average;
		}

		private static void FillImage(Vector3[,] render, DirectBitmap image)
		{
			for (int i = 0; i < image.Width; i++)
			{
				for (int j = 0; j < image.Height; j++)
				{
					image.SetPixel(i, image.Height - j - 1, render[i, j].ToColor());
				}
			}
		}

		public static Bitmap Render(Scene scene, float scale, EventHandler<RenderProgress.LineCompletedEventArgs> progress_handler = null)
		{
			int image_width = (int)(scene.Width * scale);
			int image_height = (int)(scene.Height * scale);
			DirectBitmap result = new DirectBitmap(image_width, image_height);
			
			RenderProgress progress = new RenderProgress(image_width * Environment.ProcessorCount);
			progress.LineCompletedEvent += progress_handler;
			
			Vector3[][,] passes = new Vector3[Environment.ProcessorCount][,];
			Task[] tasks = new Task[Environment.ProcessorCount];
			Random[] randoms = new Random[Environment.ProcessorCount];
			randoms[0] = new Random(Program.R.Next());
			
			for (int i = 0; i < Environment.ProcessorCount; i++)
			{
				passes[i] = new Vector3[image_width, image_height];

				if (i != 0)
				{
					randoms[i] = new Random(randoms[i - 1].Next());
				}
			}

			Parallel.ForEach(passes, (pass, state, index) =>
			{
				RenderPass(pass, scene, randoms[index], scale, progress);
			});

			Vector3[,] average = AveragePasses(passes);
			FillImage(average, result);

			return result.Bitmap;
		}
	}
}