using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001665 RID: 5733
	public class StageEndTrigger_RoleArrivedOrInTargetBed : StageEndTrigger_RolesArrived
	{
		// Token: 0x0600900B RID: 36875 RVA: 0x0034EC78 File Offset: 0x0034CE78
		protected override bool ArrivedCheck(string r, LordJob_Ritual ritual)
		{
			Pawn pawn = ritual.PawnWithRole(r);
			return (pawn != null && pawn.CurrentBed() == ritual.selectedTarget.Thing) || base.ArrivedCheck(r, ritual);
		}
	}
}
