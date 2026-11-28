using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F5 RID: 4853
	public class GenStep_MechanoidRelay : GenStep
	{
		// Token: 0x17001499 RID: 5273
		// (get) Token: 0x06007865 RID: 30821 RVA: 0x002A91E3 File Offset: 0x002A73E3
		public override int SeedPart
		{
			get
			{
				return 583937484;
			}
		}

		// Token: 0x06007866 RID: 30822 RVA: 0x002A91EC File Offset: 0x002A73EC
		public override void Generate(Map map, GenStepParams parms)
		{
			List<CellRect> usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)parms.sitePart.things).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Thing thing = enumerator.Current;
					ResolveParams resolveParams = default(ResolveParams);
					resolveParams.singleThingToSpawn = thing;
					IntVec3 center;
					if (thing.def != ThingDefOf.MechRelay || !RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => this.CellValid(x, map, thing.def.Size, usedRects), map, out center))
					{
						center = CellFinderLoose.RandomCellWith((IntVec3 x) => this.CellValid(x, map, thing.def.Size, usedRects), map, 1000);
					}
					if (!center.IsValid)
					{
						Log.Warning("Could not find valid cell for MechRelay/Stabilizer.");
					}
					resolveParams.rect = GenAdj.OccupiedRect(center, thing.def.defaultPlacingRot, thing.def.size);
					this.ChangeTerrainAround(resolveParams, map, thing, usedRects);
					BaseGen.symbolStack.Push("thing", resolveParams, null);
				}
			}
			BaseGen.globalSettings.map = map;
			BaseGen.Generate();
		}

		// Token: 0x06007867 RID: 30823 RVA: 0x002A9374 File Offset: 0x002A7574
		private void ChangeTerrainAround(ResolveParams thingParms, Map map, Thing thing, List<CellRect> usedRects)
		{
			CellRect cellRect = thingParms.rect.ExpandedBy(1);
			foreach (IntVec3 c in cellRect)
			{
				if (map.terrainGrid.TerrainAt(c).changeable)
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientConcrete);
				}
			}
			CellRect item = cellRect.ExpandedBy(1);
			bool flag = thing.def == ThingDefOf.MechRelay;
			IEnumerable<IntVec3> corners = item.Corners;
			foreach (IntVec3 intVec in item.EdgeCells)
			{
				if ((flag || !corners.Contains(intVec)) && map.terrainGrid.TerrainAt(intVec).changeable)
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.AncientTile);
				}
			}
			usedRects.Add(item);
			if (flag)
			{
				foreach (IntVec3 intVec2 in corners)
				{
					if (GenConstruct.TerrainCanSupport(CellRect.CenteredOn(intVec2, 1), map, ThingDefOf.AncientMechDropBeacon))
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientMechDropBeacon, null), intVec2, map, ThingDefOf.AncientMechDropBeacon.defaultPlacingRot, WipeMode.Vanish, false, false);
					}
				}
			}
		}

		// Token: 0x06007868 RID: 30824 RVA: 0x002A94F8 File Offset: 0x002A76F8
		private bool CellValid(IntVec3 cell, Map map, IntVec2 size, List<CellRect> usedRects)
		{
			CellRect other = CellRect.CenteredOn(cell, Mathf.Max(size.x, size.z)).ExpandedBy(2);
			foreach (CellRect cellRect in usedRects)
			{
				if (cellRect.Overlaps(other))
				{
					return false;
				}
			}
			foreach (IntVec3 c in other)
			{
				if (!c.InBounds(map))
				{
					return false;
				}
				if (c.Fogged(map))
				{
					return false;
				}
				if (!c.Walkable(map))
				{
					return false;
				}
			}
			return cell.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy);
		}
	}
}
