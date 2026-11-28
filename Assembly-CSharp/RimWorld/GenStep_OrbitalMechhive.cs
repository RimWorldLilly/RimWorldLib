using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020012AF RID: 4783
	public class GenStep_OrbitalMechhive : GenStep_LargeRuins
	{
		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x0600767C RID: 30332 RVA: 0x0029B3C0 File Offset: 0x002995C0
		protected override int RegionSize
		{
			get
			{
				return 65;
			}
		}

		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x0600767D RID: 30333 RVA: 0x0029B3C4 File Offset: 0x002995C4
		protected override FloatRange DefaultMapFillPercentRange { get; } = new FloatRange(0.5f, 0.6f);

		// Token: 0x1700141D RID: 5149
		// (get) Token: 0x0600767E RID: 30334 RVA: 0x0029B3CC File Offset: 0x002995CC
		protected override FloatRange MergeRange { get; } = new FloatRange(1f, 1f);

		// Token: 0x1700141E RID: 5150
		// (get) Token: 0x0600767F RID: 30335 RVA: 0x0029B3D4 File Offset: 0x002995D4
		protected override IntRange RuinsMinMaxRange { get; } = new IntRange(3, 4);

		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x06007680 RID: 30336 RVA: 0x0029B3DC File Offset: 0x002995DC
		protected override int MoveRangeLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001420 RID: 5152
		// (get) Token: 0x06007681 RID: 30337 RVA: 0x0029B3DF File Offset: 0x002995DF
		protected override int ContractLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001421 RID: 5153
		// (get) Token: 0x06007682 RID: 30338 RVA: 0x0029B3E2 File Offset: 0x002995E2
		protected override int MinRegionSize
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x17001422 RID: 5154
		// (get) Token: 0x06007683 RID: 30339 RVA: 0x0029B3E6 File Offset: 0x002995E6
		protected override bool UseUsedRects
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001423 RID: 5155
		// (get) Token: 0x06007684 RID: 30340 RVA: 0x0029B3E9 File Offset: 0x002995E9
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.Mechhive;
			}
		}

		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x06007685 RID: 30341 RVA: 0x0029B3F0 File Offset: 0x002995F0
		protected override Faction Faction
		{
			get
			{
				return Faction.OfMechanoids;
			}
		}

		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x06007686 RID: 30342 RVA: 0x0029B3F7 File Offset: 0x002995F7
		public override int SeedPart
		{
			get
			{
				return 345879234;
			}
		}

		// Token: 0x06007687 RID: 30343 RVA: 0x0029B400 File Offset: 0x00299600
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Orbital Mechhive"))
			{
				return;
			}
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			this.stabilizerCount = 0;
			GenStep_OrbitalMechhive.chunks = new Chunks(map.BoundsRect(20), 22);
			using (ProfilerBlock.Scope("CreatePlatform"))
			{
				try
				{
					GenStep_OrbitalMechhive.CreatePlatform(map);
				}
				catch (Exception ex)
				{
					string str = "Error in CreatePlatform ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateCerebrexCore"))
			{
				try
				{
					this.GenerateCerebrexCore(map, orGenerateVar);
				}
				catch (Exception ex3)
				{
					string str2 = "Error in GenerateCerebrexCore ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("SetupPlayerStartSpot"))
			{
				try
				{
					this.SetupPlayerStartSpot(map, parms, orGenerateVar);
				}
				catch (Exception ex5)
				{
					string str3 = "Error in SetupPlayerStartSpot ";
					Exception ex6 = ex5;
					Log.Error(str3 + ((ex6 != null) ? ex6.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateGaussCannons"))
			{
				try
				{
					this.GenerateCannons(map, orGenerateVar);
				}
				catch (Exception ex7)
				{
					string str4 = "Error in GenerateGaussCannons ";
					Exception ex8 = ex7;
					Log.Error(str4 + ((ex8 != null) ? ex8.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateRuins"))
			{
				try
				{
					this.GenerateRuins(map, parms, this.DefaultMapFillPercentRange);
				}
				catch (Exception ex9)
				{
					string str5 = "Error in GenerateRuins ";
					Exception ex10 = ex9;
					Log.Error(str5 + ((ex10 != null) ? ex10.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateStandaloneRooms"))
			{
				try
				{
					this.GenerateStandaloneRooms(map, parms, orGenerateVar);
				}
				catch (Exception ex11)
				{
					string str6 = "Error in GenerateStandaloneRooms ";
					Exception ex12 = ex11;
					Log.Error(str6 + ((ex12 != null) ? ex12.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("CreateHoles"))
			{
				try
				{
					GenStep_OrbitalMechhive.CreateHoles(map, orGenerateVar);
				}
				catch (Exception ex13)
				{
					string str7 = "Error in CreateHoles ";
					Exception ex14 = ex13;
					Log.Error(str7 + ((ex14 != null) ? ex14.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateMiniStructures"))
			{
				try
				{
					this.GenerateMiniStructures(map, orGenerateVar);
				}
				catch (Exception ex15)
				{
					string str8 = "Error in GenerateMiniStructures ";
					Exception ex16 = ex15;
					Log.Error(str8 + ((ex16 != null) ? ex16.ToString() : null));
				}
			}
			map.FogOfWarColor = new Color?(ColorLibrary.SpaceMechFog);
			map.OrbitalDebris = OrbitalDebrisDefOf.Mechanoid;
		}

		// Token: 0x06007688 RID: 30344 RVA: 0x0029B740 File Offset: 0x00299940
		protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			this.GeneratePlatformBorder(rect, map);
			return base.GenerateAndSpawn(rect, map, parms, layoutDef);
		}

		// Token: 0x06007689 RID: 30345 RVA: 0x0029B758 File Offset: 0x00299958
		private void GeneratePlatformBorder(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.ExpandedBy(1).EdgeCells)
			{
				if (c.GetTerrain(map) == TerrainDefOf.Space)
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.MechanoidPlatform);
				}
			}
		}

		// Token: 0x0600768A RID: 30346 RVA: 0x0029B7C8 File Offset: 0x002999C8
		private static void CreatePlatform(Map map)
		{
			using (map.pathing.DisableIncrementalScope())
			{
				SpaceGenUtility.ChunksSetSparsley(GenStep_OrbitalMechhive.chunks, FloatRange.One);
				foreach (CellRect cellRect in GenStep_OrbitalMechhive.chunks.EnumeratedRects)
				{
					foreach (IntVec3 c in cellRect.Cells)
					{
						map.terrainGrid.SetTerrain(c, TerrainDefOf.MechanoidPlatform);
					}
					CellRect cellRect2 = cellRect;
					MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.MechanoidPlatform, ref cellRect2, GenStep_OrbitalMechhive.PlatformPadBorderLumpLengthRange, GenStep_OrbitalMechhive.PlatformPadBorderLumpOffsetRange, null);
				}
			}
		}

		// Token: 0x0600768B RID: 30347 RVA: 0x0029B8AC File Offset: 0x00299AAC
		private void GenerateCerebrexCore(Map map, List<CellRect> usedRects)
		{
			SketchResolveParams parms = default(SketchResolveParams);
			parms.sketch = new Sketch();
			SketchGen.Generate(SketchResolverDefOf.CerebrexCore, parms).Spawn(map, map.Center, Faction.OfMechanoids, Sketch.SpawnPosType.OccupiedCenter, Sketch.SpawnMode.Normal, true, true, true, null, false, false, null, null, null);
			CellRect item = CellRect.CenteredOn(map.Center, Mathf.FloorToInt(30.5f));
			foreach (IntVec3 c in item.Cells)
			{
				map.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
			}
			item = item.ExpandedBy(4);
			usedRects.Add(item);
		}

		// Token: 0x0600768C RID: 30348 RVA: 0x0029B968 File Offset: 0x00299B68
		private void SetupPlayerStartSpot(Map map, GenStepParams parms, List<CellRect> usedRects)
		{
			if (parms.gravship == null)
			{
				return;
			}
			HashSet<IntVec3> cellsAdjacentToSubstructure = GravshipPlacementUtility.GetCellsAdjacentToSubstructure(parms.gravship.OccupiedRects, 2);
			bool enabled = map.regionAndRoomUpdater.Enabled;
			map.regionAndRoomUpdater.Enabled = true;
			GenStep_ReserveGravshipArea.SetStartSpot(map, cellsAdjacentToSubstructure, usedRects);
			map.regionAndRoomUpdater.Enabled = enabled;
			IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
			foreach (CellRect cellRect in parms.gravship.OccupiedRects)
			{
				usedRects.Add(cellRect.MovedBy(playerStartSpot).ExpandedBy(2));
			}
			foreach (IntVec3 loc in parms.gravship.Bounds.MovedBy(playerStartSpot).ContractedBy(2).Corners)
			{
				GenSpawn.Spawn(ThingDefOf.AncientShipBeacon, loc, map, WipeMode.Vanish);
			}
			MapGenerator.SetVar<bool>("DontGenerateClearedGravShipTerrain", true);
		}

		// Token: 0x0600768D RID: 30349 RVA: 0x0029BA90 File Offset: 0x00299C90
		private void GenerateCannons(Map map, List<CellRect> usedRects)
		{
			List<IntVec3> list = GenStep_OrbitalMechhive.chunks.boundary.Corners.ToList<IntVec3>();
			for (int i = 0; i < 4; i++)
			{
				CellRect item = CellRect.CenteredOn(list[i], 13, 13);
				foreach (IntVec3 c in item.Cells)
				{
					if (c.InHorDistOf(list[i], 5.9f))
					{
						map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
					}
					else if (c.InHorDistOf(list[i], 8f))
					{
						map.terrainGrid.SetTerrain(c, TerrainDefOf.MechanoidPlatform);
					}
				}
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AnticraftBeam, null), list[i], map, new Rot4((i + 2) % 4), WipeMode.Vanish, false, false);
				usedRects.Add(item);
			}
		}

		// Token: 0x0600768E RID: 30350 RVA: 0x0029BB98 File Offset: 0x00299D98
		private void GenerateStandaloneRooms(Map map, GenStepParams parms, List<CellRect> usedRects)
		{
			int randomInRange = GenStep_OrbitalMechhive.NumStandaloneRooms.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				CellRect empty = CellRect.Empty;
				if (this.TryGetFreeRect(map, GenStep_OrbitalMechhive.StandaloneRoomSize.RandomInRange, GenStep_OrbitalMechhive.StandaloneRoomSize.RandomInRange, usedRects, out empty))
				{
					this.GeneratePlatformBorder(empty, map);
					StructureGenParams parms2 = new StructureGenParams
					{
						size = empty.Size
					};
					LayoutWorker worker = LayoutDefOf.Mechhive_SingleRoom.Worker;
					LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms2);
					map.layoutStructureSketches.Add(layoutStructureSketch);
					this.structureSketches.Add(layoutStructureSketch);
					float? threatPoints = null;
					if (parms.sitePart != null)
					{
						threatPoints = new float?(parms.sitePart.parms.points);
					}
					worker.Spawn(layoutStructureSketch, map, empty.Min, threatPoints, null, true, false, Faction.OfMechanoids);
					usedRects.Add(empty.ExpandedBy(1));
				}
			}
		}

		// Token: 0x0600768F RID: 30351 RVA: 0x0029BC8C File Offset: 0x00299E8C
		private static void CreateHoles(Map map, List<CellRect> usedRects)
		{
			int approximateArea = GenStep_OrbitalMechhive.chunks.ApproximateArea;
			int num = Mathf.RoundToInt((float)approximateArea * GenStep_OrbitalMechhive.DamagedMaxPercentageRange.RandomInRange);
			List<CellRect> list = new List<CellRect>();
			Chunks chunks = GenStep_OrbitalMechhive.chunks;
			int maximum = num;
			List<CellRect> spaces = list;
			SpaceGenUtility.DamageHoles(chunks, map, maximum, ref approximateArea, null, null, 1f, (CellRect r) => !usedRects.Any((CellRect ur) => ur.Overlaps(r)), spaces);
			foreach (CellRect item in list)
			{
				usedRects.Add(item);
			}
		}

		// Token: 0x06007690 RID: 30352 RVA: 0x0029BD50 File Offset: 0x00299F50
		private void GenerateMiniStructures(Map map, List<CellRect> usedRects)
		{
			for (int i = 0; i < GenStep_OrbitalMechhive.NumLaunchPads.RandomInRange; i++)
			{
				CellRect cellRect;
				if (this.TryGetFreeRect(map, 5, 5, usedRects, out cellRect))
				{
					this.GenerateSketch(SketchResolverDefOf.MechhiveLaunchPad, map, cellRect);
					usedRects.Add(cellRect);
				}
			}
			for (int j = 0; j < GenStep_OrbitalMechhive.NumGuardOutposts.RandomInRange; j++)
			{
				CellRect cellRect2;
				if (this.TryGetFreeRect(map, 5, 5, usedRects, out cellRect2))
				{
					this.GenerateSketch(SketchResolverDefOf.MechhiveGuardOutpost, map, cellRect2);
					usedRects.Add(cellRect2);
					IEnumerable<Pawn> enumerable = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
					{
						groupKind = PawnGroupKindDefOf.Combat,
						tile = map.Tile,
						faction = Faction.OfMechanoids,
						points = (float)GenStep_OrbitalMechhive.GuardOutpostPointsRange.RandomInRange
					}, true);
					if (enumerable.Any<Pawn>())
					{
						Lord lord = LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_DefendPoint(cellRect2.CenterCell, null, null, false, true), map, null);
						foreach (Pawn pawn in enumerable)
						{
							GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(cellRect2.CenterCell, map, 5, null), map, WipeMode.Vanish);
							lord.AddPawn(pawn);
						}
					}
				}
			}
			for (int k = 0; k < GenStep_OrbitalMechhive.NumProcessorArrays.RandomInRange; k++)
			{
				int width = Rand.RangeInclusive(2, 3) * (ThingDefOf.MechanoidProcessor.size.x + 1) + 1;
				int height = Rand.RangeInclusive(2, 3) * (ThingDefOf.MechanoidProcessor.size.z + 1) + 1;
				CellRect cellRect3;
				if (this.TryGetFreeRect(map, width, height, usedRects, out cellRect3))
				{
					this.GenerateSketch(SketchResolverDefOf.MechhiveProcessorArray, map, cellRect3);
					usedRects.Add(cellRect3);
				}
			}
		}

		// Token: 0x06007691 RID: 30353 RVA: 0x0029BF34 File Offset: 0x0029A134
		private void GenerateSketch(SketchResolverDef sketchDef, Map map, CellRect rect)
		{
			SketchGen.Generate(sketchDef, new SketchResolveParams
			{
				sketch = new Sketch(),
				rect = new CellRect?(new CellRect(0, 0, rect.Width, rect.Height))
			}).Spawn(map, rect.CenterCell, Faction.OfMechanoids, Sketch.SpawnPosType.OccupiedCenter, Sketch.SpawnMode.Normal, true, true, true, null, false, false, null, null, null);
		}

		// Token: 0x06007692 RID: 30354 RVA: 0x0029BF9C File Offset: 0x0029A19C
		private bool TryGetFreeRect(Map map, int width, int height, List<CellRect> usedRects, out CellRect rect)
		{
			rect = CellRect.Empty;
			Func<IntVec3, bool> <>9__2;
			IntVec3 center;
			if (!CellFinder.TryFindRandomCell(map, delegate(IntVec3 c)
			{
				CellRect tmpRect = CellRect.CenteredOn(c, width, height);
				if (tmpRect.InBounds(map) && usedRects.All((CellRect ur) => !ur.Overlaps(tmpRect)))
				{
					IEnumerable<IntVec3> cells = tmpRect.Cells;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((IntVec3 cell) => cell.GetTerrain(map) == TerrainDefOf.MechanoidPlatform));
					}
					return cells.All(predicate);
				}
				return false;
			}, out center))
			{
				return false;
			}
			rect = CellRect.CenteredOn(center, width, height);
			return true;
		}

		// Token: 0x06007693 RID: 30355 RVA: 0x0029C00F File Offset: 0x0029A20F
		protected override StructureGenParams GetStructureGenParams(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			StructureGenParams structureGenParams = base.GetStructureGenParams(rect, map, parms, layoutDef);
			structureGenParams.spawnImportantRoom = (this.stabilizerCount < 3);
			this.stabilizerCount++;
			return structureGenParams;
		}

		// Token: 0x04004A3E RID: 19006
		private const int NumStabilizers = 3;

		// Token: 0x04004A3F RID: 19007
		private static readonly IntRange NumStandaloneRooms = new IntRange(10, 12);

		// Token: 0x04004A40 RID: 19008
		private static readonly IntRange StandaloneRoomSize = new IntRange(8, 25);

		// Token: 0x04004A41 RID: 19009
		private static readonly IntRange NumLaunchPads = new IntRange(8, 10);

		// Token: 0x04004A42 RID: 19010
		private static readonly IntRange NumProcessorArrays = new IntRange(8, 12);

		// Token: 0x04004A43 RID: 19011
		private static readonly IntRange NumGuardOutposts = new IntRange(10, 12);

		// Token: 0x04004A44 RID: 19012
		private static readonly IntRange GuardOutpostPointsRange = new IntRange(200, 350);

		// Token: 0x04004A45 RID: 19013
		private const int MapBorderPadding = 20;

		// Token: 0x04004A46 RID: 19014
		private const int PlatformRegionSize = 22;

		// Token: 0x04004A47 RID: 19015
		private static readonly IntRange PlatformPadBorderLumpLengthRange = new IntRange(8, 12);

		// Token: 0x04004A48 RID: 19016
		private static readonly IntRange PlatformPadBorderLumpOffsetRange = new IntRange(-2, 3);

		// Token: 0x04004A49 RID: 19017
		private static readonly FloatRange DamagedMaxPercentageRange = new FloatRange(0.5f, 0.5f);

		// Token: 0x04004A4A RID: 19018
		private int stabilizerCount;

		// Token: 0x04004A4B RID: 19019
		private static Chunks chunks;

		// Token: 0x04004A4F RID: 19023
		private static List<IntVec3> tmpHoleCells = new List<IntVec3>();
	}
}
