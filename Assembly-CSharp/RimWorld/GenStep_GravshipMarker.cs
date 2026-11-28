using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D1 RID: 4817
	public class GenStep_GravshipMarker : GenStep
	{
		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x0600777C RID: 30588 RVA: 0x002A26F4 File Offset: 0x002A08F4
		public override int SeedPart
		{
			get
			{
				return 2031711911;
			}
		}

		// Token: 0x0600777D RID: 30589 RVA: 0x002A26FC File Offset: 0x002A08FC
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Gravship gravship = parms.gravship;
			if (gravship == null)
			{
				return;
			}
			IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
			HashSet<IntVec3> cellsAdjacentToSubstructure = GravshipPlacementUtility.GetCellsAdjacentToSubstructure(gravship.OccupiedRects, 2);
			GravshipPlacementUtility.ClearArea(map, playerStartSpot, cellsAdjacentToSubstructure, GravshipPlacementUtility.ClearMode.BlockingBuildingsOnly);
			GravshipLandingMarker gravshipLandingMarker = ThingMaker.MakeThing(ThingDefOf.GravshipLandingMarker, null) as GravshipLandingMarker;
			gravshipLandingMarker.gravship = gravship;
			GenSpawn.Spawn(gravshipLandingMarker, playerStartSpot, map, WipeMode.Vanish);
		}

		// Token: 0x04004AFE RID: 19198
		public const int ClearEdgeRadius = 2;
	}
}
