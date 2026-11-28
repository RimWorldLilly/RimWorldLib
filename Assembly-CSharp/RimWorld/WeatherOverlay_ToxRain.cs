using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013BD RID: 5053
	[StaticConstructorOnStartup]
	public class WeatherOverlay_ToxRain : WeatherOverlayDualPanner
	{
		// Token: 0x06007D28 RID: 32040 RVA: 0x002CB48C File Offset: 0x002C968C
		public WeatherOverlay_ToxRain()
		{
			this.worldOverlayMat = WeatherOverlay_ToxRain.RainOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.015f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.022f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x04004ECF RID: 20175
		private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/ToxRainOverlayWorld", -1);
	}
}
