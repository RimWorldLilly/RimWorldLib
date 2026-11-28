using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;
using Waylith;
using Waylith.Windowing;

namespace Verse
{
	// Token: 0x02000681 RID: 1665
	public static class BackCompatibility
	{
		// Token: 0x0600349A RID: 13466 RVA: 0x001524A8 File Offset: 0x001506A8
		public static bool IsSaveCompatibleWith(string version)
		{
			if (VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) == VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) >= 1 && VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) <= VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) == 0 && VersionControl.CurrentMajor == 0)
			{
				int num = VersionControl.MinorFromVersionString(version);
				int currentMinor = VersionControl.CurrentMinor;
				for (int i = 0; i < BackCompatibility.SaveCompatibleMinorVersions.Length; i++)
				{
					if (BackCompatibility.SaveCompatibleMinorVersions[i].First == num && BackCompatibility.SaveCompatibleMinorVersions[i].Second == currentMinor)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x00152550 File Offset: 0x00150750
		public static void PreLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PreLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						string str = "Error in PreLoadSavegame of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x001525EC File Offset: 0x001507EC
		public static void PostLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						string str = "Error in PostLoadSavegame of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x00152688 File Offset: 0x00150888
		public static string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (BackCompatibility.CheckSaveIdenticalToCurrentEnvironment())
			{
				return defName;
			}
			if (GenDefDatabase.GetDefSilentFail(defType, defName, false) != null)
			{
				return defName;
			}
			string text = defName;
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (Scribe.mode == LoadSaveMode.Inactive || BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						string text2 = BackCompatibility.conversionChain[i].BackCompatibleDefName(defType, text, forDefInjections, node);
						if (text2 != null)
						{
							text = text2;
						}
					}
					catch (Exception ex)
					{
						string str = "Error in BackCompatibleDefName of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			return text;
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x0015274C File Offset: 0x0015094C
		public static object BackCompatibleEnum(Type enumType, string enumName)
		{
			if (enumType == typeof(QualityCategory))
			{
				if (enumName == "Shoddy")
				{
					return QualityCategory.Poor;
				}
				if (enumName == "Superior")
				{
					return QualityCategory.Excellent;
				}
			}
			return null;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x0015278C File Offset: 0x0015098C
		public static Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (BackCompatibility.CheckSaveIdenticalToCurrentEnvironment())
			{
				return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						Type backCompatibleType = BackCompatibility.conversionChain[i].GetBackCompatibleType(baseType, providedClassName, node);
						if (backCompatibleType != null)
						{
							return backCompatibleType;
						}
					}
					catch (Exception ex)
					{
						string str = "Error in GetBackCompatibleType of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x00152850 File Offset: 0x00150A50
		public static Type GetBackCompatibleTypeDirect(Type baseType, string providedClassName)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToVersion(VersionControl.CurrentMajor, VersionControl.CurrentMinor))
				{
					try
					{
						Type backCompatibleType = BackCompatibility.conversionChain[i].GetBackCompatibleType(baseType, providedClassName, null);
						if (backCompatibleType != null)
						{
							return backCompatibleType;
						}
					}
					catch (Exception ex)
					{
						string str = "Error in GetBackCompatibleType of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x00152914 File Offset: 0x00150B14
		public static int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			if (BackCompatibility.CheckSaveIdenticalToCurrentEnvironment())
			{
				return index;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						index = BackCompatibility.conversionChain[i].GetBackCompatibleBodyPartIndex(body, index);
					}
					catch (Exception ex)
					{
						string str = "Error in GetBackCompatibleBodyPartIndex of ";
						string str2 = (body != null) ? body.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			return index;
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x001529AC File Offset: 0x00150BAC
		public static bool WasDefRemoved(string defName, Type type)
		{
			foreach (Tuple<string, Type> tuple in BackCompatibility.RemovedDefs)
			{
				if (tuple.Item1 == defName && tuple.Item2 == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x00152A1C File Offset: 0x00150C1C
		public static void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return;
			}
			if (BackCompatibility.CheckSaveIdenticalToCurrentEnvironment())
			{
				return;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostExposeData(obj);
					}
					catch (Exception ex)
					{
						string str = "Error in PostExposeData of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x00152AC8 File Offset: 0x00150CC8
		public static void PostCouldntLoadDef(string defName)
		{
			if (BackCompatibility.CheckSaveIdenticalToCurrentEnvironment())
			{
				return;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostCouldntLoadDef(defName);
					}
					catch (Exception ex)
					{
						string str = "Error in PostCouldntLoadDef of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x00152B6C File Offset: 0x00150D6C
		public static void PawnTrainingTrackerPostLoadInit(Pawn_TrainingTracker tracker, ref DefMap<TrainableDef, bool> wantedTrainables, ref DefMap<TrainableDef, int> steps, ref DefMap<TrainableDef, bool> learned)
		{
			if (wantedTrainables == null)
			{
				wantedTrainables = new DefMap<TrainableDef, bool>();
			}
			if (steps == null)
			{
				steps = new DefMap<TrainableDef, int>();
			}
			if (learned == null)
			{
				learned = new DefMap<TrainableDef, bool>();
			}
			if (tracker.GetSteps(TrainableDefOf.Tameness) == 0 && DefDatabase<TrainableDef>.AllDefsListForReading.Any((TrainableDef td) => tracker.GetSteps(td) != 0))
			{
				tracker.Train(TrainableDefOf.Tameness, null, true);
			}
			foreach (TrainableDef trainableDef in DefDatabase<TrainableDef>.AllDefsListForReading)
			{
				if (!tracker.CanAssignToTrain(trainableDef).Accepted)
				{
					wantedTrainables[trainableDef] = false;
					learned[trainableDef] = false;
					steps[trainableDef] = 0;
					if (trainableDef == TrainableDefOf.Obedience && tracker.pawn.playerSettings != null)
					{
						tracker.pawn.playerSettings.Master = null;
						tracker.pawn.playerSettings.followDrafted = false;
						tracker.pawn.playerSettings.followFieldwork = false;
					}
				}
				if (tracker.GetSteps(trainableDef) == trainableDef.steps)
				{
					tracker.Train(trainableDef, null, true);
				}
			}
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x00152CD8 File Offset: 0x00150ED8
		public static void TriggerDataFractionColonyDamageTakenNull(Trigger_FractionColonyDamageTaken trigger, Map map)
		{
			trigger.data = new TriggerData_FractionColonyDamageTaken();
			((TriggerData_FractionColonyDamageTaken)trigger.data).startColonyDamage = map.damageWatcher.DamageTakenEver;
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x00152D00 File Offset: 0x00150F00
		public static void TriggerDataPawnCycleIndNull(Trigger_KidnapVictimPresent trigger)
		{
			trigger.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x00152D0D File Offset: 0x00150F0D
		public static void TriggerDataTicksPassedNull(Trigger_TicksPassed trigger)
		{
			trigger.data = new TriggerData_TicksPassed();
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x00152D1A File Offset: 0x00150F1A
		public static TerrainDef BackCompatibleTerrainWithShortHash(ushort hash)
		{
			if (hash == 16442)
			{
				return TerrainDefOf.WaterMovingChestDeep;
			}
			return null;
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x00152D2B File Offset: 0x00150F2B
		public static ThingDef BackCompatibleThingDefWithShortHash(ushort hash)
		{
			if (hash == 62520)
			{
				return ThingDefOf.MineableComponentsIndustrial;
			}
			return null;
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00152D3C File Offset: 0x00150F3C
		public static ThingDef BackCompatibleThingDefWithShortHash_Force(ushort hash, int major, int minor)
		{
			if (major == 0 && minor <= 18 && hash == 27292)
			{
				return ThingDefOf.MineableSteel;
			}
			return null;
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x00152D58 File Offset: 0x00150F58
		public static bool CheckSpawnBackCompatibleThingAfterLoading(Thing thing, Map map)
		{
			if (ScribeMetaHeaderUtility.loadedGameVersionMajor == 0 && ScribeMetaHeaderUtility.loadedGameVersionMinor <= 18 && thing.stackCount > thing.def.stackLimit && thing.stackCount != 1 && thing.def.stackLimit != 1)
			{
				BackCompatibility.tmpThingsToSpawnLater.Add(thing);
				return true;
			}
			return false;
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x00152DAD File Offset: 0x00150FAD
		public static void PreCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x00152DBC File Offset: 0x00150FBC
		public static void PostCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			for (int i = 0; i < BackCompatibility.tmpThingsToSpawnLater.Count; i++)
			{
				GenPlace.TryPlaceThing(BackCompatibility.tmpThingsToSpawnLater[i], BackCompatibility.tmpThingsToSpawnLater[i].Position, map, ThingPlaceMode.Near, null, null, null, 1);
			}
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x00152E18 File Offset: 0x00151018
		public static void FactionManagerPostLoadInit()
		{
			if (ModsConfig.RoyaltyActive && Find.FactionManager.FirstFactionOfDef(FactionDefOf.Empire) == null && Find.World.info.factions == null)
			{
				FactionGenerator.CreateFactionAndAddToManager(FactionDefOf.Empire);
			}
			if (ModsConfig.AnomalyActive)
			{
				if (Find.FactionManager.FirstFactionOfDef(FactionDefOf.HoraxCult) == null)
				{
					FactionGenerator.CreateFactionAndAddToManager(FactionDefOf.HoraxCult);
				}
				if (Find.FactionManager.FirstFactionOfDef(FactionDefOf.Entities) == null)
				{
					FactionGenerator.CreateFactionAndAddToManager(FactionDefOf.Entities);
				}
			}
			if (ModsConfig.OdysseyActive)
			{
				if (Find.FactionManager.FirstFactionOfDef(FactionDefOf.TradersGuild) == null)
				{
					FactionGenerator.CreateFactionAndAddToManager(Find.WorldGrid.Orbit, FactionDefOf.TradersGuild);
				}
				if (Find.FactionManager.FirstFactionOfDef(FactionDefOf.Salvagers) == null)
				{
					FactionGenerator.CreateFactionAndAddToManager(Find.WorldGrid.Orbit, FactionDefOf.Salvagers);
				}
			}
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x00152EE8 File Offset: 0x001510E8
		public static void IdeoManagerPostloadInit()
		{
			if (ModsConfig.AnomalyActive && ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode)
			{
				Faction cult = Find.FactionManager.OfHoraxCult;
				Faction cult2 = cult;
				if (((cult2 != null) ? cult2.ideos : null) != null && cult.def.fixedIdeo && !cult.def.forcedMemes.NullOrEmpty<MemeDef>() && !cult.def.forcedMemes.All((MemeDef x) => cult.ideos.PrimaryIdeo.memes.Contains(x)))
				{
					FactionDef def = cult.def;
					bool forceNoExpansionIdeo = false;
					List<PreceptDef> disallowedPrecepts = null;
					List<MemeDef> disallowedMemes = null;
					string ideoName = cult.def.ideoName;
					List<StyleCategoryDef> styles = cult.def.styles;
					List<DeityPreset> deityPresets = cult.def.deityPresets;
					bool hiddenIdeo = cult.def.hiddenIdeo;
					string ideoDescription = cult.def.ideoDescription;
					Ideo ideo = IdeoGenerator.MakeFixedIdeo(new IdeoGenerationParms(def, forceNoExpansionIdeo, disallowedPrecepts, disallowedMemes, cult.def.forcedMemes, false, false, false, true, ideoName, styles, deityPresets, hiddenIdeo, ideoDescription, cult.def.requiredPreceptsOnly));
					ideo.primaryFactionColor = new Color?(cult.Color);
					cult.ideos.SetPrimary(ideo);
					Find.IdeoManager.Add(ideo);
				}
			}
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x0015306C File Offset: 0x0015126C
		public static void ResearchManagerPostLoadInit()
		{
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if ((allDefsListForReading[i].IsFinished || allDefsListForReading[i].ProgressReal > 0f) && allDefsListForReading[i].TechprintsApplied < allDefsListForReading[i].TechprintCount)
				{
					Find.ResearchManager.AddTechprints(allDefsListForReading[i], allDefsListForReading[i].TechprintCount - allDefsListForReading[i].TechprintsApplied);
				}
			}
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x001530F8 File Offset: 0x001512F8
		public static void PrefsDataPostLoad(PrefsData prefsData)
		{
			if (prefsData.pauseOnUrgentLetter != null)
			{
				if (prefsData.pauseOnUrgentLetter.Value)
				{
					prefsData.automaticPauseMode = AutomaticPauseMode.MajorThreat;
				}
				else
				{
					prefsData.automaticPauseMode = AutomaticPauseMode.Never;
				}
			}
			if (prefsData.debugActionPalette == null)
			{
				prefsData.debugActionPalette = new List<string>();
			}
			if (!prefsData.scaleRelativeToSystem)
			{
				prefsData.scaleRelativeToSystem = true;
				prefsData.uiScale = Dialog_Options.UIScales.MinBy((float s) => Mathf.Abs(s - prefsData.uiScale / (Screen.dpi / 96f)));
				if (!prefsData.fullscreen && prefsData.screenWidth > 0 && prefsData.screenHeight > 0 && Screen.width > 0)
				{
					float num = (float)Screen.windowWidth / (float)Screen.width;
					prefsData.screenWidth = Mathf.RoundToInt((float)prefsData.screenWidth * num);
					prefsData.screenHeight = Mathf.RoundToInt((float)prefsData.screenHeight * num);
				}
				prefsData.windowedSizeLogical = true;
			}
			if (!prefsData.windowedSizeLogical)
			{
				prefsData.windowedSizeLogical = true;
				EngineApplication instance = EngineApplication.Instance;
				bool flag;
				if (!OperatingSystem.IsMacOS())
				{
					if (instance == null)
					{
						flag = true;
					}
					else
					{
						IPlatform windowSystem = instance.WindowSystem;
						flag = (((windowSystem != null) ? new Backend?(windowSystem.Backend) : null).GetValueOrDefault() != Backend.Wayland);
					}
				}
				else
				{
					flag = false;
				}
				if (flag && !prefsData.fullscreen && prefsData.screenWidth > 0 && prefsData.screenHeight > 0)
				{
					double? num2;
					if (instance == null)
					{
						num2 = null;
					}
					else
					{
						IWindow window = instance.Window;
						num2 = ((window != null) ? new double?(window.ScaleFactor) : null);
					}
					double? num3 = num2;
					float num4 = (float)num3.GetValueOrDefault(1.0);
					prefsData.screenWidth = Mathf.RoundToInt((float)prefsData.screenWidth / num4);
					prefsData.screenHeight = Mathf.RoundToInt((float)prefsData.screenHeight / num4);
				}
			}
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x00153349 File Offset: 0x00151549
		private static bool CheckSaveIdenticalToCurrentEnvironment()
		{
			return Scribe.mode != LoadSaveMode.Inactive && !ScribeMetaHeaderUtility.modListChanged && VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild && !Application.isEditor;
		}

		// Token: 0x04002269 RID: 8809
		public static readonly Pair<int, int>[] SaveCompatibleMinorVersions = new Pair<int, int>[]
		{
			new Pair<int, int>(17, 18)
		};

		// Token: 0x0400226A RID: 8810
		private static List<BackCompatibilityConverter> conversionChain = new List<BackCompatibilityConverter>
		{
			new BackCompatibilityConverter_0_17_AndLower(),
			new BackCompatibilityConverter_0_18(),
			new BackCompatibilityConverter_0_19(),
			new BackCompatibilityConverter_1_0(),
			new BackCompatibilityConverter_1_2(),
			new BackCompatibilityConverter_1_3(),
			new BackCompatibilityConverter_1_4(),
			new BackCompatibilityConverter_1_5(),
			new BackCompatibilityConverter_Universal()
		};

		// Token: 0x0400226B RID: 8811
		private static readonly List<Tuple<string, Type>> RemovedDefs = new List<Tuple<string, Type>>
		{
			new Tuple<string, Type>("PsychicSilencer", typeof(HediffDef)),
			new Tuple<string, Type>("PsychicSilencer", typeof(ThingDef)),
			new Tuple<string, Type>("Gun_Slugthrower", typeof(ThingDef)),
			new Tuple<string, Type>("LazyWorker78", typeof(BackstoryDef)),
			new Tuple<string, Type>("ExMilitary9", typeof(BackstoryDef)),
			new Tuple<string, Type>("InsuranceBroker9", typeof(BackstoryDef)),
			new Tuple<string, Type>("StreetUrchin22", typeof(BackstoryDef)),
			new Tuple<string, Type>("CardCounter25", typeof(BackstoryDef)),
			new Tuple<string, Type>("CaesicMarshal72", typeof(BackstoryDef)),
			new Tuple<string, Type>("GrownMate56", typeof(BackstoryDef)),
			new Tuple<string, Type>("DepartmentHead61", typeof(BackstoryDef)),
			new Tuple<string, Type>("ArmyCommander14", typeof(BackstoryDef)),
			new Tuple<string, Type>("DivorceKid95", typeof(BackstoryDef)),
			new Tuple<string, Type>("PetTorturer60", typeof(BackstoryDef)),
			new Tuple<string, Type>("PoliticalClimber28", typeof(BackstoryDef)),
			new Tuple<string, Type>("NoInteraction", typeof(PrisonerInteractionModeDef)),
			new Tuple<string, Type>("Study", typeof(DesignationDef)),
			new Tuple<string, Type>("Mote_HateChant", typeof(ThingDef)),
			new Tuple<string, Type>("GeneticChemicalDependency", typeof(ThoughtDef))
		};

		// Token: 0x0400226C RID: 8812
		private static List<Thing> tmpThingsToSpawnLater = new List<Thing>();
	}
}
