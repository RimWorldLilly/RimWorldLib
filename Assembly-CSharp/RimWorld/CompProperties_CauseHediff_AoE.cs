using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001848 RID: 6216
	public class CompProperties_CauseHediff_AoE : CompProperties
	{
		// Token: 0x06009B9C RID: 39836 RVA: 0x0038178A File Offset: 0x0037F98A
		public CompProperties_CauseHediff_AoE()
		{
			this.compClass = typeof(CompCauseHediff_AoE);
		}

		// Token: 0x04005C86 RID: 23686
		public HediffDef hediff;

		// Token: 0x04005C87 RID: 23687
		public BodyPartDef part;

		// Token: 0x04005C88 RID: 23688
		public float range;

		// Token: 0x04005C89 RID: 23689
		public bool onlyTargetMechs;

		// Token: 0x04005C8A RID: 23690
		public bool ignoreMechs;

		// Token: 0x04005C8B RID: 23691
		public int checkInterval = 10;

		// Token: 0x04005C8C RID: 23692
		public SoundDef activeSound;

		// Token: 0x04005C8D RID: 23693
		public bool canTargetSelf = true;

		// Token: 0x04005C8E RID: 23694
		public bool drawLines = true;
	}
}
