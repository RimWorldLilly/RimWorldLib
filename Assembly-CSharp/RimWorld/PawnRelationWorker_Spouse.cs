using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001542 RID: 5442
	public class PawnRelationWorker_Spouse : PawnRelationWorker
	{
		// Token: 0x060084F6 RID: 34038 RVA: 0x002FEBBF File Offset: 0x002FCDBF
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060084F7 RID: 34039 RVA: 0x002FEBD4 File Offset: 0x002FCDD4
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref request, generated);
		}
	}
}
