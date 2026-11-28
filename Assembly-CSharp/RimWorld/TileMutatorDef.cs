using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001307 RID: 4871
	public class TileMutatorDef : Def
	{
		// Token: 0x170014A5 RID: 5285
		// (get) Token: 0x060078FB RID: 30971 RVA: 0x002AFD70 File Offset: 0x002ADF70
		public TileMutatorWorker Worker
		{
			get
			{
				if (this.mutatorWorker != null)
				{
					return this.mutatorWorker;
				}
				if (this.workerClass != null)
				{
					return this.mutatorWorker = (TileMutatorWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					});
				}
				return null;
			}
		}

		// Token: 0x170014A6 RID: 5286
		// (get) Token: 0x060078FC RID: 30972 RVA: 0x002AFDBF File Offset: 0x002ADFBF
		public bool IsCave
		{
			get
			{
				return this.categories.Contains("Caves");
			}
		}

		// Token: 0x060078FD RID: 30973 RVA: 0x002AFDD1 File Offset: 0x002ADFD1
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.biomeWhitelist != null)
			{
				this.biomeWhitelistSet = new HashSet<BiomeDef>(this.biomeWhitelist);
			}
			if (this.biomeBlacklist != null)
			{
				this.biomeBlacklistSet = new HashSet<BiomeDef>(this.biomeBlacklist);
			}
		}

		// Token: 0x060078FE RID: 30974 RVA: 0x002AFE0B File Offset: 0x002AE00B
		public string Label(PlanetTile tileID)
		{
			TileMutatorWorker worker = this.Worker;
			return ((worker != null) ? worker.GetLabel(tileID) : null) ?? this.label;
		}

		// Token: 0x060078FF RID: 30975 RVA: 0x002AFE2A File Offset: 0x002AE02A
		public string Description(PlanetTile tileID)
		{
			TileMutatorWorker worker = this.Worker;
			return ((worker != null) ? worker.GetDescription(tileID) : null) ?? this.description;
		}

		// Token: 0x06007900 RID: 30976 RVA: 0x002AFE49 File Offset: 0x002AE049
		public bool EverValid()
		{
			return this.requiresFactionOfDef == null || Find.FactionManager.FirstFactionOfDef(this.requiresFactionOfDef) != null;
		}

		// Token: 0x06007901 RID: 30977 RVA: 0x002AFE68 File Offset: 0x002AE068
		public bool IsValidTile(PlanetTile tile, PlanetLayer layer)
		{
			if (!this.EverValid())
			{
				return false;
			}
			Tile tile2 = layer[tile];
			BiomeDef primaryBiome = tile2.PrimaryBiome;
			if (tile2.Mutators.Contains(this))
			{
				return false;
			}
			if (this.Worker != null && !this.Worker.IsValidTile(tile, layer))
			{
				return false;
			}
			foreach (string item in this.categories)
			{
				foreach (TileMutatorDef tileMutatorDef in tile2.Mutators)
				{
					if (tileMutatorDef.categories.Contains(item) && tileMutatorDef.priority >= this.priority)
					{
						return false;
					}
					if (tileMutatorDef.overrideCategories.Contains(item))
					{
						return false;
					}
				}
			}
			if (!this.animalDensityRange.Includes(primaryBiome.animalDensity))
			{
				return false;
			}
			if (!this.plantDensityRange.Includes(primaryBiome.plantDensity))
			{
				return false;
			}
			if (!this.pollutionRange.Includes(tile2.pollution))
			{
				return false;
			}
			if (!this.averageTemperatureRange.Includes(tile2.temperature))
			{
				return false;
			}
			if (this.minHilliness != Hilliness.Undefined && tile2.hilliness < this.minHilliness)
			{
				return false;
			}
			if (this.maxHilliness != Hilliness.Undefined && tile2.hilliness > this.maxHilliness)
			{
				return false;
			}
			if (this.biomeWhitelistSet != null && !this.biomeWhitelistSet.Contains(primaryBiome))
			{
				return false;
			}
			if (this.biomeBlacklistSet != null && this.biomeBlacklistSet.Contains(primaryBiome))
			{
				return false;
			}
			if (this.coastSidesRange != IntRange.Invalid)
			{
				TileMutatorDef.tmpTileNeighbors.Clear();
				layer.GetTileNeighbors(tile, TileMutatorDef.tmpTileNeighbors);
				bool flag = this.coastSidesRange.max == 0;
				int num = 0;
				for (int i = 0; i < TileMutatorDef.tmpTileNeighbors.Count; i++)
				{
					BiomeDef primaryBiome2 = layer[TileMutatorDef.tmpTileNeighbors[i]].PrimaryBiome;
					if (primaryBiome2 == BiomeDefOf.Ocean || primaryBiome2 == BiomeDefOf.Lake)
					{
						if (flag)
						{
							return false;
						}
						num++;
					}
				}
				if (!this.coastSidesRange.Includes(num))
				{
					return false;
				}
			}
			SurfaceTile surfaceTile = tile2 as SurfaceTile;
			if (surfaceTile != null)
			{
				if (!surfaceTile.Rivers.NullOrEmpty<SurfaceTile.RiverLink>() && !this.canSpawnOnRiver)
				{
					return false;
				}
				if (!surfaceTile.Roads.NullOrEmpty<SurfaceTile.RoadLink>() && !this.canSpawnOnRoad)
				{
					return false;
				}
			}
			return tile2.Landmark == null || this.canSpawnOnLandmark;
		}

		// Token: 0x04004BBF RID: 19391
		private Type workerClass;

		// Token: 0x04004BC0 RID: 19392
		public int genOrder;

		// Token: 0x04004BC1 RID: 19393
		[NoTranslate]
		public List<string> categories = new List<string>();

		// Token: 0x04004BC2 RID: 19394
		[NoTranslate]
		public List<string> overrideCategories = new List<string>();

		// Token: 0x04004BC3 RID: 19395
		public int priority;

		// Token: 0x04004BC4 RID: 19396
		public int displayPriority;

		// Token: 0x04004BC5 RID: 19397
		public float chanceOnNonLandmarkTile;

		// Token: 0x04004BC6 RID: 19398
		public List<GenStepDef> extraGenSteps = new List<GenStepDef>();

		// Token: 0x04004BC7 RID: 19399
		public List<GenStepDef> preventGenSteps = new List<GenStepDef>();

		// Token: 0x04004BC8 RID: 19400
		public TerrainDef overrideCoastalBeachTerrain;

		// Token: 0x04004BC9 RID: 19401
		public TerrainDef overrideLakeBeachTerrain;

		// Token: 0x04004BCA RID: 19402
		public TerrainDef overrideRiverbankTerrain;

		// Token: 0x04004BCB RID: 19403
		public TerrainDef overrideMudTerrain;

		// Token: 0x04004BCC RID: 19404
		public bool preventsPondGeneration;

		// Token: 0x04004BCD RID: 19405
		public bool preventsLandmarks;

		// Token: 0x04004BCE RID: 19406
		public bool preventNaturalElevation;

		// Token: 0x04004BCF RID: 19407
		public bool preventPatches;

		// Token: 0x04004BD0 RID: 19408
		public float animalDensityFactor = 1f;

		// Token: 0x04004BD1 RID: 19409
		public float plantDensityFactor = 1f;

		// Token: 0x04004BD2 RID: 19410
		public float junkDensityFactor = 1f;

		// Token: 0x04004BD3 RID: 19411
		public float geyserCountFactor = 1f;

		// Token: 0x04004BD4 RID: 19412
		public float chunkDensityFactor = 1f;

		// Token: 0x04004BD5 RID: 19413
		public float fishPopulationFactor = 1f;

		// Token: 0x04004BD6 RID: 19414
		public List<BiomePlantRecord> additionalWildPlants = new List<BiomePlantRecord>();

		// Token: 0x04004BD7 RID: 19415
		public SimpleCurve overrideDensityForFertilityCurve;

		// Token: 0x04004BD8 RID: 19416
		public bool allowRoofedEdgeWalkIn;

		// Token: 0x04004BD9 RID: 19417
		public List<RaidStrategyDef> blacklistedRaidStrategies = new List<RaidStrategyDef>();

		// Token: 0x04004BDA RID: 19418
		public List<GameConditionDef> additionalGameConditions = new List<GameConditionDef>();

		// Token: 0x04004BDB RID: 19419
		public Hilliness hillinessForElevationGen;

		// Token: 0x04004BDC RID: 19420
		public Hilliness hillinessForOreGeneration;

		// Token: 0x04004BDD RID: 19421
		public Hilliness hillinessLabel;

		// Token: 0x04004BDE RID: 19422
		public List<BiomeDef> biomeWhitelist;

		// Token: 0x04004BDF RID: 19423
		public List<BiomeDef> biomeBlacklist;

		// Token: 0x04004BE0 RID: 19424
		public FloatRange animalDensityRange = new FloatRange(0f, float.MaxValue);

		// Token: 0x04004BE1 RID: 19425
		public FloatRange plantDensityRange = new FloatRange(0f, float.MaxValue);

		// Token: 0x04004BE2 RID: 19426
		public Hilliness minHilliness;

		// Token: 0x04004BE3 RID: 19427
		public Hilliness maxHilliness;

		// Token: 0x04004BE4 RID: 19428
		public IntRange coastSidesRange = IntRange.Invalid;

		// Token: 0x04004BE5 RID: 19429
		public bool canSpawnOnRiver = true;

		// Token: 0x04004BE6 RID: 19430
		public bool canSpawnOnRoad = true;

		// Token: 0x04004BE7 RID: 19431
		public bool canSpawnOnLandmark = true;

		// Token: 0x04004BE8 RID: 19432
		public FactionDef requiresFactionOfDef;

		// Token: 0x04004BE9 RID: 19433
		public FloatRange pollutionRange = new FloatRange(0f, float.MaxValue);

		// Token: 0x04004BEA RID: 19434
		public FloatRange averageTemperatureRange = new FloatRange(float.MinValue, float.MaxValue);

		// Token: 0x04004BEB RID: 19435
		public float weatherFrequencyFactor = 1f;

		// Token: 0x04004BEC RID: 19436
		public float weatherFrequencyOffset;

		// Token: 0x04004BED RID: 19437
		public List<WeatherDef> weathersToAffect = new List<WeatherDef>();

		// Token: 0x04004BEE RID: 19438
		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		// Token: 0x04004BEF RID: 19439
		public List<ThingDef> plantKinds = new List<ThingDef>();

		// Token: 0x04004BF0 RID: 19440
		public AncientStructureGenParms structureGenParms;

		// Token: 0x04004BF1 RID: 19441
		public List<ThingDef> resourceBlacklist = new List<ThingDef>();

		// Token: 0x04004BF2 RID: 19442
		private HashSet<BiomeDef> biomeWhitelistSet;

		// Token: 0x04004BF3 RID: 19443
		private HashSet<BiomeDef> biomeBlacklistSet;

		// Token: 0x04004BF4 RID: 19444
		private TileMutatorWorker mutatorWorker;

		// Token: 0x04004BF5 RID: 19445
		private static readonly List<PlanetTile> tmpTileNeighbors = new List<PlanetTile>();
	}
}
