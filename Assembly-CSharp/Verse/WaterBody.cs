using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C4 RID: 196
	public class WaterBody : IExposable
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0002BEF2 File Offset: 0x0002A0F2
		public bool HasFish
		{
			get
			{
				return (this.commonFish.Count > 0 || this.uncommonFish.Count > 0) && this.population > 0f;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x0002BF1F File Offset: 0x0002A11F
		public float PollutionPct
		{
			get
			{
				return (float)this.numCellsPolluted / (float)this.cellCount;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0002BF30 File Offset: 0x0002A130
		public bool TotallyFrozen
		{
			get
			{
				return this.numCellsFrozen == this.cells.Count;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0002BF45 File Offset: 0x0002A145
		public int Size
		{
			get
			{
				return this.cellCount;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0002BF4D File Offset: 0x0002A14D
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x0002BF55 File Offset: 0x0002A155
		public float Population
		{
			get
			{
				return this.population;
			}
			set
			{
				this.population = value;
				this.population = Mathf.Clamp(this.population, 0f, this.MaxPopulation);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0002BF7A File Offset: 0x0002A17A
		public float MaxPopulation
		{
			get
			{
				return Mathf.Max(this.map.TileInfo.MaxFishPopulation * this.PopulationFactor, 20f);
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0002BFA0 File Offset: 0x0002A1A0
		public float PopulationFactor
		{
			get
			{
				if (this.cachedPopulationFactor < 0f)
				{
					this.cachedPopulationFactor = FishingUtility.FishPopulationFactorPerBodySizeCurve.Evaluate((float)this.Size);
					if (this.cachedPopulationFactor < 0f)
					{
						this.cachedPopulationFactor = 0f;
					}
				}
				return this.cachedPopulationFactor;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0002BFEF File Offset: 0x0002A1EF
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x0002BFF7 File Offset: 0x0002A1F7
		public int CellCount
		{
			get
			{
				return this.cellCount;
			}
			set
			{
				this.cellCount = value;
				this.cachedPopulationFactor = -1f;
				this.cachedBounds = null;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0002C018 File Offset: 0x0002A218
		public CellRect Bounds
		{
			get
			{
				CellRect value = this.cachedBounds.GetValueOrDefault();
				if (this.cachedBounds == null)
				{
					value = CellRect.FromCellList(this.cells);
					this.cachedBounds = new CellRect?(value);
				}
				return this.cachedBounds.Value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0002C061 File Offset: 0x0002A261
		public IEnumerable<ThingDef> CommonFish
		{
			get
			{
				return this.commonFish;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0002C069 File Offset: 0x0002A269
		public IEnumerable<ThingDef> UncommonFish
		{
			get
			{
				return this.uncommonFish;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0002C071 File Offset: 0x0002A271
		public IEnumerable<ThingDef> CommonFishIncludingExtras
		{
			get
			{
				WaterBody.<get_CommonFishIncludingExtras>d__39 <get_CommonFishIncludingExtras>d__ = new WaterBody.<get_CommonFishIncludingExtras>d__39(-2);
				<get_CommonFishIncludingExtras>d__.<>4__this = this;
				return <get_CommonFishIncludingExtras>d__;
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002C081 File Offset: 0x0002A281
		public WaterBody()
		{
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002C0B8 File Offset: 0x0002A2B8
		public WaterBody(Map map, IntVec3 rootCell)
		{
			this.map = map;
			this.rootCell = rootCell;
			this.cellCount = 1;
			this.waterBodyType = rootCell.GetWaterBodyType(map);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0002C119 File Offset: 0x0002A319
		public void Initialize()
		{
			this.InitializePopulation();
			this.SetFishTypes();
			this.RecacheState();
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002C12D File Offset: 0x0002A32D
		public void InitializePopulation()
		{
			this.population = this.MaxPopulation;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0002C13C File Offset: 0x0002A33C
		public void SetFishTypes()
		{
			if (this.map.Biome.fishTypes == null)
			{
				return;
			}
			this.commonFish.Clear();
			this.uncommonFish.Clear();
			this.shouldHaveFish = Rand.Chance(FishingUtility.ChanceForCommonFishFromWaterBodySizeCurve.Evaluate((float)this.Size));
			if (!this.shouldHaveFish)
			{
				return;
			}
			bool flag = Rand.Chance(FishingUtility.ChanceForUncommonFishFromWaterBodySizeCurve.Evaluate((float)this.Size));
			WaterBodyType waterBodyType = this.waterBodyType;
			if (waterBodyType != WaterBodyType.Freshwater)
			{
				if (waterBodyType != WaterBodyType.Saltwater)
				{
					return;
				}
				FishChance fishChance;
				if (this.map.Biome.fishTypes.saltwater_Common.TryRandomElement(out fishChance))
				{
					this.commonFish.Add(fishChance.fishDef);
				}
				FishChance fishChance2;
				if (flag && this.map.Biome.fishTypes.saltwater_Uncommon.TryRandomElement(out fishChance2))
				{
					this.uncommonFish.Add(fishChance2.fishDef);
				}
			}
			else
			{
				FishChance fishChance3;
				if (this.map.Biome.fishTypes.freshwater_Common.TryRandomElement(out fishChance3))
				{
					this.commonFish.Add(fishChance3.fishDef);
				}
				FishChance fishChance4;
				if (flag && this.map.Biome.fishTypes.freshwater_Uncommon.TryRandomElement(out fishChance4))
				{
					this.uncommonFish.Add(fishChance4.fishDef);
					return;
				}
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0002C288 File Offset: 0x0002A488
		public void RecacheState()
		{
			this.numCellsPolluted = 0;
			this.numCellsFrozen = 0;
			this.numCellsCanFreeze = 0;
			foreach (IntVec3 c in this.cells)
			{
				if (c.GetTerrain(this.map) == TerrainDefOf.ThinIce)
				{
					this.numCellsFrozen++;
				}
				if (this.map.terrainGrid.BaseTerrainAt(c).canFreeze)
				{
					this.numCellsCanFreeze++;
				}
				if (c.IsPolluted(this.map))
				{
					this.numCellsPolluted++;
				}
			}
			if (this.shouldHaveFish && this.commonFish.NullOrEmpty<ThingDef>())
			{
				this.SetFishTypes();
			}
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0002C368 File Offset: 0x0002A568
		public void Notify_TerrainChanged(IntVec3 cell, TerrainDef oldTerr, TerrainDef newTerr)
		{
			if (!this.cells.Contains(cell))
			{
				return;
			}
			if (newTerr == TerrainDefOf.ThinIce && oldTerr != TerrainDefOf.ThinIce)
			{
				this.numCellsFrozen++;
			}
			if (newTerr != TerrainDefOf.ThinIce && oldTerr == TerrainDefOf.ThinIce)
			{
				this.numCellsFrozen--;
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0002C3C0 File Offset: 0x0002A5C0
		public void Notify_PollutionChanged(IntVec3 cell, bool isPolluted)
		{
			if (!this.cells.Contains(cell))
			{
				return;
			}
			if (isPolluted)
			{
				this.numCellsPolluted++;
				return;
			}
			this.numCellsPolluted--;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002C3F1 File Offset: 0x0002A5F1
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(base.GetHashCode(), this.rootCell.GetHashCode()), this.cellCount), this.waterBodyType.GetHashCode());
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0002C430 File Offset: 0x0002A630
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.rootCell, "rootCell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.cellCount, "cellCount", 0, false);
			Scribe_Values.Look<WaterBodyType>(ref this.waterBodyType, "fishType", WaterBodyType.None, false);
			Scribe_Values.Look<float>(ref this.population, "population", 0f, false);
			Scribe_Values.Look<bool>(ref this.shouldHaveFish, "shouldHaveFish", false, false);
			Scribe_Collections.Look<ThingDef>(ref this.commonFish, "commonFish", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.uncommonFish, "uncommonFish", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.commonFish.NullOrEmpty<ThingDef>())
				{
					this.shouldHaveFish = true;
				}
				if (this.shouldHaveFish && this.commonFish == null)
				{
					this.commonFish = new List<ThingDef>();
					this.uncommonFish = new List<ThingDef>();
					this.SetFishTypes();
				}
				if (this.shouldHaveFish && this.Population > 0f && this.commonFish.Count == 0)
				{
					this.SetFishTypes();
				}
				this.commonFish.RemoveAll((ThingDef x) => x == null);
				this.uncommonFish.RemoveAll((ThingDef x) => x == null);
			}
		}

		// Token: 0x04000355 RID: 853
		public Map map;

		// Token: 0x04000356 RID: 854
		public IntVec3 rootCell;

		// Token: 0x04000357 RID: 855
		public WaterBodyType waterBodyType;

		// Token: 0x04000358 RID: 856
		[Unsaved(false)]
		public HashSet<IntVec3> cells = new HashSet<IntVec3>();

		// Token: 0x04000359 RID: 857
		[Unsaved(false)]
		public CellRect? cachedBounds;

		// Token: 0x0400035A RID: 858
		private bool shouldHaveFish;

		// Token: 0x0400035B RID: 859
		private List<ThingDef> commonFish = new List<ThingDef>();

		// Token: 0x0400035C RID: 860
		private List<ThingDef> uncommonFish = new List<ThingDef>();

		// Token: 0x0400035D RID: 861
		private int cellCount;

		// Token: 0x0400035E RID: 862
		private float population;

		// Token: 0x0400035F RID: 863
		[Unsaved(false)]
		private float cachedPopulationFactor = -1f;

		// Token: 0x04000360 RID: 864
		public int numCellsFrozen;

		// Token: 0x04000361 RID: 865
		public int numCellsPolluted;

		// Token: 0x04000362 RID: 866
		public int numCellsCanFreeze;
	}
}
