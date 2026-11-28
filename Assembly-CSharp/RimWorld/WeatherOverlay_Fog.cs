using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013AF RID: 5039
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fog : WeatherOverlayDualPanner
	{
		// Token: 0x06007D01 RID: 32001 RVA: 0x002CA8F8 File Offset: 0x002C8AF8
		public WeatherOverlay_Fog()
		{
			this.worldOverlayMat = WeatherOverlay_Fog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0005f;
			this.worldOverlayPanSpeed2 = 0.0004f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, -1f);
		}

		// Token: 0x04004EAD RID: 20141
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/FogOverlayWorld", -1);
	}
}
