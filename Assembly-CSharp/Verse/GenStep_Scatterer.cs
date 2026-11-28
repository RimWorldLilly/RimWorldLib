using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002CF RID: 719
	public abstract class GenStep_Scatterer : GenStep
	{
		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060015F8 RID: 5624 RVA: 0x000825F0 File Offset: 0x000807F0
		private bool HasFallbackValidators
		{
			get
			{
				return this.fallbackValidators != null && this.fallbackValidators.Count > 0;
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0008260C File Offset: 0x0008080C
		public override void Generate(Map map, GenStepParams parms)
		{
			this.useFallback = false;
			if (this.ShouldSkipMap(map))
			{
				return;
			}
			this.usedSpots.Clear();
			int num = this.CalculateFinalCount(map);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					if (!this.HasFallbackValidators)
					{
						return;
					}
					this.useFallback = true;
					if (!this.TryFindScatterCell(map, out intVec))
					{
						return;
					}
				}
				this.ScatterAt(intVec, map, parms, 1);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00082694 File Offset: 0x00080894
		protected virtual bool ShouldSkipMap(Map map)
		{
			return (!this.allowInWaterBiome && map.TileInfo.WaterCovered) || (this.onlyOnStartingMap && !map.IsStartingMap) || (ModsConfig.BiotechActive && map.TileInfo.pollution < this.minPollution) || (ModsConfig.BiotechActive && Find.History.mechanoidDatacoreReadOrLost && !this.allowMechanoidDatacoreReadOrLost);
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00082704 File Offset: 0x00080904
		protected virtual bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.nearMapCenter)
			{
				if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => this.CanScatterAt(x, map), map, out result))
				{
					return true;
				}
			}
			else
			{
				if (this.nearPlayerStart)
				{
					result = CellFinder.RandomClosewalkCellNear(MapGenerator.PlayerStartSpot, map, 20, (IntVec3 x) => this.CanScatterAt(x, map));
					return true;
				}
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(5, (IntVec3 x) => this.CanScatterAt(x, map), map, out result))
				{
					return true;
				}
			}
			if (this.warnOnFail)
			{
				if (this.HasFallbackValidators && !this.useFallback)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Scatterer ");
					defaultInterpolatedStringHandler.AppendFormatted(this.ToString());
					defaultInterpolatedStringHandler.AppendLiteral(" from def ");
					defaultInterpolatedStringHandler.AppendFormatted(this.def.defName);
					defaultInterpolatedStringHandler.AppendLiteral(" could not find cell to generate at, trying fallback validators.");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(56, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Scatterer ");
					defaultInterpolatedStringHandler2.AppendFormatted(this.ToString());
					defaultInterpolatedStringHandler2.AppendLiteral(" from def ");
					defaultInterpolatedStringHandler2.AppendFormatted(this.def.defName);
					defaultInterpolatedStringHandler2.AppendLiteral(" could not find cell to generate at.");
					Log.Warning(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			return false;
		}

		// Token: 0x060015FC RID: 5628
		protected abstract void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1);

		// Token: 0x060015FD RID: 5629 RVA: 0x00082864 File Offset: 0x00080A64
		protected virtual bool CanScatterAt(IntVec3 loc, Map map)
		{
			using (List<LayoutStructureSketch>.Enumerator enumerator = map.layoutStructureSketches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.layoutSketch.OccupiedRect.Contains(loc))
					{
						return false;
					}
				}
			}
			if (this.extraNoBuildEdgeDist > 0 && loc.CloseToEdge(map, this.extraNoBuildEdgeDist + 10))
			{
				return false;
			}
			if (this.minEdgeDist > 0 && loc.CloseToEdge(map, this.minEdgeDist))
			{
				return false;
			}
			if (this.minEdgeDistPct > 0f && loc.CloseToEdge(map, (int)(this.minEdgeDistPct * (float)Mathf.Min(map.Size.x, map.Size.z))))
			{
				return false;
			}
			if (this.NearUsedSpot(loc, this.CalculateFinalMinSpacing(map)))
			{
				return false;
			}
			if (!this.useFallback)
			{
				if (this.minDistToPlayerStart > 0 && (map.Center - loc).LengthHorizontalSquared < this.minDistToPlayerStart * this.minDistToPlayerStart)
				{
					return false;
				}
				if (this.minDistToPlayerStartPct > 0f && (map.Center - loc).LengthHorizontal < this.minDistToPlayerStartPct * (float)Mathf.Min(map.Size.x, map.Size.z))
				{
					return false;
				}
			}
			if (this.spotMustBeStandable && !loc.Standable(map))
			{
				return false;
			}
			if (!this.allowFoggedPositions && loc.Fogged(map))
			{
				return false;
			}
			if (!this.allowRoofed && loc.Roofed(map))
			{
				return false;
			}
			if (this.useFallback)
			{
				if (this.fallbackValidators != null)
				{
					for (int i = 0; i < this.fallbackValidators.Count; i++)
					{
						if (!this.fallbackValidators[i].Allows(loc, map))
						{
							return false;
						}
					}
				}
			}
			else if (this.validators != null)
			{
				for (int j = 0; j < this.validators.Count; j++)
				{
					if (!this.validators[j].Allows(loc, map))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00082A88 File Offset: 0x00080C88
		protected bool NearUsedSpot(IntVec3 c, float dist)
		{
			for (int i = 0; i < this.usedSpots.Count; i++)
			{
				if ((float)(this.usedSpots[i] - c).LengthHorizontalSquared <= dist * dist)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00082ACE File Offset: 0x00080CCE
		protected virtual int CalculateFinalCount(Map map)
		{
			if (this.count < 0)
			{
				return Mathf.RoundToInt((float)GenStep_Scatterer.CountFromPer10kCells(this.countPer10kCellsRange.RandomInRange, map, -1) * this.GetPlacementFactor(map));
			}
			return Mathf.RoundToInt((float)this.count * this.GetPlacementFactor(map));
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00082B10 File Offset: 0x00080D10
		protected virtual float GetPlacementFactor(Map map)
		{
			float num = 1f;
			if (this.isJunk)
			{
				foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
				{
					num *= tileMutatorDef.junkDensityFactor;
				}
			}
			return num;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x00082B74 File Offset: 0x00080D74
		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				mapSize = map.Size.x;
			}
			int num = Mathf.RoundToInt(10000f / countPer10kCells);
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x00082BAC File Offset: 0x00080DAC
		public virtual float CalculateFinalMinSpacing(Map map)
		{
			float placementFactor = this.GetPlacementFactor(map);
			if (placementFactor <= 0f)
			{
				return 0f;
			}
			return this.minSpacing / placementFactor;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00082BD8 File Offset: 0x00080DD8
		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, default(GenStepParams), 1);
		}

		// Token: 0x040012F0 RID: 4848
		public int count = -1;

		// Token: 0x040012F1 RID: 4849
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x040012F2 RID: 4850
		public bool nearPlayerStart;

		// Token: 0x040012F3 RID: 4851
		public bool nearMapCenter;

		// Token: 0x040012F4 RID: 4852
		public float minSpacing = 10f;

		// Token: 0x040012F5 RID: 4853
		public bool spotMustBeStandable;

		// Token: 0x040012F6 RID: 4854
		public int minDistToPlayerStart;

		// Token: 0x040012F7 RID: 4855
		public float minDistToPlayerStartPct;

		// Token: 0x040012F8 RID: 4856
		public int minEdgeDist;

		// Token: 0x040012F9 RID: 4857
		public float minEdgeDistPct;

		// Token: 0x040012FA RID: 4858
		public int extraNoBuildEdgeDist;

		// Token: 0x040012FB RID: 4859
		public List<ScattererValidator> validators = new List<ScattererValidator>();

		// Token: 0x040012FC RID: 4860
		public List<ScattererValidator> fallbackValidators = new List<ScattererValidator>();

		// Token: 0x040012FD RID: 4861
		public bool allowInWaterBiome = true;

		// Token: 0x040012FE RID: 4862
		public bool allowFoggedPositions = true;

		// Token: 0x040012FF RID: 4863
		public bool allowRoofed = true;

		// Token: 0x04001300 RID: 4864
		public bool onlyOnStartingMap;

		// Token: 0x04001301 RID: 4865
		public float minPollution;

		// Token: 0x04001302 RID: 4866
		public bool allowMechanoidDatacoreReadOrLost = true;

		// Token: 0x04001303 RID: 4867
		public bool isJunk;

		// Token: 0x04001304 RID: 4868
		public bool warnOnFail = true;

		// Token: 0x04001305 RID: 4869
		[Unsaved(false)]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		// Token: 0x04001306 RID: 4870
		[Unsaved(false)]
		protected bool useFallback;

		// Token: 0x04001307 RID: 4871
		private const int ScatterNearPlayerRadius = 20;
	}
}
