using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B1 RID: 5041
	[StaticConstructorOnStartup]
	public class WeatherOverlay_GrayPallFog : WeatherOverlayDualPanner
	{
		// Token: 0x06007D05 RID: 32005 RVA: 0x002CA9D8 File Offset: 0x002C8BD8
		public WeatherOverlay_GrayPallFog()
		{
			this.worldOverlayMat = WeatherOverlay_GrayPallFog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.001f;
			this.worldOverlayPanSpeed2 = 0.0005f;
			this.worldPanDir1 = new Vector2(1f, 0.2f);
			this.worldPanDir1.Normalize();
			this.worldPanDir2 = new Vector2(-1f, -0.2f);
			this.worldPanDir2.Normalize();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.SetOverlayColor(new Color(1f, 1f, 1f, 1f));
			});
		}

		// Token: 0x04004EAF RID: 20143
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/UnnaturalFogOverlayWorld", -1);
	}
}
