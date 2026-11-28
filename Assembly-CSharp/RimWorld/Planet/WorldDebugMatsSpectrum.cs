using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002311 RID: 8977
	[StaticConstructorOnStartup]
	public static class WorldDebugMatsSpectrum
	{
		// Token: 0x0600DDB0 RID: 56752 RVA: 0x00528A10 File Offset: 0x00526C10
		static WorldDebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				WorldDebugMatsSpectrum.spectrumMats[i] = MatsFromSpectrum.Get(WorldDebugMatsSpectrum.DebugSpectrum, (float)i / 100f, ShaderDatabase.WorldOverlayTransparent);
				WorldDebugMatsSpectrum.spectrumMats[i].renderQueue = 3510;
			}
		}

		// Token: 0x0600DDB1 RID: 56753 RVA: 0x00528A6F File Offset: 0x00526C6F
		public static Material Mat(int ind)
		{
			ind = Mathf.Clamp(ind, 0, 99);
			return WorldDebugMatsSpectrum.spectrumMats[ind];
		}

		// Token: 0x04008E63 RID: 36451
		private static readonly Material[] spectrumMats = new Material[100];

		// Token: 0x04008E64 RID: 36452
		public const int MaterialCount = 100;

		// Token: 0x04008E65 RID: 36453
		private const float Opacity = 0.25f;

		// Token: 0x04008E66 RID: 36454
		private static readonly Color[] DebugSpectrum = DebugMatsSpectrum.DebugSpectrum;
	}
}
