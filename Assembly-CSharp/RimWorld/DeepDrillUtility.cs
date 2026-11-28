using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185A RID: 6234
	public static class DeepDrillUtility
	{
		// Token: 0x17001AA4 RID: 6820
		// (get) Token: 0x06009C09 RID: 39945 RVA: 0x00383390 File Offset: 0x00381590
		public static List<ThingDef> Rocks
		{
			get
			{
				if (DeepDrillUtility.rocks == null)
				{
					DeepDrillUtility.rocks = (from def in DefDatabase<ThingDef>.AllDefs
					where def.IsNonResourceNaturalRock
					select def).ToList<ThingDef>();
				}
				return DeepDrillUtility.rocks;
			}
		}

		// Token: 0x06009C0A RID: 39946 RVA: 0x003833DC File Offset: 0x003815DC
		public static ThingDef RockForTerrain(TerrainDef terrain)
		{
			foreach (ThingDef thingDef in DeepDrillUtility.Rocks)
			{
				if (thingDef.building.naturalTerrain == terrain)
				{
					return thingDef;
				}
			}
			return null;
		}

		// Token: 0x06009C0B RID: 39947 RVA: 0x0038343C File Offset: 0x0038163C
		public static ThingDef GetNextResource(IntVec3 p, Map map)
		{
			ThingDef result;
			int num;
			IntVec3 intVec;
			DeepDrillUtility.GetNextResource(p, map, out result, out num, out intVec);
			return result;
		}

		// Token: 0x06009C0C RID: 39948 RVA: 0x00383458 File Offset: 0x00381658
		public static bool GetNextResource(IntVec3 p, Map map, out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			for (int i = 0; i < 21; i++)
			{
				IntVec3 intVec = p + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					ThingDef thingDef = map.deepResourceGrid.ThingDefAt(intVec);
					if (thingDef != null)
					{
						resDef = thingDef;
						countPresent = map.deepResourceGrid.CountAt(intVec);
						cell = intVec;
						return true;
					}
				}
			}
			resDef = DeepDrillUtility.GetBaseResource(map, p);
			countPresent = int.MaxValue;
			cell = p;
			return false;
		}

		// Token: 0x06009C0D RID: 39949 RVA: 0x003834D4 File Offset: 0x003816D4
		public static ThingDef GetBaseResource(Map map, IntVec3 cell)
		{
			if (!map.Biome.hasBedrock)
			{
				return null;
			}
			ThingDef thingDef = DeepDrillUtility.RockForTerrain(map.terrainGrid.BaseTerrainAt(cell));
			if (thingDef != null)
			{
				return thingDef.building.mineableThing;
			}
			Rand.PushState();
			Rand.Seed = cell.GetHashCode();
			ThingDef result = (from rock in Find.World.NaturalRockTypesIn(map.Tile)
			select rock.building.mineableThing).RandomElement<ThingDef>();
			Rand.PopState();
			return result;
		}

		// Token: 0x04005CC0 RID: 23744
		private static List<ThingDef> rocks;

		// Token: 0x04005CC1 RID: 23745
		public const int NumCellsToScan = 21;
	}
}
