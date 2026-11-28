using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B8 RID: 5048
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowGentle : WeatherOverlayDualPanner
	{
		// Token: 0x06007D1B RID: 32027 RVA: 0x002CB110 File Offset: 0x002C9310
		public WeatherOverlay_SnowGentle()
		{
			this.worldOverlayMat = WeatherOverlay_SnowGentle.SnowGentleOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.002f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.003f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x04004EC4 RID: 20164
		private static readonly Material SnowGentleOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
