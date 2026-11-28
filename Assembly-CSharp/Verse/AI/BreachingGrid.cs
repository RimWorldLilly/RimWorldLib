using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using UnityEngine;
using Verse.AI.Group;
using Verse.Noise;

namespace Verse.AI
{
	// Token: 0x02000910 RID: 2320
	public class BreachingGrid : IExposable
	{
		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x0600469C RID: 18076 RVA: 0x001BC12C File Offset: 0x001BA32C
		public BoolGrid WalkGrid
		{
			get
			{
				return this.walkGrid;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x0600469D RID: 18077 RVA: 0x001BC134 File Offset: 0x001BA334
		public BoolGrid BreachGrid
		{
			get
			{
				return this.breachGrid;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x0600469E RID: 18078 RVA: 0x001BC13C File Offset: 0x001BA33C
		public ByteGrid MarkerGrid
		{
			get
			{
				this.RegenerateCachedGridIfDirty();
				return this.markerGrid;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x0600469F RID: 18079 RVA: 0x001BC14A File Offset: 0x001BA34A
		public BoolGrid ReachableGrid
		{
			get
			{
				this.RegenerateCachedGridIfDirty();
				return this.reachableGrid;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x060046A0 RID: 18080 RVA: 0x001BC158 File Offset: 0x001BA358
		public Map Map
		{
			get
			{
				return this.map;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x060046A1 RID: 18081 RVA: 0x001BC160 File Offset: 0x001BA360
		public int BreachRadius
		{
			get
			{
				return this.breachRadius;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x060046A2 RID: 18082 RVA: 0x001BC168 File Offset: 0x001BA368
		public Perlin Noise
		{
			get
			{
				Perlin result;
				if ((result = this.perlinCached) == null)
				{
					result = (this.perlinCached = BreachingGrid.CreatePerlinNoise(this.perlinSeed));
				}
				return result;
			}
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x001BC193 File Offset: 0x001BA393
		public BreachingGrid()
		{
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x001BC1B4 File Offset: 0x001BA3B4
		public BreachingGrid(Map map, Lord lord)
		{
			this.map = map;
			this.lord = lord;
			this.walkGrid = new BoolGrid(map);
			this.breachGrid = new BoolGrid(map);
			this.perlinSeed = Rand.Int;
		}

		// Token: 0x060046A5 RID: 18085 RVA: 0x001BC211 File Offset: 0x001BA411
		public static Perlin CreatePerlinNoise(int seed)
		{
			return new Perlin((double)BreachingGrid.perlinFrequency, (double)BreachingGrid.perlinLacunarity, (double)BreachingGrid.perlinPersistence, (int)BreachingGrid.perlinOctaves, seed, QualityMode.Medium);
		}

		// Token: 0x060046A6 RID: 18086 RVA: 0x001BC232 File Offset: 0x001BA432
		public void Notify_PawnStateChanged(Pawn pawn)
		{
			this.cachedGridsDirty = true;
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x001BC23B File Offset: 0x001BA43B
		public void Notify_BuildingStateChanged(Building b)
		{
			this.cachedGridsDirty = true;
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x001BC244 File Offset: 0x001BA444
		public bool WithinNoise(IntVec3 cell)
		{
			return this.Noise.GetValue(cell) >= BreachingGrid.perlinThres;
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x001BC25C File Offset: 0x001BA45C
		public void CreateBreachPath(IntVec3 start, IntVec3 end, int breachRadius, int walkMargin, bool useAvoidGrid = false)
		{
			this.breachRadius = breachRadius;
			this.breachStart = start;
			PathFinderCostTuning pathFinderCostTuning = new PathFinderCostTuning
			{
				costBlockedDoor = BreachingGrid.tweakWallCost,
				costBlockedWallBase = BreachingGrid.tweakWallCost,
				costBlockedDoorPerHitPoint = BreachingGrid.tweakWallHpCost,
				costBlockedWallExtraPerHitPoint = BreachingGrid.tweakWallHpCost,
				costOffLordWalkGrid = BreachingGrid.tweakOffWalkGridPathCost,
				costBlockedWallExtraForNaturalWalls = BreachingGrid.tweakNaturalWallExtraCost
			};
			using (BreachingGrid.CustomTuning customTuning = new BreachingGrid.CustomTuning(this, pathFinderCostTuning))
			{
				TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false, useAvoidGrid, false, true, false);
				using (PawnPath pawnPath = this.map.pathFinder.FindPathNow(start, end, traverseParms, new PathFinderCostTuning?(pathFinderCostTuning), PathEndMode.OnCell, customTuning))
				{
					foreach (IntVec3 c in pawnPath.NodesReversed)
					{
						this.breachGrid[c] = true;
						this.walkGrid[c] = true;
					}
				}
				for (int i = 0; i < breachRadius; i++)
				{
					this.WidenGrid(this.breachGrid);
					this.WidenGrid(this.walkGrid);
				}
				for (int j = 0; j < walkMargin; j++)
				{
					this.WidenGrid(this.walkGrid);
				}
			}
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x001BC3D4 File Offset: 0x001BA5D4
		public Thing FindBuildingToBreach()
		{
			Building bestBuilding = null;
			int bestBuildingDist = int.MaxValue;
			int bestBuildingReachableSideCount = 0;
			this.RegenerateCachedGridIfDirty();
			this.Map.floodFiller.FloodFill(this.breachStart, (IntVec3 c) => this.BreachGrid[c], delegate(IntVec3 c, int dist)
			{
				List<Thing> thingList = c.GetThingList(this.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Building building = thingList[i] as Building;
					if (building != null && BreachingUtility.ShouldBreachBuilding(building) && BreachingUtility.IsWorthBreachingBuilding(this, building))
					{
						int num = BreachingUtility.CountReachableAdjacentCells(this, building);
						if (num > 0 && num > bestBuildingReachableSideCount)
						{
							bestBuilding = building;
							bestBuildingDist = dist;
							bestBuildingReachableSideCount = num;
							break;
						}
					}
				}
				return dist - 2 > bestBuildingDist;
			}, int.MaxValue, false, null);
			return bestBuilding;
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x001BC448 File Offset: 0x001BA648
		private void WidenGrid(BoolGrid grid)
		{
			BreachingGrid.tmpWidenGrid.ClearAndResizeTo(this.map);
			foreach (IntVec3 a in grid.ActiveCells)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = a + GenAdj.AdjacentCells[i];
					if (c.InBounds(this.map))
					{
						BreachingGrid.tmpWidenGrid[c] = true;
					}
				}
			}
			foreach (IntVec3 c2 in BreachingGrid.tmpWidenGrid.ActiveCells)
			{
				grid[c2] = true;
			}
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x001BC51C File Offset: 0x001BA71C
		public void Reset()
		{
			this.breachGrid.Clear();
			this.walkGrid.Clear();
			ByteGrid byteGrid = this.markerGrid;
			if (byteGrid != null)
			{
				byteGrid.Clear(0);
			}
			BoolGrid boolGrid = this.reachableGrid;
			if (boolGrid != null)
			{
				boolGrid.Clear();
			}
			this.cachedGridsDirty = true;
			BreachingGridDebug.ClearDebugPath();
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x001BC56E File Offset: 0x001BA76E
		private void RegenerateCachedGridIfDirty()
		{
			if (this.cachedGridsDirty)
			{
				this.RegenerateCachedGrid();
			}
		}

		// Token: 0x060046AE RID: 18094 RVA: 0x001BC580 File Offset: 0x001BA780
		private void RegenerateCachedGrid()
		{
			this.cachedGridsDirty = false;
			if (this.markerGrid == null)
			{
				this.markerGrid = new ByteGrid(this.Map);
			}
			else
			{
				this.markerGrid.Clear(0);
			}
			if (this.reachableGrid == null)
			{
				this.reachableGrid = new BoolGrid(this.Map);
			}
			else
			{
				this.reachableGrid.Clear();
			}
			BreachingGrid.cachedWalkReachabilityPainter.PaintWalkReachability(this);
			if (this.lord != null)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					Pawn_MindState mindState = pawn.mindState;
					bool flag;
					if (mindState == null)
					{
						flag = (null != null);
					}
					else
					{
						BreachingTargetData breachingTarget = mindState.breachingTarget;
						flag = (((breachingTarget != null) ? breachingTarget.target : null) != null);
					}
					if (flag && !pawn.mindState.breachingTarget.target.Destroyed)
					{
						this.PaintDangerFromPawn(pawn);
					}
				}
			}
		}

		// Token: 0x060046AF RID: 18095 RVA: 0x001BC660 File Offset: 0x001BA860
		private void PaintDangerFromPawn(Pawn pawn)
		{
			BreachingTargetData breachingTarget = pawn.mindState.breachingTarget;
			if (breachingTarget == null)
			{
				return;
			}
			IntVec3 position = breachingTarget.target.Position;
			if (!position.IsValid)
			{
				return;
			}
			Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
			if (verb != null)
			{
				IntVec3 firingPosition = breachingTarget.firingPosition;
				if (firingPosition.IsValid)
				{
					if (this.markerGrid[firingPosition] == 0)
					{
						this.markerGrid[firingPosition] = 180;
					}
					this.VisitDangerousCellsOfAttack(firingPosition, position, verb, new Action<IntVec3>(this.<PaintDangerFromPawn>g__DangerSetter|56_0));
				}
			}
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x001BC6E1 File Offset: 0x001BA8E1
		public void VisitDangerousCellsOfAttack(IntVec3 firingPosition, IntVec3 targetPosition, Verb verb, Action<IntVec3> visitor)
		{
			if (!verb.IsMeleeAttack)
			{
				BreachingGrid.cachedDangerLineOfSightPainter.PaintLoS(this.map, firingPosition, targetPosition, visitor);
				this.PaintSplashDamage(verb, targetPosition, visitor);
			}
		}

		// Token: 0x060046B1 RID: 18097 RVA: 0x001BC70C File Offset: 0x001BA90C
		private void PaintSplashDamage(Verb verb, IntVec3 center, Action<IntVec3> visitor)
		{
			float num = 2f;
			ThingDef projectile = verb.GetProjectile();
			if (projectile != null && projectile.projectile.explosionRadius > 0f)
			{
				num = Mathf.Max(num, projectile.projectile.explosionRadius);
			}
			int num2 = GenRadial.NumCellsInRadius(num);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 obj = (center + GenRadial.RadialPattern[i]).ClampInsideMap(this.map);
				visitor(obj);
			}
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x001BC78C File Offset: 0x001BA98C
		private ushort DangerousRoomCost(Room room)
		{
			if (!room.Fogged)
			{
				return 0;
			}
			foreach (Thing thing in room.ContainedAndAdjacentThings)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.mindState != null && !pawn.mindState.Active)
				{
					return 600;
				}
				if (thing.def == ThingDefOf.Hive)
				{
					return 600;
				}
				if (thing.def == ThingDefOf.AncientCryptosleepCasket)
				{
					return 600;
				}
			}
			return 0;
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x001BC838 File Offset: 0x001BAA38
		public void ExposeData()
		{
			Scribe_Deep.Look<BoolGrid>(ref this.walkGrid, "walkGrid", Array.Empty<object>());
			Scribe_Deep.Look<BoolGrid>(ref this.breachGrid, "breachGrid", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.perlinSeed, "perlinSeed", 0, false);
			Scribe_Values.Look<int>(ref this.breachRadius, "breachRadius", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.breachStart, "breachStart", default(IntVec3), false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x001BC962 File Offset: 0x001BAB62
		[CompilerGenerated]
		private void <PaintDangerFromPawn>g__DangerSetter|56_0(IntVec3 cell)
		{
			this.markerGrid[cell] = 10;
		}

		// Token: 0x04002DEB RID: 11755
		public const byte Marker_FiringPosition = 180;

		// Token: 0x04002DEC RID: 11756
		public const byte Marker_Dangerous = 10;

		// Token: 0x04002DED RID: 11757
		public const byte Marker_UnUsed = 0;

		// Token: 0x04002DEE RID: 11758
		private const ushort DangerousRoomPathCost = 600;

		// Token: 0x04002DEF RID: 11759
		private static readonly BreachingGrid.WalkReachabilityPainter cachedWalkReachabilityPainter = new BreachingGrid.WalkReachabilityPainter();

		// Token: 0x04002DF0 RID: 11760
		private static readonly BreachingGrid.DangerLineOfSightPainter cachedDangerLineOfSightPainter = new BreachingGrid.DangerLineOfSightPainter();

		// Token: 0x04002DF1 RID: 11761
		private BoolGrid walkGrid;

		// Token: 0x04002DF2 RID: 11762
		private BoolGrid breachGrid;

		// Token: 0x04002DF3 RID: 11763
		private int perlinSeed;

		// Token: 0x04002DF4 RID: 11764
		private int breachRadius = 1;

		// Token: 0x04002DF5 RID: 11765
		private IntVec3 breachStart = IntVec3.Invalid;

		// Token: 0x04002DF6 RID: 11766
		private Map map;

		// Token: 0x04002DF7 RID: 11767
		private Lord lord;

		// Token: 0x04002DF8 RID: 11768
		private Perlin perlinCached;

		// Token: 0x04002DF9 RID: 11769
		private BoolGrid reachableGrid;

		// Token: 0x04002DFA RID: 11770
		private ByteGrid markerGrid;

		// Token: 0x04002DFB RID: 11771
		private bool cachedGridsDirty = true;

		// Token: 0x04002DFC RID: 11772
		[TweakValue("Breaching", 0f, 70f)]
		private static int tweakWallCost = 20;

		// Token: 0x04002DFD RID: 11773
		[TweakValue("Breaching", 0f, 1f)]
		private static float tweakWallHpCost = 0.015f;

		// Token: 0x04002DFE RID: 11774
		[TweakValue("Breaching", 0f, 100f)]
		private static bool tweakUsePerlin = true;

		// Token: 0x04002DFF RID: 11775
		[TweakValue("Breaching", -70f, 70f)]
		private static ushort tweakPerlinCost = 30;

		// Token: 0x04002E00 RID: 11776
		[TweakValue("Breaching", 1f, 7f)]
		public static int tweakOffWalkGridPathCost = 140;

		// Token: 0x04002E01 RID: 11777
		[TweakValue("Breaching", 0f, 100f)]
		private static bool tweakAvoidDangerousRooms = true;

		// Token: 0x04002E02 RID: 11778
		[TweakValue("Breaching", -70f, 70f)]
		private static int tweakNaturalWallExtraCost = 20;

		// Token: 0x04002E03 RID: 11779
		[TweakValue("Breaching", 0f, 0.1f)]
		private static float perlinFrequency = 0.06581f;

		// Token: 0x04002E04 RID: 11780
		[TweakValue("Breaching", 1f, 2f)]
		private static float perlinLacunarity = 1.5516f;

		// Token: 0x04002E05 RID: 11781
		[TweakValue("Breaching", 0f, 2f)]
		private static float perlinPersistence = 1.6569f;

		// Token: 0x04002E06 RID: 11782
		[TweakValue("Breaching", 1f, 5f)]
		private static float perlinOctaves = 4f;

		// Token: 0x04002E07 RID: 11783
		[TweakValue("Breaching", 0f, 1f)]
		private static float perlinThres = 0.5f;

		// Token: 0x04002E08 RID: 11784
		private static readonly BoolGrid tmpWidenGrid = new BoolGrid();

		// Token: 0x02002BBC RID: 11196
		private class CustomTuning : PathRequest.IPathGridCustomizer, IDisposable
		{
			// Token: 0x06010B42 RID: 68418 RVA: 0x005D8164 File Offset: 0x005D6364
			public NativeArray<ushort> GetOffsetGrid()
			{
				return this.offsets;
			}

			// Token: 0x06010B43 RID: 68419 RVA: 0x005D816C File Offset: 0x005D636C
			public CustomTuning(BreachingGrid grid, PathFinderCostTuning tuning)
			{
				this.grid = grid;
				this.tuning = tuning;
				this.offsets = new NativeArray<ushort>(grid.map.cellIndices.NumGridCells, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				this.SetupOffsetGrid();
			}

			// Token: 0x06010B44 RID: 68420 RVA: 0x005D81A5 File Offset: 0x005D63A5
			public void Dispose()
			{
				this.offsets.Dispose();
			}

			// Token: 0x06010B45 RID: 68421 RVA: 0x005D81B4 File Offset: 0x005D63B4
			private void SetupOffsetGrid()
			{
				this.offsets.Clear<ushort>();
				if (!BreachingGrid.tweakAvoidDangerousRooms)
				{
					return;
				}
				CellIndices cellIndices = this.grid.map.cellIndices;
				foreach (Room room in this.grid.map.regionGrid.AllRooms)
				{
					ushort num = this.grid.DangerousRoomCost(room);
					if (num != 0)
					{
						foreach (IntVec3 cell in room.Cells)
						{
							this.offsets[cellIndices[cell]] = num;
						}
						foreach (IntVec3 intVec in room.BorderCells)
						{
							if (intVec.InBounds(this.grid.map))
							{
								this.offsets[cellIndices[intVec]] = num;
							}
						}
					}
				}
				for (int i = 0; i < cellIndices.NumGridCells; i++)
				{
					if (BreachingGrid.tweakUsePerlin && this.grid.WithinNoise(cellIndices[i]))
					{
						ref NativeArray<ushort> ptr = ref this.offsets;
						int index = i;
						ptr[index] += BreachingGrid.tweakPerlinCost;
					}
					Building edifice = cellIndices[i].GetEdifice(this.grid.Map);
					if (edifice != null && PathUtility.IsDestroyable(edifice))
					{
						ref NativeArray<ushort> ptr = ref this.offsets;
						int index = i;
						ptr[index] += (ushort)((float)this.tuning.costBlockedWallBase + (float)edifice.HitPoints * this.tuning.costBlockedWallExtraPerHitPoint);
						ptr = ref this.offsets;
						index = i;
						ptr[index] += (ushort)(edifice.def.IsBuildingArtificial ? 0 : this.tuning.costBlockedWallExtraForNaturalWalls);
					}
				}
			}

			// Token: 0x0400B28F RID: 45711
			private readonly BreachingGrid grid;

			// Token: 0x0400B290 RID: 45712
			private NativeArray<ushort> offsets;

			// Token: 0x0400B291 RID: 45713
			private readonly PathFinderCostTuning tuning;
		}

		// Token: 0x02002BBD RID: 11197
		private class DangerLineOfSightPainter
		{
			// Token: 0x06010B46 RID: 68422 RVA: 0x005D8404 File Offset: 0x005D6604
			public DangerLineOfSightPainter()
			{
				this.skipThenVisitFunc = new Action<IntVec3>(this.SkipThenVisit);
			}

			// Token: 0x06010B47 RID: 68423 RVA: 0x005D841E File Offset: 0x005D661E
			private void SkipThenVisit(IntVec3 cell)
			{
				if (this.skipCount <= 0)
				{
					this.visitor(cell);
				}
				this.skipCount--;
			}

			// Token: 0x06010B48 RID: 68424 RVA: 0x005D8443 File Offset: 0x005D6643
			public void PaintLoS(Map map, IntVec3 start, IntVec3 end, Action<IntVec3> visitor)
			{
				if (!start.InBounds(map) || !end.InBounds(map))
				{
					return;
				}
				this.visitor = visitor;
				this.skipCount = Mathf.FloorToInt(5f);
				GenSight.PointsOnLineOfSight(start, end, this.skipThenVisitFunc);
			}

			// Token: 0x0400B292 RID: 45714
			private Action<IntVec3> visitor;

			// Token: 0x0400B293 RID: 45715
			private int skipCount;

			// Token: 0x0400B294 RID: 45716
			private readonly Action<IntVec3> skipThenVisitFunc;
		}

		// Token: 0x02002BBE RID: 11198
		private class WalkReachabilityPainter
		{
			// Token: 0x06010B49 RID: 68425 RVA: 0x005D847D File Offset: 0x005D667D
			public WalkReachabilityPainter()
			{
				this.floodFillPassCheckFunc = new Predicate<IntVec3>(this.FloodFillPassCheck);
				this.floodFillProcessorFunc = new Func<IntVec3, int, bool>(this.FloodFillProcessor);
			}

			// Token: 0x06010B4A RID: 68426 RVA: 0x005D84A9 File Offset: 0x005D66A9
			public void PaintWalkReachability(BreachingGrid breachingGrid)
			{
				this.breachingGrid = breachingGrid;
				breachingGrid.map.floodFiller.FloodFill(this.breachingGrid.breachStart, this.floodFillPassCheckFunc, this.floodFillProcessorFunc, int.MaxValue, false, null);
			}

			// Token: 0x06010B4B RID: 68427 RVA: 0x005D84E0 File Offset: 0x005D66E0
			private bool FloodFillProcessor(IntVec3 c, int traversalDist)
			{
				this.breachingGrid.reachableGrid[c] = true;
				return false;
			}

			// Token: 0x06010B4C RID: 68428 RVA: 0x005D84F5 File Offset: 0x005D66F5
			private bool FloodFillPassCheck(IntVec3 c)
			{
				return this.breachingGrid.WalkGrid[c] && !BreachingUtility.BlocksBreaching(this.breachingGrid.map, c);
			}

			// Token: 0x0400B295 RID: 45717
			private BreachingGrid breachingGrid;

			// Token: 0x0400B296 RID: 45718
			private readonly Predicate<IntVec3> floodFillPassCheckFunc;

			// Token: 0x0400B297 RID: 45719
			private readonly Func<IntVec3, int, bool> floodFillProcessorFunc;
		}
	}
}
