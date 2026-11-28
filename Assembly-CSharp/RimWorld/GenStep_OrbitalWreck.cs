using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B3 RID: 4787
	public class GenStep_OrbitalWreck : GenStep_BaseRuins
	{
		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x060076BF RID: 30399 RVA: 0x0029D7E7 File Offset: 0x0029B9E7
		protected override LayoutDef LayoutDef
		{
			get
			{
				return this.layouts.RandomElementByWeight((LayoutParms parms) => parms.weight).def;
			}
		}

		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x060076C0 RID: 30400 RVA: 0x0029D818 File Offset: 0x0029BA18
		protected override bool UseUsedRects
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x060076C1 RID: 30401 RVA: 0x0029D81B File Offset: 0x0029BA1B
		protected override Faction Faction
		{
			get
			{
				if (this.factionDef != null)
				{
					return Find.FactionManager.FirstFactionOfDef(this.factionDef);
				}
				return null;
			}
		}

		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x060076C2 RID: 30402 RVA: 0x0029D837 File Offset: 0x0029BA37
		protected override FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17001431 RID: 5169
		// (get) Token: 0x060076C3 RID: 30403 RVA: 0x0029D848 File Offset: 0x0029BA48
		protected override FloatRange MergeRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x060076C4 RID: 30404 RVA: 0x0029D84F File Offset: 0x0029BA4F
		public override int SeedPart
		{
			get
			{
				return 982581;
			}
		}

		// Token: 0x060076C5 RID: 30405 RVA: 0x0029D856 File Offset: 0x0029BA56
		protected override CellRect GetBounds(Map map)
		{
			return GenStep_OrbitalWreck.intBoundary.ContractedBy(2);
		}

		// Token: 0x060076C6 RID: 30406 RVA: 0x0029D863 File Offset: 0x0029BA63
		protected override IEnumerable<CellRect> GetRects(CellRect area, Map map)
		{
			GenStep_OrbitalWreck.<GetRects>d__39 <GetRects>d__ = new GenStep_OrbitalWreck.<GetRects>d__39(-2);
			<GetRects>d__.<>4__this = this;
			<GetRects>d__.<>3__map = map;
			return <GetRects>d__;
		}

		// Token: 0x060076C7 RID: 30407 RVA: 0x0029D87A File Offset: 0x0029BA7A
		protected override IEnumerable<CellRect> GetRectOrder(IEnumerable<CellRect> rects, Map map)
		{
			return rects;
		}

		// Token: 0x060076C8 RID: 30408 RVA: 0x0029D880 File Offset: 0x0029BA80
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Orbital Wreck"))
			{
				return;
			}
			using (map.pathing.DisableIncrementalScope())
			{
				this.GenerateSpawnBasePlatform(map);
			}
			int approximateArea = GenStep_OrbitalWreck.chunks.ApproximateArea;
			int maximum = Mathf.RoundToInt((float)approximateArea * GenStep_OrbitalWreck.DamagedMaxPercentageRange.RandomInRange);
			Chunks chunks = GenStep_OrbitalWreck.chunks;
			List<CellRect> spaces = GenStep_OrbitalWreck.importantRects;
			SpaceGenUtility.ScatterExtrusions(map, chunks, null, null, null, null, spaces);
			this.GenerateRuins(map, parms, this.DefaultMapFillPercentRange);
			this.PostStructuresDamage(map, maximum, ref approximateArea);
			SpaceGenUtility.ScatterPrefabs(map, GenStep_OrbitalWreck.chunks, this.Faction, this.prefabs, new Predicate<CellRect>(GenStep_OrbitalWreck.ValidRect), null);
			MapGenUtility.SpawnExteriorLumps(map, ThingDefOf.RubblePile, GenStep_OrbitalWreck.RubblePilesPer10K, GenStep_OrbitalWreck.RubblePileCountRange, GenStep_OrbitalWreck.RubblePileDistanceRange, 6);
			MapGenUtility.SpawnExteriorLumps(map, ThingDefOf.ChunkSlagSteel, GenStep_OrbitalWreck.ChunksPer10K, GenStep_OrbitalWreck.ChunksCountRange, GenStep_OrbitalWreck.ChunksDistanceRange, 6);
			GenStep_OrbitalWreck.importantRects.Clear();
			map.FogOfWarColor = new Color?(ColorLibrary.SpaceHumanFog);
			map.OrbitalDebris = this.orbitalDebrisDef;
		}

		// Token: 0x060076C9 RID: 30409 RVA: 0x0029D9C8 File Offset: 0x0029BBC8
		private void GenerateSpawnBasePlatform(Map map)
		{
			GenStep_OrbitalWreck.chunks = new Chunks(map.BoundsRect(20), 22);
			GenStep_OrbitalWreck.intBoundary = GenStep_OrbitalWreck.chunks.boundary;
			SpaceGenUtility.ChunksSetSparsley(GenStep_OrbitalWreck.chunks, GenStep_OrbitalWreck.InitialPercentageRange);
			foreach (CellRect cellRect in GenStep_OrbitalWreck.chunks.EnumeratedRects)
			{
				foreach (IntVec3 c in cellRect.Cells)
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.OrbitalPlatform);
				}
				CellRect cellRect2 = cellRect;
				MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.OrbitalPlatform, ref cellRect2, GenStep_OrbitalWreck.PlatformPadBorderLumpLengthRange, GenStep_OrbitalWreck.PlatformPadBorderLumpOffsetRange, null);
			}
		}

		// Token: 0x060076CA RID: 30410 RVA: 0x0029DAA8 File Offset: 0x0029BCA8
		protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			LayoutStructureSketch layoutStructureSketch = base.GenerateAndSpawn(rect, map, parms, layoutDef);
			foreach (LayoutRoom layoutRoom in layoutStructureSketch.structureLayout.Rooms)
			{
				foreach (LayoutRoomDef layoutRoomDef in layoutRoom.defs)
				{
					if (!layoutRoomDef.canRemoveBorderDoors || !layoutRoomDef.canRemoveBorderWalls)
					{
						GenStep_OrbitalWreck.importantRects.AddRange(layoutRoom.rects);
						break;
					}
				}
			}
			MapGenerator.SetVar<CellRect>("SpawnRect", rect);
			MapGenerator.SetVar<CellRect>("RectOfInterest", rect);
			MapGenerator.UsedRects.Add(rect);
			return layoutStructureSketch;
		}

		// Token: 0x060076CB RID: 30411 RVA: 0x0029DB88 File Offset: 0x0029BD88
		private void PostStructuresDamage(Map map, int maximum, ref int cells)
		{
			SpaceGenUtility.DamageHoles(GenStep_OrbitalWreck.chunks, map, maximum, ref cells, new IntRange?(GenStep_OrbitalWreck.HoleSizeRange), new FloatRange?(GenStep_OrbitalWreck.HolesPerHundredCellsRange), 1f, new Predicate<CellRect>(GenStep_OrbitalWreck.ValidRect), GenStep_OrbitalWreck.importantRects);
		}

		// Token: 0x060076CC RID: 30412 RVA: 0x0029DBCC File Offset: 0x0029BDCC
		private static bool ValidRect(CellRect rect)
		{
			return SpaceGenUtility.NoIntersection(rect, GenStep_OrbitalWreck.importantRects, GenStep_OrbitalWreck.HoleLumpOffsetRange);
		}

		// Token: 0x04004A7D RID: 19069
		private FactionDef factionDef;

		// Token: 0x04004A7E RID: 19070
		private List<PrefabParms> prefabs = new List<PrefabParms>();

		// Token: 0x04004A7F RID: 19071
		private List<LayoutParms> layouts = new List<LayoutParms>();

		// Token: 0x04004A80 RID: 19072
		private OrbitalDebrisDef orbitalDebrisDef;

		// Token: 0x04004A81 RID: 19073
		private float? temperature;

		// Token: 0x04004A82 RID: 19074
		private static readonly List<CellRect> importantRects = new List<CellRect>();

		// Token: 0x04004A83 RID: 19075
		private static CellRect intBoundary;

		// Token: 0x04004A84 RID: 19076
		public static Chunks chunks;

		// Token: 0x04004A85 RID: 19077
		private const int MapBorderPadding = 20;

		// Token: 0x04004A86 RID: 19078
		private static readonly FloatRange InitialPercentageRange = new FloatRange(0.65f, 0.65f);

		// Token: 0x04004A87 RID: 19079
		private static readonly FloatRange DamagedMaxPercentageRange = new FloatRange(0.5f, 0.5f);

		// Token: 0x04004A88 RID: 19080
		private static readonly IntRange PlatformPadBorderLumpLengthRange = new IntRange(8, 12);

		// Token: 0x04004A89 RID: 19081
		private static readonly IntRange PlatformPadBorderLumpOffsetRange = new IntRange(-2, 3);

		// Token: 0x04004A8A RID: 19082
		private static readonly IntRange HoleLumpOffsetRange = new IntRange(-1, 2);

		// Token: 0x04004A8B RID: 19083
		private static readonly IntRange HoleSizeRange = new IntRange(3, 6);

		// Token: 0x04004A8C RID: 19084
		private static readonly FloatRange HolesPerHundredCellsRange = new FloatRange(0.025f, 0.025f);

		// Token: 0x04004A8D RID: 19085
		private static readonly FloatRange RubblePilesPer10K = new FloatRange(15f, 18f);

		// Token: 0x04004A8E RID: 19086
		private static readonly IntRange RubblePileCountRange = new IntRange(2, 12);

		// Token: 0x04004A8F RID: 19087
		private static readonly IntRange RubblePileDistanceRange = new IntRange(3, 10);

		// Token: 0x04004A90 RID: 19088
		private static readonly FloatRange ChunksPer10K = new FloatRange(1f, 2f);

		// Token: 0x04004A91 RID: 19089
		private static readonly IntRange ChunksCountRange = new IntRange(2, 4);

		// Token: 0x04004A92 RID: 19090
		private static readonly IntRange ChunksDistanceRange = new IntRange(3, 6);

		// Token: 0x04004A93 RID: 19091
		private static readonly IntRange MaxRuinSizeRange = new IntRange(60, 80);

		// Token: 0x04004A94 RID: 19092
		private static readonly IntRange RuinContractRange = new IntRange(1, 2);

		// Token: 0x04004A95 RID: 19093
		private static readonly FloatRange RuinCoverageRange = new FloatRange(0.6f, 0.65f);

		// Token: 0x04004A96 RID: 19094
		private const int PlatformRegionSize = 22;
	}
}
