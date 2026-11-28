using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002109 RID: 8457
	public class SymbolResolver_OutdoorsPath : SymbolResolver
	{
		// Token: 0x0600D45C RID: 54364 RVA: 0x004E7D6A File Offset: 0x004E5F6A
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp);
		}

		// Token: 0x0600D45D RID: 54365 RVA: 0x004E7D74 File Offset: 0x004E5F74
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_OutdoorsPath.cellsInRandomOrder.Clear();
			SymbolResolver_OutdoorsPath.cellsInRandomOrder.AddRange(rp.rect.Cells);
			SymbolResolver_OutdoorsPath.cellsInRandomOrder.Shuffle<IntVec3>();
			Map map = BaseGen.globalSettings.map;
			for (int i = 0; i < SymbolResolver_OutdoorsPath.cellsInRandomOrder.Count; i++)
			{
				IntVec3 intVec = SymbolResolver_OutdoorsPath.cellsInRandomOrder[i];
				if (intVec.GetDoor(BaseGen.globalSettings.map) != null && map.reachability.CanReachMapEdge(intVec, TraverseParms.For(TraverseMode.NoPassClosedDoorsOrWater, Danger.Deadly, false, false, false, true, false)))
				{
					bool found = false;
					IntVec3 foundDest = IntVec3.Invalid;
					map.floodFiller.FloodFill(intVec, (IntVec3 x) => !found && this.CanTraverse(x), delegate(IntVec3 x)
					{
						if (x.OnEdge(map))
						{
							found = true;
							foundDest = x;
						}
					}, int.MaxValue, true, null);
					if (found)
					{
						SymbolResolver_OutdoorsPath.path.Clear();
						map.floodFiller.ReconstructLastFloodFillPath(foundDest, SymbolResolver_OutdoorsPath.path);
						for (int j = 0; j < SymbolResolver_OutdoorsPath.path.Count; j++)
						{
							IntVec3 intVec2 = SymbolResolver_OutdoorsPath.path[j];
							if (!Rand.Chance(SymbolResolver_OutdoorsPath.ChanceToSkipPathOverDistanceCurve.Evaluate(intVec.DistanceTo(intVec2))) && this.CanPlacePath(intVec2))
							{
								List<Thing> thingList = intVec2.GetThingList(map);
								for (int k = thingList.Count - 1; k >= 0; k--)
								{
									if (thingList[k].def.destroyable)
									{
										thingList[k].Destroy(DestroyMode.Vanish);
									}
								}
								map.terrainGrid.SetTerrain(intVec2, rp.floorDef ?? TerrainDefOf.Gravel);
							}
						}
						break;
					}
				}
			}
			SymbolResolver_OutdoorsPath.path.Clear();
			SymbolResolver_OutdoorsPath.cellsInRandomOrder.Clear();
		}

		// Token: 0x0600D45E RID: 54366 RVA: 0x004E7F94 File Offset: 0x004E6194
		private bool CanTraverse(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			if (c.GetDoor(map) == null)
			{
				Room room = c.GetRoom(map);
				if (room != null && !room.PsychologicallyOutdoors)
				{
					return false;
				}
			}
			Building edifice = c.GetEdifice(map);
			return !this.IsWallOrRock(edifice);
		}

		// Token: 0x0600D45F RID: 54367 RVA: 0x004E7FE0 File Offset: 0x004E61E0
		private bool CanPlacePath(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			return c.GetDoor(map) == null && !c.GetTerrain(map).IsWater;
		}

		// Token: 0x0600D460 RID: 54368 RVA: 0x004E8012 File Offset: 0x004E6212
		private bool IsWallOrRock(Building b)
		{
			return b != null && (b.def == ThingDefOf.Wall || b.def.building.isNaturalRock);
		}

		// Token: 0x040086D0 RID: 34512
		private static readonly List<IntVec3> cellsInRandomOrder = new List<IntVec3>();

		// Token: 0x040086D1 RID: 34513
		private static readonly List<IntVec3> path = new List<IntVec3>();

		// Token: 0x040086D2 RID: 34514
		private static readonly SimpleCurve ChanceToSkipPathOverDistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 0f),
				true
			},
			{
				new CurvePoint(20f, 0f),
				true
			},
			{
				new CurvePoint(50f, 1f),
				true
			}
		};
	}
}
