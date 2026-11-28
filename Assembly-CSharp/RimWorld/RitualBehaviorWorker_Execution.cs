using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200168C RID: 5772
	public class RitualBehaviorWorker_Execution : RitualBehaviorWorker
	{
		// Token: 0x060090AF RID: 37039 RVA: 0x003514F8 File Offset: 0x0034F6F8
		public RitualBehaviorWorker_Execution()
		{
		}

		// Token: 0x060090B0 RID: 37040 RVA: 0x00351500 File Offset: 0x0034F700
		public RitualBehaviorWorker_Execution(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x060090B1 RID: 37041 RVA: 0x0035150C File Offset: 0x0034F70C
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("executioner");
			Pawn pawn = ritual.PawnWithRole("prisoner");
			if (pawn != null && pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}
	}
}
