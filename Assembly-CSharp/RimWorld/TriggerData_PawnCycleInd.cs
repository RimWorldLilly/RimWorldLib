using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D27 RID: 3367
	public class TriggerData_PawnCycleInd : TriggerData
	{
		// Token: 0x06005E64 RID: 24164 RVA: 0x00227F82 File Offset: 0x00226182
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.pawnCycleInd, "pawnCycleInd", 0, false);
		}

		// Token: 0x0400357D RID: 13693
		public int pawnCycleInd;
	}
}
