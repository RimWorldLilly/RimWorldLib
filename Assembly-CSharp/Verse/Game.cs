using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.Profile;

namespace Verse
{
	// Token: 0x020001E9 RID: 489
	public class Game : IExposable, IDisposable
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00046B24 File Offset: 0x00044D24
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x00046B2C File Offset: 0x00044D2C
		public Scenario Scenario
		{
			get
			{
				return this.scenarioInt;
			}
			set
			{
				this.scenarioInt = value;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x00046B35 File Offset: 0x00044D35
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x00046B3D File Offset: 0x00044D3D
		public World World
		{
			get
			{
				return this.worldInt;
			}
			set
			{
				if (this.worldInt == value)
				{
					return;
				}
				this.worldInt = value;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00046B50 File Offset: 0x00044D50
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x00046B70 File Offset: 0x00044D70
		public Map CurrentMap
		{
			get
			{
				if (this.currentMapIndex < 0)
				{
					return null;
				}
				return this.maps[(int)this.currentMapIndex];
			}
			set
			{
				int num;
				if (value == null)
				{
					num = -1;
				}
				else
				{
					num = this.maps.IndexOf(value);
					if (num < 0)
					{
						Log.Error("Could not set current map because it does not exist.");
						return;
					}
				}
				if ((int)this.currentMapIndex != num)
				{
					this.currentMapIndex = (sbyte)num;
					Find.MapUI.Notify_SwitchedMap();
					AmbientSoundManager.Notify_SwitchedMap();
				}
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00046BC0 File Offset: 0x00044DC0
		public Map AnyPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				for (int i = 0; i < this.maps.Count; i++)
				{
					if (this.maps[i].IsPlayerHome)
					{
						return this.maps[i];
					}
				}
				if (ModsConfig.OdysseyActive)
				{
					for (int j = 0; j < this.maps.Count; j++)
					{
						if (GravshipUtility.PlayerHasGravEngine(this.maps[j]))
						{
							return this.maps[j];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00046C4A File Offset: 0x00044E4A
		public bool PlayerHasControl
		{
			get
			{
				return !ScreenFader.IsFading() && (!WorldComponent_GravshipController.CutsceneInProgress || Find.CameraDriver.config.gravshipFreeCam);
			}
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00046C70 File Offset: 0x00044E70
		public bool IsPlayerTile(PlanetTile tile)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				Map map = this.maps[i];
				if (map.Tile == tile && map.IsPlayerHome)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00046CBC File Offset: 0x00044EBC
		public IReadOnlyList<Map> PlayerHomeMaps
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				return Game.tmpPlayerHomeMaps;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00046D18 File Offset: 0x00044F18
		public Map RandomPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				if (Game.tmpPlayerHomeMaps.Any<Map>())
				{
					Map result = Game.tmpPlayerHomeMaps.RandomElement<Map>();
					Game.tmpPlayerHomeMaps.Clear();
					return result;
				}
				return null;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00046D90 File Offset: 0x00044F90
		public Map RandomRootSurfacePlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome && map.Tile.Layer.IsRootSurface)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				if (Game.tmpPlayerHomeMaps.Any<Map>())
				{
					Map result = Game.tmpPlayerHomeMaps.RandomElement<Map>();
					Game.tmpPlayerHomeMaps.Clear();
					return result;
				}
				return null;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00046E20 File Offset: 0x00045020
		public Map RandomSurfacePlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome && map.Tile.LayerDef.SurfaceTiles)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				if (Game.tmpPlayerHomeMaps.Any<Map>())
				{
					Map result = Game.tmpPlayerHomeMaps.RandomElement<Map>();
					Game.tmpPlayerHomeMaps.Clear();
					return result;
				}
				return null;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00046EAD File Offset: 0x000450AD
		public List<Map> Maps
		{
			get
			{
				return this.maps;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00046EB5 File Offset: 0x000450B5
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x00046EBD File Offset: 0x000450BD
		public GameInitData InitData
		{
			get
			{
				return this.initData;
			}
			set
			{
				this.initData = value;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x00046EC6 File Offset: 0x000450C6
		public GameInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00046ECE File Offset: 0x000450CE
		public GameRules Rules
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x00046ED6 File Offset: 0x000450D6
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x00046EDE File Offset: 0x000450DE
		public Gravship Gravship
		{
			get
			{
				return this.gravshipInt;
			}
			set
			{
				this.gravshipInt = value;
			}
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00046EE8 File Offset: 0x000450E8
		public Game()
		{
			this.FillComponents();
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00047074 File Offset: 0x00045274
		public void AddMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to add null map.");
				return;
			}
			if (this.maps.Contains(map))
			{
				Log.Error("Tried to add map but it's already here.");
				return;
			}
			if (this.maps.Count > 127)
			{
				Log.Error("Can't add map. Reached maps count limit (" + sbyte.MaxValue.ToString() + ").");
				return;
			}
			this.maps.Add(map);
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x000470EC File Offset: 0x000452EC
		public Map FindMap(MapParent mapParent)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].info.parent == mapParent)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00047138 File Offset: 0x00045338
		public Map FindMap(PlanetTile tile)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].Tile == tile)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x00047184 File Offset: 0x00045384
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Error("You must use special LoadData method to load Game.");
				return;
			}
			Scribe_Values.Look<sbyte>(ref this.currentMapIndex, "currentMapIndex", -1, false);
			this.ExposeSmallComponents();
			Scribe_Deep.Look<World>(ref this.worldInt, "world", Array.Empty<object>());
			Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
			Find.CameraDriver.Expose();
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x000471F4 File Offset: 0x000453F4
		private void ExposeSmallComponents()
		{
			Scribe_Deep.Look<GameInfo>(ref this.info, "info", Array.Empty<object>());
			Scribe_Deep.Look<GameRules>(ref this.rules, "rules", Array.Empty<object>());
			Scribe_Deep.Look<Scenario>(ref this.scenarioInt, "scenario", Array.Empty<object>());
			Scribe_Deep.Look<TickManager>(ref this.tickManager, "tickManager", Array.Empty<object>());
			Scribe_Deep.Look<PlaySettings>(ref this.playSettings, "playSettings", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher>(ref this.storyWatcher, "storyWatcher", Array.Empty<object>());
			Scribe_Deep.Look<GameEnder>(ref this.gameEnder, "gameEnder", Array.Empty<object>());
			Scribe_Deep.Look<LetterStack>(ref this.letterStack, "letterStack", Array.Empty<object>());
			Scribe_Deep.Look<ResearchManager>(ref this.researchManager, "researchManager", Array.Empty<object>());
			Scribe_Deep.Look<AnalysisManager>(ref this.analysisManager, "analysisManager", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.analysisManager == null)
			{
				this.analysisManager = new AnalysisManager();
			}
			Scribe_Deep.Look<Storyteller>(ref this.storyteller, "storyteller", Array.Empty<object>());
			Scribe_Deep.Look<History>(ref this.history, "history", Array.Empty<object>());
			Scribe_Deep.Look<TaleManager>(ref this.taleManager, "taleManager", Array.Empty<object>());
			Scribe_Deep.Look<PlayLog>(ref this.playLog, "playLog", Array.Empty<object>());
			Scribe_Deep.Look<BattleLog>(ref this.battleLog, "battleLog", Array.Empty<object>());
			Scribe_Deep.Look<OutfitDatabase>(ref this.outfitDatabase, "outfitDatabase", Array.Empty<object>());
			Scribe_Deep.Look<DrugPolicyDatabase>(ref this.drugPolicyDatabase, "drugPolicyDatabase", Array.Empty<object>());
			Scribe_Deep.Look<FoodRestrictionDatabase>(ref this.foodRestrictionDatabase, "foodRestrictionDatabase", Array.Empty<object>());
			Scribe_Deep.Look<ReadingPolicyDatabase>(ref this.readingPolicyDatabase, "readingPolicyDatabase", Array.Empty<object>());
			Scribe_Deep.Look<Tutor>(ref this.tutor, "tutor", Array.Empty<object>());
			Scribe_Deep.Look<DateNotifier>(ref this.dateNotifier, "dateNotifier", Array.Empty<object>());
			Scribe_Deep.Look<UniqueIDsManager>(ref this.uniqueIDsManager, "uniqueIDsManager", Array.Empty<object>());
			Scribe_Deep.Look<QuestManager>(ref this.questManager, "questManager", Array.Empty<object>());
			Scribe_Deep.Look<TransportShipManager>(ref this.transportShipManager, "transportShipManager", Array.Empty<object>());
			Scribe_Deep.Look<StudyManager>(ref this.studyManager, "studyManager", Array.Empty<object>());
			Scribe_Deep.Look<CustomXenogermDatabase>(ref this.customXenogermDatabase, "customXenogermDatabase", Array.Empty<object>());
			Scribe_Deep.Look<CustomXenotypeDatabase>(ref this.customXenotypeDatabase, "customXenotypeDatabase", Array.Empty<object>());
			Scribe_Deep.Look<RelationshipRecords>(ref this.relationshipRecords, "relationshipRecords", Array.Empty<object>());
			Scribe_Deep.Look<HiddenItemsManager>(ref this.hiddenItemsManager, "hiddenItemsManager", Array.Empty<object>());
			Scribe_Deep.Look<EntityCodex>(ref this.entityCodex, "entityCodex", Array.Empty<object>());
			Scribe_Collections.Look<GameComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.FillComponents();
				if (this.rules == null)
				{
					Log.Warning("Save game was missing rules. Replacing with a blank GameRules.");
					this.rules = new GameRules();
				}
				if (this.relationshipRecords == null)
				{
					this.relationshipRecords = new RelationshipRecords();
				}
				if (this.readingPolicyDatabase == null)
				{
					this.readingPolicyDatabase = new ReadingPolicyDatabase();
				}
				if (this.hiddenItemsManager == null)
				{
					this.hiddenItemsManager = new HiddenItemsManager();
				}
				if (this.entityCodex == null)
				{
					this.entityCodex = new EntityCodex();
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0004752C File Offset: 0x0004572C
		private void FillComponents()
		{
			this.components.RemoveAll((GameComponent component) => component == null);
			foreach (Type type in typeof(GameComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						GameComponent item = (GameComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate a GameComponent of type ";
						Type type2 = type;
						string str2 = (type2 != null) ? type2.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00047618 File Offset: 0x00045818
		public void InitNewGame()
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.PackageIdPlayerFacing + ((!mod.ModMetaData.VersionCompatible) ? " (incompatible version)" : "")).ToLineList("  - ", false);
			Log.Message("Initializing new game with mods:\n" + str);
			if (this.maps.Any<Map>())
			{
				Log.Error("Called InitNewGame() but there already is a map. There should be 0 maps...");
				return;
			}
			if (this.initData == null)
			{
				Log.Error("Called InitNewGame() but init data is null. Create it first.");
				return;
			}
			Game.ClearCaches();
			MemoryUtility.UnloadUnusedUnityAssets();
			try
			{
				Current.ProgramState = ProgramState.MapInitializing;
				IntVec3 intVec = new IntVec3(this.initData.mapSize, 1, this.initData.mapSize);
				Settlement settlement = null;
				List<Settlement> settlements = Find.WorldObjects.Settlements;
				for (int i = 0; i < settlements.Count; i++)
				{
					if (settlements[i].Faction == Faction.OfPlayer)
					{
						settlement = settlements[i];
						break;
					}
				}
				if (settlement == null)
				{
					Log.Error("Could not generate starting map because there is no any player faction base.");
				}
				this.tickManager.gameStartAbsTick = GenTicks.ConfiguredTicksAbsAtGameStart;
				this.info.startingTile = this.initData.startingTile;
				this.info.startingAndOptionalPawns = this.initData.startingAndOptionalPawns;
				Map currentMap = MapGenerator.GenerateMap(intVec, settlement, this.initData.mapGeneratorDef ?? settlement.MapGeneratorDef, settlement.ExtraGenStepDefs, null, false, false);
				this.worldInt.info.initialMapSize = intVec;
				if (this.initData.permadeath)
				{
					this.info.permadeathMode = true;
					this.info.permadeathModeUniqueName = PermadeathModeUtility.GeneratePermadeathSaveName();
				}
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.NewColonyOptimism);
				this.FinalizeInit();
				Current.Game.CurrentMap = currentMap;
				Find.CameraDriver.JumpToCurrentMapLoc(MapGenerator.PlayerStartSpot);
				Find.CameraDriver.ResetSize();
				if (Prefs.PauseOnLoad && this.initData.startedFromEntry)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						this.tickManager.DoSingleTick();
						this.tickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				Find.Scenario.PostGameStart();
				this.history.FinalizeInit();
				ResearchUtility.ApplyPlayerStartingResearch();
				GameComponentUtility.StartedNewGame();
				this.initData = null;
			}
			finally
			{
			}
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00047850 File Offset: 0x00045A50
		public void LoadGame()
		{
			if (this.maps.Any<Map>())
			{
				Log.Error("Called LoadGame() but there already is a map. There should be 0 maps...");
				return;
			}
			Game.ClearCaches();
			MemoryUtility.UnloadUnusedUnityAssets();
			BackCompatibility.PreLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
			Current.ProgramState = ProgramState.MapInitializing;
			this.ExposeSmallComponents();
			LongEventHandler.SetCurrentEventText("LoadingWorld".Translate());
			if (Scribe.EnterNode("world"))
			{
				try
				{
					this.World = new World();
					this.World.ExposeData();
					Scribe.loader.crossRefs.RegisterForCrossRefResolve(this.World);
					goto IL_96;
				}
				finally
				{
					Scribe.ExitNode();
				}
				goto IL_8B;
				IL_96:
				DeepProfiler.Start("World.FinalizeInit");
				this.World.FinalizeInit(true);
				DeepProfiler.End();
				LongEventHandler.SetCurrentEventText("LoadingMap".Translate());
				Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
				if (this.maps.RemoveAll((Map x) => x == null) != 0)
				{
					Log.Warning("Some maps were null after loading.");
				}
				int num = -1;
				Scribe_Values.Look<int>(ref num, "currentMapIndex", -1, false);
				if (num < 0 && this.maps.Any<Map>())
				{
					Log.Error("Current map is null after loading but there are maps available. Setting current map to [0].");
					num = 0;
				}
				if (num >= this.maps.Count)
				{
					Log.Error("Current map index out of bounds after loading.");
					if (this.maps.Any<Map>())
					{
						num = 0;
					}
					else
					{
						num = -1;
					}
				}
				this.currentMapIndex = sbyte.MinValue;
				this.CurrentMap = ((num >= 0) ? this.maps[num] : null);
				LongEventHandler.SetCurrentEventText("InitializingGame".Translate());
				Find.CameraDriver.Expose();
				DeepProfiler.Start("Scribe.loader.FinalizeLoading");
				Scribe.loader.FinalizeLoading();
				DeepProfiler.End();
				LongEventHandler.SetCurrentEventText("SpawningAllThings".Translate());
				DeepProfiler.Start("maps.FinalizeLoading");
				for (int i = 0; i < this.maps.Count; i++)
				{
					try
					{
						this.maps[i].FinalizeLoading();
					}
					catch (Exception ex)
					{
						string str = "Error in Map.FinalizeLoading(): ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					try
					{
						MapParent parent = this.maps[i].Parent;
						if (parent != null)
						{
							parent.FinalizeLoading();
						}
					}
					catch (Exception ex3)
					{
						string str2 = "Error in MapParent.FinalizeLoading(): ";
						Exception ex4 = ex3;
						Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
				}
				DeepProfiler.End();
				DeepProfiler.Start("Game.FinalizeInit");
				this.FinalizeInit();
				DeepProfiler.End();
				if (Prefs.PauseOnLoad)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						Find.TickManager.DoSingleTick();
						Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				GameComponentUtility.LoadedGame();
				BackCompatibility.PostLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
				return;
			}
			IL_8B:
			Log.Error("Could not find world XML node.");
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00047B44 File Offset: 0x00045D44
		public void UpdateEntry()
		{
			GameComponentUtility.GameComponentUpdate();
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00047B4C File Offset: 0x00045D4C
		public void UpdatePlay()
		{
			try
			{
				Find.LetterStack.OpenAutomaticLetters();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.tickManager.TickManagerUpdate();
			this.letterStack.LetterStackUpdate();
			this.World.WorldUpdate();
			for (int i = 0; i < this.maps.Count; i++)
			{
				this.maps[i].MapUpdate();
			}
			this.Info.GameInfoUpdate();
			GameComponentUtility.GameComponentUpdate();
			this.signalManager.SignalManagerUpdate();
			GlobalTextureAtlasManager.GlobalTextureAtlasManagerUpdate();
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00047BEC File Offset: 0x00045DEC
		public T GetComponent<T>() where T : GameComponent
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

		// Token: 0x06000DDA RID: 3546 RVA: 0x00047C3C File Offset: 0x00045E3C
		public GameComponent GetComponent(Type type)
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

		// Token: 0x06000DDB RID: 3547 RVA: 0x00047C84 File Offset: 0x00045E84
		public void FinalizeInit()
		{
			LogSimple.FlushToFileAndOpen();
			this.researchManager.ReapplyAllMods();
			MessagesRepeatAvoider.Reset();
			GameComponentUtility.FinalizeInit();
			Current.ProgramState = ProgramState.Playing;
			Current.Game.World.ideoManager.Notify_GameStarted();
			RecipeDefGenerator.ResetRecipeIngredientsForDifficulty();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DebugSettings.devPalette = Prefs.StartDevPaletteOn;
				Find.UIRoot.debugWindowOpener.TryOpenOrClosePalette();
			});
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00047CF0 File Offset: 0x00045EF0
		public void DeinitAndRemoveMap(Map map, bool notifyPlayer)
		{
			if (map == null)
			{
				Log.Error("Tried to remove null map.");
				return;
			}
			if (!this.maps.Contains(map))
			{
				Log.Error("Tried to remove map " + ((map != null) ? map.ToString() : null) + " but it's not here.");
				return;
			}
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapAboutToBeRemoved();
			}
			Map currentMap = this.CurrentMap;
			MapDeiniter.Deinit(map, notifyPlayer);
			this.maps.Remove(map);
			if (currentMap != null)
			{
				sbyte b = (sbyte)this.maps.IndexOf(currentMap);
				if (b < 0)
				{
					if (this.maps.Any<Map>())
					{
						this.CurrentMap = this.maps[0];
					}
					else
					{
						this.CurrentMap = null;
					}
					Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				}
				else
				{
					this.currentMapIndex = b;
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			MapComponentUtility.MapRemoved(map);
			Find.Scenario.MapRemoved(map);
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapRemoved(map);
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps.ToList<PocketMapParent>())
			{
				if (pocketMapParent.sourceMap == map && pocketMapParent.Map.generatorDef.pocketMapProperties.destroyOnParentMapAbandoned)
				{
					PocketMapUtility.DestroyPocketMap(pocketMapParent.Map);
				}
			}
			map.Dispose();
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00047E70 File Offset: 0x00046070
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Game debug data:");
			stringBuilder.AppendLine("initData:");
			if (this.initData == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine(this.initData.ToString());
			}
			stringBuilder.AppendLine("Scenario:");
			if (this.scenarioInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "   ";
				Scenario scenario = this.scenarioInt;
				stringBuilder2.AppendLine(str + ((scenario != null) ? scenario.ToString() : null));
			}
			stringBuilder.AppendLine("World:");
			if (this.worldInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   name: " + this.worldInt.info.name);
			}
			stringBuilder.AppendLine("Maps count: " + this.maps.Count.ToString());
			for (int i = 0; i < this.maps.Count; i++)
			{
				stringBuilder.AppendLine("   Map " + this.maps[i].Index.ToString() + ":");
				StringBuilder stringBuilder3 = stringBuilder;
				string str2 = "      tile: ";
				Tile tileInfo = this.maps[i].TileInfo;
				stringBuilder3.AppendLine(str2 + ((tileInfo != null) ? tileInfo.ToString() : null));
			}
			stringBuilder.AppendLine("Game components:");
			for (int j = 0; j < this.components.Count; j++)
			{
				this.components[j].AppendDebugString(stringBuilder);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0004801C File Offset: 0x0004621C
		public void Dispose()
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				this.maps[i].Dispose();
			}
			World world = this.worldInt;
			if (world != null)
			{
				world.Dispose();
			}
			SteadyEnvironmentEffects.Reset();
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00048068 File Offset: 0x00046268
		public static void ClearCaches()
		{
			Find.ClearCache();
			ChildcareUtility.ClearCache();
			SlaveRebellionUtility.ClearCache();
			Alert_NeedMeditationSpot.ClearCache();
			BuildCopyCommandUtility.ClearCache();
			MechanitorUtility.ClearCache();
			SocialCardUtility.ClearCaches();
			foreach (StatDef statDef in DefDatabase<StatDef>.AllDefsListForReading)
			{
				statDef.Worker.TryClearCache();
			}
		}

		// Token: 0x04000DBC RID: 3516
		private GameInitData initData;

		// Token: 0x04000DBD RID: 3517
		private Gravship gravshipInt;

		// Token: 0x04000DBE RID: 3518
		public sbyte currentMapIndex = -1;

		// Token: 0x04000DBF RID: 3519
		private GameInfo info = new GameInfo();

		// Token: 0x04000DC0 RID: 3520
		public List<GameComponent> components = new List<GameComponent>();

		// Token: 0x04000DC1 RID: 3521
		private GameRules rules = new GameRules();

		// Token: 0x04000DC2 RID: 3522
		private Scenario scenarioInt;

		// Token: 0x04000DC3 RID: 3523
		private World worldInt;

		// Token: 0x04000DC4 RID: 3524
		private List<Map> maps = new List<Map>();

		// Token: 0x04000DC5 RID: 3525
		public PlaySettings playSettings = new PlaySettings();

		// Token: 0x04000DC6 RID: 3526
		public StoryWatcher storyWatcher = new StoryWatcher();

		// Token: 0x04000DC7 RID: 3527
		public LetterStack letterStack = new LetterStack();

		// Token: 0x04000DC8 RID: 3528
		public ResearchManager researchManager = new ResearchManager();

		// Token: 0x04000DC9 RID: 3529
		public AnalysisManager analysisManager = new AnalysisManager();

		// Token: 0x04000DCA RID: 3530
		public GameEnder gameEnder = new GameEnder();

		// Token: 0x04000DCB RID: 3531
		public Storyteller storyteller = new Storyteller();

		// Token: 0x04000DCC RID: 3532
		public History history = new History();

		// Token: 0x04000DCD RID: 3533
		public TaleManager taleManager = new TaleManager();

		// Token: 0x04000DCE RID: 3534
		public PlayLog playLog = new PlayLog();

		// Token: 0x04000DCF RID: 3535
		public BattleLog battleLog = new BattleLog();

		// Token: 0x04000DD0 RID: 3536
		public OutfitDatabase outfitDatabase = new OutfitDatabase();

		// Token: 0x04000DD1 RID: 3537
		public DrugPolicyDatabase drugPolicyDatabase = new DrugPolicyDatabase();

		// Token: 0x04000DD2 RID: 3538
		public ReadingPolicyDatabase readingPolicyDatabase = new ReadingPolicyDatabase();

		// Token: 0x04000DD3 RID: 3539
		public FoodRestrictionDatabase foodRestrictionDatabase = new FoodRestrictionDatabase();

		// Token: 0x04000DD4 RID: 3540
		public TickManager tickManager = new TickManager();

		// Token: 0x04000DD5 RID: 3541
		public Tutor tutor = new Tutor();

		// Token: 0x04000DD6 RID: 3542
		public Autosaver autosaver = new Autosaver();

		// Token: 0x04000DD7 RID: 3543
		public DateNotifier dateNotifier = new DateNotifier();

		// Token: 0x04000DD8 RID: 3544
		public SignalManager signalManager = new SignalManager();

		// Token: 0x04000DD9 RID: 3545
		public UniqueIDsManager uniqueIDsManager = new UniqueIDsManager();

		// Token: 0x04000DDA RID: 3546
		public QuestManager questManager = new QuestManager();

		// Token: 0x04000DDB RID: 3547
		public TransportShipManager transportShipManager = new TransportShipManager();

		// Token: 0x04000DDC RID: 3548
		public StudyManager studyManager = new StudyManager();

		// Token: 0x04000DDD RID: 3549
		public CustomXenogermDatabase customXenogermDatabase = new CustomXenogermDatabase();

		// Token: 0x04000DDE RID: 3550
		public CustomXenotypeDatabase customXenotypeDatabase = new CustomXenotypeDatabase();

		// Token: 0x04000DDF RID: 3551
		public RelationshipRecords relationshipRecords = new RelationshipRecords();

		// Token: 0x04000DE0 RID: 3552
		public HiddenItemsManager hiddenItemsManager = new HiddenItemsManager();

		// Token: 0x04000DE1 RID: 3553
		public EntityCodex entityCodex = new EntityCodex();

		// Token: 0x04000DE2 RID: 3554
		private static readonly List<Map> tmpPlayerHomeMaps = new List<Map>();
	}
}
