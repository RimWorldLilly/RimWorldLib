using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF9 RID: 8185
	public class ResurrectCorpseData : IExposable
	{
		// Token: 0x0600CF62 RID: 53090 RVA: 0x004C605F File Offset: 0x004C425F
		public ResurrectCorpseData()
		{
		}

		// Token: 0x0600CF63 RID: 53091 RVA: 0x004C6067 File Offset: 0x004C4267
		public ResurrectCorpseData(Corpse corpse, IntVec3 castPosition)
		{
			this.corpse = corpse;
			this.castPosition = castPosition;
		}

		// Token: 0x0600CF64 RID: 53092 RVA: 0x004C607D File Offset: 0x004C427D
		public void ExposeData()
		{
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<IntVec3>(ref this.castPosition, "castPosition", IntVec3.Invalid, false);
		}

		// Token: 0x040083E8 RID: 33768
		public Corpse corpse;

		// Token: 0x040083E9 RID: 33769
		public IntVec3 castPosition;
	}
}
