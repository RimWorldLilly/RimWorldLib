using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B50 RID: 6992
	public static class DropPodUtility
	{
		// Token: 0x0600B37C RID: 45948 RVA: 0x003F412C File Offset: 0x003F232C
		public static void MakeDropPodAt(IntVec3 c, Map map, ActiveTransporterInfo info, Faction faction = null)
		{
			ThingDef thingDef;
			if (info == null)
			{
				thingDef = null;
			}
			else
			{
				ThingDef sentTransporterDef = info.sentTransporterDef;
				thingDef = ((sentTransporterDef != null) ? sentTransporterDef.dropPodActive : null);
			}
			ThingDef def;
			if ((def = thingDef) == null)
			{
				def = (((faction != null) ? faction.def.dropPodActive : null) ?? ThingDefOf.ActiveDropPod);
			}
			ActiveTransporter activeTransporter = (ActiveTransporter)ThingMaker.MakeThing(def, null);
			activeTransporter.Contents = info;
			ThingDef thingDef2;
			if (info == null)
			{
				thingDef2 = null;
			}
			else
			{
				ThingDef sentTransporterDef2 = info.sentTransporterDef;
				thingDef2 = ((sentTransporterDef2 != null) ? sentTransporterDef2.dropPodFaller : null);
			}
			ThingDef skyfaller;
			if ((skyfaller = thingDef2) == null)
			{
				skyfaller = (((faction != null) ? faction.def.dropPodIncoming : null) ?? ThingDefOf.DropPodIncoming);
			}
			SkyfallerMaker.SpawnSkyfaller(skyfaller, activeTransporter, c, map);
			foreach (Thing thing in ((IEnumerable<Thing>)activeTransporter.Contents.innerContainer))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.IsWorldPawn())
				{
					Find.WorldPawns.RemovePawn(pawn);
					Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
					if (psychicEntropy != null)
					{
						psychicEntropy.SetInitialPsyfocusLevel();
					}
				}
			}
		}

		// Token: 0x0600B37D RID: 45949 RVA: 0x003F422C File Offset: 0x003F242C
		public static void DropThingsNear(IntVec3 dropCenter, Map map, IEnumerable<Thing> things, int openDelay = 110, bool canInstaDropDuringInit = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true, bool allowFogged = true, Faction faction = null)
		{
			DropPodUtility.tempList.Clear();
			foreach (Thing item in things)
			{
				List<Thing> list = new List<Thing>();
				list.Add(item);
				DropPodUtility.tempList.Add(list);
			}
			DropPodUtility.DropThingGroupsNear(dropCenter, map, DropPodUtility.tempList, openDelay, canInstaDropDuringInit, leaveSlag, canRoofPunch, forbid, allowFogged, false, faction);
			DropPodUtility.tempList.Clear();
		}

		// Token: 0x0600B37E RID: 45950 RVA: 0x003F42B4 File Offset: 0x003F24B4
		public static void DropThingGroupsNear(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int openDelay = 110, bool instaDrop = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true, bool allowFogged = true, bool canTransfer = false, Faction faction = null)
		{
			Predicate<IntVec3> <>9__0;
			foreach (List<Thing> list in thingsGroups)
			{
				IntVec3 intVec;
				if (!DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, canRoofPunch, true, null, true) && (canRoofPunch || !DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, true, true, null, true)))
				{
					if (!dropCenter.IsValid)
					{
						continue;
					}
					string[] array = new string[5];
					array[0] = "DropThingsNear failed to find a place to drop ";
					int num = 1;
					Thing thing = list.FirstOrDefault<Thing>();
					array[num] = ((thing != null) ? thing.ToString() : null);
					array[2] = " near ";
					int num2 = 3;
					IntVec3 intVec2 = dropCenter;
					array[num2] = intVec2.ToString();
					array[4] = ". Dropping on random square instead.";
					Log.Warning(string.Concat(array));
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = delegate(IntVec3 c)
						{
							if (c.Walkable(map))
							{
								RoofDef roof = c.GetRoof(map);
								return roof == null || !roof.isThickRoof;
							}
							return false;
						});
					}
					intVec = CellFinderLoose.RandomCellWith(validator, map, 1000);
				}
				if (forbid)
				{
					for (int i = 0; i < list.Count; i++)
					{
						list[i].SetForbidden(true, false);
					}
				}
				if (instaDrop)
				{
					using (List<Thing>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Thing thing2 = enumerator2.Current;
							GenPlace.TryPlaceThing(thing2, intVec, map, ThingPlaceMode.Near, null, null, null, 1);
						}
						continue;
					}
				}
				ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
				foreach (Thing item in list)
				{
					activeTransporterInfo.innerContainer.TryAdd(item, true);
				}
				activeTransporterInfo.openDelay = openDelay;
				activeTransporterInfo.leaveSlag = leaveSlag;
				DropPodUtility.MakeDropPodAt(intVec, map, activeTransporterInfo, faction);
			}
		}

		// Token: 0x0600B37F RID: 45951 RVA: 0x003F44F8 File Offset: 0x003F26F8
		public static bool IsShuttle(this List<ActiveTransporterInfo> transporters)
		{
			return transporters.Count == 1 && transporters[0].GetShuttle() != null;
		}

		// Token: 0x0600B380 RID: 45952 RVA: 0x003F4514 File Offset: 0x003F2714
		public static bool IsShuttle(this List<CompTransporter> transporters)
		{
			return transporters.Count == 1 && transporters[0].Shuttle != null;
		}

		// Token: 0x0600B381 RID: 45953 RVA: 0x003F4530 File Offset: 0x003F2730
		public static CompShuttle AsShuttle(this List<CompTransporter> transporters)
		{
			if (transporters.Count != 1)
			{
				return null;
			}
			return transporters[0].Shuttle;
		}

		// Token: 0x0400678A RID: 26506
		private static readonly List<List<Thing>> tempList = new List<List<Thing>>();
	}
}
