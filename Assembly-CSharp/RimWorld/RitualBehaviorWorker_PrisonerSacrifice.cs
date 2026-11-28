using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200168B RID: 5771
	public class RitualBehaviorWorker_PrisonerSacrifice : RitualBehaviorWorker
	{
		// Token: 0x060090AC RID: 37036 RVA: 0x003514A0 File Offset: 0x0034F6A0
		public RitualBehaviorWorker_PrisonerSacrifice()
		{
		}

		// Token: 0x060090AD RID: 37037 RVA: 0x003514A8 File Offset: 0x0034F6A8
		public RitualBehaviorWorker_PrisonerSacrifice(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x060090AE RID: 37038 RVA: 0x003514B4 File Offset: 0x0034F6B4
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("moralist");
			Pawn pawn = ritual.PawnWithRole("prisoner");
			if (pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}
	}
}
