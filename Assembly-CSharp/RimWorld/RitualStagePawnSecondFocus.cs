using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CAA RID: 3242
	public class RitualStagePawnSecondFocus : IExposable
	{
		// Token: 0x06005BC2 RID: 23490 RVA: 0x0021B7A8 File Offset: 0x002199A8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stageIndex, "stageIndex", 0, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_TargetInfo.Look(ref this.target, "target");
		}

		// Token: 0x0400347D RID: 13437
		public int stageIndex;

		// Token: 0x0400347E RID: 13438
		public Pawn pawn;

		// Token: 0x0400347F RID: 13439
		public TargetInfo target;
	}
}
