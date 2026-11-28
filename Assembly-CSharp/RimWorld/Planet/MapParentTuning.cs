using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023B0 RID: 9136
	public static class MapParentTuning
	{
		// Token: 0x04009106 RID: 37126
		public const float ShortDetectionCountdownDays = 4f;

		// Token: 0x04009107 RID: 37127
		public const float DefaultDetectionCountdownDays = 4f;

		// Token: 0x04009108 RID: 37128
		public static readonly FloatRange SiteDetectionCountdownMultiplier = new FloatRange(0.85f, 1.15f);
	}
}
