using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152C RID: 5420
	public class PawnRelationWorker_Child : PawnRelationWorker
	{
		// Token: 0x060084B7 RID: 33975 RVA: 0x002FD855 File Offset: 0x002FBA55
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && (other.GetMother() == me || other.GetFather() == me || (ModsConfig.BiotechActive && other.GetBirthParent() == me));
		}

		// Token: 0x060084B8 RID: 33976 RVA: 0x002FD884 File Offset: 0x002FBA84
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 0f;
			if (!ChildRelationUtility.XenotypesCompatible(generated, other))
			{
				return 0f;
			}
			if (other.IsDuplicate)
			{
				return 0f;
			}
			if (generated.gender == Gender.Male)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(other, generated, other.GetMother(), null, new PawnGenerationRequest?(request), null);
			}
			else if (generated.gender == Gender.Female)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(other, other.GetFather(), generated, null, null, new PawnGenerationRequest?(request));
			}
			if (ModsConfig.BiotechActive && request.Context == PawnGenerationContext.PlayerStarter && other.DevelopmentalStage.Juvenile())
			{
				num *= 10f;
			}
			return num * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060084B9 RID: 33977 RVA: 0x002FD948 File Offset: 0x002FBB48
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (generated.gender == Gender.Male)
			{
				other.SetFather(generated);
				PawnRelationWorker_Child.ResolveMyName(ref request, other, other.GetMother());
				if (other.GetMother() != null)
				{
					if (other.GetMother().story.traits.HasTrait(TraitDefOf.Gay))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other.GetMother());
						return;
					}
					if (Rand.Value < 0.85f && !LovePartnerRelationUtility.HasAnyLovePartner(other.GetMother(), true))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other.GetMother());
						SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref request, generated);
						return;
					}
					LovePartnerRelationUtility.GiveRandomExLoverOrExSpouseRelation(generated, other.GetMother());
					return;
				}
			}
			else if (generated.gender == Gender.Female)
			{
				other.SetMother(generated);
				PawnRelationWorker_Child.ResolveMyName(ref request, other, other.GetFather());
				if (other.GetFather() != null)
				{
					if (other.GetFather().story.traits.HasTrait(TraitDefOf.Gay))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other.GetFather());
						return;
					}
					if (Rand.Value < 0.85f && !LovePartnerRelationUtility.HasAnyLovePartner(other.GetFather(), true))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other.GetFather());
						SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref request, generated);
						return;
					}
					LovePartnerRelationUtility.GiveRandomExLoverOrExSpouseRelation(generated, other.GetFather());
				}
			}
		}

		// Token: 0x060084BA RID: 33978 RVA: 0x002FDA98 File Offset: 0x002FBC98
		private static void ResolveMyName(ref PawnGenerationRequest request, Pawn child, Pawn otherParent)
		{
			if (request.FixedLastName != null)
			{
				return;
			}
			if (ChildRelationUtility.DefinitelyHasNotBirthName(child))
			{
				return;
			}
			if (ChildRelationUtility.ChildWantsNameOfAnyParent(child))
			{
				if (otherParent == null || !(otherParent.Name is NameTriple))
				{
					float num = 0.875f;
					if (Rand.Value < num)
					{
						request.SetFixedLastName(((NameTriple)child.Name).Last);
						return;
					}
				}
				else
				{
					string last = ((NameTriple)child.Name).Last;
					string last2 = ((NameTriple)otherParent.Name).Last;
					if (last != last2)
					{
						request.SetFixedLastName(last);
					}
				}
			}
		}

		// Token: 0x040051D4 RID: 20948
		private const float PlayerStartRelationFactor = 10f;
	}
}
