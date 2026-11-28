using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x0200233C RID: 9020
	public sealed class World : IThingHolder, IExposable, IIncidentTarget, ILoadReferenceable, IDisposable
	{
		// Token: 0x1700271F RID: 10015
		// (get) Token: 0x0600DEDE RID: 57054 RVA: 0x0052E839 File Offset: 0x0052CA39
		public float PlanetCoverage
		{
			get
			{
				return this.info.planetCoverage;
			}
		}

		// Token: 0x17002720 RID: 10016
		// (get) Token: 0x0600DEDF RID: 57055 RVA: 0x0052E846 File Offset: 0x0052CA46
		public IThingHolder ParentHolder
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002721 RID: 10017
		// (get) Token: 0x0600DEE0 RID: 57056 RVA: 0x0052E849 File Offset: 0x0052CA49
		public PlanetTile Tile
		{
			get
			{
				return PlanetTile.Invalid;
			}
		}

		// Token: 0x17002722 RID: 10018
		// (get) Token: 0x0600DEE1 RID: 57057 RVA: 0x0052E850 File Offset: 0x0052CA50
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17002723 RID: 10019
		// (get) Token: 0x0600DEE2 RID: 57058 RVA: 0x0052E858 File Offset: 0x0052CA58
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x17002724 RID: 10020
		// (get) Token: 0x0600DEE3 RID: 57059 RVA: 0x0052E860 File Offset: 0x0052CA60
		public float PlayerWealthForStoryteller
		{
			get
			{
				float num = 0f;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					num += maps[i].PlayerWealthForStoryteller;
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					num += caravans[j].PlayerWealthForStoryteller;
				}
				return num;
			}
		}

		// Token: 0x17002725 RID: 10021
		// (get) Token: 0x0600DEE4 RID: 57060 RVA: 0x0052E8CA File Offset: 0x0052CACA
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction;
			}
		}

		// Token: 0x17002726 RID: 10022
		// (get) Token: 0x0600DEE5 RID: 57061 RVA: 0x0052E8D1 File Offset: 0x0052CAD1
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x0600DEE6 RID: 57062 RVA: 0x0052E8D8 File Offset: 0x0052CAD8
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			return new World.<IncidentTargetTags>d__41(-2);
		}

		// Token: 0x0600DEE7 RID: 57063 RVA: 0x0052E8E4 File Offset: 0x0052CAE4
		public void ExposeData()
		{
			Scribe_Deep.Look<WorldInfo>(ref this.info, "info", Array.Empty<object>());
			Scribe_Deep.Look<WorldGrid>(ref this.grid, "grid", Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				this.ExposeComponents();
				return;
			}
			if (this.grid == null || !this.grid.HasWorldData)
			{
				WorldGenerator.GenerateWithoutWorldData(this.info.seedString);
				return;
			}
			WorldGenerator.GenerateFromScribe(this.info.seedString);
		}

		// Token: 0x0600DEE8 RID: 57064 RVA: 0x0052E960 File Offset: 0x0052CB60
		public void ExposeComponents()
		{
			Scribe_Deep.Look<FactionManager>(ref this.factionManager, "factionManager", Array.Empty<object>());
			Scribe_Deep.Look<IdeoManager>(ref this.ideoManager, "ideoManager", Array.Empty<object>());
			Scribe_Deep.Look<WorldPawns>(ref this.worldPawns, "worldPawns", Array.Empty<object>());
			Scribe_Deep.Look<WorldObjectsHolder>(ref this.worldObjects, "worldObjects", Array.Empty<object>());
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<WorldFeatures>(ref this.features, "features", Array.Empty<object>());
			Scribe_Deep.Look<WorldLandmarks>(ref this.landmarks, "landmarks", Array.Empty<object>());
			Scribe_Collections.Look<WorldComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			Scribe_Collections.Look<PocketMapParent>(ref this.pocketMaps, "pocketMaps", LookMode.Deep, Array.Empty<object>());
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.pocketMaps == null)
				{
					this.pocketMaps = new List<PocketMapParent>();
				}
				if (this.landmarks == null)
				{
					this.landmarks = new WorldLandmarks();
				}
				if (ModsConfig.OdysseyActive && this.landmarks.landmarks.NullOrEmpty<PlanetTile, Landmark>())
				{
					foreach (Tile tile in this.grid.Surface.Tiles)
					{
						List<TileMutatorDef> mutatorsNullable = tile.mutatorsNullable;
						if (mutatorsNullable != null)
						{
							mutatorsNullable.Clear();
						}
					}
					WorldGenStep_Mutators.AddMutatorsFromTile(this.grid.Surface);
					new WorldGenStep_Landmarks().GenerateFresh(this.info.seedString, this.grid.Surface);
				}
			}
		}

		// Token: 0x0600DEE9 RID: 57065 RVA: 0x0052EB34 File Offset: 0x0052CD34
		public void ConstructComponents()
		{
			this.worldObjects = new WorldObjectsHolder();
			this.factionManager = new FactionManager();
			this.ideoManager = new IdeoManager();
			this.worldPawns = new WorldPawns();
			this.gameConditionManager = new GameConditionManager(this);
			this.storyState = new StoryState(this);
			this.renderer = new WorldRenderer();
			this.UI = new WorldInterface();
			this.debugDrawer = new WorldDebugDrawer();
			this.dynamicDrawManager = new WorldDynamicDrawManager();
			this.pathPool = new WorldPathPool();
			this.reachability = new WorldReachability();
			this.ticksAbsCache = new ConfiguredTicksAbsAtGameStartCache();
			this.tilesInRandomOrder = new WorldTilesInRandomOrder();
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x0600DEEA RID: 57066 RVA: 0x0052EBF0 File Offset: 0x0052CDF0
		private void FillComponents()
		{
			this.components.RemoveAll((WorldComponent component) => component == null);
			foreach (Type type in typeof(WorldComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						WorldComponent item = (WorldComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate a WorldComponent of type ";
						Type type2 = type;
						string str2 = (type2 != null) ? type2.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			this.tileTemperatures = this.GetComponent<TileTemperaturesComp>();
			this.genData = this.GetComponent<WorldGenData>();
		}

		// Token: 0x0600DEEB RID: 57067 RVA: 0x0052ECF4 File Offset: 0x0052CEF4
		public void FinalizeInit(bool fromLoad)
		{
			this.pathGrid.RecalculateAllLayersPathCosts();
			AmbientSoundManager.EnsureWorldAmbientSoundCreated();
			WorldComponentUtility.FinalizeInit(this, fromLoad);
		}

		// Token: 0x0600DEEC RID: 57068 RVA: 0x0052ED10 File Offset: 0x0052CF10
		public void WorldTick()
		{
			this.worldPawns.WorldPawnsTick();
			this.factionManager.FactionManagerTick();
			this.worldObjects.WorldObjectsHolderTick();
			this.debugDrawer.WorldDebugDrawerTick();
			this.pathGrid.WorldPathGridTick();
			WorldComponentUtility.WorldComponentTick(this);
			this.ideoManager.IdeoManagerTick();
		}

		// Token: 0x0600DEED RID: 57069 RVA: 0x0052ED68 File Offset: 0x0052CF68
		public void WorldPostTick()
		{
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
		}

		// Token: 0x0600DEEE RID: 57070 RVA: 0x0052EDA0 File Offset: 0x0052CFA0
		public void WorldUpdate()
		{
			bool worldRendered = WorldRendererUtility.WorldRendered;
			this.renderer.CheckActivateWorldCamera();
			if (worldRendered)
			{
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsUpdate();
				if (ModsConfig.OdysseyActive)
				{
					ExpandableLandmarksUtility.ExpandableLandmarksUpdate();
				}
				this.renderer.DrawWorldLayers();
				this.dynamicDrawManager.DrawDynamicWorldObjects();
				this.features.UpdateFeatures();
				NoiseDebugUI.RenderPlanetNoise();
			}
			WorldComponentUtility.WorldComponentUpdate(this);
			if (ModsConfig.BiotechActive)
			{
				try
				{
					CompDissolutionEffect_Goodwill.WorldUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
				try
				{
					CompDissolutionEffect_Pollution.WorldUpdate();
				}
				catch (Exception ex2)
				{
					Log.Error(ex2.ToString());
				}
			}
		}

		// Token: 0x0600DEEF RID: 57071 RVA: 0x0052EE48 File Offset: 0x0052D048
		public void WorldOnGUI()
		{
			WorldComponentUtility.WorldComponentOnGUI(this);
		}

		// Token: 0x0600DEF0 RID: 57072 RVA: 0x0052EE50 File Offset: 0x0052D050
		public T GetComponent<T>() where T : WorldComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				T t = this.components[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600DEF1 RID: 57073 RVA: 0x0052EEA0 File Offset: 0x0052D0A0
		public WorldComponent GetComponent(Type type)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (type.IsInstanceOfType(this.components[i]))
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x0600DEF2 RID: 57074 RVA: 0x0052EEE8 File Offset: 0x0052D0E8
		public float? CoastAngleAt(PlanetTile tile, BiomeDef waterBiome)
		{
			World.tmpNeighbors.Clear();
			Find.World.grid.GetTileNeighbors(tile, World.tmpNeighbors);
			IEnumerable<PlanetTile> source = from t in World.tmpNeighbors
			where t.Tile.PrimaryBiome == waterBiome
			select t;
			if (!source.Any<PlanetTile>())
			{
				return null;
			}
			float num = GenMath.MeanAngle((from t in source
			select Find.WorldGrid.GetHeadingFromTo(t, tile)).ToList<float>());
			return new float?((450f - num) % 360f);
		}

		// Token: 0x0600DEF3 RID: 57075 RVA: 0x0052EF88 File Offset: 0x0052D188
		public Rot4 CoastDirectionAt(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return Rot4.Invalid;
			}
			if (!tile.Tile.PrimaryBiome.canBuildBase)
			{
				return Rot4.Invalid;
			}
			World.tTpOceanDirs.Clear();
			this.grid.GetTileNeighbors(tile, World.tmpNeighbors);
			int i = 0;
			int count = World.tmpNeighbors.Count;
			while (i < count)
			{
				if (this.grid[World.tmpNeighbors[i]].PrimaryBiome == BiomeDefOf.Ocean)
				{
					Rot4 rotFromTo = this.grid.GetRotFromTo(tile, World.tmpNeighbors[i]);
					if (!World.tTpOceanDirs.Contains(rotFromTo))
					{
						World.tTpOceanDirs.Add(rotFromTo);
					}
				}
				i++;
			}
			if (World.tTpOceanDirs.Count == 0)
			{
				return Rot4.Invalid;
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			int index = Rand.Range(0, World.tTpOceanDirs.Count);
			Rand.PopState();
			return World.tTpOceanDirs[index];
		}

		// Token: 0x0600DEF4 RID: 57076 RVA: 0x0052F08C File Offset: 0x0052D28C
		public Rot4 LakeDirectionAt(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return Rot4.Invalid;
			}
			if (!tile.Tile.PrimaryBiome.canBuildBase)
			{
				return Rot4.Invalid;
			}
			World.tTpOceanDirs.Clear();
			this.grid.GetTileNeighbors(tile, World.tmpNeighbors);
			int i = 0;
			int count = World.tmpNeighbors.Count;
			while (i < count)
			{
				if (this.grid[World.tmpNeighbors[i]].PrimaryBiome == BiomeDefOf.Lake)
				{
					Rot4 rotFromTo = this.grid.GetRotFromTo(tile, World.tmpNeighbors[i]);
					if (!World.tTpOceanDirs.Contains(rotFromTo))
					{
						World.tTpOceanDirs.Add(rotFromTo);
					}
				}
				i++;
			}
			if (World.tTpOceanDirs.Count == 0)
			{
				return Rot4.Invalid;
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			int index = Rand.Range(0, World.tTpOceanDirs.Count);
			Rand.PopState();
			return World.tTpOceanDirs[index];
		}

		// Token: 0x0600DEF5 RID: 57077 RVA: 0x0052F18F File Offset: 0x0052D38F
		public bool HasCaves(PlanetTile tile)
		{
			return tile.Tile.Mutators.Any((TileMutatorDef m) => m.IsCave);
		}

		// Token: 0x0600DEF6 RID: 57078 RVA: 0x0052F1C4 File Offset: 0x0052D3C4
		public IEnumerable<ThingDef> NaturalRockTypesIn(PlanetTile tile)
		{
			if (tile.Valid)
			{
				List<ThingDef> forceRockTypes = tile.Tile.PrimaryBiome.forceRockTypes;
				if (forceRockTypes != null)
				{
					return forceRockTypes;
				}
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			if (this.allNaturalRockDefs == null)
			{
				this.allNaturalRockDefs = (from def in DefDatabase<ThingDef>.AllDefs
				where def.IsNonResourceNaturalRock
				select def).ToList<ThingDef>();
			}
			int num = Rand.RangeInclusive(2, 3);
			if (num > this.allNaturalRockDefs.Count)
			{
				num = this.allNaturalRockDefs.Count;
			}
			World.tmpNaturalRockDefs.Clear();
			World.tmpNaturalRockDefs.AddRange(this.allNaturalRockDefs);
			IEnumerable<ThingDef> result = (from def in World.tmpNaturalRockDefs
			where World.RockAllowedInBiome(def, tile)
			select def).TakeRandomDistinct(num);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600DEF7 RID: 57079 RVA: 0x0052F2BA File Offset: 0x0052D4BA
		private static bool RockAllowedInBiome(ThingDef def, PlanetTile tile)
		{
			return !def.building.biomeSpecific || (tile.Valid && tile.Tile.PrimaryBiome.extraRockTypes.NotNullAndContains(def));
		}

		// Token: 0x0600DEF8 RID: 57080 RVA: 0x0052F2ED File Offset: 0x0052D4ED
		public bool Impassable(PlanetTile tileID)
		{
			return !this.pathGrid.Passable(tileID);
		}

		// Token: 0x0600DEF9 RID: 57081 RVA: 0x0052F2FE File Offset: 0x0052D4FE
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x0600DEFA RID: 57082 RVA: 0x0052F304 File Offset: 0x0052D504
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			List<WorldObject> allWorldObjects = this.worldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				IThingHolder thingHolder = allWorldObjects[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
				List<WorldObjectComp> allComps = allWorldObjects[i].AllComps;
				for (int j = 0; j < allComps.Count; j++)
				{
					IThingHolder thingHolder2 = allComps[j] as IThingHolder;
					if (thingHolder2 != null)
					{
						outChildren.Add(thingHolder2);
					}
				}
			}
			for (int k = 0; k < this.components.Count; k++)
			{
				IThingHolder thingHolder3 = this.components[k] as IThingHolder;
				if (thingHolder3 != null)
				{
					outChildren.Add(thingHolder3);
				}
			}
		}

		// Token: 0x0600DEFB RID: 57083 RVA: 0x0052F3C8 File Offset: 0x0052D5C8
		public string GetUniqueLoadID()
		{
			return "World";
		}

		// Token: 0x17002727 RID: 10023
		// (get) Token: 0x0600DEFC RID: 57084 RVA: 0x0052F3CF File Offset: 0x0052D5CF
		public int ConstantRandSeed
		{
			get
			{
				return this.info.persistentRandomValue;
			}
		}

		// Token: 0x0600DEFD RID: 57085 RVA: 0x0052F3DC File Offset: 0x0052D5DC
		public override string ToString()
		{
			return "World-" + this.info.name;
		}

		// Token: 0x0600DEFE RID: 57086 RVA: 0x0052F3F3 File Offset: 0x0052D5F3
		public void Dispose()
		{
			WorldGrid worldGrid = this.grid;
			if (worldGrid == null)
			{
				return;
			}
			worldGrid.Dispose();
		}

		// Token: 0x04008F14 RID: 36628
		public WorldInfo info = new WorldInfo();

		// Token: 0x04008F15 RID: 36629
		public List<WorldComponent> components = new List<WorldComponent>();

		// Token: 0x04008F16 RID: 36630
		public FactionManager factionManager;

		// Token: 0x04008F17 RID: 36631
		public IdeoManager ideoManager;

		// Token: 0x04008F18 RID: 36632
		public WorldPawns worldPawns;

		// Token: 0x04008F19 RID: 36633
		public WorldObjectsHolder worldObjects;

		// Token: 0x04008F1A RID: 36634
		public GameConditionManager gameConditionManager;

		// Token: 0x04008F1B RID: 36635
		public StoryState storyState;

		// Token: 0x04008F1C RID: 36636
		public WorldFeatures features;

		// Token: 0x04008F1D RID: 36637
		public WorldLandmarks landmarks;

		// Token: 0x04008F1E RID: 36638
		public List<PocketMapParent> pocketMaps = new List<PocketMapParent>();

		// Token: 0x04008F1F RID: 36639
		public WorldGrid grid;

		// Token: 0x04008F20 RID: 36640
		public WorldPathGrid pathGrid;

		// Token: 0x04008F21 RID: 36641
		public WorldRenderer renderer;

		// Token: 0x04008F22 RID: 36642
		public WorldInterface UI;

		// Token: 0x04008F23 RID: 36643
		public WorldDebugDrawer debugDrawer;

		// Token: 0x04008F24 RID: 36644
		public WorldDynamicDrawManager dynamicDrawManager;

		// Token: 0x04008F25 RID: 36645
		public WorldPathPool pathPool;

		// Token: 0x04008F26 RID: 36646
		public WorldReachability reachability;

		// Token: 0x04008F27 RID: 36647
		public WorldTilesInRandomOrder tilesInRandomOrder;

		// Token: 0x04008F28 RID: 36648
		public ConfiguredTicksAbsAtGameStartCache ticksAbsCache;

		// Token: 0x04008F29 RID: 36649
		public TileTemperaturesComp tileTemperatures;

		// Token: 0x04008F2A RID: 36650
		public WorldGenData genData;

		// Token: 0x04008F2B RID: 36651
		private List<ThingDef> allNaturalRockDefs;

		// Token: 0x04008F2C RID: 36652
		private static readonly List<ThingDef> tmpNaturalRockDefs = new List<ThingDef>();

		// Token: 0x04008F2D RID: 36653
		private static readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x04008F2E RID: 36654
		private static readonly List<Rot4> tTpOceanDirs = new List<Rot4>();
	}
}
