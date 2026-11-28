using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153E RID: 5438
	public class PawnRelationWorker_Parent : PawnRelationWorker
	{
		// Token: 0x060084E7 RID: 34023 RVA: 0x002FE2F4 File Offset: 0x002FC4F4
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 0f;
			if (!ChildRelationUtility.XenotypesCompatible(generated, other))
			{
				return 0f;
			}
			if (other.gender == Gender.Male)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other, other.GetFirstSpouseOfOppositeGender(), new PawnGenerationRequest?(request), null, null);
			}
			else if (other.gender == Gender.Female)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other.GetFirstSpouseOfOppositeGender(), other, new PawnGenerationRequest?(request), null, null);
			}
			if (ModsConfig.BiotechActive && request.Context == PawnGenerationContext.PlayerStarter && generated.DevelopmentalStage.Juvenile())
			{
				num *= 10f;
			}
			return num * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060084E8 RID: 34024 RVA: 0x002FE3A8 File Offset: 0x002FC5A8
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (other.gender == Gender.Male)
			{
				generated.SetFather(other);
				Pawn firstSpouseOfOppositeGender = other.GetFirstSpouseOfOppositeGender();
				if (firstSpouseOfOppositeGender != null)
				{
					generated.SetMother(firstSpouseOfOppositeGender);
				}
				PawnRelationWorker_Parent.ResolveMyName(ref request, generated);
				return;
			}
			if (other.gender == Gender.Female)
			{
				generated.SetMother(other);
				Pawn firstSpouseOfOppositeGender2 = other.GetFirstSpouseOfOppositeGender();
				if (firstSpouseOfOppositeGender2 != null)
				{
					generated.SetFather(firstSpouseOfOppositeGender2);
				}
				PawnRelationWorker_Parent.ResolveMyName(ref request, generated);
			}
		}

		// Token: 0x060084E9 RID: 34025 RVA: 0x002FE408 File Offset: 0x002FC608
		private static void ResolveMyName(ref PawnGenerationRequest request, Pawn generatedChild)
		{
			if (request.FixedLastName != null)
			{
				return;
			}
			if (ChildRelationUtility.ChildWantsNameOfAnyParent(generatedChild))
			{
				bool flag = Rand.Value < 0.5f || generatedChild.GetMother() == null;
				if (generatedChild.GetFather() == null)
				{
					flag = false;
				}
				if (flag)
				{
					request.SetFixedLastName(((NameTriple)generatedChild.GetFather().Name).Last);
					return;
				}
				request.SetFixedLastName(((NameTriple)generatedChild.GetMother().Name).Last);
			}
		}

		// Token: 0x040051D5 RID: 20949
		private const float PlayerStartRelationFactor = 10f;
	}
}
