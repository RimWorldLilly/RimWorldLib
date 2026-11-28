using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002DD RID: 733
	public static class MapGenerator
	{
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x00085549 File Offset: 0x00083749
		public static MapGenFloatGrid Elevation
		{
			get
			{
				return MapGenerator.FloatGridNamed("Elevation");
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x0600163D RID: 5693 RVA: 0x00085555 File Offset: 0x00083755
		public static MapGenFloatGrid Fertility
		{
			get
			{
				return MapGenerator.FloatGridNamed("Fertility");
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x0600163E RID: 5694 RVA: 0x00085561 File Offset: 0x00083761
		public static MapGenFloatGrid Caves
		{
			get
			{
				return MapGenerator.FloatGridNamed("Caves");
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x0600163F RID: 5695 RVA: 0x0008556D File Offset: 0x0008376D
		public static List<CellRect> UsedRects
		{
			get
			{
				return MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x00085579 File Offset: 0x00083779
		// (set) Token: 0x06001641 RID: 5697 RVA: 0x00085597 File Offset: 0x00083797
		public static IntVec3 PlayerStartSpot
		{
			get
			{
				if (!MapGenerator.PlayerStartSpotValid)
				{
					Log.Error("Accessing player start spot before setting it.");
					return IntVec3.Invalid;
				}
				return MapGenerator.playerStartSpotInt;
			}
			set
			{
				MapGenerator.playerStartSpotInt = value;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x0008559F File Offset: 0x0008379F
		public static bool PlayerStartSpotValid
		{
			get
			{
				return MapGenerator.playerStartSpotInt.IsValid;
			}
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x000855AC File Offset: 0x000837AC
		public static Map GenerateMap(IntVec3 mapSize, MapParent parent, MapGeneratorDef mapGenerator, IEnumerable<GenStepWithParams> extraGenStepDefs = null, Action<Map> extraInitBeforeContentGen = null, bool isPocketMap = false, bool stepDebugger = false)
		{
			ProgramState programState = Current.ProgramState;
			Current.ProgramState = ProgramState.MapInitializing;
			MapGenerator.ClearWorkingData();
			MapGenerator.playerStartSpotInt = IntVec3.Invalid;
			MapGenerator.rootsToUnfog.Clear();
			MapGenerator.mapBeingGenerated = null;
			MapGenerator.gravship = null;
			DeepProfiler.Start("InitNewGeneratedMap");
			Rand.PushState();
			int seed = Gen.HashCombineInt(Find.World.info.Seed, (parent != null) ? parent.Tile.GetHashCode() : 0);
			if (isPocketMap)
			{
				seed = Gen.HashCombineInt(Find.World.info.Seed, (parent != null) ? parent.ID : Rand.Int);
			}
			Rand.Seed = seed;
			if (stepDebugger)
			{
				MapGenerator.debugMode = true;
				MapGenerator.debugSeed = seed;
				MapGenerator.debugGenStepIndex = 0;
			}
			Map map2;
			try
			{
				if (parent != null && parent.HasMap)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(137, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to generate a new map and set ");
					defaultInterpolatedStringHandler.AppendFormatted<MapParent>(parent);
					defaultInterpolatedStringHandler.AppendLiteral(" as its parent, but this world object already has a map. One world object can't have more than 1 map.");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					parent = null;
				}
				DeepProfiler.Start("Set up map");
				Map map = new Map();
				map.uniqueID = Find.UniqueIDsManager.GetNextMapID();
				map.generationTick = GenTicks.TicksGame;
				map.events = new MapEvents(map);
				MapGenerator.mapBeingGenerated = map;
				map.info.Size = mapSize;
				map.info.parent = parent;
				if (ModsConfig.OdysseyActive && extraGenStepDefs != null)
				{
					if (extraGenStepDefs.Any((GenStepWithParams step) => step.def == GenStepDefOf.ReserveGravshipArea))
					{
						map.wasSpawnedViaGravShipLanding = true;
					}
				}
				if (mapGenerator == null)
				{
					Log.Error("Attempted to generate map without generator; falling back on encounter map");
					mapGenerator = MapGeneratorDefOf.Encounter;
				}
				map.generatorDef = mapGenerator;
				map.info.disableSunShadows = mapGenerator.disableShadows;
				if (isPocketMap)
				{
					map.info.isPocketMap = true;
					map.pocketTileInfo = new Tile
					{
						PrimaryBiome = mapGenerator.pocketMapProperties.biome
					};
					foreach (TileMutatorDef mutator in mapGenerator.pocketMapProperties.tileMutators)
					{
						map.TileInfo.AddMutator(mutator);
					}
				}
				map.ConstructComponents();
				foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
				{
					TileMutatorWorker worker = tileMutatorDef.Worker;
					if (worker != null)
					{
						worker.Init(map);
					}
				}
				DeepProfiler.End();
				Current.Game.AddMap(map);
				if (mapGenerator.isUnderground)
				{
					foreach (IntVec3 c in map.AllCells)
					{
						map.roofGrid.SetRoof(c, mapGenerator.roofDef ?? RoofDefOf.RoofRockThick);
					}
				}
				if (extraInitBeforeContentGen != null)
				{
					extraInitBeforeContentGen(map);
				}
				IEnumerable<GenStepWithParams> enumerable = mapGenerator.genSteps.Where(new Func<GenStepDef, bool>(MapGenerator.<GenerateMap>g__IsValidBiome|32_1)).Select(new Func<GenStepDef, GenStepWithParams>(MapGenerator.<GenerateMap>g__GetGenStepParms|32_2));
				foreach (TileMutatorDef tileMutatorDef2 in map.TileInfo.Mutators)
				{
					if (tileMutatorDef2.extraGenSteps.Any<GenStepDef>())
					{
						enumerable = enumerable.Concat(tileMutatorDef2.extraGenSteps.Select(new Func<GenStepDef, GenStepWithParams>(MapGenerator.<GenerateMap>g__GetGenStepParms|32_2)));
					}
				}
				if (map.Biome.extraGenSteps.Any<GenStepDef>())
				{
					enumerable = enumerable.Concat(map.Biome.extraGenSteps.Where(new Func<GenStepDef, bool>(MapGenerator.<GenerateMap>g__IsValidBiome|32_1)).Select(new Func<GenStepDef, GenStepWithParams>(MapGenerator.<GenerateMap>g__GetGenStepParms|32_2)));
				}
				if (map.Biome.preventGenSteps.Any<GenStepDef>())
				{
					enumerable = from step in enumerable
					where !map.Biome.preventGenSteps.Contains(step.def)
					select step;
				}
				using (IEnumerator<TileMutatorDef> enumerator2 = map.TileInfo.Mutators.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						TileMutatorDef mut = enumerator2.Current;
						if (mut.preventGenSteps.Any<GenStepDef>())
						{
							enumerable = from step in enumerable
							where !mut.preventGenSteps.Contains(step.def)
							select step;
						}
					}
				}
				if (extraGenStepDefs != null)
				{
					enumerable = enumerable.Concat(extraGenStepDefs);
				}
				enumerable = enumerable.Distinct<GenStepWithParams>();
				map.areaManager.AddStartingAreas();
				map.weatherDecider.StartInitialWeather();
				DeepProfiler.Start("Generate contents into map");
				MapGenerator.GenerateContentsIntoMap(enumerable, map, seed, stepDebugger);
				DeepProfiler.End();
				Find.Scenario.PostMapGenerate(map);
				DeepProfiler.Start("Finalize map init");
				map.FinalizeInit();
				DeepProfiler.End();
				DeepProfiler.Start("MapComponent.MapGenerated()");
				MapComponentUtility.MapGenerated(map);
				DeepProfiler.End();
				if (parent != null)
				{
					parent.PostMapGenerate();
				}
				DeepProfiler.Start("Map generator post init");
				if (!stepDebugger)
				{
					MapGenerator.MapGeneratorPostInit(enumerable, map);
				}
				DeepProfiler.End();
				if (MapGenerator.gravship != null && !stepDebugger && map.Parent.CanBeSettled)
				{
					GravshipUtility.SettleTile(map);
				}
				if (map.TileInfo.Layer.Def.isSpace)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Orbit, OpportunityType.Critical);
				}
				map2 = map;
			}
			finally
			{
				DeepProfiler.End();
				if (!stepDebugger)
				{
					MapGenerator.ClearWorkingData();
					MapGenerator.mapBeingGenerated = null;
					MapGenerator.gravship = null;
				}
				Current.ProgramState = programState;
				Rand.PopState();
			}
			return map2;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00085C60 File Offset: 0x00083E60
		public static bool DebugDoNextGenStep(Map map)
		{
			if (MapGenerator.debugGenStepIndex >= MapGenerator.tmpGenSteps.Count)
			{
				MapGenerator.ClearDebugMode();
				return false;
			}
			GenStepWithParams genStepWithParams = MapGenerator.tmpGenSteps[MapGenerator.debugGenStepIndex];
			Log.Message("Doing gen step " + genStepWithParams.def.defName);
			Rand.PushState(Gen.HashCombineInt(MapGenerator.debugSeed, MapGenerator.GetSeedPart(MapGenerator.tmpGenSteps, MapGenerator.debugGenStepIndex)));
			try
			{
				GenStepParams parms = genStepWithParams.parms;
				if (MapGenerator.gravship != null)
				{
					parms.gravship = MapGenerator.gravship;
				}
				genStepWithParams.def.genStep.Generate(map, parms);
				genStepWithParams.def.genStep.PostMapInitialized(map, parms);
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error stepping GenStep ");
				defaultInterpolatedStringHandler.AppendFormatted(genStepWithParams.def.defName);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Rand.PopState();
			map.FinalizeInit();
			MapGenerator.debugGenStepIndex++;
			return true;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00085D80 File Offset: 0x00083F80
		public static void ClearDebugMode()
		{
			MapGenerator.debugMode = false;
			MapGenerator.debugSeed = 0;
			MapGenerator.debugGenStepIndex = 0;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00085D94 File Offset: 0x00083F94
		public static void MapGeneratorPostInit(IEnumerable<GenStepWithParams> genStepDefs, Map map)
		{
			MapGenerator.tmpGenSteps.Clear();
			MapGenerator.tmpGenSteps.AddRange(from x in genStepDefs
			orderby x.def.order, x.def.index
			select x);
			MapGenerator.tmpGenSteps.RemoveWhere((GenStepWithParams a) => MapGenerator.tmpGenSteps.Any((GenStepWithParams b) => b.def.preventsGenSteps != null && b.def.preventsGenSteps.Contains(a.def)));
			for (int i = 0; i < MapGenerator.tmpGenSteps.Count; i++)
			{
				GenStepParams parms = MapGenerator.tmpGenSteps[i].parms;
				if (MapGenerator.gravship != null)
				{
					parms.gravship = MapGenerator.gravship;
				}
				MapGenerator.tmpGenSteps[i].def.genStep.PostMapInitialized(map, parms);
			}
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x00085E7C File Offset: 0x0008407C
		public static void GenerateContentsIntoMap(IEnumerable<GenStepWithParams> genStepDefs, Map map, int seed, bool stepDebugger = false)
		{
			MapGenerator.ClearWorkingData();
			if (ModsConfig.OdysseyActive)
			{
				foreach (GenStepWithParams genStepWithParams in genStepDefs)
				{
					if (genStepWithParams.def == GenStepDefOf.GravshipMarker)
					{
						MapGenerator.gravship = genStepWithParams.parms.gravship;
						break;
					}
				}
			}
			Rand.PushState();
			try
			{
				Rand.Seed = seed;
				RockNoises.Init(map);
				MapGenerator.tmpGenSteps.Clear();
				MapGenerator.tmpGenSteps.AddRange(from x in genStepDefs
				orderby x.def.order, x.def.index
				select x);
				MapGenerator.tmpGenSteps.RemoveWhere((GenStepWithParams a) => MapGenerator.tmpGenSteps.Any((GenStepWithParams b) => b.def.preventsGenSteps != null && b.def.preventsGenSteps.Contains(a.def)));
				if (!stepDebugger)
				{
					for (int i = 0; i < MapGenerator.tmpGenSteps.Count; i++)
					{
						string str = "GenStep - ";
						GenStepDef def = MapGenerator.tmpGenSteps[i].def;
						DeepProfiler.Start(str + ((def != null) ? def.ToString() : null));
						try
						{
							GenStepParams parms = MapGenerator.tmpGenSteps[i].parms;
							if (MapGenerator.gravship != null)
							{
								parms.gravship = MapGenerator.gravship;
							}
							Rand.PushState();
							Rand.Seed = Gen.HashCombineInt(seed, MapGenerator.GetSeedPart(MapGenerator.tmpGenSteps, i));
							MapGenerator.tmpGenSteps[i].def.genStep.Generate(map, parms);
							if (map.pathing.IncrementalDirtyingDisabled)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(116, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Genstep [");
								defaultInterpolatedStringHandler.AppendFormatted<int>(i);
								defaultInterpolatedStringHandler.AppendLiteral("] ");
								defaultInterpolatedStringHandler.AppendFormatted<GenStepDef>(MapGenerator.tmpGenSteps[i].def);
								defaultInterpolatedStringHandler.AppendLiteral(" ended with path incremental dirtying disabled, for safety reasons it must be reenabled before returning.");
								Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
								map.pathing.ReEnableIncrementalDirtying();
							}
							Rand.PopState();
						}
						catch (Exception value)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(18, 1);
							defaultInterpolatedStringHandler2.AppendLiteral("Error in GenStep: ");
							defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value);
							Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
						}
						finally
						{
							DeepProfiler.End();
						}
					}
				}
			}
			finally
			{
				if (!stepDebugger)
				{
					Rand.PopState();
					RockNoises.Reset();
				}
			}
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0008613C File Offset: 0x0008433C
		private static void ClearWorkingData()
		{
			List<CellRect> source;
			if (Prefs.DevMode && MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out source))
			{
				MapGenerator.cachedUsedRects = source.ToList<CellRect>();
			}
			foreach (KeyValuePair<string, object> keyValuePair in MapGenerator.data)
			{
				string text;
				object obj;
				keyValuePair.Deconstruct(out text, out obj);
				IDisposable disposable = obj as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			MapGenerator.data.Clear();
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x000861D0 File Offset: 0x000843D0
		public static void DebugDraw()
		{
			if (DebugViewSettings.drawUsedRects)
			{
				float y = AltitudeLayer.MetaOverlays.AltitudeFor();
				foreach (CellRect cellRect in MapGenerator.cachedUsedRects)
				{
					GenDraw.DrawLineBetween(new Vector3((float)cellRect.minX, y, (float)cellRect.minZ), new Vector3((float)cellRect.minX, y, (float)(cellRect.maxZ + 1)), SimpleColor.Red, 0.2f);
					GenDraw.DrawLineBetween(new Vector3((float)(cellRect.maxX + 1), y, (float)cellRect.minZ), new Vector3((float)(cellRect.maxX + 1), y, (float)(cellRect.maxZ + 1)), SimpleColor.Red, 0.2f);
					GenDraw.DrawLineBetween(new Vector3((float)cellRect.minX, y, (float)cellRect.minZ), new Vector3((float)(cellRect.maxX + 1), y, (float)cellRect.minZ), SimpleColor.Red, 0.2f);
					GenDraw.DrawLineBetween(new Vector3((float)cellRect.minX, y, (float)(cellRect.maxZ + 1)), new Vector3((float)(cellRect.maxX + 1), y, (float)(cellRect.maxZ + 1)), SimpleColor.Red, 0.2f);
				}
			}
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00086310 File Offset: 0x00084510
		public static T GetVar<T>(string name)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0008633C File Offset: 0x0008453C
		public static bool TryGetVar<T>(string name, out T var)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				var = (T)((object)obj);
				return true;
			}
			var = default(T);
			return false;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00086370 File Offset: 0x00084570
		public static T GetOrGenerateVar<T>(string name)
		{
			T t = MapGenerator.GetVar<T>(name);
			if (t != null)
			{
				return t;
			}
			t = (T)((object)Activator.CreateInstance(typeof(T)));
			MapGenerator.SetVar<T>(name, t);
			return t;
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000863AB File Offset: 0x000845AB
		public static void SetVar<T>(string name, T var)
		{
			MapGenerator.data[name] = var;
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000863C0 File Offset: 0x000845C0
		public static MapGenFloatGrid FloatGridNamed(string name)
		{
			MapGenFloatGrid var = MapGenerator.GetVar<MapGenFloatGrid>(name);
			if (var != null)
			{
				return var;
			}
			MapGenFloatGrid mapGenFloatGrid = new MapGenFloatGrid(MapGenerator.mapBeingGenerated);
			MapGenerator.SetVar<MapGenFloatGrid>(name, mapGenFloatGrid);
			return mapGenFloatGrid;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x000863EC File Offset: 0x000845EC
		private static int GetSeedPart(List<GenStepWithParams> genSteps, int index)
		{
			int seedPart = genSteps[index].def.genStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (MapGenerator.tmpGenSteps[i].def.genStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0008649C File Offset: 0x0008469C
		[CompilerGenerated]
		internal static bool <GenerateMap>g__IsValidBiome|32_1(GenStepDef g)
		{
			return !Find.Scenario.parts.Any((ScenPart p) => typeof(ScenPart_DisableMapGen).IsAssignableFrom(p.def.scenPartClass) && p.def.genStep == g);
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x000864D4 File Offset: 0x000846D4
		[CompilerGenerated]
		internal static GenStepWithParams <GenerateMap>g__GetGenStepParms|32_2(GenStepDef x)
		{
			return new GenStepWithParams(x, default(GenStepParams));
		}

		// Token: 0x0400133C RID: 4924
		public static Map mapBeingGenerated;

		// Token: 0x0400133D RID: 4925
		private static IntVec3 playerStartSpotInt = IntVec3.Invalid;

		// Token: 0x0400133E RID: 4926
		private static Gravship gravship;

		// Token: 0x0400133F RID: 4927
		public static List<IntVec3> rootsToUnfog = new List<IntVec3>();

		// Token: 0x04001340 RID: 4928
		private static Dictionary<string, object> data = new Dictionary<string, object>();

		// Token: 0x04001341 RID: 4929
		private static List<GenStepWithParams> tmpGenSteps = new List<GenStepWithParams>();

		// Token: 0x04001342 RID: 4930
		private static int debugSeed;

		// Token: 0x04001343 RID: 4931
		public static bool debugMode = false;

		// Token: 0x04001344 RID: 4932
		private static int debugGenStepIndex = 0;

		// Token: 0x04001345 RID: 4933
		private static List<CellRect> cachedUsedRects = new List<CellRect>();

		// Token: 0x04001346 RID: 4934
		public const string ElevationName = "Elevation";

		// Token: 0x04001347 RID: 4935
		public const string FertilityName = "Fertility";

		// Token: 0x04001348 RID: 4936
		public const string CavesName = "Caves";

		// Token: 0x04001349 RID: 4937
		public const string RectOfInterestName = "RectOfInterest";

		// Token: 0x0400134A RID: 4938
		public const string UsedRectsName = "UsedRects";

		// Token: 0x0400134B RID: 4939
		public const string RectOfInterestTurretsGenStepsCount = "RectOfInterestTurretsGenStepsCount";

		// Token: 0x0400134C RID: 4940
		public const string DontGenerateClearedGravShipTerrain = "DontGenerateClearedGravShipTerrain";

		// Token: 0x0400134D RID: 4941
		public const string GravshipSpawnSet = "GravshipSpawnSet";

		// Token: 0x0400134E RID: 4942
		public const string SpawnRectName = "SpawnRect";

		// Token: 0x0400134F RID: 4943
		public static List<MapGenUtility.CellData> tmpDataDump = new List<MapGenUtility.CellData>();
	}
}
