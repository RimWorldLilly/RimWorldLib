using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001531 RID: 5425
	public class PawnRelationWorker_ExSpouse : PawnRelationWorker
	{
		// Token: 0x060084C5 RID: 33989 RVA: 0x002FDCEC File Offset: 0x002FBEEC
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060084C6 RID: 33990 RVA: 0x002FDD01 File Offset: 0x002FBF01
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
		}
	}
}
