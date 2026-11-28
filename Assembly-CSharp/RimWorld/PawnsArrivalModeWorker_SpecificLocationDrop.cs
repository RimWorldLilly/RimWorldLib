using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EB RID: 5355
	public class PawnsArrivalModeWorker_SpecificLocationDrop : PawnsArrivalModeWorker
	{
		// Token: 0x060082DA RID: 33498 RVA: 0x002EF5A0 File Offset: 0x002ED7A0
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool flag = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			List<List<Thing>> thingsGroups = (from p in pawns
			select new List<Thing>
			{
				p
			}).ToList<List<Thing>>();
			PawnsArrivalModeWorker_SpecificLocationDrop.DropThingGroupsNear(parms.spawnCenter, map, thingsGroups, parms.dropInRadius, parms.podOpenDelay, false, true, flag || parms.raidArrivalModeForQuickMilitaryAid, true, true, false, parms.faction);
		}

		// Token: 0x060082DB RID: 33499 RVA: 0x002EF630 File Offset: 0x002ED830
		public override void TravellingTransportersArrived(List<ActiveTransporterInfo> transporters, Map map)
		{
			throw new NotImplementedException("PawnsArrivalModeWorker_SpecificLocationDrop is only for raids");
		}

		// Token: 0x060082DC RID: 33500 RVA: 0x002EF63C File Offset: 0x002ED83C
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.raidArrivalModeForQuickMilitaryAid)
			{
				parms.podOpenDelay = 520;
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}

		// Token: 0x060082DD RID: 33501 RVA: 0x002EF66C File Offset: 0x002ED86C
		private static void DropThingGroupsNear(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int radius, int openDelay = 110, bool instaDrop = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true, bool allowFogged = true, bool canTransfer = false, Faction faction = null)
		{
			Predicate<IntVec3> <>9__0;
			foreach (List<Thing> list in thingsGroups)
			{
				IntVec3 intVec;
				if (!DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, canRoofPunch, radius, true, null, true) && (canRoofPunch || !DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, true, radius, true, null, true)))
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

		// Token: 0x040050FE RID: 20734
		public const int PodOpenDelay = 520;
	}
}
