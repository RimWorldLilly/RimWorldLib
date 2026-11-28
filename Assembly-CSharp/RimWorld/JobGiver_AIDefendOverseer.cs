using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD4 RID: 2772
	public class JobGiver_AIDefendOverseer : JobGiver_AIDefendPawn
	{
		// Token: 0x0600530B RID: 21259 RVA: 0x001EEEB1 File Offset: 0x001ED0B1
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null);
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x001EEEC4 File Offset: 0x001ED0C4
		protected override float GetFlagRadius(Pawn pawn)
		{
			return 5f;
		}
	}
}
