using System;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002085 RID: 8325
	public static class HealthAIUtility
	{
		// Token: 0x0600D1FD RID: 53757 RVA: 0x004D248F File Offset: 0x004D068F
		public static bool ShouldSeekMedicalRestUrgent(Pawn pawn)
		{
			return (pawn.Downed && !LifeStageUtility.AlwaysDowned(pawn)) || pawn.health.HasHediffsNeedingTend(false) || HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn) || pawn.health.hediffSet.InLabor(true);
		}

		// Token: 0x0600D1FE RID: 53758 RVA: 0x004D24CA File Offset: 0x004D06CA
		public static bool ShouldSeekMedicalRest(Pawn pawn)
		{
			return HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || pawn.health.hediffSet.HasTendedAndHealingInjury() || pawn.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		// Token: 0x0600D1FF RID: 53759 RVA: 0x004D24F8 File Offset: 0x004D06F8
		public static bool ShouldBeTendedNowByPlayerUrgent(Pawn pawn)
		{
			return HealthAIUtility.ShouldBeTendedNowByPlayer(pawn) && HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 45000;
		}

		// Token: 0x0600D200 RID: 53760 RVA: 0x004D2511 File Offset: 0x004D0711
		public static bool ShouldBeTendedNowByPlayer(Pawn pawn)
		{
			return pawn.playerSettings != null && HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(pawn) && pawn.health.HasHediffsNeedingTendByPlayer(false);
		}

		// Token: 0x0600D201 RID: 53761 RVA: 0x004D2534 File Offset: 0x004D0734
		public static bool ShouldEverReceiveMedicalCareFromPlayer(Pawn pawn)
		{
			Pawn_PlayerSettings playerSettings = pawn.playerSettings;
			return (playerSettings == null || playerSettings.medCare != MedicalCareCategory.NoCare) && (pawn.guest == null || !pawn.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Execution)) && !pawn.ShouldBeSlaughtered();
		}

		// Token: 0x0600D202 RID: 53762 RVA: 0x004D2581 File Offset: 0x004D0781
		public static bool ShouldHaveSurgeryDoneNow(Pawn pawn)
		{
			return pawn.health.surgeryBills.AnyShouldDoNow && WorkGiver_PatientGoToBedTreatment.AnyAvailableDoctorFor(pawn);
		}

		// Token: 0x0600D203 RID: 53763 RVA: 0x004D25A0 File Offset: 0x004D07A0
		public static bool WantsToBeRescued(Pawn pawn)
		{
			if (!pawn.Downed)
			{
				return false;
			}
			if (pawn.InBed() || pawn.IsCharging())
			{
				return false;
			}
			if (pawn.IsDeactivated())
			{
				return false;
			}
			if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(pawn))
			{
				return false;
			}
			if (pawn.IsMutant && !pawn.mutant.Def.entitledToMedicalCare)
			{
				return false;
			}
			if (pawn.InMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				return false;
			}
			if (pawn.ShouldBeSlaughtered())
			{
				return false;
			}
			Lord lord;
			if (pawn.TryGetLord(out lord))
			{
				LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
				RitualRole ritualRole;
				if (lordJob_Ritual != null && lordJob_Ritual.TryGetRoleFor(pawn, out ritualRole, false) && ritualRole.allowDowned)
				{
					return false;
				}
			}
			return !LifeStageUtility.AlwaysDowned(pawn) || HealthAIUtility.ShouldSeekMedicalRest(pawn);
		}

		// Token: 0x0600D204 RID: 53764 RVA: 0x004D2664 File Offset: 0x004D0864
		public static bool CanRescueNow(Pawn rescuer, Pawn patient, bool forced = false)
		{
			return (forced || patient.Faction == rescuer.Faction) && HealthAIUtility.WantsToBeRescued(patient) && (forced || !patient.IsForbidden(rescuer)) && (forced || !GenAI.EnemyIsNear(patient, 25f, false)) && rescuer.CanReserveAndReach(patient, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, forced);
		}

		// Token: 0x0600D205 RID: 53765 RVA: 0x004D26C8 File Offset: 0x004D08C8
		public static Thing FindBestMedicine(Pawn healer, Pawn patient, bool onlyUseInventory = false)
		{
			HealthAIUtility.<>c__DisplayClass9_0 CS$<>8__locals1 = new HealthAIUtility.<>c__DisplayClass9_0();
			CS$<>8__locals1.patient = patient;
			CS$<>8__locals1.onlyUseInventory = onlyUseInventory;
			CS$<>8__locals1.healer = healer;
			if (CS$<>8__locals1.patient.playerSettings != null && CS$<>8__locals1.patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
			{
				return null;
			}
			if (Medicine.GetMedicineCountToFullyHeal(CS$<>8__locals1.patient) <= 0)
			{
				return null;
			}
			CS$<>8__locals1.validator = delegate(Thing m)
			{
				bool flag = ((CS$<>8__locals1.patient.playerSettings != null) ? CS$<>8__locals1.patient.playerSettings.medCare : MedicalCareCategory.NoMeds).AllowsMedicine(m.def);
				if (CS$<>8__locals1.patient.playerSettings == null & CS$<>8__locals1.onlyUseInventory)
				{
					flag = true;
				}
				return !m.IsForbidden(CS$<>8__locals1.healer) && flag && CS$<>8__locals1.healer.CanReserve(m, 10, 1, null, false);
			};
			Thing thing = CS$<>8__locals1.<FindBestMedicine>g__GetBestMedInInventory|2(CS$<>8__locals1.healer.inventory.innerContainer);
			if (CS$<>8__locals1.onlyUseInventory)
			{
				return thing;
			}
			Thing thing2 = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals1.patient.PositionHeld, CS$<>8__locals1.patient.MapHeld, CS$<>8__locals1.patient.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Medicine), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.healer, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, CS$<>8__locals1.validator, new Func<Thing, float>(HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0), false);
			if (thing == null || thing2 == null)
			{
				if (thing == null && thing2 == null && CS$<>8__locals1.healer.IsColonist && CS$<>8__locals1.healer.Map != null)
				{
					foreach (Pawn pawn in CS$<>8__locals1.healer.Map.mapPawns.SpawnedColonyAnimals)
					{
						Thing thing3 = CS$<>8__locals1.<FindBestMedicine>g__GetBestMedInInventory|2(pawn.inventory.innerContainer);
						if (thing3 != null && (thing2 == null || HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0(thing2) < HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0(thing3)) && !pawn.IsForbidden(CS$<>8__locals1.healer) && CS$<>8__locals1.healer.CanReach(pawn, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
						{
							thing2 = thing3;
						}
					}
				}
				return thing ?? thing2;
			}
			if (HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0(thing) < HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0(thing2))
			{
				return thing2;
			}
			return thing;
		}

		// Token: 0x0600D206 RID: 53766 RVA: 0x004D28A0 File Offset: 0x004D0AA0
		[CompilerGenerated]
		internal static float <FindBestMedicine>g__PriorityOf|9_0(Thing t)
		{
			return t.def.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
		}

		// Token: 0x04008512 RID: 34066
		private const float MinDistFromEnemyToRescue = 25f;
	}
}
