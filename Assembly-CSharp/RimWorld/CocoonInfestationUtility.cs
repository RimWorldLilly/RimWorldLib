using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011AB RID: 4523
	public static class CocoonInfestationUtility
	{
		// Token: 0x06007105 RID: 28933 RVA: 0x0027897C File Offset: 0x00276B7C
		public static bool TryFindCocoonSpawnPositionNear(IntVec3 root, Map map, ThingDef cocoon, out IntVec3 result)
		{
			result = CellFinder.FindNoWipeSpawnLocNear(root, map, cocoon, Rot4.North, Mathf.FloorToInt(GenRadial.MaxRadialPatternRadius), (IntVec3 c) => CocoonInfestationUtility.CanSpawnCocoonAt(c, map));
			return result.IsValid;
		}

		// Token: 0x06007106 RID: 28934 RVA: 0x002789CC File Offset: 0x00276BCC
		public static bool CanSpawnCocoonAt(IntVec3 c, Map map)
		{
			if (!c.Walkable(map) || c.GetFirstThing(map, ThingDefOf.InsectJelly) != null || c.GetFirstThing(map, ThingDefOf.GlowPod) != null)
			{
				return false;
			}
			foreach (IntVec3 c2 in GenRadial.RadialCellsAround(c, 2f, true))
			{
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] is CocoonSpawner)
						{
							return false;
						}
						if (thingList[i].def.building != null)
						{
							if (thingList[i].def.building.isInsectCocoon)
							{
								return false;
							}
							if (thingList[i].def.passability == Traversability.Impassable)
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06007107 RID: 28935 RVA: 0x00278AC8 File Offset: 0x00276CC8
		public static IEnumerable<ThingDef> GetCocoonsToSpawn(float points)
		{
			CocoonInfestationUtility.<GetCocoonsToSpawn>d__3 <GetCocoonsToSpawn>d__ = new CocoonInfestationUtility.<GetCocoonsToSpawn>d__3(-2);
			<GetCocoonsToSpawn>d__.<>3__points = points;
			return <GetCocoonsToSpawn>d__;
		}

		// Token: 0x06007108 RID: 28936 RVA: 0x00278AD8 File Offset: 0x00276CD8
		public static List<Thing> SpawnCocoonInfestation(IntVec3 root, Map map, float points)
		{
			List<Thing> list = new List<Thing>();
			if (!ModLister.CheckBiotech("Spawn cocoon infestation"))
			{
				return list;
			}
			CocoonInfestationUtility.tmpCocoonsToSpawn.Clear();
			CocoonInfestationUtility.tmpCocoonsToSpawn.AddRange(CocoonInfestationUtility.GetCocoonsToSpawn(points));
			if (CocoonInfestationUtility.tmpCocoonsToSpawn.Count == 0)
			{
				Log.Warning("Failed to find cocoons to spawn with points:" + points.ToString());
				return list;
			}
			int nextCocoonGroupID = Find.UniqueIDsManager.GetNextCocoonGroupID();
			int num = 0;
			IntVec3 loc;
			while (num < CocoonInfestationUtility.tmpCocoonsToSpawn.Count && CocoonInfestationUtility.TryFindCocoonSpawnPositionNear(root, map, CocoonInfestationUtility.tmpCocoonsToSpawn[num], out loc))
			{
				list.Add(GenSpawn.Spawn(CocoonInfestationUtility.MakeCocoon(CocoonInfestationUtility.tmpCocoonsToSpawn[num], nextCocoonGroupID), loc, map, WipeMode.FullRefund));
				num++;
			}
			CocoonInfestationUtility.tmpCocoonsToSpawn.Clear();
			return list;
		}

		// Token: 0x06007109 RID: 28937 RVA: 0x00278B97 File Offset: 0x00276D97
		private static Thing MakeCocoon(ThingDef cocoonDef, int groupID)
		{
			CocoonSpawner cocoonSpawner = (CocoonSpawner)ThingMaker.MakeThing(ThingDefOf.CocoonSpawner, null);
			cocoonSpawner.cocoon = cocoonDef;
			cocoonSpawner.groupID = groupID;
			return cocoonSpawner;
		}

		// Token: 0x040047C1 RID: 18369
		private const float MinDistanceBetweenCocoons = 2f;

		// Token: 0x040047C2 RID: 18370
		private static List<ThingDef> tmpCocoonsToSpawn = new List<ThingDef>();
	}
}
