using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A3 RID: 6307
	public class CompProperties_GiveHediffSeverity : CompProperties
	{
		// Token: 0x06009DF9 RID: 40441 RVA: 0x0038B6EB File Offset: 0x003898EB
		public CompProperties_GiveHediffSeverity()
		{
			this.compClass = typeof(CompGiveHediffSeverity);
		}

		// Token: 0x04005D74 RID: 23924
		public HediffDef hediff;

		// Token: 0x04005D75 RID: 23925
		public float range;

		// Token: 0x04005D76 RID: 23926
		public float severityPerSecond;

		// Token: 0x04005D77 RID: 23927
		public bool drugExposure;

		// Token: 0x04005D78 RID: 23928
		public ChemicalDef chemical;

		// Token: 0x04005D79 RID: 23929
		public bool allowMechs = true;
	}
}
