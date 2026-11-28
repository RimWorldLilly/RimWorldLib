using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CF1 RID: 3313
	public class LordToilData_ReimplantXenogerm : LordToilData
	{
		// Token: 0x06005D85 RID: 23941 RVA: 0x002240E8 File Offset: 0x002222E8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.waitSpot, "waitSpot", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x04003516 RID: 13590
		public IntVec3 waitSpot;

		// Token: 0x04003517 RID: 13591
		public Pawn target;
	}
}
