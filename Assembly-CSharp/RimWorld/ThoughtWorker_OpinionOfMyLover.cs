using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF8 RID: 3576
	public class ThoughtWorker_OpinionOfMyLover : ThoughtWorker
	{
		// Token: 0x060060D6 RID: 24790 RVA: 0x00230B30 File Offset: 0x0022ED30
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false);
			if (directPawnRelation == null)
			{
				return false;
			}
			if (!directPawnRelation.otherPawn.IsColonist || directPawnRelation.otherPawn.IsWorldPawn() || !directPawnRelation.otherPawn.relations.everSeenByPlayer)
			{
				return false;
			}
			return p.relations.OpinionOf(directPawnRelation.otherPawn) != 0;
		}
	}
}
