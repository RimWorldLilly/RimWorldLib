using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008AD RID: 2221
	public static class TouchPathEndModeUtility
	{
		// Token: 0x060044DD RID: 17629 RVA: 0x001B02B4 File Offset: 0x001AE4B4
		[Obsolete("Use IsCornerTouchAllowed_NewTemp()")]
		public static bool IsCornerTouchAllowed(IntVec3 corner, IntVec3 adjCardinalX, IntVec3 adjCardinalZ, PathingContext pc, bool forEdifice)
		{
			return TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(adjCardinalX, adjCardinalZ, pc, forEdifice ? pc.map.edificeGrid[corner] : null);
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x001B02D8 File Offset: 0x001AE4D8
		public static bool IsCornerTouchAllowed_NewTemp(IntVec3 adjCardinalX, IntVec3 adjCardinalZ, PathingContext pc, Thing target = null)
		{
			return (target != null && (target.IsConsideredEdificeForCornerTouch() || TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(target.def))) || ((pc.pathGrid.Walkable(adjCardinalX) && adjCardinalX.GetDoor(pc.map) == null) || (pc.pathGrid.Walkable(adjCardinalZ) && adjCardinalZ.GetDoor(pc.map) == null));
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x001B033C File Offset: 0x001AE53C
		public static bool MakesOccupiedCellsAlwaysReachableDiagonally(ThingDef def)
		{
			ThingDef thingDef = def.IsFrame ? (def.entityDefToBuild as ThingDef) : def;
			return thingDef != null && thingDef.CanInteractThroughCorners;
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x001B036C File Offset: 0x001AE56C
		public static bool IsAdjacentCornerAndNotAllowed(IntVec3 corner, IntVec3 BL, IntVec3 TL, IntVec3 TR, IntVec3 BR, PathingContext pc, Thing forThing)
		{
			IntVec3 b = new IntVec3(1, 0, 0);
			IntVec3 b2 = new IntVec3(0, 0, 1);
			return (corner == BL && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(BL + b2, BL + b, pc, forThing)) || (corner == TL && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(TL - b2, TL + b, pc, forThing)) || (corner == TR && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(TR - b2, TR - b, pc, forThing)) || (corner == BR && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(BR + b2, BR - b, pc, forThing));
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x001B0424 File Offset: 0x001AE624
		public static void AddAllowedAdjacentRegions(LocalTargetInfo dest, TraverseParms traverseParams, Map map, List<Region> regions)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(dest, out bl, out tl, out tr, out br);
			PathingContext pc = map.pathing.For(traverseParams);
			if (!dest.HasThing || (dest.Thing.def.size.x == 1 && dest.Thing.def.size.z == 1))
			{
				IntVec3 cell = dest.Cell;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = GenAdj.AdjacentCells[i] + cell;
					if (intVec.InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(intVec, bl, tl, tr, br, pc, dest.Thing))
					{
						Region region = intVec.GetRegion(map, RegionType.Set_Passable);
						if (region != null && region.Allows(traverseParams, true))
						{
							regions.Add(region);
						}
					}
				}
				return;
			}
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(dest);
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(list[j], bl, tl, tr, br, pc, dest.Thing))
				{
					Region region2 = list[j].GetRegion(map, RegionType.Set_Passable);
					if (region2 != null && region2.Allows(traverseParams, true))
					{
						regions.Add(region2);
					}
				}
			}
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x001B0574 File Offset: 0x001AE774
		public static bool IsAdjacentOrInsideAndAllowedToTouch(IntVec3 root, LocalTargetInfo target, PathingContext pc)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(target, out bl, out tl, out tr, out br);
			return root.AdjacentTo8WayOrInside(target) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(root, bl, tl, tr, br, pc, target.Thing);
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x001B05B0 File Offset: 0x001AE7B0
		private static bool IsConsideredEdificeForCornerTouch(this Thing thing)
		{
			if (thing == null)
			{
				return false;
			}
			ThingDef thingDef = (thing.def.IsFrame || thing.def.IsBlueprint) ? (thing.def.entityDefToBuild as ThingDef) : thing.def;
			if (thingDef == null)
			{
				return false;
			}
			if (!thingDef.IsEdifice() || !thingDef.holdsRoof)
			{
				BuildingProperties building = thingDef.building;
				return building != null && building.canConstructInCorner;
			}
			return true;
		}
	}
}
