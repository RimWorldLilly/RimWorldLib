using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153B RID: 5435
	public class PawnRelationWorker_Lover : PawnRelationWorker
	{
		// Token: 0x060084DD RID: 34013 RVA: 0x002FE068 File Offset: 0x002FC268
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060084DE RID: 34014 RVA: 0x002FE07D File Offset: 0x002FC27D
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Lover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
		}
	}
}
