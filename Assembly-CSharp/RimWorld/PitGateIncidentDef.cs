using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBF RID: 4031
	public class PitGateIncidentDef : Def
	{
		// Token: 0x04003ECB RID: 16075
		public Type workerClass = typeof(PitGateIncidentWorker);

		// Token: 0x04003ECC RID: 16076
		public float baseChance;

		// Token: 0x04003ECD RID: 16077
		public IntRange durationRangeTicks;

		// Token: 0x04003ECE RID: 16078
		public bool usesThreatPoints;

		// Token: 0x04003ECF RID: 16079
		public int disableEnteringTicks;

		// Token: 0x04003ED0 RID: 16080
		[MustTranslate]
		public string disableEnteringReason;

		// Token: 0x04003ED1 RID: 16081
		[MustTranslate]
		public string letterText;

		// Token: 0x04003ED2 RID: 16082
		[MustTranslate]
		public string letterLabel;
	}
}
