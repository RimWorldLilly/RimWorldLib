using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013BB RID: 5051
	[StaticConstructorOnStartup]
	public class WeatherOverlay_HateChant : WeatherOverlayDualPanner
	{
		// Token: 0x06007D21 RID: 32033 RVA: 0x002CB2A0 File Offset: 0x002C94A0
		public WeatherOverlay_HateChant()
		{
			this.worldOverlayMat = WeatherOverlay_HateChant.HateChantOverlayWorld;
		}

		// Token: 0x04004EC7 RID: 20167
		private static readonly Material HateChantOverlayWorld = MatLoader.LoadMat("Weather/HateChantOverlayWorld", -1);
	}
}
