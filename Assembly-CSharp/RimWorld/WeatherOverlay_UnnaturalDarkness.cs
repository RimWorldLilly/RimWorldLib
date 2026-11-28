using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013BA RID: 5050
	[StaticConstructorOnStartup]
	public class WeatherOverlay_UnnaturalDarkness : WeatherOverlayDualPanner
	{
		// Token: 0x06007D1F RID: 32031 RVA: 0x002CB220 File Offset: 0x002C9420
		public WeatherOverlay_UnnaturalDarkness()
		{
			this.worldOverlayMat = WeatherOverlay_UnnaturalDarkness.DarkParticlesOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.004f;
			this.worldPanDir1 = new Vector2(-0.4f, -0.6f).normalized;
			this.worldOverlayPanSpeed2 = 0.005f;
			this.worldPanDir2 = new Vector2(0.2f, -0.8f).normalized;
		}

		// Token: 0x04004EC6 RID: 20166
		private static readonly Material DarkParticlesOverlayWorld = MatLoader.LoadMat("Weather/DarknessOverlayWorld", -1);
	}
}
