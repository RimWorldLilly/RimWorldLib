using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024E RID: 590
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x0600114E RID: 4430 RVA: 0x00063330 File Offset: 0x00061530
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00063430 File Offset: 0x00061630
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0006349A File Offset: 0x0006169A
		public static Material Mat(int ind, bool transparent)
		{
			if (ind >= 100)
			{
				ind = 99;
			}
			if (ind < 0)
			{
				ind = 0;
			}
			if (!transparent)
			{
				return DebugMatsSpectrum.spectrumMatsOpaque[ind];
			}
			return DebugMatsSpectrum.spectrumMatsTranparent[ind];
		}

		// Token: 0x04000FD2 RID: 4050
		private static readonly Material[] spectrumMatsTranparent = new Material[100];

		// Token: 0x04000FD3 RID: 4051
		private static readonly Material[] spectrumMatsOpaque = new Material[100];

		// Token: 0x04000FD4 RID: 4052
		public const int MaterialCount = 100;

		// Token: 0x04000FD5 RID: 4053
		public static Color[] DebugSpectrum = new Color[]
		{
			new Color(0.75f, 0f, 0f),
			new Color(0.5f, 0.3f, 0f),
			new Color(0f, 1f, 0f),
			new Color(0f, 0f, 1f),
			new Color(0.7f, 0f, 1f)
		};
	}
}
