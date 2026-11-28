using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000299 RID: 665
	public sealed class Map : IIncidentTarget, ILoadReferenceable, IThingHolder, IExposable, IDisposable
	{
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x00072DC8 File Offset: 0x00070FC8
		public int Index
		{
			get
			{
				return Find.Maps.IndexOf(this);
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x00072DD5 File Offset: 0x00070FD5
		public IntVec3 Size
		{
			get
			{
				return this.info.Size;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x00072DE2 File Offset: 0x00070FE2
		public IntVec3 Center
		{
			get
			{
				return new IntVec3(this.Size.x / 2, 0, this.Size.z / 2);
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x00072E04 File Offset: 0x00071004
		public Faction ParentFaction
		{
			get
			{
				MapParent parent = this.info.parent;
				if (parent == null)
				{
					return null;
				}
				return parent.Faction;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x00072E1C File Offset: 0x0007101C
		public int Area
		{
			get
			{
				return this.Size.x * this.Size.z;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x00072E35 File Offset: 0x00071035
		public IThingHolder ParentHolder
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x00072E42 File Offset: 0x00071042
		public bool DrawMapClippers
		{
			get
			{
				return !this.generatorDef.disableMapClippers;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x00072E52 File Offset: 0x00071052
		public bool CanEverExit
		{
			get
			{
				return !this.info.isPocketMap && this.Biome.canExitMap;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x00072E70 File Offset: 0x00071070
		// (set) Token: 0x0600143A RID: 5178 RVA: 0x00072E9A File Offset: 0x0007109A
		public Color? FogOfWarColor
		{
			get
			{
				Color? result = this.fogOfWarColor;
				if (result == null)
				{
					return this.Biome.fogOfWarColor;
				}
				return result;
			}
			set
			{
				this.fogOfWarColor = value;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x00072EA3 File Offset: 0x000710A3
		// (set) Token: 0x0600143C RID: 5180 RVA: 0x00072EBA File Offset: 0x000710BA
		public OrbitalDebrisDef OrbitalDebris
		{
			get
			{
				return this.orbitalDebris ?? this.Biome.orbitalDebris;
			}
			set
			{
				this.orbitalDebris = value;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00072EC4 File Offset: 0x000710C4
		public Material MapEdgeMaterial
		{
			get
			{
				if (ModsConfig.AnomalyActive && this.generatorDef == MapGeneratorDefOf.MetalHell)
				{
					return MapEdgeClipDrawer.ClipMatMetalhell;
				}
				WorldObject parent = this.Parent;
				if (parent != null && parent.def.MapEdgeMaterial != null)
				{
					return parent.def.MapEdgeMaterial;
				}
				if (this.generatorDef.mapClipperShader == null)
				{
					return MapEdgeClipDrawer.ClipMat;
				}
				if (!this.generatorDef.mapClipperTexturePath.NullOrEmpty())
				{
					return MaterialPool.MatFrom(this.generatorDef.mapClipperTexturePath, this.generatorDef.mapClipperShader.Shader);
				}
				return MaterialPool.MatFrom(this.generatorDef.mapClipperShader.Shader);
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x00072F6F File Offset: 0x0007116F
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x00072F77 File Offset: 0x00071177
		public bool Disposed { get; private set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x00072F80 File Offset: 0x00071180
		public IEnumerable<IntVec3> AllCells
		{
			get
			{
				Map.<get_AllCells>d__150 <get_AllCells>d__ = new Map.<get_AllCells>d__150(-2);
				<get_AllCells>d__.<>4__this = this;
				return <get_AllCells>d__;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x00072F90 File Offset: 0x00071190
		public bool IsPlayerHome
		{
			get
			{
				if (!this.wasSpawnedViaGravShipLanding)
				{
					MapInfo mapInfo = this.info;
					if (((mapInfo != null) ? mapInfo.parent : null) == null || this.info.parent.Faction != Faction.OfPlayer || !this.info.parent.def.canBePlayerHome)
					{
						return GravshipUtility.PlayerHasGravEngine(this);
					}
				}
				return true;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x00072FEF File Offset: 0x000711EF
		public bool TreatAsPlayerHomeForThreatPoints
		{
			get
			{
				return this.IsPlayerHome || (this.info.parent != null && this.info.parent.def.treatAsPlayerHome);
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x00073022 File Offset: 0x00071222
		public bool IsTempIncidentMap
		{
			get
			{
				return this.info.parent.def.isTempIncidentMapOwner;
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00073039 File Offset: 0x00071239
		public IEnumerator<IntVec3> GetEnumerator()
		{
			Map.<GetEnumerator>d__157 <GetEnumerator>d__ = new Map.<GetEnumerator>d__157(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x00073048 File Offset: 0x00071248
		public PlanetTile Tile
		{
			get
			{
				return this.info.Tile;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x00073055 File Offset: 0x00071255
		public Tile TileInfo
		{
			get
			{
				if (!this.IsPocketMap)
				{
					return Find.WorldGrid[this.Tile];
				}
				return this.pocketTileInfo;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x00073076 File Offset: 0x00071276
		public BiomeDef Biome
		{
			get
			{
				return this.TileInfo.PrimaryBiome;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x00073083 File Offset: 0x00071283
		public IEnumerable<BiomeDef> Biomes
		{
			get
			{
				return this.TileInfo.Biomes;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x00073090 File Offset: 0x00071290
		public MixedBiomeMapComponent MixedBiomeComp
		{
			get
			{
				MixedBiomeMapComponent result;
				if ((result = this.mixedBiomeComp) == null)
				{
					result = (this.mixedBiomeComp = this.GetComponent<MixedBiomeMapComponent>());
				}
				return result;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x000730B6 File Offset: 0x000712B6
		public bool IsStartingMap
		{
			get
			{
				return Find.GameInfo.startingTile == this.Tile;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x000730CD File Offset: 0x000712CD
		public bool IsPocketMap
		{
			get
			{
				return this.info.isPocketMap;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x000730DA File Offset: 0x000712DA
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x000730E2 File Offset: 0x000712E2
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x000730EC File Offset: 0x000712EC
		public float PlayerWealthForStoryteller
		{
			get
			{
				if (!this.TreatAsPlayerHomeForThreatPoints)
				{
					float num = 0f;
					foreach (Pawn pawn in this.mapPawns.PawnsInFaction(Faction.OfPlayer))
					{
						if (pawn.IsFreeColonist)
						{
							num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(pawn);
						}
						if (pawn.IsAnimal)
						{
							num += pawn.MarketValue;
						}
					}
					return num;
				}
				if (Find.Storyteller.difficulty.fixedWealthMode)
				{
					return StorytellerUtility.FixedWealthModeMapWealthFromTimeCurve.Evaluate(this.AgeInDays * Find.Storyteller.difficulty.fixedWealthTimeFactor);
				}
				return this.wealthWatcher.WealthItems + this.wealthWatcher.WealthBuildings * 0.5f + this.wealthWatcher.WealthPawns;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x000731D0 File Offset: 0x000713D0
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return this.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x000731E2 File Offset: 0x000713E2
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x000731E9 File Offset: 0x000713E9
		public MapParent Parent
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x000731F6 File Offset: 0x000713F6
		public PocketMapParent PocketMapParent
		{
			get
			{
				if (!this.IsPocketMap)
				{
					return null;
				}
				return this.Parent as PocketMapParent;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001453 RID: 5203 RVA: 0x0007320D File Offset: 0x0007140D
		public IEnumerable<Map> ChildPocketMaps
		{
			get
			{
				Map.<get_ChildPocketMaps>d__188 <get_ChildPocketMaps>d__ = new Map.<get_ChildPocketMaps>d__188(-2);
				<get_ChildPocketMaps>d__.<>4__this = this;
				return <get_ChildPocketMaps>d__;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x0007321D File Offset: 0x0007141D
		public float AgeInDays
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - this.generationTick) / 60000f;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x00073237 File Offset: 0x00071437
		public bool AnyBuildingBlockingMapRemoval
		{
			get
			{
				if (ModsConfig.OdysseyActive)
				{
					if (this.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
					{
						return true;
					}
					if (this.listerThings.AnyThingWithDef(ThingDefOf.GravEngine))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x00073269 File Offset: 0x00071469
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			MapParent parent = this.info.parent;
			return ((parent != null) ? parent.IncidentTargetTags() : null) ?? Enumerable.Empty<IncidentTargetTagDef>();
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0007328C File Offset: 0x0007148C
		public void ConstructComponents()
		{
			this.spawnedThings = new ThingOwner<Thing>(this);
			this.cellIndices = new CellIndices(this);
			this.listerThings = new ListerThings(ListerThingsUse.Global, this.thingListChangedCallbacks);
			this.listerBuildings = new ListerBuildings();
			this.mapPawns = new MapPawns(this);
			this.dynamicDrawManager = new DynamicDrawManager(this);
			this.mapDrawer = new MapDrawer(this);
			this.tooltipGiverList = new TooltipGiverList();
			this.pawnDestinationReservationManager = new PawnDestinationReservationManager();
			this.reservationManager = new ReservationManager(this);
			this.enrouteManager = new EnrouteManager(this);
			this.physicalInteractionReservationManager = new PhysicalInteractionReservationManager();
			this.designationManager = new DesignationManager(this);
			this.lordManager = new LordManager(this);
			this.debugDrawer = new DebugCellDrawer();
			this.passingShipManager = new PassingShipManager(this);
			this.haulDestinationManager = new HaulDestinationManager(this);
			this.gameConditionManager = new GameConditionManager(this);
			this.weatherManager = new WeatherManager(this);
			this.zoneManager = new ZoneManager(this);
			this.planManager = new PlanManager(this);
			this.resourceCounter = new ResourceCounter(this);
			this.mapTemperature = new MapTemperature(this);
			this.TemperatureVacuumCache = new TemperatureVacuumCache(this);
			this.areaManager = new AreaManager(this);
			this.attackTargetsCache = new AttackTargetsCache(this);
			this.attackTargetReservationManager = new AttackTargetReservationManager(this);
			this.lordsStarter = new VoluntarilyJoinableLordsStarter(this);
			this.flecks = new FleckManager(this);
			this.deferredSpawner = new DeferredSpawner(this);
			this.thingGrid = new ThingGrid(this);
			this.coverGrid = new CoverGrid(this);
			this.edificeGrid = new EdificeGrid(this);
			this.blueprintGrid = new BlueprintGrid(this);
			this.fogGrid = new FogGrid(this);
			this.glowGrid = new GlowGrid(this);
			this.regionGrid = new RegionGrid(this);
			this.terrainGrid = new TerrainGrid(this);
			this.pathing = new Pathing(this);
			this.roofGrid = new RoofGrid(this);
			this.fertilityGrid = new FertilityGrid(this);
			this.snowGrid = new SnowGrid(this);
			this.gasGrid = new GasGrid(this);
			this.pollutionGrid = new PollutionGrid(this);
			this.deepResourceGrid = new DeepResourceGrid(this);
			this.exitMapGrid = new ExitMapGrid(this);
			this.avoidGrid = new AvoidGrid(this);
			this.linkGrid = new LinkGrid(this);
			this.powerNetManager = new PowerNetManager(this);
			this.powerNetGrid = new PowerNetGrid(this);
			this.regionMaker = new RegionMaker(this);
			this.pathFinder = new PathFinder(this);
			this.pawnPathPool = new PawnPathPool(this);
			this.regionAndRoomUpdater = new RegionAndRoomUpdater(this);
			this.regionLinkDatabase = new RegionLinkDatabase();
			this.moteCounter = new MoteCounter();
			this.gatherSpotLister = new GatherSpotLister();
			this.windManager = new WindManager(this);
			this.listerBuildingsRepairable = new ListerBuildingsRepairable();
			this.listerHaulables = new ListerHaulables(this);
			this.listerMergeables = new ListerMergeables(this);
			this.listerFilthInHomeArea = new ListerFilthInHomeArea(this);
			this.listerArtificialBuildingsForMeditation = new ListerArtificialBuildingsForMeditation(this);
			this.listerBuldingOfDefInProximity = new ListerBuldingOfDefInProximity(this);
			this.listerBuildingWithTagInProximity = new ListerBuildingWithTagInProximity(this);
			this.reachability = new Reachability(this);
			this.itemAvailability = new ItemAvailability(this);
			this.autoBuildRoofAreaSetter = new AutoBuildRoofAreaSetter(this);
			this.roofCollapseBufferResolver = new RoofCollapseBufferResolver(this);
			this.roofCollapseBuffer = new RoofCollapseBuffer();
			this.wildAnimalSpawner = new WildAnimalSpawner(this);
			this.wildPlantSpawner = new WildPlantSpawner(this);
			this.steadyEnvironmentEffects = new SteadyEnvironmentEffects(this);
			this.tempTerrain = new TempTerrainManager(this);
			this.skyManager = new SkyManager(this);
			this.overlayDrawer = new OverlayDrawer();
			this.floodFiller = new FloodFiller(this);
			this.weatherDecider = new WeatherDecider(this);
			this.fireWatcher = new FireWatcher(this);
			this.dangerWatcher = new DangerWatcher(this);
			this.damageWatcher = new DamageWatcher();
			this.strengthWatcher = new StrengthWatcher(this);
			this.wealthWatcher = new WealthWatcher(this);
			this.regionDirtyer = new RegionDirtyer(this);
			this.cellsInRandomOrder = new MapCellsInRandomOrder(this);
			this.rememberedCameraPos = new RememberedCameraPos(this);
			this.mineStrikeManager = new MineStrikeManager();
			this.storyState = new StoryState(this);
			this.retainedCaravanData = new RetainedCaravanData(this);
			this.temporaryThingDrawer = new TemporaryThingDrawer();
			this.animalPenManager = new AnimalPenManager(this);
			this.plantGrowthRateCalculator = new MapPlantGrowthRateCalculator();
			this.autoSlaughterManager = new AutoSlaughterManager(this);
			this.treeDestructionTracker = new TreeDestructionTracker(this);
			this.storageGroups = new StorageGroupManager(this);
			this.effecterMaintainer = new EffecterMaintainer(this);
			this.postTickVisuals = new PostTickVisuals(this);
			if (ModsConfig.OdysseyActive)
			{
				this.substructureGrid = new SubstructureGrid(this);
				this.waterBodyTracker = new WaterBodyTracker(this);
				this.freezeManager = new FreezeManager(this);
				this.sandGrid = new SandGrid(this);
			}
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x00073764 File Offset: 0x00071964
		public int NextGenSeed
		{
			get
			{
				int value = this.TileInfo.tile.Valid ? this.TileInfo.tile.GetHashCode() : this.uniqueID;
				int num = this.generatedId;
				this.generatedId = num + 1;
				return HashCode.Combine<int, int, int>(value, num, Find.World.info.Seed);
			}
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x000737C8 File Offset: 0x000719C8
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.events = new MapEvents(this);
			}
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Values.Look<int>(ref this.generationTick, "generationTick", 0, false);
			Scribe_Values.Look<bool>(ref this.wasSpawnedViaGravShipLanding, "wasSpawnedViaGravShipLanding", false, false);
			Scribe_Values.Look<Color?>(ref this.fogOfWarColor, "fogOfWarColor", null, false);
			Scribe_Values.Look<int>(ref this.generatedId, "generatedId", 0, false);
			Scribe_Defs.Look<OrbitalDebrisDef>(ref this.orbitalDebris, "orbitalDebris");
			Scribe_Defs.Look<MapGeneratorDef>(ref this.generatorDef, "generatorDef");
			Scribe_Deep.Look<Tile>(ref this.pocketTileInfo, "pocketTileInfo", Array.Empty<object>());
			Scribe_Deep.Look<MapInfo>(ref this.info, "mapInfo", Array.Empty<object>());
			Scribe_Collections.Look<LayoutStructureSketch>(ref this.layoutStructureSketches, "layoutStructureSketches", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<CellRect>(ref this.landingBlockers, "landingBlockers", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.compressor = new MapFileCompressor(this);
				this.compressor.BuildCompressedString();
				this.ExposeComponents();
				this.compressor.ExposeData();
				HashSet<string> hashSet = new HashSet<string>();
				if (Scribe.EnterNode("things"))
				{
					try
					{
						using (List<Thing>.Enumerator enumerator = this.listerThings.AllThings.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Thing thing = enumerator.Current;
								try
								{
									if (thing.def.isSaveable && !thing.IsSaveCompressible())
									{
										if (!hashSet.Add(thing.ThingID))
										{
											Log.Error("Saving Thing with already-used ID " + thing.ThingID);
										}
										else
										{
											hashSet.Add(thing.ThingID);
										}
										Thing thing2 = thing;
										Scribe_Deep.Look<Thing>(ref thing2, "thing", Array.Empty<object>());
									}
								}
								catch (OutOfMemoryException)
								{
									throw;
								}
								catch (Exception value)
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
									defaultInterpolatedStringHandler.AppendLiteral("Exception saving ");
									defaultInterpolatedStringHandler.AppendFormatted<Thing>(thing);
									defaultInterpolatedStringHandler.AppendLiteral(": ");
									defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
									Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
								}
							}
							goto IL_221;
						}
					}
					finally
					{
						Scribe.ExitNode();
					}
				}
				Log.Error("Could not enter the things node while saving.");
				IL_221:
				this.compressor = null;
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					this.ConstructComponents();
					this.regionAndRoomUpdater.Enabled = false;
					this.compressor = new MapFileCompressor(this);
				}
				else if (Scribe.mode == LoadSaveMode.PostLoadInit && this.landingBlockers == null)
				{
					this.landingBlockers = new List<CellRect>();
				}
				this.ExposeComponents();
				DeepProfiler.Start("Load compressed things");
				this.compressor.ExposeData();
				DeepProfiler.End();
				DeepProfiler.Start("Load non-compressed things");
				Scribe_Collections.Look<Thing>(ref this.loadedFullThings, "things", LookMode.Deep, Array.Empty<object>());
				DeepProfiler.End();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00073AC4 File Offset: 0x00071CC4
		private void FillComponents()
		{
			this.components.RemoveAll((MapComponent component) => component == null);
			foreach (Type type in typeof(MapComponent).AllSubclassesNonAbstract())
			{
				if (!typeof(CustomMapComponent).IsAssignableFrom(type) && this.GetComponent(type) == null)
				{
					try
					{
						MapComponent item = (MapComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate a MapComponent of type ";
						Type type2 = type;
						string str2 = (type2 != null) ? type2.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			MapGeneratorDef mapGeneratorDef = this.generatorDef;
			if (((mapGeneratorDef != null) ? mapGeneratorDef.customMapComponents : null) != null)
			{
				foreach (Type type3 in this.generatorDef.customMapComponents)
				{
					if (this.GetComponent(type3) == null)
					{
						try
						{
							MapComponent item2 = (MapComponent)Activator.CreateInstance(type3, new object[]
							{
								this
							});
							this.components.Add(item2);
						}
						catch (Exception ex3)
						{
							string str4 = "Could not instantiate a MapComponent of type ";
							Type type4 = type3;
							string str5 = (type4 != null) ? type4.ToString() : null;
							string str6 = ": ";
							Exception ex4 = ex3;
							Log.Error(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null));
						}
					}
				}
			}
			this.roadInfo = this.GetComponent<RoadInfo>();
			this.waterInfo = this.GetComponent<WaterInfo>();
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00073CA4 File Offset: 0x00071EA4
		public void FinalizeLoading()
		{
			this.regionAndRoomUpdater.Enabled = true;
			List<Thing> list = this.compressor.ThingsToSpawnAfterLoad().ToList<Thing>();
			this.compressor = null;
			DeepProfiler.Start("Merge compressed and non-compressed thing lists");
			List<Thing> list2 = new List<Thing>(this.loadedFullThings.Count + list.Count);
			foreach (Thing item in this.loadedFullThings.Concat(list))
			{
				list2.Add(item);
			}
			this.loadedFullThings.Clear();
			DeepProfiler.End();
			DeepProfiler.Start("Spawn everything into the map");
			BackCompatibility.PreCheckSpawnBackCompatibleThingAfterLoading(this);
			foreach (Thing thing in list2)
			{
				if (!(thing is Building))
				{
					try
					{
						if (!BackCompatibility.CheckSpawnBackCompatibleThingAfterLoading(thing, this))
						{
							GenSpawn.Spawn(thing, thing.Position, this, thing.Rotation, WipeMode.FullRefund, true, false);
						}
					}
					catch (Exception ex)
					{
						string str = "Exception spawning loaded thing ";
						string str2 = thing.ToStringSafe<Thing>();
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			foreach (Building building in from t in list2.OfType<Building>()
			orderby t.def.size.Magnitude
			select t)
			{
				try
				{
					GenSpawn.SpawnBuildingAsPossible(building, this, true);
				}
				catch (Exception ex3)
				{
					string str4 = "Exception spawning loaded thing ";
					string str5 = building.ToStringSafe<Building>();
					string str6 = ": ";
					Exception ex4 = ex3;
					Log.Error(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
			BackCompatibility.PostCheckSpawnBackCompatibleThingAfterLoading(this);
			DeepProfiler.End();
			this.FinalizeInit();
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00073EB4 File Offset: 0x000720B4
		public void FinalizeInit()
		{
			DeepProfiler.Start("Finalize geometry");
			this.pathing.RecalculateAllPerceivedPathCosts();
			this.regionAndRoomUpdater.Enabled = true;
			this.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.TemperatureVacuumCache.TemperatureVacuumSaveLoad.ApplyLoadedDataToRegions();
			this.avoidGrid.Regenerate();
			this.animalPenManager.RebuildAllPens();
			this.plantGrowthRateCalculator.BuildFor(this);
			this.gasGrid.RecalculateEverHadGas();
			DeepProfiler.End();
			DeepProfiler.Start("Thing.PostMapInit()");
			foreach (Thing thing in this.listerThings.AllThings.ToList<Thing>())
			{
				try
				{
					thing.PostMapInit();
				}
				catch (Exception ex)
				{
					string str = "Error in PostMapInit() for ";
					string str2 = thing.ToStringSafe<Thing>();
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			DeepProfiler.End();
			DeepProfiler.Start("listerFilthInHomeArea.RebuildAll()");
			this.listerFilthInHomeArea.RebuildAll();
			DeepProfiler.End();
			if (ModsConfig.OdysseyActive)
			{
				this.GetComponent<VacuumComponent>().SetDrawerDirty();
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mapDrawer.RegenerateEverythingNow();
			});
			DeepProfiler.Start("resourceCounter.UpdateResourceCounts()");
			this.resourceCounter.UpdateResourceCounts();
			DeepProfiler.End();
			DeepProfiler.Start("wealthWatcher.ForceRecount()");
			this.wealthWatcher.ForceRecount(true);
			DeepProfiler.End();
			if (ModsConfig.OdysseyActive)
			{
				using (new ProfilerBlock("WaterBodyTracker.ConstructBodies()"))
				{
					WaterBodyTracker waterBodyTracker = this.waterBodyTracker;
					if (waterBodyTracker != null)
					{
						waterBodyTracker.ConstructBodies();
					}
				}
			}
			MapComponentUtility.FinalizeInit(this);
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Find.MusicManagerPlay.CheckTransitions();
			});
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x000740AC File Offset: 0x000722AC
		private void ExposeComponents()
		{
			Scribe_Deep.Look<WeatherManager>(ref this.weatherManager, "weatherManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<ReservationManager>(ref this.reservationManager, "reservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<EnrouteManager>(ref this.enrouteManager, "enrouteManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PhysicalInteractionReservationManager>(ref this.physicalInteractionReservationManager, "physicalInteractionReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<PlanManager>(ref this.planManager, "planManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<DesignationManager>(ref this.designationManager, "designationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PawnDestinationReservationManager>(ref this.pawnDestinationReservationManager, "pawnDestinationReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<LordManager>(ref this.lordManager, "lordManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PassingShipManager>(ref this.passingShipManager, "visitorManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<FogGrid>(ref this.fogGrid, "fogGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<RoofGrid>(ref this.roofGrid, "roofGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<TerrainGrid>(ref this.terrainGrid, "terrainGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<ZoneManager>(ref this.zoneManager, "zoneManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemperatureVacuumCache>(ref this.TemperatureVacuumCache, "temperatureCache", new object[]
			{
				this
			});
			Scribe_Deep.Look<SnowGrid>(ref this.snowGrid, "snowGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<GasGrid>(ref this.gasGrid, "gasGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<PollutionGrid>(ref this.pollutionGrid, "pollutionGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<WaterBodyTracker>(ref this.waterBodyTracker, "waterBodyTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<AreaManager>(ref this.areaManager, "areaManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<VoluntarilyJoinableLordsStarter>(ref this.lordsStarter, "lordsStarter", new object[]
			{
				this
			});
			Scribe_Deep.Look<AttackTargetReservationManager>(ref this.attackTargetReservationManager, "attackTargetReservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<DeepResourceGrid>(ref this.deepResourceGrid, "deepResourceGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<WeatherDecider>(ref this.weatherDecider, "weatherDecider", new object[]
			{
				this
			});
			Scribe_Deep.Look<DamageWatcher>(ref this.damageWatcher, "damageWatcher", Array.Empty<object>());
			Scribe_Deep.Look<RememberedCameraPos>(ref this.rememberedCameraPos, "rememberedCameraPos", new object[]
			{
				this
			});
			Scribe_Deep.Look<MineStrikeManager>(ref this.mineStrikeManager, "mineStrikeManager", Array.Empty<object>());
			Scribe_Deep.Look<RetainedCaravanData>(ref this.retainedCaravanData, "retainedCaravanData", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<TempTerrainManager>(ref this.tempTerrain, "tempTerrain", new object[]
			{
				this
			});
			Scribe_Deep.Look<WildPlantSpawner>(ref this.wildPlantSpawner, "wildPlantSpawner", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemporaryThingDrawer>(ref this.temporaryThingDrawer, "temporaryThingDrawer", Array.Empty<object>());
			Scribe_Deep.Look<FleckManager>(ref this.flecks, "flecks", new object[]
			{
				this
			});
			Scribe_Deep.Look<DeferredSpawner>(ref this.deferredSpawner, "deferredSpawner", new object[]
			{
				this
			});
			Scribe_Deep.Look<AutoSlaughterManager>(ref this.autoSlaughterManager, "autoSlaughterManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TreeDestructionTracker>(ref this.treeDestructionTracker, "treeDestructionTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<StorageGroupManager>(ref this.storageGroups, "storageGroups", new object[]
			{
				this
			});
			Scribe_Deep.Look<SandGrid>(ref this.sandGrid, "sandGrid", new object[]
			{
				this
			});
			Scribe_Collections.Look<MapComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.planManager == null)
				{
					this.planManager = new PlanManager(this);
				}
				if (ModsConfig.BiotechActive && this.pollutionGrid == null)
				{
					this.pollutionGrid = new PollutionGrid(this);
				}
				if (ModsConfig.OdysseyActive)
				{
					if (this.sandGrid == null)
					{
						this.sandGrid = new SandGrid(this);
					}
					if (this.substructureGrid == null)
					{
						this.substructureGrid = new SubstructureGrid(this);
					}
					if (this.waterBodyTracker == null)
					{
						this.waterBodyTracker = new WaterBodyTracker(this);
					}
					if (this.freezeManager == null)
					{
						this.freezeManager = new FreezeManager(this);
					}
				}
			}
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x00074534 File Offset: 0x00072734
		public void MapPreTick()
		{
			this.itemAvailability.Tick();
			this.listerHaulables.ListerHaulablesTick();
			try
			{
				this.autoBuildRoofAreaSetter.AutoBuildRoofAreaSetterTick_First();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			this.windManager.WindManagerTick();
			try
			{
				this.mapTemperature.MapTemperatureTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			this.temporaryThingDrawer.Tick();
			try
			{
				this.pathFinder.PathFinderTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x000745EC File Offset: 0x000727EC
		public void MapPostTick()
		{
			try
			{
				this.wildAnimalSpawner.WildAnimalSpawnerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			try
			{
				this.wildPlantSpawner.WildPlantSpawnerTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			try
			{
				this.powerNetManager.PowerNetsTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
			try
			{
				this.steadyEnvironmentEffects.SteadyEnvironmentEffectsTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString());
			}
			try
			{
				this.tempTerrain.Tick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString());
			}
			try
			{
				this.gasGrid.Tick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString());
			}
			if (ModsConfig.BiotechActive)
			{
				try
				{
					this.pollutionGrid.PollutionTick();
				}
				catch (Exception ex7)
				{
					Log.Error(ex7.ToString());
				}
			}
			try
			{
				this.deferredSpawner.DeferredSpawnerTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString());
			}
			try
			{
				this.lordManager.LordManagerTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString());
			}
			try
			{
				this.passingShipManager.PassingShipManagerTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString());
			}
			try
			{
				this.debugDrawer.DebugDrawerTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString());
			}
			try
			{
				this.lordsStarter.VoluntarilyJoinableLordsStarterTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString());
			}
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString());
			}
			try
			{
				this.weatherManager.WeatherManagerTick();
			}
			catch (Exception ex14)
			{
				Log.Error(ex14.ToString());
			}
			try
			{
				this.resourceCounter.ResourceCounterTick();
			}
			catch (Exception ex15)
			{
				Log.Error(ex15.ToString());
			}
			try
			{
				this.weatherDecider.WeatherDeciderTick();
			}
			catch (Exception ex16)
			{
				Log.Error(ex16.ToString());
			}
			try
			{
				this.fireWatcher.FireWatcherTick();
			}
			catch (Exception ex17)
			{
				Log.Error(ex17.ToString());
			}
			if (ModsConfig.OdysseyActive)
			{
				try
				{
					WaterBodyTracker waterBodyTracker = this.waterBodyTracker;
					if (waterBodyTracker != null)
					{
						waterBodyTracker.Tick();
					}
				}
				catch (Exception ex18)
				{
					Log.Error(ex18.ToString());
				}
			}
			try
			{
				this.flecks.FleckManagerTick();
			}
			catch (Exception ex19)
			{
				Log.Error(ex19.ToString());
			}
			try
			{
				this.effecterMaintainer.EffecterMaintainerTick();
			}
			catch (Exception ex20)
			{
				Log.Error(ex20.ToString());
			}
			MapComponentUtility.MapComponentTick(this);
			try
			{
				foreach (TileMutatorDef tileMutatorDef in this.TileInfo.Mutators)
				{
					TileMutatorWorker worker = tileMutatorDef.Worker;
					if (worker != null)
					{
						worker.Tick(this);
					}
				}
			}
			catch (Exception ex21)
			{
				Log.Error(ex21.ToString());
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00074A78 File Offset: 0x00072C78
		public void MapUpdate()
		{
			if (this.Disposed)
			{
				return;
			}
			bool drawingMap = WorldRendererUtility.DrawingMap;
			this.skyManager.SkyManagerUpdate();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.regionGrid.UpdateClean();
			this.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			this.glowGrid.GlowGridUpdate_First();
			this.lordManager.LordManagerUpdate();
			this.postTickVisuals.ProcessPostTickVisuals();
			if (drawingMap && Find.CurrentMap == this)
			{
				if (Map.AlwaysRedrawShadows)
				{
					this.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.Things);
				}
				GlobalRendererUtility.UpdateGlobalShadersParams();
				PlantFallColors.SetFallShaderGlobals(this);
				this.waterInfo.SetTextures();
				this.avoidGrid.DebugDrawOnMap();
				BreachingGridDebug.DebugDrawAllOnMap(this);
				this.mapDrawer.MapMeshDrawerUpdate_First();
				this.powerNetGrid.DrawDebugPowerNetGrid();
				DoorsDebugDrawer.DrawDebug();
				this.mapDrawer.DrawMapMesh();
				this.dynamicDrawManager.DrawDynamicThings();
				this.gameConditionManager.GameConditionManagerDraw(this);
				MapEdgeClipDrawer.DrawClippers(this);
				this.designationManager.DrawDesignations();
				this.overlayDrawer.DrawAllOverlays();
				this.temporaryThingDrawer.Draw();
				this.flecks.FleckManagerDraw();
			}
			try
			{
				this.areaManager.AreaManagerUpdate();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.weatherManager.WeatherManagerUpdate();
			try
			{
				this.flecks.FleckManagerUpdate();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			MapComponentUtility.MapComponentUpdate(this);
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00074C04 File Offset: 0x00072E04
		public T GetComponent<T>() where T : MapComponent
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

		// Token: 0x06001462 RID: 5218 RVA: 0x00074C54 File Offset: 0x00072E54
		public MapComponent GetComponent(Type type)
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

		// Token: 0x06001463 RID: 5219 RVA: 0x00074C99 File Offset: 0x00072E99
		public void MapOnGUI()
		{
			this.DevGUISketches();
			Map.DevRoadPaths();
			this.pathFinder.OnGUI();
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x00074CB4 File Offset: 0x00072EB4
		private static void DevRoadPaths()
		{
			if (!DebugViewSettings.drawRoadPaths)
			{
				return;
			}
			for (int i = 0; i < GenStep_Roads.paths.Count; i++)
			{
				foreach (IntVec3 intVec in GenStep_Roads.paths[i])
				{
					Vector2 vector = intVec.ToVector3Shifted().MapToUIPosition();
					DevGUI.DrawRect(new Rect(vector.x, vector.y, 5f, 5f), (i % 2 == 0) ? Color.yellow : Color.blue);
				}
			}
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x00074D64 File Offset: 0x00072F64
		private void DevGUISketches()
		{
			if ((!DebugViewSettings.drawMapGraphs && !DebugViewSettings.drawMapRooms) || this.layoutStructureSketches.NullOrEmpty<LayoutStructureSketch>())
			{
				return;
			}
			foreach (LayoutStructureSketch layoutStructureSketch in this.layoutStructureSketches)
			{
				this.DebugGUILayoutStructure(layoutStructureSketch);
			}
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00074DD4 File Offset: 0x00072FD4
		private void DebugGUILayoutStructure(LayoutStructureSketch layoutStructureSketch)
		{
			Map.DevDrawOutline(layoutStructureSketch.structureLayout.container, Color.yellow);
			Vector2 pos = (layoutStructureSketch.structureLayout.container.Min - IntVec3.South).ToVector3().MapToUIPosition();
			Map.DevDrawLabel(layoutStructureSketch.layoutDef.defName, pos);
			if (DebugViewSettings.drawMapGraphs)
			{
				StructureLayout structureLayout = layoutStructureSketch.structureLayout;
				if (((structureLayout != null) ? structureLayout.neighbours : null) != null)
				{
					foreach (KeyValuePair<Vector2, List<Vector2>> keyValuePair in layoutStructureSketch.structureLayout.neighbours.connections)
					{
						foreach (Vector2 b in keyValuePair.Value)
						{
							Vector2 a = layoutStructureSketch.center.ToVector2();
							Vector2 vector = a + keyValuePair.Key;
							Vector2 vector2 = a + b;
							Vector2 start = new Vector3(vector.x, 0f, vector.y).MapToUIPosition();
							Vector2 end = new Vector3(vector2.x, 0f, vector2.y).MapToUIPosition();
							DevGUI.DrawLine(start, end, Color.green, 2f);
						}
					}
				}
			}
			if (DebugViewSettings.drawMapRooms)
			{
				StructureLayout structureLayout2 = layoutStructureSketch.structureLayout;
				if (((structureLayout2 != null) ? structureLayout2.Rooms : null) != null)
				{
					foreach (LayoutRoom layoutRoom in layoutStructureSketch.structureLayout.Rooms)
					{
						string name = "NA";
						if (!layoutRoom.defs.NullOrEmpty<LayoutRoomDef>())
						{
							name = (from x in layoutRoom.defs
							select x.defName).ToCommaList(false, false);
						}
						Map.DevDrawLabel(name, layoutRoom.rects[0].CenterVector3.MapToUIPosition());
						foreach (CellRect r in layoutRoom.rects)
						{
							Map.DevDrawOutline(r, Color.blue);
						}
					}
				}
			}
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0007506C File Offset: 0x0007326C
		private static void DevDrawLabel(string name, Vector2 pos)
		{
			float widthCached = name.GetWidthCached();
			DevGUI.Label(new Rect(pos.x - widthCached / 2f, pos.y, widthCached, 20f), name);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000750A8 File Offset: 0x000732A8
		private static void DevDrawOutline(CellRect r, Color color)
		{
			IntVec3 min = r.Min;
			IntVec3 intVec = r.Max + new IntVec3(1, 0, 1);
			IntVec3 a = new IntVec3(min.x, 0, min.z);
			IntVec3 intVec2 = new IntVec3(intVec.x, 0, min.z);
			IntVec3 intVec3 = new IntVec3(min.x, 0, intVec.z);
			IntVec3 b = new IntVec3(intVec.x, 0, intVec.z);
			Map.DevDrawLine(a, intVec2, color);
			Map.DevDrawLine(a, intVec3, color);
			Map.DevDrawLine(intVec3, b, color);
			Map.DevDrawLine(intVec2, b, color);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x00075144 File Offset: 0x00073344
		private static void DevDrawLine(IntVec3 a, IntVec3 b, Color color)
		{
			Vector2 start = a.ToVector3().MapToUIPosition();
			Vector2 end = b.ToVector3().MapToUIPosition();
			DevGUI.DrawLine(start, end, color, 2f);
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x00075176 File Offset: 0x00073376
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueID ^ 16622162;
			}
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00075184 File Offset: 0x00073384
		public string GetUniqueLoadID()
		{
			return "Map_" + this.uniqueID.ToString();
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0007519C File Offset: 0x0007339C
		public override string ToString()
		{
			string text = "Map-" + this.uniqueID.ToString();
			if (this.IsPlayerHome)
			{
				text += "-PlayerHome";
			}
			return text;
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x000751D4 File Offset: 0x000733D4
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.spawnedThings;
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x000751DC File Offset: 0x000733DC
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder));
			List<PassingShip> passingShips = this.passingShipManager.passingShips;
			for (int i = 0; i < passingShips.Count; i++)
			{
				IThingHolder thingHolder = passingShips[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
			}
			for (int j = 0; j < this.components.Count; j++)
			{
				IThingHolder thingHolder2 = this.components[j] as IThingHolder;
				if (thingHolder2 != null)
				{
					outChildren.Add(thingHolder2);
				}
			}
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00075268 File Offset: 0x00073468
		public void Dispose()
		{
			if (this.Disposed)
			{
				return;
			}
			this.Disposed = true;
			foreach (MapComponent mapComponent in this.components)
			{
				IDisposable disposable = mapComponent as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (this.regionAndRoomUpdater != null)
			{
				this.regionAndRoomUpdater.Enabled = false;
			}
			PathFinder pathFinder = this.pathFinder;
			if (pathFinder != null)
			{
				pathFinder.Dispose();
			}
			LordManager lordManager = this.lordManager;
			if (lordManager != null)
			{
				lordManager.Dispose();
			}
			FogGrid fogGrid = this.fogGrid;
			if (fogGrid != null)
			{
				fogGrid.Dispose();
			}
			SnowGrid snowGrid = this.snowGrid;
			if (snowGrid != null)
			{
				snowGrid.Dispose();
			}
			GlowGrid glowGrid = this.glowGrid;
			if (glowGrid != null)
			{
				glowGrid.Dispose();
			}
			SandGrid sandGrid = this.sandGrid;
			if (sandGrid != null)
			{
				sandGrid.Dispose();
			}
			AvoidGrid avoidGrid = this.avoidGrid;
			if (avoidGrid != null)
			{
				avoidGrid.Dispose();
			}
			ListerBuildings listerBuildings = this.listerBuildings;
			if (listerBuildings != null)
			{
				listerBuildings.Dispose();
			}
			ListerThings listerThings = this.listerThings;
			if (listerThings != null)
			{
				listerThings.Clear();
			}
			RegionDirtyer regionDirtyer = this.regionDirtyer;
			if (regionDirtyer != null)
			{
				regionDirtyer.SetAllDirty();
			}
			RegionGrid regionGrid = this.regionGrid;
			if (regionGrid != null)
			{
				regionGrid.Dispose();
			}
			Pathing pathing = this.pathing;
			if (pathing != null)
			{
				pathing.Dispose();
			}
			MapDrawer mapDrawer = this.mapDrawer;
			if (mapDrawer != null)
			{
				mapDrawer.Dispose();
			}
			Resources.UnloadUnusedAssets();
			MapGenerator.ClearDebugMode();
		}

		// Token: 0x0400119C RID: 4508
		public MapFileCompressor compressor;

		// Token: 0x0400119D RID: 4509
		private List<Thing> loadedFullThings;

		// Token: 0x0400119E RID: 4510
		public MapGeneratorDef generatorDef;

		// Token: 0x0400119F RID: 4511
		public int uniqueID = -1;

		// Token: 0x040011A0 RID: 4512
		public int generationTick;

		// Token: 0x040011A1 RID: 4513
		public bool wasSpawnedViaGravShipLanding;

		// Token: 0x040011A2 RID: 4514
		private Color? fogOfWarColor;

		// Token: 0x040011A3 RID: 4515
		private OrbitalDebrisDef orbitalDebris;

		// Token: 0x040011A4 RID: 4516
		private int generatedId;

		// Token: 0x040011A5 RID: 4517
		public MapInfo info = new MapInfo();

		// Token: 0x040011A6 RID: 4518
		public MapEvents events;

		// Token: 0x040011A7 RID: 4519
		public List<MapComponent> components = new List<MapComponent>();

		// Token: 0x040011A8 RID: 4520
		public ThingOwner spawnedThings;

		// Token: 0x040011A9 RID: 4521
		public CellIndices cellIndices;

		// Token: 0x040011AA RID: 4522
		public ListerThings listerThings;

		// Token: 0x040011AB RID: 4523
		public ListerBuildings listerBuildings;

		// Token: 0x040011AC RID: 4524
		public MapPawns mapPawns;

		// Token: 0x040011AD RID: 4525
		public DynamicDrawManager dynamicDrawManager;

		// Token: 0x040011AE RID: 4526
		public MapDrawer mapDrawer;

		// Token: 0x040011AF RID: 4527
		public PawnDestinationReservationManager pawnDestinationReservationManager;

		// Token: 0x040011B0 RID: 4528
		public TooltipGiverList tooltipGiverList;

		// Token: 0x040011B1 RID: 4529
		public ReservationManager reservationManager;

		// Token: 0x040011B2 RID: 4530
		public EnrouteManager enrouteManager;

		// Token: 0x040011B3 RID: 4531
		public PhysicalInteractionReservationManager physicalInteractionReservationManager;

		// Token: 0x040011B4 RID: 4532
		public DesignationManager designationManager;

		// Token: 0x040011B5 RID: 4533
		public LordManager lordManager;

		// Token: 0x040011B6 RID: 4534
		public PassingShipManager passingShipManager;

		// Token: 0x040011B7 RID: 4535
		public HaulDestinationManager haulDestinationManager;

		// Token: 0x040011B8 RID: 4536
		public DebugCellDrawer debugDrawer;

		// Token: 0x040011B9 RID: 4537
		public GameConditionManager gameConditionManager;

		// Token: 0x040011BA RID: 4538
		public WeatherManager weatherManager;

		// Token: 0x040011BB RID: 4539
		public ZoneManager zoneManager;

		// Token: 0x040011BC RID: 4540
		public PlanManager planManager;

		// Token: 0x040011BD RID: 4541
		public ResourceCounter resourceCounter;

		// Token: 0x040011BE RID: 4542
		public MapTemperature mapTemperature;

		// Token: 0x040011BF RID: 4543
		public TemperatureVacuumCache TemperatureVacuumCache;

		// Token: 0x040011C0 RID: 4544
		public AreaManager areaManager;

		// Token: 0x040011C1 RID: 4545
		public AttackTargetsCache attackTargetsCache;

		// Token: 0x040011C2 RID: 4546
		public AttackTargetReservationManager attackTargetReservationManager;

		// Token: 0x040011C3 RID: 4547
		public VoluntarilyJoinableLordsStarter lordsStarter;

		// Token: 0x040011C4 RID: 4548
		public FleckManager flecks;

		// Token: 0x040011C5 RID: 4549
		public DeferredSpawner deferredSpawner;

		// Token: 0x040011C6 RID: 4550
		public ThingGrid thingGrid;

		// Token: 0x040011C7 RID: 4551
		public CoverGrid coverGrid;

		// Token: 0x040011C8 RID: 4552
		public EdificeGrid edificeGrid;

		// Token: 0x040011C9 RID: 4553
		public BlueprintGrid blueprintGrid;

		// Token: 0x040011CA RID: 4554
		public FogGrid fogGrid;

		// Token: 0x040011CB RID: 4555
		public RegionGrid regionGrid;

		// Token: 0x040011CC RID: 4556
		public GlowGrid glowGrid;

		// Token: 0x040011CD RID: 4557
		public TerrainGrid terrainGrid;

		// Token: 0x040011CE RID: 4558
		public Pathing pathing;

		// Token: 0x040011CF RID: 4559
		public RoofGrid roofGrid;

		// Token: 0x040011D0 RID: 4560
		public FertilityGrid fertilityGrid;

		// Token: 0x040011D1 RID: 4561
		public SnowGrid snowGrid;

		// Token: 0x040011D2 RID: 4562
		public DeepResourceGrid deepResourceGrid;

		// Token: 0x040011D3 RID: 4563
		public ExitMapGrid exitMapGrid;

		// Token: 0x040011D4 RID: 4564
		public AvoidGrid avoidGrid;

		// Token: 0x040011D5 RID: 4565
		public GasGrid gasGrid;

		// Token: 0x040011D6 RID: 4566
		public PollutionGrid pollutionGrid;

		// Token: 0x040011D7 RID: 4567
		public SubstructureGrid substructureGrid;

		// Token: 0x040011D8 RID: 4568
		public WaterBodyTracker waterBodyTracker;

		// Token: 0x040011D9 RID: 4569
		public SandGrid sandGrid;

		// Token: 0x040011DA RID: 4570
		public LinkGrid linkGrid;

		// Token: 0x040011DB RID: 4571
		public PowerNetManager powerNetManager;

		// Token: 0x040011DC RID: 4572
		public PowerNetGrid powerNetGrid;

		// Token: 0x040011DD RID: 4573
		public RegionMaker regionMaker;

		// Token: 0x040011DE RID: 4574
		public PathFinder pathFinder;

		// Token: 0x040011DF RID: 4575
		public PawnPathPool pawnPathPool;

		// Token: 0x040011E0 RID: 4576
		public RegionAndRoomUpdater regionAndRoomUpdater;

		// Token: 0x040011E1 RID: 4577
		public RegionLinkDatabase regionLinkDatabase;

		// Token: 0x040011E2 RID: 4578
		public MoteCounter moteCounter;

		// Token: 0x040011E3 RID: 4579
		public GatherSpotLister gatherSpotLister;

		// Token: 0x040011E4 RID: 4580
		public WindManager windManager;

		// Token: 0x040011E5 RID: 4581
		public ListerBuildingsRepairable listerBuildingsRepairable;

		// Token: 0x040011E6 RID: 4582
		public ListerHaulables listerHaulables;

		// Token: 0x040011E7 RID: 4583
		public ListerMergeables listerMergeables;

		// Token: 0x040011E8 RID: 4584
		public ListerArtificialBuildingsForMeditation listerArtificialBuildingsForMeditation;

		// Token: 0x040011E9 RID: 4585
		public ListerBuldingOfDefInProximity listerBuldingOfDefInProximity;

		// Token: 0x040011EA RID: 4586
		public ListerBuildingWithTagInProximity listerBuildingWithTagInProximity;

		// Token: 0x040011EB RID: 4587
		public ListerFilthInHomeArea listerFilthInHomeArea;

		// Token: 0x040011EC RID: 4588
		public Reachability reachability;

		// Token: 0x040011ED RID: 4589
		public ItemAvailability itemAvailability;

		// Token: 0x040011EE RID: 4590
		public AutoBuildRoofAreaSetter autoBuildRoofAreaSetter;

		// Token: 0x040011EF RID: 4591
		public RoofCollapseBufferResolver roofCollapseBufferResolver;

		// Token: 0x040011F0 RID: 4592
		public RoofCollapseBuffer roofCollapseBuffer;

		// Token: 0x040011F1 RID: 4593
		public WildAnimalSpawner wildAnimalSpawner;

		// Token: 0x040011F2 RID: 4594
		public WildPlantSpawner wildPlantSpawner;

		// Token: 0x040011F3 RID: 4595
		public SteadyEnvironmentEffects steadyEnvironmentEffects;

		// Token: 0x040011F4 RID: 4596
		public TempTerrainManager tempTerrain;

		// Token: 0x040011F5 RID: 4597
		public FreezeManager freezeManager;

		// Token: 0x040011F6 RID: 4598
		public SkyManager skyManager;

		// Token: 0x040011F7 RID: 4599
		public OverlayDrawer overlayDrawer;

		// Token: 0x040011F8 RID: 4600
		public FloodFiller floodFiller;

		// Token: 0x040011F9 RID: 4601
		public WeatherDecider weatherDecider;

		// Token: 0x040011FA RID: 4602
		public FireWatcher fireWatcher;

		// Token: 0x040011FB RID: 4603
		public DangerWatcher dangerWatcher;

		// Token: 0x040011FC RID: 4604
		public DamageWatcher damageWatcher;

		// Token: 0x040011FD RID: 4605
		public StrengthWatcher strengthWatcher;

		// Token: 0x040011FE RID: 4606
		public WealthWatcher wealthWatcher;

		// Token: 0x040011FF RID: 4607
		public RegionDirtyer regionDirtyer;

		// Token: 0x04001200 RID: 4608
		public MapCellsInRandomOrder cellsInRandomOrder;

		// Token: 0x04001201 RID: 4609
		public RememberedCameraPos rememberedCameraPos;

		// Token: 0x04001202 RID: 4610
		public MineStrikeManager mineStrikeManager;

		// Token: 0x04001203 RID: 4611
		public StoryState storyState;

		// Token: 0x04001204 RID: 4612
		public RoadInfo roadInfo;

		// Token: 0x04001205 RID: 4613
		public WaterInfo waterInfo;

		// Token: 0x04001206 RID: 4614
		public RetainedCaravanData retainedCaravanData;

		// Token: 0x04001207 RID: 4615
		public TemporaryThingDrawer temporaryThingDrawer;

		// Token: 0x04001208 RID: 4616
		public AnimalPenManager animalPenManager;

		// Token: 0x04001209 RID: 4617
		public MapPlantGrowthRateCalculator plantGrowthRateCalculator;

		// Token: 0x0400120A RID: 4618
		public AutoSlaughterManager autoSlaughterManager;

		// Token: 0x0400120B RID: 4619
		public TreeDestructionTracker treeDestructionTracker;

		// Token: 0x0400120C RID: 4620
		public StorageGroupManager storageGroups;

		// Token: 0x0400120D RID: 4621
		public EffecterMaintainer effecterMaintainer;

		// Token: 0x0400120E RID: 4622
		public PostTickVisuals postTickVisuals;

		// Token: 0x0400120F RID: 4623
		public List<LayoutStructureSketch> layoutStructureSketches = new List<LayoutStructureSketch>();

		// Token: 0x04001210 RID: 4624
		public ThingListChangedCallbacks thingListChangedCallbacks = new ThingListChangedCallbacks();

		// Token: 0x04001211 RID: 4625
		public List<CellRect> landingBlockers = new List<CellRect>();

		// Token: 0x04001212 RID: 4626
		public Tile pocketTileInfo;

		// Token: 0x04001213 RID: 4627
		public const string ThingSaveKey = "thing";

		// Token: 0x04001214 RID: 4628
		[TweakValue("Graphics_Shadow", 0f, 100f)]
		private static bool AlwaysRedrawShadows;

		// Token: 0x04001216 RID: 4630
		private MixedBiomeMapComponent mixedBiomeComp;
	}
}
