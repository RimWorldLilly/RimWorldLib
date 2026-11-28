using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001529 RID: 5417
	public static class LovePartnerRelationUtility
	{
		// Token: 0x0600847D RID: 33917 RVA: 0x002FBF67 File Offset: 0x002FA167
		public static bool HasAnyLovePartner(Pawn pawn, bool allowDead = true)
		{
			return LovePartnerRelationUtility.ExistingLovePartner(pawn, allowDead) != null;
		}

		// Token: 0x0600847E RID: 33918 RVA: 0x002FBF73 File Offset: 0x002FA173
		public static bool IsLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.Lover || relation == PawnRelationDefOf.Fiance || relation == PawnRelationDefOf.Spouse;
		}

		// Token: 0x0600847F RID: 33919 RVA: 0x002FBF8F File Offset: 0x002FA18F
		public static bool IsExLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.ExLover || relation == PawnRelationDefOf.ExSpouse;
		}

		// Token: 0x06008480 RID: 33920 RVA: 0x002FBFA4 File Offset: 0x002FA1A4
		public static bool HasAnyLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x06008481 RID: 33921 RVA: 0x002FBFE4 File Offset: 0x002FA1E4
		public static bool HasAnyExLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x06008482 RID: 33922 RVA: 0x002FC024 File Offset: 0x002FA224
		public static bool HasAnyLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x06008483 RID: 33923 RVA: 0x002FC064 File Offset: 0x002FA264
		public static bool HasAnyExLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x06008484 RID: 33924 RVA: 0x002FC0A4 File Offset: 0x002FA2A4
		public static Pawn ExistingLovePartner(Pawn pawn, bool allowDead = true)
		{
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (!directRelations[i].otherPawn.Destroyed || allowDead))
				{
					return directRelations[i].otherPawn;
				}
			}
			return null;
		}

		// Token: 0x06008485 RID: 33925 RVA: 0x002FC108 File Offset: 0x002FA308
		public static List<DirectPawnRelation> ExistingLovePartners(Pawn pawn, bool allowDead = true)
		{
			LovePartnerRelationUtility.tmpExistingLovePartners.Clear();
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (!directRelations[i].otherPawn.Destroyed || allowDead))
				{
					LovePartnerRelationUtility.tmpExistingLovePartners.Add(directRelations[i]);
				}
			}
			return LovePartnerRelationUtility.tmpExistingLovePartners;
		}

		// Token: 0x06008486 RID: 33926 RVA: 0x002FC180 File Offset: 0x002FA380
		public static DirectPawnRelation ExistingLoveRealtionshipBetween(Pawn pawn, Pawn other, bool allowDead = true)
		{
			return (from r in LovePartnerRelationUtility.ExistingLovePartners(pawn, allowDead)
			where r.otherPawn == other
			select r).FirstOrDefault<DirectPawnRelation>();
		}

		// Token: 0x06008487 RID: 33927 RVA: 0x002FC1B8 File Offset: 0x002FA3B8
		public static int ExistingLovePartnersCount(Pawn pawn, bool allowDead = true)
		{
			int num = 0;
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (!directRelations[i].otherPawn.Destroyed || allowDead))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06008488 RID: 33928 RVA: 0x002FC214 File Offset: 0x002FA414
		public static bool LovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.Lover, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Fiance, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Spouse, second);
		}

		// Token: 0x06008489 RID: 33929 RVA: 0x002FC24F File Offset: 0x002FA44F
		public static bool ExLovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.ExSpouse, second) || first.relations.DirectRelationExists(PawnRelationDefOf.ExLover, second);
		}

		// Token: 0x0600848A RID: 33930 RVA: 0x002FC278 File Offset: 0x002FA478
		public static void GiveRandomExLoverOrExSpouseRelation(Pawn first, Pawn second)
		{
			PawnRelationDef def;
			if (Rand.Value < 0.5f)
			{
				def = PawnRelationDefOf.ExLover;
			}
			else
			{
				def = PawnRelationDefOf.ExSpouse;
			}
			first.relations.AddDirectRelation(def, second);
		}

		// Token: 0x0600848B RID: 33931 RVA: 0x002FC2AC File Offset: 0x002FA4AC
		public static Pawn GetPartnerInMyBed(Pawn pawn)
		{
			Building_Bed building_Bed = pawn.CurrentBed();
			if (building_Bed == null)
			{
				return null;
			}
			if (building_Bed.SleepingSlotsCount <= 1)
			{
				return null;
			}
			if (!LovePartnerRelationUtility.HasAnyLovePartner(pawn, true))
			{
				return null;
			}
			foreach (Pawn pawn2 in building_Bed.CurOccupants)
			{
				if (pawn2 != pawn && LovePartnerRelationUtility.LovePartnerRelationExists(pawn, pawn2))
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x0600848C RID: 33932 RVA: 0x002FC328 File Offset: 0x002FA528
		public static Pawn ExistingLeastLikedPawnWithRelation(Pawn p, Func<DirectPawnRelation, bool> validator)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingLeastLikedRel(p, validator);
			if (directPawnRelation != null)
			{
				return directPawnRelation.otherPawn;
			}
			return null;
		}

		// Token: 0x0600848D RID: 33933 RVA: 0x002FC348 File Offset: 0x002FA548
		public static DirectPawnRelation ExistingLeastLikedRel(Pawn p, Func<DirectPawnRelation, bool> validator)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MaxValue;
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (validator(directRelations[i]))
				{
					int num2 = p.relations.OpinionOf(directRelations[i].otherPawn);
					if (directPawnRelation == null || num2 < num)
					{
						directPawnRelation = directRelations[i];
						num = num2;
					}
				}
			}
			return directPawnRelation;
		}

		// Token: 0x0600848E RID: 33934 RVA: 0x002FC3C8 File Offset: 0x002FA5C8
		public static Pawn ExistingMostLikedLovePartner(Pawn p, bool allowDead)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, allowDead);
			if (directPawnRelation != null)
			{
				return directPawnRelation.otherPawn;
			}
			return null;
		}

		// Token: 0x0600848F RID: 33935 RVA: 0x002FC3E8 File Offset: 0x002FA5E8
		public static DirectPawnRelation ExistingMostLikedLovePartnerRel(Pawn p, bool allowDead)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MinValue;
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if ((allowDead || !directRelations[i].otherPawn.Dead) && LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def))
				{
					int num2 = p.relations.OpinionOf(directRelations[i].otherPawn);
					if (directPawnRelation == null || num2 > num)
					{
						directPawnRelation = directRelations[i];
						num = num2;
					}
				}
			}
			return directPawnRelation;
		}

		// Token: 0x06008490 RID: 33936 RVA: 0x002FC480 File Offset: 0x002FA680
		public static HistoryEventDef GetHistoryEventLoveRelationCount(this Pawn pawn)
		{
			int count = pawn.GetLoveRelations(false, false).Count;
			if (count <= 1)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (count <= 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (count <= 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (count <= 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x06008491 RID: 33937 RVA: 0x002FC4C8 File Offset: 0x002FA6C8
		public static HistoryEventDef GetHistoryEventForLoveRelationCountPlusOne(this Pawn pawn)
		{
			int count = pawn.GetLoveRelations(false, false).Count;
			if (count == 0)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (count < 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (count < 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (count < 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x06008492 RID: 33938 RVA: 0x002FC510 File Offset: 0x002FA710
		public static float GetLovinMtbHours(Pawn pawn, Pawn partner)
		{
			if (pawn.Dead || partner.Dead)
			{
				return -1f;
			}
			if (DebugSettings.alwaysDoLovin)
			{
				return 0.1f;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			bool? flag;
			if (needs == null)
			{
				flag = null;
			}
			else
			{
				Need_Food food = needs.food;
				flag = ((food != null) ? new bool?(food.Starving) : null);
			}
			bool? flag2 = flag;
			if (!flag2.GetValueOrDefault())
			{
				Pawn_NeedsTracker needs2 = partner.needs;
				bool? flag3;
				if (needs2 == null)
				{
					flag3 = null;
				}
				else
				{
					Need_Food food2 = needs2.food;
					flag3 = ((food2 != null) ? new bool?(food2.Starving) : null);
				}
				flag2 = flag3;
				if (!flag2.GetValueOrDefault())
				{
					if (pawn.health.hediffSet.BleedRateTotal > 0f || partner.health.hediffSet.BleedRateTotal > 0f)
					{
						return -1f;
					}
					if (pawn.health.hediffSet.InLabor(true) || partner.health.hediffSet.InLabor(true))
					{
						return -1f;
					}
					float num = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(pawn);
					if (num <= 0f)
					{
						return -1f;
					}
					float num2 = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(partner);
					if (num2 <= 0f)
					{
						return -1f;
					}
					float num3 = 12f;
					num3 *= num;
					num3 *= num2;
					num3 /= Mathf.Max(pawn.relations.SecondaryLovinChanceFactor(partner), 0.1f);
					num3 /= Mathf.Max(partner.relations.SecondaryLovinChanceFactor(pawn), 0.1f);
					num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)pawn.relations.OpinionOf(partner));
					num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)partner.relations.OpinionOf(pawn));
					if (pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicLove, false))
					{
						num3 /= 4f;
					}
					return num3;
				}
			}
			return -1f;
		}

		// Token: 0x06008493 RID: 33939 RVA: 0x002FC700 File Offset: 0x002FA900
		private static float LovinMtbSinglePawnFactor(Pawn pawn)
		{
			float num = 1f;
			num /= 1f - pawn.health.hediffSet.PainTotal;
			float level = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
			if (level < 0.5f)
			{
				num /= level * 2f;
			}
			return num / GenMath.FlatHill(0f, 14f, 16f, 25f, 80f, 0.2f, pawn.ageTracker.AgeBiologicalYearsFloat);
		}

		// Token: 0x06008494 RID: 33940 RVA: 0x002FC786 File Offset: 0x002FA986
		public static void TryToShareBed(Pawn first, Pawn second)
		{
			if (LovePartnerRelationUtility.TryToShareBed_Int(first, second))
			{
				return;
			}
			LovePartnerRelationUtility.TryToShareBed_Int(second, first);
		}

		// Token: 0x06008495 RID: 33941 RVA: 0x002FC79C File Offset: 0x002FA99C
		private static bool TryToShareBed_Int(Pawn bedOwner, Pawn otherPawn)
		{
			Building_Bed ownedBed = bedOwner.ownership.OwnedBed;
			if (ownedBed != null && ownedBed.AnyUnownedSleepingSlot && BedUtility.WillingToShareBed(bedOwner, otherPawn))
			{
				otherPawn.ownership.ClaimBedIfNonMedical(ownedBed);
				return true;
			}
			return false;
		}

		// Token: 0x06008496 RID: 33942 RVA: 0x002FC7DC File Offset: 0x002FA9DC
		public static float LovePartnerRelationGenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request, bool ex)
		{
			if (generated.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			if (other.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			if (other.story == null)
			{
				return 0f;
			}
			if (generated.gender == other.gender && (!other.story.traits.HasTrait(TraitDefOf.Gay) || !request.AllowGay))
			{
				return 0f;
			}
			if (generated.gender != other.gender && other.story.traits.HasTrait(TraitDefOf.Gay))
			{
				return 0f;
			}
			if (ModsConfig.BiotechActive)
			{
				if (((generated != null) ? generated.records : null) != null && generated.records.GetValue(RecordDefOf.TimeAsChildInColony) > 0f)
				{
					return 0f;
				}
				if (((other != null) ? other.records : null) != null && other.records.GetValue(RecordDefOf.TimeAsChildInColony) > 0f)
				{
					return 0f;
				}
			}
			float num = 1f;
			if (ex)
			{
				int num2 = 0;
				List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (LovePartnerRelationUtility.IsExLovePartnerRelation(directRelations[i].def))
					{
						num2++;
					}
				}
				num = Mathf.Pow(0.2f, (float)num2);
			}
			else if (LovePartnerRelationUtility.HasAnyLovePartner(other, true))
			{
				return 0f;
			}
			float num3 = (generated.gender == other.gender) ? 0.01f : 1f;
			float generationChanceAgeFactor = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(generated);
			float generationChanceAgeFactor2 = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(other);
			float generationChanceAgeGapFactor = LovePartnerRelationUtility.GetGenerationChanceAgeGapFactor(generated, other, ex);
			float num4 = 1f;
			if (generated.GetRelations(other).Any((PawnRelationDef x) => x.familyByBloodRelation))
			{
				num4 = 0.01f;
			}
			return num * generationChanceAgeFactor * generationChanceAgeFactor2 * generationChanceAgeGapFactor * num3 * num4;
		}

		// Token: 0x06008497 RID: 33943 RVA: 0x002FC9C7 File Offset: 0x002FABC7
		private static float GetGenerationChanceAgeFactor(Pawn p)
		{
			return Mathf.Clamp(GenMath.LerpDouble(14f, 27f, 0f, 1f, p.ageTracker.AgeBiologicalYearsFloat), 0f, 1f);
		}

		// Token: 0x06008498 RID: 33944 RVA: 0x002FC9FC File Offset: 0x002FABFC
		private static float GetGenerationChanceAgeGapFactor(Pawn p1, Pawn p2, bool ex)
		{
			float num = Mathf.Abs(p1.ageTracker.AgeBiologicalYearsFloat - p2.ageTracker.AgeBiologicalYearsFloat);
			if (ex)
			{
				float num2 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p1, p2);
				if (num2 >= 0f)
				{
					num = Mathf.Min(num, num2);
				}
				float num3 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p2, p1);
				if (num3 >= 0f)
				{
					num = Mathf.Min(num, num3);
				}
			}
			if (num > 40f)
			{
				return 0f;
			}
			return Mathf.Clamp(GenMath.LerpDouble(0f, 20f, 1f, 0.001f, num), 0.001f, 1f);
		}

		// Token: 0x06008499 RID: 33945 RVA: 0x002FCA90 File Offset: 0x002FAC90
		private static float MinPossibleAgeGapAtMinAgeToGenerateAsLovers(Pawn p1, Pawn p2)
		{
			float num = p1.ageTracker.AgeChronologicalYearsFloat - 14f;
			if (num < 0f)
			{
				return 0f;
			}
			float num2 = PawnRelationUtility.MaxPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, p2.ageTracker.AgeChronologicalYearsFloat, num);
			float num3 = PawnRelationUtility.MinPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, num);
			if (num2 < 0f)
			{
				return -1f;
			}
			if (num2 < 14f)
			{
				return -1f;
			}
			if (num3 <= 14f)
			{
				return 0f;
			}
			return num3 - 14f;
		}

		// Token: 0x0600849A RID: 33946 RVA: 0x002FCB20 File Offset: 0x002FAD20
		public static void TryToShareChildrenForGeneratedLovePartner(Pawn generated, Pawn other, PawnGenerationRequest request, float extraChanceFactor)
		{
			if (generated.gender == other.gender)
			{
				return;
			}
			List<Pawn> list = other.relations.Children.ToList<Pawn>();
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				float num = 1f;
				if (generated.gender == Gender.Male)
				{
					num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, generated, other, null, new PawnGenerationRequest?(request), null);
				}
				else if (generated.gender == Gender.Female)
				{
					num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, other, generated, null, null, new PawnGenerationRequest?(request));
				}
				num *= extraChanceFactor;
				if (Rand.Value < num)
				{
					if (generated.gender == Gender.Male)
					{
						pawn.SetFather(generated);
					}
					else if (generated.gender == Gender.Female)
					{
						pawn.SetMother(generated);
					}
				}
			}
		}

		// Token: 0x0600849B RID: 33947 RVA: 0x002FCBFC File Offset: 0x002FADFC
		public static void ChangeSpouseRelationsToExSpouse(Pawn pawn)
		{
			List<Pawn> spouses = pawn.GetSpouses(true);
			for (int i = spouses.Count - 1; i >= 0; i--)
			{
				HistoryEvent ev = new HistoryEvent(pawn.GetHistoryEventForSpouseCountPlusOne(), pawn.Named(HistoryEventArgsNames.Doer));
				if (spouses[i].Dead || !ev.DoerWillingToDo())
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, spouses[i]);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, spouses[i]);
				}
			}
		}

		// Token: 0x0600849C RID: 33948 RVA: 0x002FCC80 File Offset: 0x002FAE80
		public static Pawn GetMostDislikedNonPartnerBedOwner(Pawn p)
		{
			Building_Bed ownedBed = p.ownership.OwnedBed;
			if (ownedBed == null)
			{
				return null;
			}
			Pawn pawn = null;
			int num = 0;
			for (int i = 0; i < ownedBed.OwnersForReading.Count; i++)
			{
				if (ownedBed.OwnersForReading[i] != p && !LovePartnerRelationUtility.LovePartnerRelationExists(p, ownedBed.OwnersForReading[i]))
				{
					int num2 = p.relations.OpinionOf(ownedBed.OwnersForReading[i]);
					if (pawn == null || num2 < num)
					{
						pawn = ownedBed.OwnersForReading[i];
						num = num2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x0600849D RID: 33949 RVA: 0x002FCD10 File Offset: 0x002FAF10
		public static float IncestOpinionOffsetFor(Pawn other, Pawn pawn)
		{
			float num = 0f;
			List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && directRelations[i].otherPawn != pawn && !directRelations[i].otherPawn.Dead)
				{
					foreach (PawnRelationDef pawnRelationDef in other.GetRelations(directRelations[i].otherPawn))
					{
						float incestOpinionOffset = pawnRelationDef.incestOpinionOffset;
						if (incestOpinionOffset < num)
						{
							num = incestOpinionOffset;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600849E RID: 33950 RVA: 0x002FCDCC File Offset: 0x002FAFCC
		public static bool AreNearEachOther(Pawn p1, Pawn p2)
		{
			return !p1.DestroyedOrNull() && !p2.DestroyedOrNull() && ((p1.MapHeld != null && p1.MapHeld == p2.MapHeld) || (p1.GetCaravan() != null && p1.GetCaravan() == p2.GetCaravan()) || (p1.ParentHolder != null && p1.ParentHolder == p2.ParentHolder));
		}

		// Token: 0x040051D1 RID: 20945
		private const float MinAgeToGenerateWithLovePartnerRelation = 14f;

		// Token: 0x040051D2 RID: 20946
		private static List<DirectPawnRelation> tmpExistingLovePartners = new List<DirectPawnRelation>();
	}
}
