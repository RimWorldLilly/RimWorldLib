using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000765 RID: 1893
	public static class GradientPresets
	{
		// Token: 0x06003BC5 RID: 15301 RVA: 0x0017B904 File Offset: 0x00179B04
		static GradientPresets()
		{
			List<GradientColorKey> list = new List<GradientColorKey>();
			list.Add(new GradientColorKey(Color.black, 0f));
			list.Add(new GradientColorKey(Color.white, 1f));
			List<GradientColorKey> list2 = new List<GradientColorKey>();
			list2.Add(new GradientColorKey(Color.red, 0f));
			list2.Add(new GradientColorKey(Color.green, 0.5f));
			list2.Add(new GradientColorKey(Color.blue, 1f));
			List<GradientColorKey> list3 = new List<GradientColorKey>();
			list3.Add(new GradientColorKey(Color.red, 0f));
			list3.Add(new GradientColorKey(Color.green, 0.33333334f));
			list3.Add(new GradientColorKey(Color.blue, 0.6666667f));
			list3.Add(new GradientColorKey(Color.black, 1f));
			List<GradientAlphaKey> list4 = new List<GradientAlphaKey>();
			list4.Add(new GradientAlphaKey(0f, 0.6666667f));
			list4.Add(new GradientAlphaKey(1f, 1f));
			List<GradientColorKey> list5 = new List<GradientColorKey>();
			list5.Add(new GradientColorKey(new Color(0f, 0f, 0.5f), 0f));
			list5.Add(new GradientColorKey(new Color(0.125f, 0.25f, 0.5f), 0.4f));
			list5.Add(new GradientColorKey(new Color(0.25f, 0.375f, 0.75f), 0.48f));
			list5.Add(new GradientColorKey(new Color(0f, 0.75f, 0f), 0.5f));
			list5.Add(new GradientColorKey(new Color(0.75f, 0.75f, 0f), 0.625f));
			list5.Add(new GradientColorKey(new Color(0.625f, 0.375f, 0.25f), 0.75f));
			list5.Add(new GradientColorKey(new Color(0.5f, 1f, 1f), 0.875f));
			list5.Add(new GradientColorKey(Color.white, 1f));
			List<GradientAlphaKey> list6 = new List<GradientAlphaKey>();
			list6.Add(new GradientAlphaKey(1f, 0f));
			list6.Add(new GradientAlphaKey(1f, 1f));
			GradientPresets._empty = new Gradient();
			GradientPresets._rgb = new Gradient();
			GradientPresets._rgb.SetKeys(list2.ToArray(), list6.ToArray());
			GradientPresets._rgba = new Gradient();
			GradientPresets._rgba.SetKeys(list3.ToArray(), list4.ToArray());
			GradientPresets._grayscale = new Gradient();
			GradientPresets._grayscale.SetKeys(list.ToArray(), list6.ToArray());
			GradientPresets._terrain = new Gradient();
			GradientPresets._terrain.SetKeys(list5.ToArray(), list6.ToArray());
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06003BC6 RID: 15302 RVA: 0x0017BBF1 File Offset: 0x00179DF1
		public static Gradient Empty
		{
			get
			{
				return GradientPresets._empty;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x0017BBF8 File Offset: 0x00179DF8
		public static Gradient Grayscale
		{
			get
			{
				return GradientPresets._grayscale;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06003BC8 RID: 15304 RVA: 0x0017BBFF File Offset: 0x00179DFF
		public static Gradient RGB
		{
			get
			{
				return GradientPresets._rgb;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06003BC9 RID: 15305 RVA: 0x0017BC06 File Offset: 0x00179E06
		public static Gradient RGBA
		{
			get
			{
				return GradientPresets._rgba;
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06003BCA RID: 15306 RVA: 0x0017BC0D File Offset: 0x00179E0D
		public static Gradient Terrain
		{
			get
			{
				return GradientPresets._terrain;
			}
		}

		// Token: 0x0400270C RID: 9996
		private static Gradient _empty;

		// Token: 0x0400270D RID: 9997
		private static Gradient _grayscale;

		// Token: 0x0400270E RID: 9998
		private static Gradient _rgb;

		// Token: 0x0400270F RID: 9999
		private static Gradient _rgba;

		// Token: 0x04002710 RID: 10000
		private static Gradient _terrain;
	}
}
