using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B85 RID: 2949
	public static class SickPawnVisitUtility
	{
		// Token: 0x0600558F RID: 21903 RVA: 0x001FD1BC File Offset: 0x001FB3BC
		public static Pawn FindRandomSickPawn(Pawn pawn, JoyCategory maxPatientJoy)
		{
			Pawn result;
			if (!(from x in pawn.Map.mapPawns.FreeColonistsSpawned
			where SickPawnVisitUtility.CanVisit(pawn, x, maxPatientJoy)
			select x).TryRandomElementByWeight((Pawn x) => SickPawnVisitUtility.VisitChanceScore(pawn, x), out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005590 RID: 21904 RVA: 0x001FD21C File Offset: 0x001FB41C
		public static bool CanVisit(Pawn pawn, Pawn sick, JoyCategory maxPatientJoy)
		{
			if (sick.IsColonist && !sick.IsSlave && !pawn.IsSlave && pawn.RaceProps.Humanlike && !sick.Dead && pawn != sick && sick.InBed() && sick.Awake() && !sick.Fogged() && !sick.IsForbidden(pawn))
			{
				Pawn_NeedsTracker needs = sick.needs;
				if (((needs != null) ? needs.joy : null) != null && sick.needs.joy.CurCategory <= maxPatientJoy && SocialInteractionUtility.CanReceiveInteraction(sick, null))
				{
					Pawn_NeedsTracker needs2 = sick.needs;
					if (((needs2 != null) ? needs2.food : null) == null || !sick.needs.food.Starving)
					{
						Pawn_NeedsTracker needs3 = sick.needs;
						if ((((needs3 != null) ? needs3.rest : null) == null || sick.needs.rest.CurLevel > 0.33f) && pawn.CanReserveAndReach(sick, PathEndMode.InteractionCell, Danger.None, 1, -1, null, false) && !SickPawnVisitUtility.AboutToRecover(sick))
						{
							return !sick.VacuumConcernTo(pawn);
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06005591 RID: 21905 RVA: 0x001FD348 File Offset: 0x001FB548
		public static Thing FindChair(Pawn forPawn, Pawn nearPawn)
		{
			SickPawnVisitUtility.<>c__DisplayClass2_0 CS$<>8__locals1 = new SickPawnVisitUtility.<>c__DisplayClass2_0();
			CS$<>8__locals1.forPawn = forPawn;
			CS$<>8__locals1.nearPawn = nearPawn;
			return GenClosest.ClosestThingReachable(CS$<>8__locals1.nearPawn.Position, CS$<>8__locals1.nearPawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.forPawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 2.2f, new Predicate<Thing>(CS$<>8__locals1.<FindChair>g__Validator|0), null, 0, 5, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x001FD3B8 File Offset: 0x001FB5B8
		private static bool AboutToRecover(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return false;
			}
			if (!HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				return true;
			}
			if (pawn.health.hediffSet.HasImmunizableNotImmuneHediff())
			{
				return false;
			}
			float num = 0f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && (hediff_Injury.CanHealFromTending() || hediff_Injury.CanHealNaturally() || hediff_Injury.Bleeding))
				{
					num += hediff_Injury.Severity;
				}
			}
			return num < 8f * pawn.RaceProps.baseHealthScale;
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x001FD45C File Offset: 0x001FB65C
		private static float VisitChanceScore(Pawn pawn, Pawn sick)
		{
			float num = GenMath.LerpDouble(-100f, 100f, 0.05f, 2f, (float)pawn.relations.OpinionOf(sick));
			float lengthHorizontal = (pawn.Position - sick.Position).LengthHorizontal;
			float num2 = Mathf.Clamp(GenMath.LerpDouble(0f, 150f, 1f, 0.2f, lengthHorizontal), 0.2f, 1f);
			return num * num2;
		}
	}
}
