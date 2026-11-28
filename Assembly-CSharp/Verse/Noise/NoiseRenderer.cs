using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000753 RID: 1875
	public static class NoiseRenderer
	{
		// Token: 0x06003B70 RID: 15216 RVA: 0x0017AA3B File Offset: 0x00178C3B
		public static Texture2D NoiseRendered(ModuleBase noise)
		{
			return NoiseRenderer.NoiseRendered(new CellRect(0, 0, NoiseRenderer.renderSize.x, NoiseRenderer.renderSize.z), noise);
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x0017AA60 File Offset: 0x00178C60
		public static Texture2D NoiseRendered(CellRect rect, ModuleBase noise)
		{
			Texture2D texture2D = new Texture2D(rect.Width, rect.Height);
			texture2D.name = "NoiseRender";
			foreach (IntVec2 intVec in rect.Cells2D)
			{
				texture2D.SetPixel(intVec.x, intVec.z, NoiseRenderer.ColorForValue(noise.GetValue(intVec)));
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x0017AAEC File Offset: 0x00178CEC
		private static Color ColorForValue(float val)
		{
			val = val * 0.5f + 0.5f;
			return ColorsFromSpectrum.Get(NoiseRenderer.spectrum, val);
		}

		// Token: 0x040026E3 RID: 9955
		public static IntVec2 renderSize = new IntVec2(200, 200);

		// Token: 0x040026E4 RID: 9956
		private static Color[] spectrum = new Color[]
		{
			Color.black,
			Color.blue,
			Color.green,
			Color.white
		};
	}
}
