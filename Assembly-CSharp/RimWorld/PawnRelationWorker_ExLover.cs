using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001530 RID: 5424
	public class PawnRelationWorker_ExLover : PawnRelationWorker
	{
		// Token: 0x060084C2 RID: 33986 RVA: 0x002FDCAA File Offset: 0x002FBEAA
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060084C3 RID: 33987 RVA: 0x002FDCBF File Offset: 0x002FBEBF
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
		}
	}
}
