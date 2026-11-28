using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B9 RID: 5049
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fallout : WeatherOverlayDualPanner
	{
		// Token: 0x06007D1D RID: 32029 RVA: 0x002CB198 File Offset: 0x002C9398
		public WeatherOverlay_Fallout()
		{
			this.worldOverlayMat = WeatherOverlay_Fallout.FalloutOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0008f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.0012f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x04004EC5 RID: 20165
		private static readonly Material FalloutOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
