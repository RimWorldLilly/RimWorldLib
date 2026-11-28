using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D0B RID: 3339
	public struct CachedPawnRitualDuty
	{
		// Token: 0x04003549 RID: 13641
		public DutyDef duty;

		// Token: 0x0400354A RID: 13642
		public IntVec3 spot;

		// Token: 0x0400354B RID: 13643
		public Thing usedThing;

		// Token: 0x0400354C RID: 13644
		public Rot4 overrideFacing;

		// Token: 0x0400354D RID: 13645
		public LocalTargetInfo secondFocus;
	}
}
