using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B7 RID: 5047
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowHard : WeatherOverlayDualPanner
	{
		// Token: 0x06007D19 RID: 32025 RVA: 0x002CB088 File Offset: 0x002C9288
		public WeatherOverlay_SnowHard()
		{
			this.worldOverlayMat = WeatherOverlay_SnowHard.SnowOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.008f;
			this.worldPanDir1 = new Vector2(-0.5f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.009f;
			this.worldPanDir2 = new Vector2(-0.48f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x04004EC3 RID: 20163
		private static readonly Material SnowOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
