using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013AD RID: 5037
	[StaticConstructorOnStartup]
	public class WeatherOverlay_DeathpallFog : WeatherOverlayDualPanner
	{
		// Token: 0x06007CFC RID: 31996 RVA: 0x002CA798 File Offset: 0x002C8998
		public WeatherOverlay_DeathpallFog()
		{
			this.worldOverlayMat = WeatherOverlay_DeathpallFog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.001f;
			this.worldOverlayPanSpeed2 = 0.0005f;
			this.worldPanDir1 = new Vector2(1f, 0.2f);
			this.worldPanDir1.Normalize();
			this.worldPanDir2 = new Vector2(-1f, -0.2f);
			this.worldPanDir2.Normalize();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.SetOverlayColor(new Color(1f, 1f, 1f, 0.7f));
			});
		}

		// Token: 0x04004EAB RID: 20139
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/DeathpallFogOverlayWorld", -1);
	}
}
