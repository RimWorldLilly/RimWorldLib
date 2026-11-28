using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013AC RID: 5036
	[StaticConstructorOnStartup]
	public class WeatherOverlay_DeepBloodFog : WeatherOverlayDualPanner
	{
		// Token: 0x06007CFA RID: 31994 RVA: 0x002CA6F0 File Offset: 0x002C88F0
		public WeatherOverlay_DeepBloodFog()
		{
			this.worldOverlayMat = WeatherOverlay_DeepBloodFog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.001f;
			this.worldOverlayPanSpeed2 = 0.002f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir1.Normalize();
			this.worldPanDir2 = new Vector2(0.5f, -0.1f);
			this.worldPanDir2.Normalize();
			base.ForcedOverlayColor = new Color?(new Color(0.6f, 0f, 0f));
		}

		// Token: 0x04004EAA RID: 20138
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/DeepBloodFogOverlayWorld", -1);
	}
}
