using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001532 RID: 5426
	public class PawnRelationWorker_Fiance : PawnRelationWorker
	{
		// Token: 0x060084C8 RID: 33992 RVA: 0x002FDD30 File Offset: 0x002FBF30
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			num *= this.GetOldAgeFactor(generated);
			num *= this.GetOldAgeFactor(other);
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request) * num;
		}

		// Token: 0x060084C9 RID: 33993 RVA: 0x002FDD6C File Offset: 0x002FBF6C
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Fiance, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.7f);
		}

		// Token: 0x060084CA RID: 33994 RVA: 0x002FDD91 File Offset: 0x002FBF91
		private float GetOldAgeFactor(Pawn pawn)
		{
			return Mathf.Clamp(GenMath.LerpDouble(50f, 80f, 1f, 0.01f, (float)pawn.ageTracker.AgeBiologicalYears), 0.01f, 1f);
		}

		// Token: 0x060084CB RID: 33995 RVA: 0x002FDDC8 File Offset: 0x002FBFC8
		public override void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
			Pawn pawn;
			if (firstPawn.Ideo == secondPawn.Ideo)
			{
				pawn = firstPawn;
			}
			else
			{
				pawn = ((Rand.Value < 0.5f) ? firstPawn : secondPawn);
			}
			firstPawn.relations.nextMarriageNameChange = (secondPawn.relations.nextMarriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage(pawn));
		}
	}
}
