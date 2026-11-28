using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BB RID: 4795
	public class GenStep_RocksFromGrid : GenStep
	{
		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x060076FF RID: 30463 RVA: 0x0029F428 File Offset: 0x0029D628
		public override int SeedPart
		{
			get
			{
				return 1182952823;
			}
		}

		// Token: 0x06007700 RID: 30464 RVA: 0x0029F430 File Offset: 0x0029D630
		public static ThingDef RockDefAt(IntVec3 c)
		{
			ThingDef thingDef = null;
			float num = -999999f;
			for (int i = 0; i < RockNoises.rockNoises.Count; i++)
			{
				float value = RockNoises.rockNoises[i].noise.GetValue(c);
				if (value > num)
				{
					thingDef = RockNoises.rockNoises[i].rockDef;
					num = value;
				}
			}
			if (thingDef == null)
			{
				string str = "Did not get rock def to generate at ";
				IntVec3 intVec = c;
				Log.ErrorOnce(str + intVec.ToString(), 50812);
				thingDef = ThingDefOf.Sandstone;
			}
			return thingDef;
		}

		// Token: 0x06007701 RID: 30465 RVA: 0x0029F4B8 File Offset: 0x0029D6B8
		public override void Generate(Map map, GenStepParams parms)
		{
			if (map.TileInfo.WaterCovered)
			{
				return;
			}
			map.regionAndRoomUpdater.Enabled = false;
			float num = 0.7f;
			List<GenStep_RocksFromGrid.RoofThreshold> list = new List<GenStep_RocksFromGrid.RoofThreshold>();
			list.Add(new GenStep_RocksFromGrid.RoofThreshold
			{
				roofDef = RoofDefOf.RoofRockThick,
				minGridVal = num * 1.14f
			});
			list.Add(new GenStep_RocksFromGrid.RoofThreshold
			{
				roofDef = RoofDefOf.RoofRockThin,
				minGridVal = num * 1.04f
			});
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float num2 = elevation[intVec];
				if (num2 > num)
				{
					if (caves[intVec] <= 0f)
					{
						GenSpawn.Spawn(GenStep_RocksFromGrid.RockDefAt(intVec), intVec, map, WipeMode.Vanish);
					}
					for (int i = 0; i < list.Count; i++)
					{
						if (num2 > list[i].minGridVal)
						{
							map.roofGrid.SetRoof(intVec, list[i].roofDef);
							break;
						}
					}
				}
			}
			BoolGrid visited = new BoolGrid(map);
			List<IntVec3> toRemove = new List<IntVec3>();
			Predicate<IntVec3> <>9__0;
			Action<IntVec3> <>9__1;
			foreach (IntVec3 intVec2 in map.AllCells)
			{
				if (!visited[intVec2] && this.IsNaturalRoofAt(intVec2, map))
				{
					toRemove.Clear();
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec2;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => this.IsNaturalRoofAt(x, map)));
					}
					Action<IntVec3> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(IntVec3 x)
						{
							visited[x] = true;
							toRemove.Add(x);
						});
					}
					floodFiller.FloodFill(root, passCheck, processor, int.MaxValue, false, null);
					if (toRemove.Count < 20)
					{
						for (int j = 0; j < toRemove.Count; j++)
						{
							map.roofGrid.SetRoof(toRemove[j], null);
						}
					}
				}
			}
			GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new GenStep_ScatterLumpsMineable();
			if (!ModsConfig.OdysseyActive || !this.ignoreMaxIfGravship || !map.wasSpawnedViaGravShipLanding)
			{
				genStep_ScatterLumpsMineable.maxValue = this.maxMineableValue;
			}
			genStep_ScatterLumpsMineable.useNomadicMineables = true;
			float num3 = GenStep_RocksFromGrid.GetResourceBlotchesPer10KCellsForMap(map);
			if (this.overrideBlotchesPer10kCells != null)
			{
				num3 = this.overrideBlotchesPer10kCells.Value;
			}
			genStep_ScatterLumpsMineable.countPer10kCellsRange = new FloatRange(num3, num3);
			genStep_ScatterLumpsMineable.Generate(map, parms);
			map.regionAndRoomUpdater.Enabled = true;
		}

		// Token: 0x06007702 RID: 30466 RVA: 0x0029F7F0 File Offset: 0x0029D9F0
		public static float GetResourceBlotchesPer10KCellsForMap(Map map)
		{
			float result = 10f;
			switch (map.TileInfo.HillinessForOreGeneration)
			{
			case Hilliness.Flat:
				result = 4f;
				break;
			case Hilliness.SmallHills:
				result = 8f;
				break;
			case Hilliness.LargeHills:
				result = 11f;
				break;
			case Hilliness.Mountainous:
				result = 15f;
				break;
			case Hilliness.Impassable:
				result = 16f;
				break;
			}
			return result;
		}

		// Token: 0x06007703 RID: 30467 RVA: 0x0029F854 File Offset: 0x0029DA54
		private bool IsNaturalRoofAt(IntVec3 c, Map map)
		{
			return c.Roofed(map) && c.GetRoof(map).isNatural;
		}

		// Token: 0x04004AAF RID: 19119
		private float maxMineableValue = float.MaxValue;

		// Token: 0x04004AB0 RID: 19120
		private bool ignoreMaxIfGravship;

		// Token: 0x04004AB1 RID: 19121
		private float? overrideBlotchesPer10kCells;

		// Token: 0x04004AB2 RID: 19122
		private const int MinRoofedCellsPerGroup = 20;

		// Token: 0x020031B9 RID: 12729
		private class RoofThreshold
		{
			// Token: 0x0400C8CB RID: 51403
			public RoofDef roofDef;

			// Token: 0x0400C8CC RID: 51404
			public float minGridVal;
		}
	}
}
