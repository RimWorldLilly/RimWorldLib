using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D2D RID: 3373
	public static class RestUtility
	{
		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x06005E88 RID: 24200 RVA: 0x00228B6D File Offset: 0x00226D6D
		public static List<ThingDef> AllBedDefBestToWorst
		{
			get
			{
				return RestUtility.bedDefsBestToWorst_RestEffectiveness;
			}
		}

		// Token: 0x06005E89 RID: 24201 RVA: 0x00228B74 File Offset: 0x00226D74
		public static void Reset()
		{
			RestUtility.bedDefsBestToWorst_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_SlabBed_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby (!d.building.bed_slabBed) ? 1 : 0, d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null) descending, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_SlabBed_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby (!d.building.bed_slabBed) ? 1 : 0, d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null) descending, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
		}

		// Token: 0x06005E8A RID: 24202 RVA: 0x00228E00 File Offset: 0x00227000
		public static bool BedOwnerWillShare(Building_Bed bed, Pawn sleeper, GuestStatus? guestStatus)
		{
			if (!bed.OwnersForReading.Any<Pawn>())
			{
				return true;
			}
			if (sleeper.IsPrisoner || guestStatus.GetValueOrDefault() == GuestStatus.Prisoner || sleeper.IsSlave || guestStatus.GetValueOrDefault() == GuestStatus.Slave)
			{
				if (!bed.AnyUnownedSleepingSlot)
				{
					return false;
				}
			}
			else
			{
				if (!bed.AnyUnownedSleepingSlot)
				{
					return false;
				}
				if (!RestUtility.IsAnyOwnerLovePartnerOf(bed, sleeper))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005E8B RID: 24203 RVA: 0x00228E60 File Offset: 0x00227060
		public static bool CanUseBedNow(Thing bedThing, Pawn sleeper, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false, GuestStatus? guestStatusOverride = null)
		{
			Building_Bed building_Bed = bedThing as Building_Bed;
			if (building_Bed == null)
			{
				return false;
			}
			if (!building_Bed.Spawned)
			{
				return false;
			}
			if (building_Bed.Map != sleeper.MapHeld)
			{
				return false;
			}
			if (building_Bed.IsBurning())
			{
				return false;
			}
			if (sleeper.HarmedByVacuum && building_Bed.Position.GetVacuum(bedThing.Map) >= 0.5f)
			{
				return false;
			}
			if (!RestUtility.CanUseBedEver(sleeper, building_Bed.def))
			{
				return false;
			}
			if (building_Bed.CompAssignableToPawn.IdeoligionForbids(sleeper))
			{
				return false;
			}
			int? num;
			bool flag = building_Bed.IsOwner(sleeper, out num);
			int? num2;
			bool flag2 = sleeper.CurrentBed(out num2) == building_Bed;
			if (!building_Bed.AnyUnoccupiedSleepingSlot && !flag && !flag2)
			{
				return false;
			}
			GuestStatus? guestStatus = guestStatusOverride;
			GuestStatus? guestStatus2 = (guestStatus != null) ? guestStatus : sleeper.GuestStatus;
			bool flag3 = guestStatus2.GetValueOrDefault() == GuestStatus.Prisoner;
			bool flag4 = guestStatus2.GetValueOrDefault() == GuestStatus.Slave;
			if (checkSocialProperness && !building_Bed.IsSociallyProper(sleeper, flag3, false))
			{
				return false;
			}
			if (building_Bed.ForPrisoners != flag3)
			{
				return false;
			}
			if (building_Bed.ForSlaves != flag4)
			{
				return false;
			}
			if (building_Bed.ForPrisoners && !building_Bed.Position.IsInPrisonCell(building_Bed.Map))
			{
				return false;
			}
			if (building_Bed.Medical)
			{
				if (!allowMedBedEvenIfSetToNoCare && !HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(sleeper))
				{
					return false;
				}
				if (!HealthAIUtility.ShouldSeekMedicalRest(sleeper))
				{
					return false;
				}
			}
			else
			{
				if (!flag && !RestUtility.BedOwnerWillShare(building_Bed, sleeper, guestStatusOverride))
				{
					return false;
				}
				if (flag2)
				{
					int? num3 = num2;
					int? num4 = num;
					if (!(num3.GetValueOrDefault() == num4.GetValueOrDefault() & num3 != null == (num4 != null)))
					{
						return false;
					}
				}
			}
			if (sleeper.IsColonist && !flag3)
			{
				Job curJob = sleeper.CurJob;
				if ((curJob == null || !curJob.ignoreForbidden) && !sleeper.Downed && building_Bed.IsForbidden(sleeper))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005E8C RID: 24204 RVA: 0x00229010 File Offset: 0x00227210
		public static bool IsValidBedFor(Thing bedThing, Pawn sleeper, Pawn traveler, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false, bool ignoreOtherReservations = false, GuestStatus? guestStatus = null)
		{
			if (!RestUtility.CanUseBedNow(bedThing, sleeper, checkSocialProperness, allowMedBedEvenIfSetToNoCare, guestStatus))
			{
				return false;
			}
			Building_Bed building_Bed = (Building_Bed)bedThing;
			if (!traveler.CanReach(building_Bed, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
			{
				return false;
			}
			if (!sleeper.HasReserved(building_Bed, null) && !traveler.CanReserve(building_Bed, building_Bed.SleepingSlotsCount, 0, null, ignoreOtherReservations))
			{
				return false;
			}
			if (traveler.HasReserved(building_Bed, new LocalTargetInfo?(sleeper), null, null))
			{
				return false;
			}
			if (building_Bed.IsForbidden(traveler))
			{
				return false;
			}
			bool flag = guestStatus.GetValueOrDefault() == GuestStatus.Prisoner;
			bool flag2 = guestStatus.GetValueOrDefault() == GuestStatus.Slave;
			return (flag || flag2 || building_Bed.Faction == traveler.Faction || (traveler.HostFaction != null && building_Bed.Faction == traveler.HostFaction)) && (!ModsConfig.AnomalyActive || !sleeper.IsMutant || sleeper.needs.rest != null || !sleeper.mutant.Def.entitledToMedicalCare || building_Bed.Medical);
		}

		// Token: 0x06005E8D RID: 24205 RVA: 0x00229124 File Offset: 0x00227324
		public static void TuckIntoBed(Building_Bed bed, Pawn taker, Pawn takee, bool rescued)
		{
			IntVec3 position = bed.Position;
			if (taker != takee)
			{
				Thing thing;
				taker.carryTracker.TryDropCarriedThing(position, ThingPlaceMode.Direct, out thing, null);
			}
			if (RestUtility.CanUseBedNow(bed, takee, false, false, null))
			{
				takee.jobs.Notify_TuckedIntoBed(bed);
				if (taker != takee && rescued)
				{
					takee.relations.Notify_RescuedBy(taker);
				}
				takee.mindState.Notify_TuckedIntoBed();
			}
			if (takee.IsPrisonerOfColony)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.PrisonerTab, takee, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06005E8E RID: 24206 RVA: 0x002291A4 File Offset: 0x002273A4
		private static bool IsAnyOwnerLovePartnerOf(Building_Bed bed, Pawn sleeper)
		{
			for (int i = 0; i < bed.OwnersForReading.Count; i++)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(sleeper, bed.OwnersForReading[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005E8F RID: 24207 RVA: 0x002291DE File Offset: 0x002273DE
		public static Building_Bed FindBedFor(Pawn p)
		{
			return RestUtility.FindBedFor(p, p, true, false, p.GuestStatus);
		}

		// Token: 0x06005E90 RID: 24208 RVA: 0x002291F0 File Offset: 0x002273F0
		public static Building_Bed FindBedFor(Pawn sleeper, Pawn traveler, bool checkSocialProperness, bool ignoreOtherReservations = false, GuestStatus? guestStatus = null)
		{
			RestUtility.<>c__DisplayClass14_0 CS$<>8__locals1 = new RestUtility.<>c__DisplayClass14_0();
			CS$<>8__locals1.sleeper = sleeper;
			CS$<>8__locals1.traveler = traveler;
			CS$<>8__locals1.checkSocialProperness = checkSocialProperness;
			CS$<>8__locals1.ignoreOtherReservations = ignoreOtherReservations;
			CS$<>8__locals1.guestStatus = guestStatus;
			if (CS$<>8__locals1.sleeper.RaceProps.IsMechanoid)
			{
				return null;
			}
			if (ModsConfig.BiotechActive && CS$<>8__locals1.sleeper.Deathresting)
			{
				Building_Bed assignedDeathrestCasket = CS$<>8__locals1.sleeper.ownership.AssignedDeathrestCasket;
				if (assignedDeathrestCasket != null && RestUtility.IsValidBedFor(assignedDeathrestCasket, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, true, false, false, null))
				{
					CompDeathrestBindable compDeathrestBindable = assignedDeathrestCasket.TryGetComp<CompDeathrestBindable>();
					if (compDeathrestBindable != null && (compDeathrestBindable.BoundPawn == CS$<>8__locals1.sleeper || compDeathrestBindable.BoundPawn == null))
					{
						return assignedDeathrestCasket;
					}
				}
			}
			bool flag = false;
			if (CS$<>8__locals1.sleeper.Ideo != null)
			{
				using (List<Precept>.Enumerator enumerator = CS$<>8__locals1.sleeper.Ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.prefersSlabBed)
						{
							flag = true;
							break;
						}
					}
				}
			}
			List<ThingDef> list = flag ? RestUtility.bedDefsBestToWorst_SlabBed_Medical : RestUtility.bedDefsBestToWorst_Medical;
			List<ThingDef> list2 = flag ? RestUtility.bedDefsBestToWorst_SlabBed_RestEffectiveness : RestUtility.bedDefsBestToWorst_RestEffectiveness;
			if (HealthAIUtility.ShouldSeekMedicalRest(CS$<>8__locals1.sleeper))
			{
				if (CS$<>8__locals1.sleeper.InBed() && CS$<>8__locals1.sleeper.CurrentBed().Medical && RestUtility.IsValidBedFor(CS$<>8__locals1.sleeper.CurrentBed(), CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus))
				{
					return CS$<>8__locals1.sleeper.CurrentBed();
				}
				for (int i = 0; i < list.Count; i++)
				{
					ThingDef thingDef = list[i];
					if (RestUtility.CanUseBedEver(CS$<>8__locals1.sleeper, thingDef))
					{
						for (int j = 0; j < 2; j++)
						{
							Danger maxDanger = (j == 0) ? Danger.None : Danger.Deadly;
							Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(CS$<>8__locals1.sleeper.Position, CS$<>8__locals1.sleeper.MapHeld, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing b) => ((Building_Bed)b).Medical && b.Position.GetDangerFor(CS$<>8__locals1.sleeper, CS$<>8__locals1.sleeper.Map) <= maxDanger && RestUtility.IsValidBedFor(b, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus), null, 0, -1, false, RegionType.Set_Passable, false, false);
							if (building_Bed != null)
							{
								return building_Bed;
							}
						}
					}
				}
			}
			if (CS$<>8__locals1.sleeper.RaceProps.Dryad)
			{
				return null;
			}
			if (CS$<>8__locals1.sleeper.ownership != null && CS$<>8__locals1.sleeper.ownership.OwnedBed != null && RestUtility.IsValidBedFor(CS$<>8__locals1.sleeper.ownership.OwnedBed, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus))
			{
				return CS$<>8__locals1.sleeper.ownership.OwnedBed;
			}
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(CS$<>8__locals1.sleeper, false);
			if (directPawnRelation != null)
			{
				Building_Bed ownedBed = directPawnRelation.otherPawn.ownership.OwnedBed;
				if (ownedBed != null && RestUtility.IsValidBedFor(ownedBed, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus))
				{
					return ownedBed;
				}
			}
			int dg;
			int dg2;
			for (dg = 0; dg < 3; dg = dg2 + 1)
			{
				Danger maxDanger = (dg <= 1) ? Danger.None : Danger.Deadly;
				Predicate<Thing> <>9__1;
				for (int k = 0; k < list2.Count; k++)
				{
					ThingDef thingDef2 = list2[k];
					if (RestUtility.CanUseBedEver(CS$<>8__locals1.sleeper, thingDef2))
					{
						IntVec3 positionHeld = CS$<>8__locals1.sleeper.PositionHeld;
						Map mapHeld = CS$<>8__locals1.sleeper.MapHeld;
						ThingRequest thingReq = ThingRequest.ForDef(thingDef2);
						PathEndMode peMode = PathEndMode.OnCell;
						TraverseParms traverseParams = TraverseParms.For(CS$<>8__locals1.traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
						float maxDistance = 9999f;
						Predicate<Thing> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = delegate(Thing b)
							{
								if (((Building_Bed)b).Medical || b.Position.GetDangerFor(CS$<>8__locals1.sleeper, CS$<>8__locals1.sleeper.MapHeld) > maxDanger || !RestUtility.IsValidBedFor(b, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus))
								{
									return false;
								}
								if (dg <= 0)
								{
									return !b.Position.GetItems(b.Map).Any((Thing thing) => thing.def.IsCorpse);
								}
								return true;
							});
						}
						Building_Bed building_Bed2 = (Building_Bed)GenClosest.ClosestThingReachable(positionHeld, mapHeld, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
						if (building_Bed2 != null)
						{
							return building_Bed2;
						}
					}
				}
				dg2 = dg;
			}
			return null;
		}

		// Token: 0x06005E91 RID: 24209 RVA: 0x00229688 File Offset: 0x00227888
		public static Building_Bed FindPatientBedFor(Pawn pawn)
		{
			Predicate<Thing> medBedValidator = delegate(Thing t)
			{
				Building_Bed building_Bed2 = t as Building_Bed;
				return building_Bed2 != null && building_Bed2.Medical && RestUtility.IsValidBedFor(building_Bed2, pawn, pawn, false, true, false, pawn.GuestStatus);
			};
			if (pawn.InBed() && medBedValidator(pawn.CurrentBed()))
			{
				return pawn.CurrentBed();
			}
			for (int i = 0; i < 2; i++)
			{
				Danger maxDanger = (i == 0) ? Danger.None : Danger.Deadly;
				Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Bed), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing b) => b.Position.GetDangerFor(pawn, pawn.Map) <= maxDanger && medBedValidator(b), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (building_Bed != null)
				{
					return building_Bed;
				}
			}
			return RestUtility.FindBedFor(pawn);
		}

		// Token: 0x06005E92 RID: 24210 RVA: 0x00229784 File Offset: 0x00227984
		public static IntVec3 GetBedSleepingSlotPosFor(Pawn pawn, Building_Bed bed)
		{
			int? num;
			if (bed.IsOwner(pawn, out num))
			{
				return bed.GetSleepingSlotPos(num.Value);
			}
			for (int i = 0; i < bed.SleepingSlotsCount; i++)
			{
				Pawn curOccupant = bed.GetCurOccupant(i);
				if ((i >= bed.OwnersForReading.Count || bed.OwnersForReading[i] == null) && curOccupant == pawn)
				{
					return bed.GetSleepingSlotPos(i);
				}
			}
			for (int j = 0; j < bed.SleepingSlotsCount; j++)
			{
				Pawn curOccupant2 = bed.GetCurOccupant(j);
				if ((j >= bed.OwnersForReading.Count || bed.OwnersForReading[j] == null) && curOccupant2 == null)
				{
					return bed.GetSleepingSlotPos(j);
				}
			}
			Log.Error("Could not find good sleeping slot position for " + ((pawn != null) ? pawn.ToString() : null) + ". Perhaps AnyUnoccupiedSleepingSlot check is missing somewhere.");
			return bed.GetSleepingSlotPos(0);
		}

		// Token: 0x06005E93 RID: 24211 RVA: 0x00229858 File Offset: 0x00227A58
		public static void KickOutOfBed(Pawn pawn, Building_Bed bed)
		{
			if (pawn == null)
			{
				return;
			}
			if (!pawn.Spawned)
			{
				Log.Error("Tried to kick unspawned pawn " + pawn.ToStringSafe<Pawn>() + " out of bed.");
			}
			if (!pawn.Dead && !pawn.GetPosture().InBed())
			{
				Log.Error("Tried to kick pawn " + pawn.ToStringSafe<Pawn>() + " out of bed when they weren't in bed.");
			}
			int? num;
			Building_Bed building_Bed = pawn.CurrentBed(out num);
			if (building_Bed != bed)
			{
				if (building_Bed == null)
				{
					bed = null;
				}
				else
				{
					Log.Error("Tried to kick pawn " + pawn.ToStringSafe<Pawn>() + " out of a bed they're not currently in.");
				}
			}
			pawn.jobs.posture &= ~PawnPosture.InBedMask;
			if (bed != null && (pawn.Downed || pawn.Deathresting))
			{
				pawn.Position = bed.GetFootSlotPos(num.Value);
			}
		}

		// Token: 0x06005E94 RID: 24212 RVA: 0x00229928 File Offset: 0x00227B28
		public static bool CanUseBedEver(Pawn p, ThingDef bedDef)
		{
			return !p.RaceProps.IsMechanoid && p.BodySize <= bedDef.building.bed_maxBodySize && p.RaceProps.Humanlike == bedDef.building.bed_humanlike && (!ModsConfig.BiotechActive || bedDef != ThingDefOf.DeathrestCasket || p.CanDeathrest());
		}

		// Token: 0x06005E95 RID: 24213 RVA: 0x00229990 File Offset: 0x00227B90
		public static bool TimetablePreventsLayDown(Pawn pawn)
		{
			Pawn_TimetableTracker timetable = pawn.timetable;
			if (((timetable != null) ? timetable.CurrentAssignment : null) != null && !pawn.timetable.CurrentAssignment.allowRest)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (((needs != null) ? needs.rest : null) != null && pawn.needs.rest.CurLevel >= 0.2f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005E96 RID: 24214 RVA: 0x002299F1 File Offset: 0x00227BF1
		public static bool DisturbancePreventsLyingDown(Pawn pawn)
		{
			return !pawn.Downed && Find.TickManager.TicksGame - pawn.mindState.lastDisturbanceTick < 400;
		}

		// Token: 0x06005E97 RID: 24215 RVA: 0x00229A1A File Offset: 0x00227C1A
		public static bool Awake(this Pawn p)
		{
			return p.health.capacities.CanBeAwake && (p.CurJob == null || p.jobs.curDriver == null || !p.jobs.curDriver.asleep);
		}

		// Token: 0x06005E98 RID: 24216 RVA: 0x00229A5A File Offset: 0x00227C5A
		public static bool IsSelfShutdown(this Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			return ((needs != null) ? needs.energy : null) != null && p.needs.energy.IsSelfShutdown;
		}

		// Token: 0x06005E99 RID: 24217 RVA: 0x00229A84 File Offset: 0x00227C84
		public static bool IsDeactivated(this Pawn p)
		{
			CompMechanoid compMechanoid = p.TryGetComp<CompMechanoid>();
			return compMechanoid != null && compMechanoid.Deactivated;
		}

		// Token: 0x06005E9A RID: 24218 RVA: 0x00229AA4 File Offset: 0x00227CA4
		public static bool IsActivityDormant(this Pawn p)
		{
			CompActivity activity = p.activity;
			return activity != null && activity.IsActive;
		}

		// Token: 0x06005E9B RID: 24219 RVA: 0x00229AC3 File Offset: 0x00227CC3
		public static bool IsCharging(this Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			object obj;
			if (needs == null)
			{
				obj = null;
			}
			else
			{
				Need_MechEnergy energy = needs.energy;
				obj = ((energy != null) ? energy.currentCharger : null);
			}
			return obj != null;
		}

		// Token: 0x06005E9C RID: 24220 RVA: 0x00229AE8 File Offset: 0x00227CE8
		public static Building_Bed CurrentBed(this Pawn p)
		{
			int? num;
			return p.CurrentBed(out num);
		}

		// Token: 0x06005E9D RID: 24221 RVA: 0x00229B00 File Offset: 0x00227D00
		public static Building_Bed CurrentBed(this Pawn p, out int? sleepingSlot)
		{
			sleepingSlot = null;
			if (!p.Spawned || p.CurJob == null || !p.GetPosture().InBed())
			{
				return null;
			}
			Building_Bed building_Bed = null;
			List<Thing> thingList = p.Position.GetThingList(p.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				building_Bed = (thingList[i] as Building_Bed);
				if (building_Bed != null)
				{
					break;
				}
			}
			if (building_Bed == null)
			{
				return null;
			}
			for (int j = 0; j < building_Bed.SleepingSlotsCount; j++)
			{
				if (building_Bed.GetCurOccupant(j) == p)
				{
					sleepingSlot = new int?(j);
					return building_Bed;
				}
			}
			return null;
		}

		// Token: 0x06005E9E RID: 24222 RVA: 0x00229B97 File Offset: 0x00227D97
		public static bool InBed(this Pawn p)
		{
			return p.CurrentBed() != null;
		}

		// Token: 0x06005E9F RID: 24223 RVA: 0x00229BA2 File Offset: 0x00227DA2
		public static bool IsLayingForJobCleanup(Pawn p)
		{
			return p.InBed() || (p.CurJob != null && p.CurJob.def == JobDefOf.LayDown && p.GetPosture().Laying());
		}

		// Token: 0x06005EA0 RID: 24224 RVA: 0x00229BD8 File Offset: 0x00227DD8
		public static void WakeUp(Pawn p, bool startNewJob = true)
		{
			if (p.CurJob != null && (p.GetPosture().Laying() || p.CurJobDef == JobDefOf.LayDown) && !p.Downed)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, startNewJob, true);
			}
			CompCanBeDormant comp = p.GetComp<CompCanBeDormant>();
			if (comp != null)
			{
				comp.WakeUp();
			}
			if (p.mindState != null)
			{
				p.mindState.hibernationEndedTick = GenTicks.TicksGame;
			}
		}

		// Token: 0x06005EA1 RID: 24225 RVA: 0x00229C48 File Offset: 0x00227E48
		public static bool ShouldWakeUp(Pawn pawn)
		{
			if (pawn.Deathresting)
			{
				return false;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			return ((needs != null) ? needs.rest : null) == null || pawn.needs.rest.CurLevel >= RestUtility.WakeThreshold(pawn) || pawn.health.hediffSet.HasHediffBlocksSleeping();
		}

		// Token: 0x06005EA2 RID: 24226 RVA: 0x00229CA0 File Offset: 0x00227EA0
		public static bool CanFallAsleep(Pawn pawn)
		{
			if (pawn.Deathresting)
			{
				return true;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			bool flag;
			if (needs == null)
			{
				flag = false;
			}
			else
			{
				Need_Food food = needs.food;
				flag = ((food != null) ? new bool?(food.Starving) : null).GetValueOrDefault();
			}
			if (flag && !pawn.ageTracker.CurLifeStage.canSleepWhenStarving)
			{
				return false;
			}
			if (pawn.mindState != null && Find.TickManager.TicksGame - pawn.mindState.lastDisturbanceTick < 400)
			{
				return false;
			}
			Pawn_NeedsTracker needs2 = pawn.needs;
			float? num;
			if (needs2 == null)
			{
				num = null;
			}
			else
			{
				Need_Rest rest = needs2.rest;
				num = ((rest != null) ? new float?(rest.CurLevel) : null);
			}
			float? num2 = num;
			float num3 = RestUtility.FallAsleepMaxLevel(pawn);
			if ((num2.GetValueOrDefault() < num3 & num2 != null) && !pawn.health.hediffSet.HasHediffBlocksSleeping())
			{
				JobDef curJobDef = pawn.CurJobDef;
				return curJobDef == null || curJobDef.sleepCanInterrupt;
			}
			return false;
		}

		// Token: 0x06005EA3 RID: 24227 RVA: 0x00229D9C File Offset: 0x00227F9C
		private static float WakeThreshold(Pawn p)
		{
			Lord lord = p.GetLord();
			if (lord != null && lord.CurLordToil != null && lord.CurLordToil.CustomWakeThreshold != null)
			{
				return lord.CurLordToil.CustomWakeThreshold.Value;
			}
			LifeStageDef curLifeStage = p.ageTracker.CurLifeStage;
			return ((curLifeStage != null) ? curLifeStage.naturalWakeThresholdOverride : null).GetValueOrDefault(1f);
		}

		// Token: 0x06005EA4 RID: 24228 RVA: 0x00229E10 File Offset: 0x00228010
		private static float FallAsleepMaxLevel(Pawn p)
		{
			LifeStageDef curLifeStage = p.ageTracker.CurLifeStage;
			return Mathf.Min(((curLifeStage != null) ? curLifeStage.fallAsleepMaxThresholdOverride : null).GetValueOrDefault(0.75f), RestUtility.WakeThreshold(p) - 0.01f);
		}

		// Token: 0x0400358E RID: 13710
		public const int NoSleepingDurationAfterBeingDisturbed = 400;

		// Token: 0x0400358F RID: 13711
		private static List<ThingDef> bedDefsBestToWorst_RestEffectiveness;

		// Token: 0x04003590 RID: 13712
		private static List<ThingDef> bedDefsBestToWorst_Medical;

		// Token: 0x04003591 RID: 13713
		private static List<ThingDef> bedDefsBestToWorst_SlabBed_RestEffectiveness;

		// Token: 0x04003592 RID: 13714
		private static List<ThingDef> bedDefsBestToWorst_SlabBed_Medical;
	}
}
