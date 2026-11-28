using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013AE RID: 5038
	[StaticConstructorOnStartup]
	public class WeatherOverlay_DeathpallAshes : WeatherOverlayDualPanner
	{
		// Token: 0x06007CFF RID: 31999 RVA: 0x002CA850 File Offset: 0x002C8A50
		public WeatherOverlay_DeathpallAshes()
		{
			this.worldOverlayMat = WeatherOverlay_DeathpallAshes.AshesMat;
			this.worldOverlayPanSpeed1 = 0.003f;
			this.worldPanDir1 = new Vector2(-0.12f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.001f;
			this.worldPanDir2 = new Vector2(0.12f, -1f);
			this.worldPanDir2.Normalize();
			base.ForcedOverlayColor = new Color?(new Color(0.5f, 0.5f, 0.5f));
		}

		// Token: 0x04004EAC RID: 20140
		private static readonly Material AshesMat = MatLoader.LoadMat("Weather/DeathpallAshesOverlayWorld", -1);
	}
}
