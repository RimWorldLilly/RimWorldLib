using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002101 RID: 8449
	public class SymbolResolver_EnsureCanReachMapEdge : SymbolResolver
	{
		// Token: 0x0600D43C RID: 54332 RVA: 0x004E6FC0 File Offset: 0x004E51C0
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Clear();
			foreach (IntVec3 item in rp.rect)
			{
				SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Add(item);
			}
			SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Shuffle<IntVec3>();
			this.TryMakeAllCellsReachable(false, rp);
			this.TryMakeAllCellsReachable(true, rp);
		}

		// Token: 0x0600D43D RID: 54333 RVA: 0x004E703C File Offset: 0x004E523C
		private void TryMakeAllCellsReachable(bool canPathThroughNonStandable, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_EnsureCanReachMapEdge.visited.Clear();
			for (int i = 0; i < SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Count; i++)
			{
				IntVec3 intVec = SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder[i];
				if (this.CanTraverse(intVec, canPathThroughNonStandable))
				{
					District district = intVec.GetDistrict(map, RegionType.Set_Passable);
					if (district != null && !SymbolResolver_EnsureCanReachMapEdge.visited.Contains(district))
					{
						SymbolResolver_EnsureCanReachMapEdge.visited.Add(district);
						TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
						if (!map.reachability.CanReachMapEdge(intVec, traverseParms))
						{
							bool found = false;
							IntVec3 foundDest = IntVec3.Invalid;
							map.floodFiller.FloodFill(intVec, (IntVec3 x) => !found && this.CanTraverse(x, canPathThroughNonStandable), delegate(IntVec3 x)
							{
								if (found)
								{
									return;
								}
								if (map.reachability.CanReachMapEdge(x, traverseParms))
								{
									found = true;
									foundDest = x;
								}
							}, int.MaxValue, true, null);
							if (found)
							{
								this.ReconstructPathAndDestroyWalls(foundDest, district, rp);
							}
						}
					}
				}
			}
			SymbolResolver_EnsureCanReachMapEdge.visited.Clear();
		}

		// Token: 0x0600D43E RID: 54334 RVA: 0x004E7194 File Offset: 0x004E5394
		private void ReconstructPathAndDestroyWalls(IntVec3 foundDest, District room, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			map.floodFiller.ReconstructLastFloodFillPath(foundDest, SymbolResolver_EnsureCanReachMapEdge.path);
			while (SymbolResolver_EnsureCanReachMapEdge.path.Count >= 2 && SymbolResolver_EnsureCanReachMapEdge.path[0].AdjacentToCardinal(room) && SymbolResolver_EnsureCanReachMapEdge.path[1].AdjacentToCardinal(room))
			{
				SymbolResolver_EnsureCanReachMapEdge.path.RemoveAt(0);
			}
			IntVec3 intVec = IntVec3.Invalid;
			ThingDef thingDef = null;
			IntVec3 intVec2 = IntVec3.Invalid;
			ThingDef thingDef2 = null;
			for (int i = 0; i < SymbolResolver_EnsureCanReachMapEdge.path.Count; i++)
			{
				Building edifice = SymbolResolver_EnsureCanReachMapEdge.path[i].GetEdifice(map);
				if (this.IsWallOrRock(edifice))
				{
					if (!intVec.IsValid)
					{
						intVec = SymbolResolver_EnsureCanReachMapEdge.path[i];
						thingDef = edifice.Stuff;
					}
					intVec2 = SymbolResolver_EnsureCanReachMapEdge.path[i];
					thingDef2 = edifice.Stuff;
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
			if (intVec.IsValid)
			{
				ThingDef thingDef3;
				if ((thingDef3 = thingDef) == null)
				{
					thingDef3 = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff = thingDef3;
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Door, stuff);
				thing.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing, intVec, map, WipeMode.Vanish);
			}
			if (intVec2.IsValid && intVec2 != intVec && !intVec2.AdjacentToCardinal(intVec))
			{
				ThingDef thingDef4;
				if ((thingDef4 = thingDef2) == null)
				{
					thingDef4 = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff2 = thingDef4;
				Thing thing2 = ThingMaker.MakeThing(ThingDefOf.Door, stuff2);
				thing2.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing2, intVec2, map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600D43F RID: 54335 RVA: 0x004E7330 File Offset: 0x004E5530
		private bool CanTraverse(IntVec3 c, bool canPathThroughNonStandable)
		{
			Map map = BaseGen.globalSettings.map;
			Building edifice = c.GetEdifice(map);
			return this.IsWallOrRock(edifice) || ((canPathThroughNonStandable || (c.Standable(map) && c.GetEdifice(map) == null)) && !c.Impassable(map));
		}

		// Token: 0x0600D440 RID: 54336 RVA: 0x004E737E File Offset: 0x004E557E
		private bool IsWallOrRock(Building b)
		{
			return b != null && (b.def == ThingDefOf.Wall || b.def.building.isNaturalRock);
		}

		// Token: 0x040086CC RID: 34508
		private static HashSet<District> visited = new HashSet<District>();

		// Token: 0x040086CD RID: 34509
		private static List<IntVec3> path = new List<IntVec3>();

		// Token: 0x040086CE RID: 34510
		private static List<IntVec3> cellsInRandomOrder = new List<IntVec3>();
	}
}
