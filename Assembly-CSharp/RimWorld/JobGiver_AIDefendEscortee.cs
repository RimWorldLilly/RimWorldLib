using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD1 RID: 2769
	public class JobGiver_AIDefendEscortee : JobGiver_AIDefendPawn
	{
		// Token: 0x06005302 RID: 21250 RVA: 0x001EEE13 File Offset: 0x001ED013
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return ((Thing)pawn.mindState.duty.focus) as Pawn;
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x001EEE2F File Offset: 0x001ED02F
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
