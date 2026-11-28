using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CD6 RID: 3286
	public class LordToilData_EntitySwarm : LordToilData
	{
		// Token: 0x06005D08 RID: 23816 RVA: 0x00222120 File Offset: 0x00220320
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.pos, "pos", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastMoved, "lastMoved", 0, false);
		}

		// Token: 0x040034F6 RID: 13558
		public IntVec3 pos;

		// Token: 0x040034F7 RID: 13559
		public IntVec3 dest;

		// Token: 0x040034F8 RID: 13560
		public int lastMoved;
	}
}
