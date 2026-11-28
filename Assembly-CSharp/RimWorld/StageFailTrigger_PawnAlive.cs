using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001677 RID: 5751
	public class StageFailTrigger_PawnAlive : StageFailTrigger
	{
		// Token: 0x0600903F RID: 36927 RVA: 0x0034F6E0 File Offset: 0x0034D8E0
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			Pawn pawn = ritual.PawnWithRole(this.pawnId);
			return pawn != null && !pawn.Dead;
		}

		// Token: 0x06009040 RID: 36928 RVA: 0x0034F708 File Offset: 0x0034D908
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.pawnId, "pawnId", null, false);
		}

		// Token: 0x04005806 RID: 22534
		[NoTranslate]
		public string pawnId;
	}
}
