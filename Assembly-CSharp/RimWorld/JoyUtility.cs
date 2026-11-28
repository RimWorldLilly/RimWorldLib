using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B87 RID: 2951
	public static class JoyUtility
	{
		// Token: 0x06005594 RID: 21908 RVA: 0x001FD4D8 File Offset: 0x001FB6D8
		public static bool EnjoyableOutsideNow(Map map, StringBuilder outFailReason = null)
		{
			if (map.weatherManager.RainRate >= 0.25f)
			{
				if (outFailReason != null)
				{
					outFailReason.Append(map.weatherManager.curWeather.label);
				}
				return false;
			}
			GameConditionDef gameConditionDef;
			if (!map.gameConditionManager.AllowEnjoyableOutsideNow(map, out gameConditionDef))
			{
				if (outFailReason != null)
				{
					outFailReason.Append(gameConditionDef.label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x001FD538 File Offset: 0x001FB738
		public static bool EnjoyableOutsideNow(Pawn pawn, StringBuilder outFailReason = null)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return true;
			}
			if (!JoyUtility.EnjoyableOutsideNow(mapHeld, outFailReason))
			{
				return false;
			}
			if (!pawn.ComfortableTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("NotEnjoyableOutsideTemperature".Translate());
				}
				return false;
			}
			if (mapHeld.Biome.inVacuum && pawn.ConcernedByVacuum)
			{
				if (outFailReason != null)
				{
					outFailReason.Append("Vacuum".Translate());
				}
				return false;
			}
			return true;
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x001FD5C4 File Offset: 0x001FB7C4
		public static bool JoyTickCheckEnd(Pawn pawn, int delta, JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.EndJob, float extraJoyGainFactor = 1f, Building joySource = null)
		{
			Job curJob = pawn.CurJob;
			if (curJob.def.joyKind == null)
			{
				Log.Warning("This method can only be called for jobs with joyKind.");
				return false;
			}
			if (joySource != null)
			{
				if (joySource.def.building.joyKind != null && pawn.CurJob.def.joyKind != joySource.def.building.joyKind)
				{
					Log.ErrorOnce("Joy source joyKind and jobDef.joyKind are not the same. building=" + joySource.ToStringSafe<Building>() + ", jobDef=" + pawn.CurJob.def.ToStringSafe<JobDef>(), joySource.thingIDNumber ^ 876598732);
				}
				extraJoyGainFactor *= joySource.GetStatValue(StatDefOf.JoyGainFactor, true, -1);
			}
			if (pawn.needs.joy == null && !curJob.doUntilGatheringEnded)
			{
				pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
				return false;
			}
			Need_Joy joy = pawn.needs.joy;
			if (joy != null)
			{
				joy.GainJoy(extraJoyGainFactor * curJob.def.joyGainRate * 0.36f / 2500f * (float)delta, curJob.def.joyKind);
			}
			if (curJob.def.joySkill != null)
			{
				pawn.skills.GetSkill(curJob.def.joySkill).Learn(curJob.def.joyXpPerTick * (float)delta, false, false);
			}
			if (!curJob.ignoreJoyTimeAssignment && !pawn.GetTimeAssignment().allowJoy && !curJob.doUntilGatheringEnded)
			{
				pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
				return true;
			}
			Need_Joy joy2 = pawn.needs.joy;
			if (joy2 != null && joy2.CurLevel > 0.9999f && !curJob.doUntilGatheringEnded)
			{
				if (fullJoyAction == JoyTickFullJoyAction.EndJob)
				{
					pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
					return true;
				}
				if (fullJoyAction == JoyTickFullJoyAction.GoToNextToil)
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x001FD79C File Offset: 0x001FB99C
		public static void TryGainRecRoomThought(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
				if (pawn.needs.mood != null && ThoughtDefOf.JoyActivityInImpressiveRecRoom.stages[scoreStageIndex] != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.JoyActivityInImpressiveRecRoom, scoreStageIndex), null);
				}
			}
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x001FD810 File Offset: 0x001FBA10
		public static bool LordPreventsGettingJoy(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds;
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x001FD837 File Offset: 0x001FBA37
		public static bool TimetablePreventsGettingJoy(Pawn pawn)
		{
			return !((pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment).allowJoy;
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x001FD860 File Offset: 0x001FBA60
		public static int JoyKindsOnMapCount(Map map)
		{
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(map);
			int count = list.Count;
			list.Clear();
			return count;
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x001FD880 File Offset: 0x001FBA80
		public static List<JoyKindDef> JoyKindsOnMapTempList(Map map)
		{
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					JoyUtility.tempKindList.Add(joyKindDef);
				}
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				if (building.def.building.joyKind != null && !JoyUtility.tempKindList.Contains(building.def.building.joyKind))
				{
					JoyUtility.tempKindList.Add(building.def.building.joyKind);
				}
			}
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing.def.IsIngestible && thing.def.ingestible.joyKind != null && !JoyUtility.tempKindList.Contains(thing.def.ingestible.joyKind) && !thing.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(thing.def.ingestible.joyKind);
				}
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null && !JoyUtility.tempKindList.Contains(thing2.def.ingestible.joyKind) && !thing2.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(thing2.def.ingestible.joyKind);
				}
			}
			JoyUtility.CheckForReadingRecreation(map);
			return JoyUtility.tempKindList;
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x001FDAB0 File Offset: 0x001FBCB0
		private static void CheckForReadingRecreation(Map map)
		{
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Book))
			{
				if (!JoyUtility.tempKindList.Contains(JoyKindDefOf.Reading) && !thing.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(JoyKindDefOf.Reading);
					return;
				}
			}
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				Pawn_CarryTracker carryTracker = pawn.carryTracker;
				if (((carryTracker != null) ? carryTracker.CarriedThing : null) is Book)
				{
					JoyUtility.tempKindList.Add(JoyKindDefOf.Reading);
					return;
				}
			}
			foreach (Building_Bookcase building_Bookcase in map.listerThings.GetThingsOfType<Building_Bookcase>())
			{
				if (building_Bookcase.HeldBooks.Any<Book>() && !JoyUtility.tempKindList.Contains(JoyKindDefOf.Reading) && !building_Bookcase.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(JoyKindDefOf.Reading);
					break;
				}
			}
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x001FDC1C File Offset: 0x001FBE1C
		public static string JoyKindsOnMapString(Map map)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, null, joyKindDef, map);
				}
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				if (building.def.building.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, building, building.def.building.joyKind, map);
				}
			}
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing.def.IsIngestible && thing.def.ingestible.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, thing, thing.def.ingestible.joyKind, map);
				}
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, thing2, thing2.def.ingestible.joyKind, map);
				}
			}
			bool flag = false;
			using (List<Thing>.Enumerator enumerator2 = map.listerThings.ThingsInGroup(ThingRequestGroup.Book).GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Thing thing3 = enumerator2.Current;
					JoyUtility.CheckAppendJoyKind(stringBuilder, null, JoyKindDefOf.Reading, map);
					flag = true;
				}
			}
			if (!flag)
			{
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					Pawn_CarryTracker carryTracker = pawn.carryTracker;
					if (((carryTracker != null) ? carryTracker.CarriedThing : null) is Book)
					{
						JoyUtility.CheckAppendJoyKind(stringBuilder, null, JoyKindDefOf.Reading, map);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				using (IEnumerator<Building_Bookcase> enumerator4 = map.listerBuildings.AllColonistBuildingsOfType<Building_Bookcase>().GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						if (enumerator4.Current.HeldBooks.Any<Book>())
						{
							JoyUtility.CheckAppendJoyKind(stringBuilder, null, JoyKindDefOf.Reading, map);
							break;
						}
					}
				}
			}
			JoyUtility.listedJoyKinds.Clear();
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x001FDF04 File Offset: 0x001FC104
		private static void CheckAppendJoyKind(StringBuilder sb, Thing t, JoyKindDef kind, Map map)
		{
			if (JoyUtility.listedJoyKinds.Contains(kind))
			{
				return;
			}
			sb.Append("  - ");
			if (t == null)
			{
				sb.AppendLine(kind.LabelCap);
			}
			else
			{
				if (t.def.category == ThingCategory.Item && t.Position.Fogged(map))
				{
					return;
				}
				sb.AppendLine(kind.LabelCap + " (" + t.def.label + ")");
			}
			JoyUtility.listedJoyKinds.Add(kind);
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x001FDFA0 File Offset: 0x001FC1A0
		public static string JoyKindsNotOnMapString(Map map)
		{
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(map);
			if (allDefsListForReading.Count == list.Count)
			{
				return "(" + "None".Translate() + ")";
			}
			string text = "";
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				if (!list.Contains(joyKindDef))
				{
					text += "  - " + joyKindDef.LabelCap + "\n";
				}
			}
			list.Clear();
			return text.TrimEndNewlines();
		}

		// Token: 0x040032E9 RID: 13033
		private static readonly List<JoyKindDef> tempKindList = new List<JoyKindDef>();

		// Token: 0x040032EA RID: 13034
		private static readonly List<JoyKindDef> listedJoyKinds = new List<JoyKindDef>();
	}
}
