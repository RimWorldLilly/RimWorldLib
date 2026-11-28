using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001182 RID: 4482
	public static class InfestationUtility
	{
		// Token: 0x06007068 RID: 28776 RVA: 0x00275A14 File Offset: 0x00273C14
		private static IntVec3 FindRootTunnelLoc(Map map, bool spawnAnywhereIfNoGoodCell = false, bool ignoreRoofIfNoGoodCell = false)
		{
			IntVec3 result;
			if (InfestationCellFinder.TryFindCell(out result, map))
			{
				return result;
			}
			if (!spawnAnywhereIfNoGoodCell)
			{
				return IntVec3.Invalid;
			}
			Func<IntVec3, bool, bool> validator = delegate(IntVec3 x, bool canIgnoreRoof)
			{
				if (!x.Standable(map) || x.Fogged(map))
				{
					return false;
				}
				if (!canIgnoreRoof)
				{
					bool flag = false;
					int num = GenRadial.NumCellsInRadius(3f);
					for (int i = 0; i < num; i++)
					{
						IntVec3 c = x + GenRadial.RadialPattern[i];
						if (c.InBounds(map))
						{
							RoofDef roof = c.GetRoof(map);
							if (roof != null && roof.isThickRoof)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return true;
			};
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => validator(x, false), map, out result))
			{
				return result;
			}
			if (ignoreRoofIfNoGoodCell && RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => validator(x, true), map, out result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06007069 RID: 28777 RVA: 0x00275A9C File Offset: 0x00273C9C
		public static Thing SpawnTunnels(int hiveCount, Map map, bool spawnAnywhereIfNoGoodCell = false, bool ignoreRoofedRequirement = false, string questTag = null, IntVec3? overrideLoc = null, float? insectsPoints = null)
		{
			IntVec3 loc = (overrideLoc != null) ? overrideLoc.Value : default(IntVec3);
			if (overrideLoc == null)
			{
				loc = InfestationUtility.FindRootTunnelLoc(map, spawnAnywhereIfNoGoodCell, false);
			}
			if (!loc.IsValid)
			{
				return null;
			}
			TunnelHiveSpawner tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null);
			Thing thing = GenSpawn.Spawn(tunnelHiveSpawner, loc, map, WipeMode.FullRefund);
			if (insectsPoints != null)
			{
				tunnelHiveSpawner.insectsPoints = insectsPoints.Value;
			}
			QuestUtility.AddQuestTag(thing, questTag);
			for (int i = 0; i < hiveCount - 1; i++)
			{
				loc = CompSpawnerHives.FindChildHiveLocation(thing.Position, map, ThingDefOf.Hive, ThingDefOf.Hive.GetCompProperties<CompProperties_SpawnerHives>(), ignoreRoofedRequirement, true);
				if (loc.IsValid)
				{
					tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null);
					thing = GenSpawn.Spawn(tunnelHiveSpawner, loc, map, WipeMode.FullRefund);
					if (insectsPoints != null)
					{
						tunnelHiveSpawner.insectsPoints = insectsPoints.Value;
					}
					QuestUtility.AddQuestTag(thing, questTag);
				}
			}
			return thing;
		}

		// Token: 0x0600706A RID: 28778 RVA: 0x00275B90 File Offset: 0x00273D90
		public static Thing SpawnJellyTunnels(int tunnelCount, int jellyCount, Map map)
		{
			IntVec3 loc = InfestationUtility.FindRootTunnelLoc(map, true, true);
			if (!loc.IsValid)
			{
				return null;
			}
			TunnelJellySpawner tunnelJellySpawner = (TunnelJellySpawner)ThingMaker.MakeThing(ThingDefOf.TunnelJellySpawner, null);
			tunnelJellySpawner.jellyCount = jellyCount / tunnelCount;
			int num = jellyCount - tunnelJellySpawner.jellyCount;
			GenSpawn.Spawn(tunnelJellySpawner, loc, map, WipeMode.FullRefund);
			for (int i = 0; i < tunnelCount - 1; i++)
			{
				loc = CompSpawnerHives.FindChildHiveLocation(tunnelJellySpawner.Position, map, ThingDefOf.Hive, ThingDefOf.Hive.GetCompProperties<CompProperties_SpawnerHives>(), true, false);
				if (loc.IsValid)
				{
					tunnelJellySpawner = (TunnelJellySpawner)ThingMaker.MakeThing(ThingDefOf.TunnelJellySpawner, null);
					if (i < tunnelCount - 2)
					{
						tunnelJellySpawner.jellyCount = jellyCount / tunnelCount;
						num -= tunnelJellySpawner.jellyCount;
					}
					else
					{
						tunnelJellySpawner.jellyCount = num;
					}
					GenSpawn.Spawn(tunnelJellySpawner, loc, map, WipeMode.FullRefund);
				}
			}
			return tunnelJellySpawner;
		}
	}
}
