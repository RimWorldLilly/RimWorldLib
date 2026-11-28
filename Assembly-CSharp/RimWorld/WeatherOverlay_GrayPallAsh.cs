using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B2 RID: 5042
	[StaticConstructorOnStartup]
	public class WeatherOverlay_GrayPallAsh : WeatherOverlayDualPanner
	{
		// Token: 0x06007D08 RID: 32008 RVA: 0x002CAA90 File Offset: 0x002C8C90
		public WeatherOverlay_GrayPallAsh()
		{
			this.worldOverlayMat = WeatherOverlay_GrayPallAsh.DarkParticlesOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.003f;
			this.worldPanDir1 = new Vector2(-0.12f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.001f;
			this.worldPanDir2 = new Vector2(0.12f, -1f);
			this.worldPanDir2.Normalize();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.SetOverlayColor(new Color(1f, 1f, 1f, 0.5f));
			});
		}

		// Token: 0x04004EB0 RID: 20144
		private static readonly Material DarkParticlesOverlayWorld = MatLoader.LoadMat("Weather/GraypallAshesOverlayWorld", -1);
	}
}
