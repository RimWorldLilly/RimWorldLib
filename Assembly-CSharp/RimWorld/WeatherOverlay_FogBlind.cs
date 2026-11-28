using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B0 RID: 5040
	[StaticConstructorOnStartup]
	public class WeatherOverlay_FogBlind : WeatherOverlayDualPanner
	{
		// Token: 0x06007D03 RID: 32003 RVA: 0x002CA968 File Offset: 0x002C8B68
		public WeatherOverlay_FogBlind()
		{
			this.worldOverlayMat = WeatherOverlay_FogBlind.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 5E-05f;
			this.worldOverlayPanSpeed2 = 4E-05f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, -1f);
		}

		// Token: 0x04004EAE RID: 20142
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/BlindFogOverlayWorld", -1);
	}
}
