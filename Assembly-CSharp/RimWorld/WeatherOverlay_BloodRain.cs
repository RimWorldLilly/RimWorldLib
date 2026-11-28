using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013AA RID: 5034
	[StaticConstructorOnStartup]
	public class WeatherOverlay_BloodRain : WeatherOverlayDualPanner
	{
		// Token: 0x06007CF6 RID: 31990 RVA: 0x002CA5A0 File Offset: 0x002C87A0
		public WeatherOverlay_BloodRain()
		{
			this.worldOverlayMat = WeatherOverlay_BloodRain.BloodRainMat;
			this.worldOverlayPanSpeed1 = 0.015f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.022f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
			base.ForcedOverlayColor = new Color?(new Color(0.6f, 0f, 0f));
		}

		// Token: 0x04004EA8 RID: 20136
		private static readonly Material BloodRainMat = MatLoader.LoadMat("Weather/BloodRainOverlayWorld", -1);
	}
}
