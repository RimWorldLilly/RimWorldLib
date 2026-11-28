using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD2 RID: 2770
	public class JobGiver_AIDefendSelf : JobGiver_AIDefendPawn
	{
		// Token: 0x06005305 RID: 21253 RVA: 0x001EEE49 File Offset: 0x001ED049
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn;
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x001EEE4C File Offset: 0x001ED04C
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
