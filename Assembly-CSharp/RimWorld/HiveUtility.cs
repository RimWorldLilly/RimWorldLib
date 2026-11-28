using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001B24 RID: 6948
	public static class HiveUtility
	{
		// Token: 0x0600B212 RID: 45586 RVA: 0x003EB364 File Offset: 0x003E9564
		public static int TotalSpawnedHivesCount(Map map, bool filterFogged = false)
		{
			List<Thing> list = map.listerThings.ThingsOfDef(ThingDefOf.Hive);
			if (filterFogged)
			{
				return list.Count((Thing h) => !h.Position.Fogged(h.Map));
			}
			return list.Count;
		}

		// Token: 0x0600B213 RID: 45587 RVA: 0x003EB3B4 File Offset: 0x003E95B4
		public static bool AnyHivePreventsClaiming(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = thing.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(thing.Map) && c.GetFirstThing(thing.Map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B214 RID: 45588 RVA: 0x003EB418 File Offset: 0x003E9618
		public static void Notify_HiveDespawned(Hive hive, Map map)
		{
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = hive.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (Faction.OfInsects != null && thing.Faction == Faction.OfInsects && thing.def.Claimable && !HiveUtility.AnyHivePreventsClaiming(thing) && !(thing is Pawn))
						{
							thing.SetFaction(null, null);
						}
					}
				}
			}
		}

		// Token: 0x0600B215 RID: 45589 RVA: 0x003EB4C8 File Offset: 0x003E96C8
		public static Hive SpawnHive(IntVec3 spawnCell, Map map, WipeMode wipeMode = WipeMode.VanishOrMoveAside, bool spawnInsectsImmediately = false, bool canSpawnHives = true, bool canSpawnInsects = true, bool dormant = false, bool aggressive = true, bool spawnJellyImmediately = false, bool spawnSludge = true)
		{
			Hive hive = (Hive)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Hive, null), spawnCell, map, wipeMode);
			hive.SetFaction(Faction.OfInsects, null);
			hive.PawnSpawner.aggressive = aggressive;
			if (dormant)
			{
				hive.CompDormant.ToSleep();
			}
			if (spawnInsectsImmediately)
			{
				hive.PawnSpawner.SpawnPawnsUntilPoints(Rand.Range(200f, 500f));
				hive.CompDormant.WakeUp();
			}
			if (spawnJellyImmediately)
			{
				foreach (CompSpawner compSpawner in hive.GetComps<CompSpawner>())
				{
					if (compSpawner.PropsSpawner.thingToSpawn == ThingDefOf.InsectJelly)
					{
						compSpawner.TryDoSpawn();
						break;
					}
				}
			}
			hive.PawnSpawner.canSpawnPawns = canSpawnInsects;
			hive.GetComp<CompSpawnerHives>().canSpawnHives = canSpawnHives;
			HiveUtility.SpawnExtras(hive, map, wipeMode, spawnSludge);
			return hive;
		}

		// Token: 0x0600B216 RID: 45590 RVA: 0x003EB5BC File Offset: 0x003E97BC
		public static void SpawnHives(IntVec3 spawnCell, Map map, int count, float radius, WipeMode wipeMode = WipeMode.VanishOrMoveAside, bool spawnInsectsImmediately = false, bool canSpawnHives = true, bool canSpawnInsects = true, bool dormant = false, bool aggressive = true, bool spawnSludge = true)
		{
			Hive hive = HiveUtility.SpawnHive(spawnCell, map, wipeMode, spawnInsectsImmediately, canSpawnHives, canSpawnInsects, dormant, aggressive, false, true);
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < count - 1; i++)
			{
				Map map2 = map;
				TraverseParms traverseParms = TraverseMode.NoPassClosedDoorsOrWater;
				Predicate<IntVec3> cellValidator;
				if ((cellValidator = <>9__0) == null)
				{
					cellValidator = (<>9__0 = ((IntVec3 cell) => cell.Standable(map)));
				}
				IntVec3 loc;
				Hive hive2;
				if (CellFinder.TryFindRandomReachableNearbyCell(spawnCell, map2, radius, traverseParms, cellValidator, null, out loc, 999999) && hive.GetComp<CompSpawnerHives>().TrySpawnChildHive(loc, out hive2))
				{
					if (dormant)
					{
						hive.CompDormant.ToSleep();
					}
					hive.PawnSpawner.aggressive = aggressive;
					hive.PawnSpawner.canSpawnPawns = canSpawnInsects;
					hive.GetComp<CompSpawnerHives>().canSpawnHives = canSpawnHives;
					HiveUtility.SpawnExtras(hive2, map, wipeMode, spawnSludge);
				}
			}
		}

		// Token: 0x0600B217 RID: 45591 RVA: 0x003EB69C File Offset: 0x003E989C
		private static void SpawnExtras(Hive hive, Map map, WipeMode wipeMode, bool spawnSludge = true)
		{
			HiveUtility.<>c__DisplayClass6_0 CS$<>8__locals1 = new HiveUtility.<>c__DisplayClass6_0();
			CS$<>8__locals1.map = map;
			if (ModsConfig.OdysseyActive && spawnSludge)
			{
				foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(hive.Position, CS$<>8__locals1.map, 30, (IntVec3 c) => true))
				{
					if (intVec.GetEdifice(CS$<>8__locals1.map) == null && CS$<>8__locals1.map.reachability.CanReach(intVec, hive.Position, PathEndMode.OnCell, TraverseMode.PassDoors) && GenConstruct.CanBuildOnTerrain(TerrainDefOf.InsectSludge, intVec, CS$<>8__locals1.map, Rot4.North, null, null))
					{
						if (CS$<>8__locals1.map.terrainGrid.TopTerrainAt(intVec).IsFloor)
						{
							CS$<>8__locals1.map.terrainGrid.RemoveTopLayer(intVec, wipeMode == WipeMode.FullRefund);
						}
						CS$<>8__locals1.map.terrainGrid.SetTerrain(intVec, TerrainDefOf.InsectSludge);
						if (Rand.Chance(0.2f))
						{
							GenSpawn.Spawn(ThingDefOf.Filth_Slime, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
						}
					}
				}
			}
			int num = Rand.RangeInclusive(0, 2);
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc;
				if (CellFinder.TryFindRandomReachableNearbyCell(hive.Position, CS$<>8__locals1.map, 5f, TraverseMode.NoPassClosedDoorsOrWater, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnExtras>g__CellValidator|0), null, out loc, 999999))
				{
					GenSpawn.Spawn(ThingDefOf.GlowPod, loc, CS$<>8__locals1.map, wipeMode).SetFaction(Faction.OfInsects, null);
				}
			}
			if (ModsConfig.OdysseyActive)
			{
				int num2 = Rand.RangeInclusive(0, 1);
				for (int j = 0; j < num2; j++)
				{
					IntVec3 loc2;
					if (CellFinder.TryFindRandomReachableNearbyCell(hive.Position, CS$<>8__locals1.map, 5f, TraverseMode.NoPassClosedDoorsOrWater, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnExtras>g__CellValidator|0), null, out loc2, 999999))
					{
						GenSpawn.Spawn(ThingDefOf.EggSac, loc2, CS$<>8__locals1.map, wipeMode).SetFaction(Faction.OfInsects, null);
					}
				}
			}
		}

		// Token: 0x040066BA RID: 26298
		private const float HivePreventsClaimingInRadius = 2f;
	}
}
