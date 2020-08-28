using System;
using System.Numerics;

namespace Raytrace_Scene
{
	internal class Program
	{
		public static Random R;
	
		public static void Main(string[] args)
		{
			R = new Random();
			
			Scene scene = new Scene(@"D:\Downloads3\simple-scene.txt");
			CPURenderer.Render(scene).Save(@"D:\Downloads3\simple-render.png");
			Console.WriteLine("Done");
		}
	}
}