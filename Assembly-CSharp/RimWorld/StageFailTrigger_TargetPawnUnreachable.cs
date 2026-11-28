using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001675 RID: 5749
	public class StageFailTrigger_TargetPawnUnreachable : StageFailTrigger
	{
		// Token: 0x06009039 RID: 36921 RVA: 0x0034F634 File Offset: 0x0034D834
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			Pawn pawn = ritual.PawnWithRole(this.takerId);
			Pawn t = ritual.PawnWithRole(this.takeeId);
			return !pawn.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x0600903A RID: 36922 RVA: 0x0034F66D File Offset: 0x0034D86D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.takerId, "takerId", null, false);
			Scribe_Values.Look<string>(ref this.takeeId, "takeeId", null, false);
		}

		// Token: 0x04005803 RID: 22531
		[NoTranslate]
		public string takerId;

		// Token: 0x04005804 RID: 22532
		[NoTranslate]
		public string takeeId;
	}
}
