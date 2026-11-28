using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B89 RID: 2953
	public class MeditationUtility
	{
		// Token: 0x060055A4 RID: 21924 RVA: 0x001FE090 File Offset: 0x001FC290
		public static Job GetMeditationJob(Pawn pawn, bool forJoy = false)
		{
			MeditationSpotAndFocus meditationSpotAndFocus = MeditationUtility.FindMeditationSpot(pawn);
			if (meditationSpotAndFocus.IsValid && pawn.CanReserveAndReach(meditationSpotAndFocus.spot, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				Building_Throne building_Throne = meditationSpotAndFocus.focus.Thing as Building_Throne;
				Job job;
				if (building_Throne != null)
				{
					job = JobMaker.MakeJob(JobDefOf.Reign, building_Throne, null, building_Throne);
				}
				else
				{
					JobDef def = JobDefOf.Meditate;
					if (forJoy && ModsConfig.IdeologyActive && pawn.Ideo != null)
					{
						IdeoFoundation_Deity ideoFoundation_Deity = pawn.Ideo.foundation as IdeoFoundation_Deity;
						if (ideoFoundation_Deity != null && ideoFoundation_Deity.DeitiesListForReading.Any<IdeoFoundation_Deity.Deity>())
						{
							def = JobDefOf.MeditatePray;
						}
					}
					job = JobMaker.MakeJob(def, meditationSpotAndFocus.spot, null, meditationSpotAndFocus.focus);
				}
				job.ignoreJoyTimeAssignment = !forJoy;
				return job;
			}
			return null;
		}

		// Token: 0x060055A5 RID: 21925 RVA: 0x001FE168 File Offset: 0x001FC368
		public static MeditationSpotAndFocus FindMeditationSpot(Pawn pawn)
		{
			float num = float.MinValue;
			LocalTargetInfo spot = LocalTargetInfo.Invalid;
			LocalTargetInfo focus = LocalTargetInfo.Invalid;
			if (!ModLister.CheckRoyalty("Psyfocus"))
			{
				return new MeditationSpotAndFocus(spot, focus);
			}
			Room ownedRoom = pawn.ownership.OwnedRoom;
			foreach (LocalTargetInfo localTargetInfo in MeditationUtility.AllMeditationSpotCandidates(pawn, true))
			{
				if (MeditationUtility.SafeEnvironmentalConditions(pawn, localTargetInfo.Cell, pawn.Map) && localTargetInfo.Cell.Standable(pawn.Map) && !localTargetInfo.Cell.IsForbidden(pawn))
				{
					float num2 = 1f / Mathf.Max((float)localTargetInfo.Cell.DistanceToSquared(pawn.Position), 0.1f);
					LocalTargetInfo localTargetInfo2 = (localTargetInfo.Thing is Building_Throne) ? localTargetInfo.Thing : MeditationUtility.BestFocusAt(localTargetInfo, pawn);
					if (pawn.HasPsylink && localTargetInfo2.IsValid)
					{
						num2 += localTargetInfo2.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) * 100f;
					}
					Room room = localTargetInfo.Cell.GetRoom(pawn.Map);
					if (room != null && ownedRoom == room)
					{
						num2 += 1f;
					}
					if (localTargetInfo.Thing != null)
					{
						Building building = localTargetInfo.Thing as Building;
						if (building != null && building.GetAssignedPawn() == pawn)
						{
							num2 += (float)((building.def == ThingDefOf.MeditationSpot) ? 200 : 100);
						}
					}
					if (room != null && ModsConfig.IdeologyActive && room.Role == RoomRoleDefOf.WorshipRoom)
					{
						num2 += 100f;
						foreach (Thing thing in room.ContainedAndAdjacentThings)
						{
							num2 += thing.GetStatValue(StatDefOf.StyleDominance, true, -1);
						}
					}
					if (num2 > num)
					{
						spot = localTargetInfo;
						focus = localTargetInfo2;
						num = num2;
					}
				}
			}
			return new MeditationSpotAndFocus(spot, focus);
		}

		// Token: 0x060055A6 RID: 21926 RVA: 0x001FE3B4 File Offset: 0x001FC5B4
		public static bool IsValidMeditationBuildingForPawn(Building spot, Pawn pawn)
		{
			if (spot.DestroyedOrNull() || !spot.Spawned || spot.IsForbidden(pawn) || !spot.Position.Standable(spot.Map))
			{
				return false;
			}
			if (spot.GetAssignedPawn() != null && spot.GetAssignedPawn() != pawn)
			{
				return false;
			}
			Room room = spot.GetRoom(RegionType.Set_All);
			return (room == null || MeditationUtility.CanUseRoomToMeditate(room, pawn)) && (!(spot is Building_Throne) || RoomRoleWorker_ThroneRoom.Validate(room) == null) && pawn.CanReserveAndReach(spot, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false);
		}

		// Token: 0x060055A7 RID: 21927 RVA: 0x001FE442 File Offset: 0x001FC642
		public static IEnumerable<LocalTargetInfo> AllMeditationSpotCandidates(Pawn pawn, bool allowFallbackSpots = true)
		{
			MeditationUtility.<AllMeditationSpotCandidates>d__6 <AllMeditationSpotCandidates>d__ = new MeditationUtility.<AllMeditationSpotCandidates>d__6(-2);
			<AllMeditationSpotCandidates>d__.<>3__pawn = pawn;
			<AllMeditationSpotCandidates>d__.<>3__allowFallbackSpots = allowFallbackSpots;
			return <AllMeditationSpotCandidates>d__;
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x001FE459 File Offset: 0x001FC659
		public static IEnumerable<Room> UsableWorshipRooms(Pawn pawn)
		{
			MeditationUtility.<UsableWorshipRooms>d__7 <UsableWorshipRooms>d__ = new MeditationUtility.<UsableWorshipRooms>d__7(-2);
			<UsableWorshipRooms>d__.<>3__pawn = pawn;
			return <UsableWorshipRooms>d__;
		}

		// Token: 0x060055A9 RID: 21929 RVA: 0x001FE46C File Offset: 0x001FC66C
		public static bool SafeEnvironmentalConditions(Pawn pawn, IntVec3 cell, Map map)
		{
			return (!map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) || cell.Roofed(map)) && (!ModsConfig.BiotechActive || !NoxiousHazeUtility.IsExposedToNoxiousHaze(pawn, cell, map)) && cell.GetDangerFor(pawn, map) == Danger.None && !cell.VacuumConcernTo(pawn);
		}

		// Token: 0x060055AA RID: 21930 RVA: 0x001FE4C4 File Offset: 0x001FC6C4
		public static bool CanMeditateNow(Pawn pawn)
		{
			if (pawn.needs.rest != null && pawn.needs.rest.CurCategory >= RestCategory.VeryTired)
			{
				return false;
			}
			if (pawn.needs.food != null && pawn.needs.food.Starving)
			{
				return false;
			}
			if (pawn.Downed || !pawn.Awake())
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal <= 0f)
			{
				if (HealthAIUtility.ShouldSeekMedicalRest(pawn))
				{
					Pawn_TimetableTracker timetable = pawn.timetable;
					if (((timetable != null) ? timetable.CurrentAssignment : null) != TimeAssignmentDefOf.Meditate)
					{
						return false;
					}
				}
				if (!HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060055AB RID: 21931 RVA: 0x001FE569 File Offset: 0x001FC769
		public static bool CanOnlyMeditateInBed(Pawn pawn)
		{
			return pawn.Downed;
		}

		// Token: 0x060055AC RID: 21932 RVA: 0x001FE571 File Offset: 0x001FC771
		public static bool ShouldMeditateInBed(Pawn pawn)
		{
			return pawn.Downed || pawn.health.hediffSet.AnyHediffMakesSickThought;
		}

		// Token: 0x060055AD RID: 21933 RVA: 0x001FE594 File Offset: 0x001FC794
		public static LocalTargetInfo BestFocusAt(LocalTargetInfo spot, Pawn pawn)
		{
			float num = 0f;
			LocalTargetInfo result = LocalTargetInfo.Invalid;
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(spot.Cell, pawn.MapHeld, MeditationUtility.FocusObjectSearchRadius, false))
			{
				if (GenSight.LineOfSightToThing(spot.Cell, thing, pawn.Map, false, null) && !(thing is Building_Throne))
				{
					CompMeditationFocus compMeditationFocus = thing.TryGetComp<CompMeditationFocus>();
					if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn))
					{
						float statValueForPawn = thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true);
						if (statValueForPawn > num)
						{
							result = thing;
							num = statValueForPawn;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060055AE RID: 21934 RVA: 0x001FE64C File Offset: 0x001FC84C
		public static IEnumerable<LocalTargetInfo> FocusSpotsInTheRoom(Pawn pawn, Room r)
		{
			MeditationUtility.<FocusSpotsInTheRoom>d__13 <FocusSpotsInTheRoom>d__ = new MeditationUtility.<FocusSpotsInTheRoom>d__13(-2);
			<FocusSpotsInTheRoom>d__.<>3__pawn = pawn;
			<FocusSpotsInTheRoom>d__.<>3__r = r;
			return <FocusSpotsInTheRoom>d__;
		}

		// Token: 0x060055AF RID: 21935 RVA: 0x001FE664 File Offset: 0x001FC864
		public static LocalTargetInfo MeditationSpotForFocus(Thing t, Pawn p, Func<IntVec3, bool> validator = null)
		{
			return (from cell in t.OccupiedRect().ExpandedBy(2).AdjacentCellsCardinal
			where cell.InBounds(t.Map) && (validator == null || validator(cell)) && !cell.IsForbidden(p) && p.CanReserveAndReach(cell, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false) && cell.Standable(p.Map)
			select cell).RandomElementWithFallback(IntVec3.Invalid);
		}

		// Token: 0x060055B0 RID: 21936 RVA: 0x001FE6C8 File Offset: 0x001FC8C8
		public static IEnumerable<MeditationFocusDef> FocusTypesAvailableForPawn(Pawn pawn)
		{
			MeditationUtility.<FocusTypesAvailableForPawn>d__15 <FocusTypesAvailableForPawn>d__ = new MeditationUtility.<FocusTypesAvailableForPawn>d__15(-2);
			<FocusTypesAvailableForPawn>d__.<>3__pawn = pawn;
			return <FocusTypesAvailableForPawn>d__;
		}

		// Token: 0x060055B1 RID: 21937 RVA: 0x001FE6D8 File Offset: 0x001FC8D8
		public static string FocusTypesAvailableForPawnString(Pawn pawn)
		{
			return (from f in MeditationUtility.FocusTypesAvailableForPawn(pawn)
			select f.label).ToCommaList(false, false);
		}

		// Token: 0x060055B2 RID: 21938 RVA: 0x001FE70B File Offset: 0x001FC90B
		public static IEnumerable<Dialog_InfoCard.Hyperlink> FocusObjectsForPawnHyperlinks(Pawn pawn)
		{
			MeditationUtility.<FocusObjectsForPawnHyperlinks>d__18 <FocusObjectsForPawnHyperlinks>d__ = new MeditationUtility.<FocusObjectsForPawnHyperlinks>d__18(-2);
			<FocusObjectsForPawnHyperlinks>d__.<>3__pawn = pawn;
			return <FocusObjectsForPawnHyperlinks>d__;
		}

		// Token: 0x060055B3 RID: 21939 RVA: 0x001FE71C File Offset: 0x001FC91C
		public static string FocusTypeAvailableExplanation(Pawn pawn)
		{
			string text = "";
			for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i++)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					text = string.Concat(new string[]
					{
						text,
						"MeditationFocusCanUse".Translate(meditationFocusDef.label).Resolve(),
						":\n",
						meditationFocusDef.EnablingThingsExplanation(pawn),
						"\n\n"
					});
					if (!MeditationUtility.focusObjectsPerTypeCache.ContainsKey(meditationFocusDef))
					{
						List<string> list = new List<string>();
						foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
						{
							CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
							if (compProperties != null && compProperties.focusTypes.Contains(meditationFocusDef))
							{
								list.AddUnique(thingDef.label);
							}
						}
						MeditationUtility.focusObjectsPerTypeCache[meditationFocusDef] = list.ToLineList("  - ", true);
					}
					text += "MeditationFocusObjects".Translate(meditationFocusDef.label).CapitalizeFirst() + ":\n" + MeditationUtility.focusObjectsPerTypeCache[meditationFocusDef] + "\n\n";
				}
			}
			return text;
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x001FE890 File Offset: 0x001FCA90
		public static void DrawMeditationSpotOverlay(IntVec3 center, Map map)
		{
			GenDraw.DrawRadiusRing(center, MeditationUtility.FocusObjectSearchRadius);
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(center, map, MeditationUtility.FocusObjectSearchRadius, false))
			{
				if (!(thing is Building_Throne) && (thing.def.building == null || !thing.def.building.supportsWallAttachments) && thing.TryGetComp<CompMeditationFocus>() != null && GenSight.LineOfSight(center, thing.Position, map))
				{
					GenDraw.DrawLineBetween(center.ToVector3() + new Vector3(0.5f, 0f, 0.5f), thing.TrueCenter(), SimpleColor.White, 0.2f);
				}
			}
		}

		// Token: 0x060055B5 RID: 21941 RVA: 0x001FE958 File Offset: 0x001FCB58
		public static bool CanUseRoomToMeditate(Room r, Pawn p)
		{
			return (r.Owners.EnumerableNullOrEmpty<Pawn>() || r.Owners.Contains(p)) && (!r.IsPrisonCell || p.IsPrisoner);
		}

		// Token: 0x060055B6 RID: 21942 RVA: 0x001FE98A File Offset: 0x001FCB8A
		public static IEnumerable<Thing> GetMeditationFociAffectedByBuilding(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			MeditationUtility.<GetMeditationFociAffectedByBuilding>d__23 <GetMeditationFociAffectedByBuilding>d__ = new MeditationUtility.<GetMeditationFociAffectedByBuilding>d__23(-2);
			<GetMeditationFociAffectedByBuilding>d__.<>3__map = map;
			<GetMeditationFociAffectedByBuilding>d__.<>3__def = def;
			<GetMeditationFociAffectedByBuilding>d__.<>3__faction = faction;
			<GetMeditationFociAffectedByBuilding>d__.<>3__pos = pos;
			<GetMeditationFociAffectedByBuilding>d__.<>3__rotation = rotation;
			return <GetMeditationFociAffectedByBuilding>d__;
		}

		// Token: 0x060055B7 RID: 21943 RVA: 0x001FE9B8 File Offset: 0x001FCBB8
		public static void DrawMeditationFociAffectedByBuildingOverlay(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			int num = 0;
			foreach (Thing thing in MeditationUtility.GetMeditationFociAffectedByBuilding(map, def, faction, pos, rotation))
			{
				if (num++ > 10)
				{
					break;
				}
				CompToggleDrawAffectedMeditationFoci compToggleDrawAffectedMeditationFoci = thing.TryGetComp<CompToggleDrawAffectedMeditationFoci>();
				if (compToggleDrawAffectedMeditationFoci == null || compToggleDrawAffectedMeditationFoci.Enabled)
				{
					GenAdj.OccupiedRect(pos, rotation, def.size);
					GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), thing.TrueCenter(), SimpleColor.Red, 0.2f);
				}
			}
		}

		// Token: 0x060055B8 RID: 21944 RVA: 0x001FEA58 File Offset: 0x001FCC58
		public static bool CountsAsArtificialBuilding(ThingDef def, Faction faction)
		{
			return (def.category == ThingCategory.Building || def.thingCategories.NotNullAndContains(ThingCategoryDefOf.BuildingsSpecial)) && faction != null && def.building.artificialForMeditationPurposes;
		}

		// Token: 0x060055B9 RID: 21945 RVA: 0x001FEA85 File Offset: 0x001FCC85
		public static bool CountsAsArtificialBuilding(Thing t)
		{
			return MeditationUtility.CountsAsArtificialBuilding(t.def, t.Faction);
		}

		// Token: 0x060055BA RID: 21946 RVA: 0x001FEA98 File Offset: 0x001FCC98
		public static void DrawArtificialBuildingOverlay(IntVec3 pos, ThingDef def, Map map, float radius)
		{
			GenDraw.DrawRadiusRing(pos, radius, MeditationUtility.ArtificialBuildingRingColor, null);
			int num = 0;
			foreach (Thing t in map.listerArtificialBuildingsForMeditation.GetForCell(pos, radius))
			{
				if (num++ > 10)
				{
					break;
				}
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, Rot4.North, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red, 0.2f);
			}
		}

		// Token: 0x060055BB RID: 21947 RVA: 0x001FEB30 File Offset: 0x001FCD30
		public static float PsyfocusGainPerTick(Pawn pawn, Thing focus = null)
		{
			float num = pawn.GetStatValue(StatDefOf.MeditationFocusGain, true, -1);
			if (focus != null && !focus.Destroyed)
			{
				num += focus.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true);
			}
			return num / 60000f;
		}

		// Token: 0x060055BC RID: 21948 RVA: 0x001FEB70 File Offset: 0x001FCD70
		public static void CheckMeditationScheduleTeachOpportunity(Pawn pawn)
		{
			if (pawn.Dead || !pawn.Spawned || !pawn.HasPsylink)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer || pawn.IsQuestLodger())
			{
				return;
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationSchedule, pawn, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationDesiredPsyfocus, pawn, OpportunityType.GoodToKnow);
		}

		// Token: 0x040032ED RID: 13037
		public static float FocusObjectSearchRadius = 3.9f;

		// Token: 0x040032EE RID: 13038
		private static float WanderRadius = 10f;

		// Token: 0x040032EF RID: 13039
		public static readonly Color ArtificialBuildingRingColor = new Color(0.8f, 0.49f, 0.43f);

		// Token: 0x040032F0 RID: 13040
		private static Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>> focusObjectHyperlinksPerTypeCache = new Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>>();

		// Token: 0x040032F1 RID: 13041
		private static Dictionary<MeditationFocusDef, string> focusObjectsPerTypeCache = new Dictionary<MeditationFocusDef, string>();
	}
}
