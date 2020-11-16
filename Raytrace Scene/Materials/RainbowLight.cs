using System;
using System.Dynamic;
using System.Numerics;
using Raytrace_Scene.Maths;
using Raytrace_Scene.Objects;
using Raytrace_Scene.Shapes;

namespace Raytrace_Scene.Materials
{
	public class RainbowLight : EmissiveMaterial
	{
		public float Intensity { get; set; }
		public float BandLength { get; set; }

		public RainbowLight(float intensity, float band_length)
		{
			Intensity = intensity;
			BandLength = band_length;
		}
		
		public override bool BounceRay(Shape shape, Ray ray)
		{
			throw new System.NotImplementedException();
		}

		public override Vector3 GetColor(SolidObject solid_object, Vector2 point)
		{
			float hue = point.X + point.Y;
			//hue -= (int) hue;
			Vector3 hsv = new Vector3(hue, 1, 1);
			return HsvToRgb(hsv) * Intensity;
		}
		
		private Vector3 HsvToRgb(Vector3 hsv)
		{    
			float H = hsv.X * BandLength;
			while (H < 0) { H += 360; };
			while (H >= 360) { H -= 360; };
			float r, g, b;
			if (hsv.Z <= 0)
			{ r = g = b = 0; }
			else if (hsv.Y <= 0)
			{
				r = g = b = hsv.Z;
			}
			else
			{
				float hf = H / 60.0f;
				int i = (int)Math.Floor(hf);
				float f = hf - i;
				float pv = hsv.Z * (1 - hsv.Y);
				float qv = hsv.Z * (1 - hsv.Y * f);
				float tv = hsv.Z * (1 - hsv.Y * (1 - f));
				switch (i)
				{

					// Red is the dominant color

					case 0:
						r = hsv.Z;
						g = tv;
						b = pv;
						break;

					// Green is the dominant color

					case 1:
						r = qv;
						g = hsv.Z;
						b = pv;
						break;
					case 2:
						r = pv;
						g = hsv.Z;
						b = tv;
						break;

					// Blue is the dominant color

					case 3:
						r = pv;
						g = qv;
						b = hsv.Z;
						break;
					case 4:
						r = tv;
						g = pv;
						b = hsv.Z;
						break;

					// Red is the dominant color

					case 5:
						r = hsv.Z;
						g = pv;
						b = qv;
						break;

					// Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

					case 6:
						r = hsv.Z;
						g = tv;
						b = pv;
						break;
					case -1:
						r = hsv.Z;
						g = pv;
						b = qv;
						break;

					// The color is not defined, we should throw an error.

					default:
						//LFATAL("i Value error in Pixel conversion, Value is %d", i);
						r = g = b = hsv.Z; // Just pretend its black/white
						break;
				}
			}
			
			return new Vector3(r, g ,b);
		}
	}
}