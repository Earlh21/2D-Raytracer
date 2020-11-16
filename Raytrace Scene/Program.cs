using System;
using System.IO;
using System.Numerics;

namespace Raytrace_Scene
{
	internal class Program
	{
		public static Random R;
	
		public static void Main(string[] args)
		{
			if (args.Length < 4)
			{
				Console.WriteLine("Usage: <scale> <samples> <scene file> <output file>");
				return;
			}
			
			float scale = (float)Convert.ToDouble(args[0]);
			int samples = Convert.ToInt32(args[1]);
			string dir = Directory.GetCurrentDirectory();
			string scene_file = dir + "\\" + args[2];
			string render_out = dir + "\\" + args[3];

			CPURenderer.Samples = samples;

			R = new Random(100);
			
			Scene scene = new Scene(scene_file);
			CPURenderer.Render(scene, scale, LineCompletedHandler).Save(render_out);
			
			Console.WriteLine("Done");
		}

		public static void LineCompletedHandler(object sender, RenderProgress.LineCompletedEventArgs args)
		{
			float progress = (float)args.LinesCompleted / args.TotalLines;
			int percent = (int)(progress * 100);
			Console.WriteLine($"{percent}% done...");
		}
	}
}