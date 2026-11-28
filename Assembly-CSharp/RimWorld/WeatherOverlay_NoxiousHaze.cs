using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B3 RID: 5043
	[StaticConstructorOnStartup]
	public class WeatherOverlay_NoxiousHaze : WeatherOverlayDualPanner
	{
		// Token: 0x06007D0B RID: 32011 RVA: 0x002CAB48 File Offset: 0x002C8D48
		public WeatherOverlay_NoxiousHaze()
		{
			this.worldOverlayMat = WeatherOverlay_NoxiousHaze.NoxiusHazeOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0004f;
			this.worldOverlayPanSpeed2 = 0.0003f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, -1f);
		}

		// Token: 0x04004EB1 RID: 20145
		private static readonly Material NoxiusHazeOverlayWorld = MatLoader.LoadMat("Weather/NoxiousHazeOverlayWorld", -1);
	}
}
