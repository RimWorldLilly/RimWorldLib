using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200068B RID: 1675
	public class BackCompatibilityConverter_Universal : BackCompatibilityConverter
	{
		// Token: 0x060034EF RID: 13551 RVA: 0x00157BA0 File Offset: 0x00155DA0
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return true;
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x00157BA4 File Offset: 0x00155DA4
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(ThingDef))
			{
				if (defName == "WoolYak")
				{
					return "WoolSheep";
				}
				if (defName == "Plant_TreeAnimus" || defName == "Plant_TreeAnimusSmall" || defName == "Plant_TreeAnimaSmall" || defName == "Plant_TreeAnimaNormal" || defName == "Plant_TreeAnimaHardy")
				{
					return "Plant_TreeAnima";
				}
				if (defName == "Psytrainer_EntropyLink")
				{
					return "Psytrainer_EntropyDump";
				}
				if (defName == "PsylinkNeuroformer")
				{
					return "PsychicAmplifier";
				}
				if (defName == "PsychicShockLance")
				{
					return "Apparel_PsychicShockLance";
				}
				if (defName == "PsychicInsanityLance")
				{
					return "Apparel_PsychicInsanityLance";
				}
				if (defName == "Nutrifungus")
				{
					return "Plant_Nutrifungus";
				}
				if (defName == "Mech_Centipede")
				{
					return "Mech_CentipedeBlaster";
				}
				if (defName == "Corpse_Mech_Centipede")
				{
					return "Corpse_Mech_CentipedeBlaster";
				}
				if (defName == "AncientDiabolusRemains")
				{
					return "AncientUltraDiabolusRemains";
				}
				if (defName == "AncientUltraDiabolusRemains")
				{
					return "AncientExostriderRemains";
				}
				if (defName == "MegaspiderCocoon")
				{
					return "CocoonMegaspider";
				}
				if (defName == "MegascarabCocoon")
				{
					return "CocoonMegascarab";
				}
				if (defName == "SpelopedeCocoon")
				{
					return "CocoonSpelopede";
				}
				if (defName == "AncientCentipedeShell")
				{
					return "ChunkMechanoidSlag";
				}
				if (defName == "BasicSubcore")
				{
					return "SubcoreBasic";
				}
				if (defName == "RegularSubcore")
				{
					return "SubcoreRegular";
				}
				if (defName == "HighSubcore")
				{
					return "SubcoreHigh";
				}
				if (defName == "AncientMechanoidShell")
				{
					return "ChunkMechanoidSlag";
				}
				if (defName == "XenogermExtractor")
				{
					return "GeneExtractor";
				}
				if (defName == "Mech_Purger")
				{
					return "Mech_Tunneler";
				}
				if (defName == "RemoteCharger")
				{
					return "MechBooster";
				}
				if (defName == "StandingLamp_Red")
				{
					return "StandingLamp";
				}
				if (defName == "StandingLamp_Green")
				{
					return "StandingLamp";
				}
				if (defName == "StandingLamp_Blue")
				{
					return "StandingLamp";
				}
				if (defName == "Darklamp")
				{
					return "StandingLamp";
				}
				if (defName == "MechanitorComplexMap")
				{
					return "MechanoidTransponder";
				}
			}
			if (defType == typeof(HediffDef))
			{
				if (defName == "Psylink")
				{
					return "PsychicAmplifier";
				}
				if (defName == "RemoteCharge")
				{
					return "MechBoost";
				}
			}
			if (defType == typeof(PreceptDef) && defName == "FuneralDestroyed")
			{
				return "FuneralNoCorpse";
			}
			if (defType == typeof(RitualOutcomeEffectDef) && defName == "AttendedFuneralDestroyed")
			{
				return "AttendedFuneralNoCorpse";
			}
			if (defType == typeof(AbilityDef))
			{
				if (defName == "PreachingOfHealing")
				{
					return "PreachHealth";
				}
				if (defName == "HeartenHealth")
				{
					return "PreachHealth";
				}
			}
			if (defType == typeof(IdeoIconDef))
			{
				if (defName == "PoliticalA")
				{
					return "Eagle";
				}
				if (defName == "NatureA")
				{
					return "Treeflat";
				}
				if (defName == "PirateA")
				{
					return "Steer";
				}
				if (defName == "PirateB")
				{
					return "Skull";
				}
				if (defName == "PoliticalB")
				{
					return "OliveBranches";
				}
				if (defName == "ReligionA")
				{
					return "DownBurst";
				}
				if (defName == "ReligionB")
				{
					return "TripleCross";
				}
			}
			if (defType == typeof(PawnKindDef))
			{
				if (defName == "Mech_Centipede")
				{
					return "Mech_CentipedeBlaster";
				}
				if (defName == "Mech_Purger")
				{
					return "Mech_Tunneler";
				}
			}
			if (defType == typeof(ThoughtDef))
			{
				if (defName == "AteFungus_Prefered")
				{
					return "AteFungus_Preferred";
				}
				if (defName == "AteFungusAsIngredient_Prefered")
				{
					return "AteFungusAsIngredient_Preferred";
				}
			}
			if (defType == typeof(JobDef))
			{
				if (defName == "StudyThing")
				{
					return "StudyInteract";
				}
				if (defName == "StudyBuilding")
				{
					return "StudyInteract";
				}
			}
			if (defType == typeof(ThingStyleDef))
			{
				if (defName.EndsWith("StandingLamp_Red") || defName.EndsWith("StandingLamp_Green") || defName.EndsWith("StandingLamp_Blue"))
				{
					return defName.Substring(0, defName.IndexOf("StandingLamp") + "StandingLamp".Length);
				}
				if (defName.EndsWith("DarklampStanding"))
				{
					return defName.Substring(0, defName.IndexOf("DarklampStanding")) + "StandingLamp";
				}
			}
			return null;
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x0015807C File Offset: 0x0015627C
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (providedClassName == "Hediff_PsychicAmplifier")
			{
				return typeof(Hediff_Psylink);
			}
			if (providedClassName == "Graphic_MotePulse")
			{
				return typeof(Graphic_MoteWithAgeSecs);
			}
			if (node != null && (providedClassName == "ThingWithComps" || providedClassName == "Verse.ThingWithComps"))
			{
				XmlElement xmlElement = node["def"];
				if (xmlElement != null)
				{
					if (xmlElement.InnerText == "PsychicShockLance")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "PsychicInsanityLance")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "OrbitalTargeterBombardment")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "OrbitalTargeterPowerBeam")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "OrbitalTargeterMechCluster")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "TornadoGenerator")
					{
						return typeof(Apparel);
					}
				}
			}
			if (providedClassName == "Building_AncientUltraDiabolusRemains")
			{
				return typeof(Building_AncientMechRemains);
			}
			if (baseType == typeof(WorldObject) && node != null && providedClassName == typeof(WorldObject).FullName)
			{
				XmlElement xmlElement2 = node["def"];
				if (((xmlElement2 != null) ? xmlElement2.InnerText : null) == "AbandonedSettlement")
				{
					return typeof(AbandonedSettlement);
				}
			}
			if ((providedClassName == "Precept_Ritual" || providedClassName == "RimWorld.Precept") && node["def"].InnerText == "GravshipLaunch")
			{
				return typeof(Precept_GravshipLaunch);
			}
			return null;
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x00158250 File Offset: 0x00156450
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int loadedGameVersionBuild = ScribeMetaHeaderUtility.loadedGameVersionBuild;
				Pawn_RoyaltyTracker pawn_RoyaltyTracker = obj as Pawn_RoyaltyTracker;
				if (pawn_RoyaltyTracker != null && loadedGameVersionBuild <= 2575)
				{
					foreach (RoyalTitle royalTitle in pawn_RoyaltyTracker.AllTitlesForReading)
					{
						royalTitle.conceited = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(pawn_RoyaltyTracker.pawn);
					}
				}
				if (loadedGameVersionBuild < 3167)
				{
					this.MealRestrictionsReworkBackCompat(obj);
				}
				if (loadedGameVersionBuild < 3156)
				{
					this.BiosculpterReworkBackCompat(obj);
				}
				this.ApplyLampColor(obj);
				Pawn_NeedsTracker pawn_NeedsTracker = obj as Pawn_NeedsTracker;
				if (pawn_NeedsTracker != null)
				{
					pawn_NeedsTracker.AllNeeds.RemoveAll((Need n) => n.def.defName == "Authority");
				}
				History history = obj as History;
				if (history != null && history.historyEventsManager == null)
				{
					history.historyEventsManager = new HistoryEventsManager();
				}
			}
			Pawn pawn = obj as Pawn;
			if (pawn != null)
			{
				if (pawn.abilities == null)
				{
					if (!pawn.RaceProps.ShouldHaveAbilityTracker)
					{
						PawnKindDef kindDef = pawn.kindDef;
						if (kindDef == null || kindDef.abilities.NullOrEmpty<AbilityDef>())
						{
							goto IL_19B;
						}
					}
					pawn.abilities = new Pawn_AbilityTracker(pawn);
					PawnKindDef kindDef2 = pawn.kindDef;
					if (kindDef2 != null && !kindDef2.abilities.NullOrEmpty<AbilityDef>())
					{
						Pawn pawn2 = pawn;
						if (pawn2.abilities == null)
						{
							pawn2.abilities = new Pawn_AbilityTracker(pawn);
						}
						for (int i = 0; i < pawn.kindDef.abilities.Count; i++)
						{
							pawn.abilities.GainAbility(pawn.kindDef.abilities[i]);
						}
					}
				}
				IL_19B:
				Pawn_AbilityTracker abilities = pawn.abilities;
				Ability ability;
				if (abilities == null)
				{
					ability = null;
				}
				else
				{
					ability = abilities.abilities.FirstOrFallback((Ability x) => x.def != null && x.def.defName == "AnimaTreeLinking", null);
				}
				Ability ability2 = ability;
				if (ability2 != null)
				{
					pawn.abilities.RemoveAbility(ability2.def);
				}
				if (pawn.RaceProps.Humanlike)
				{
					if (pawn.surroundings == null)
					{
						pawn.surroundings = new Pawn_SurroundingsTracker(pawn);
					}
					if (ModsConfig.IdeologyActive)
					{
						if (pawn.styleObserver == null)
						{
							pawn.styleObserver = new Pawn_StyleObserverTracker(pawn);
						}
						if (pawn.connections == null)
						{
							pawn.connections = new Pawn_ConnectionsTracker(pawn);
						}
					}
				}
				if (pawn.health != null)
				{
					if (pawn.health.hediffSet.hediffs.RemoveAll((Hediff x) => x == null) != 0)
					{
						Log.Error(pawn.ToStringSafe<Pawn>() + " had some null hediffs.");
					}
					Pawn_HealthTracker health = pawn.health;
					Hediff hediff;
					if (health == null)
					{
						hediff = null;
					}
					else
					{
						HediffSet hediffSet = health.hediffSet;
						hediff = ((hediffSet != null) ? hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicHangover, false) : null);
					}
					Hediff hediff2 = hediff;
					if (hediff2 != null)
					{
						pawn.health.hediffSet.hediffs.Remove(hediff2);
					}
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.WakeUpTolerance, false);
					if (firstHediffOfDef != null)
					{
						pawn.health.hediffSet.hediffs.Remove(firstHediffOfDef);
					}
					Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.GoJuiceTolerance, false);
					if (firstHediffOfDef2 != null)
					{
						pawn.health.hediffSet.hediffs.Remove(firstHediffOfDef2);
					}
					if (pawn.mechanitor != null)
					{
						Hediff firstHediffOfDef3 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BandNode, false);
						if (firstHediffOfDef3 != null && !(firstHediffOfDef3 is Hediff_BandNode))
						{
							pawn.health.RemoveHediff(firstHediffOfDef3);
							pawn.health.AddHediff(HediffDefOf.BandNode, pawn.health.hediffSet.GetBrain(), null, null);
						}
					}
					if (!pawn.Dead)
					{
						if (pawn.thinker == null)
						{
							pawn.thinker = new Pawn_Thinker(pawn);
						}
						if (pawn.jobs == null)
						{
							pawn.jobs = new Pawn_JobTracker(pawn);
						}
						if (pawn.stances == null)
						{
							pawn.stances = new Pawn_StanceTracker(pawn);
						}
						if (ModsConfig.AnomalyActive && pawn.RaceProps.Humanlike && pawn.infectionVectors == null)
						{
							pawn.infectionVectors = new Pawn_InfectionVectorTracker(pawn);
						}
					}
				}
				if (pawn.equipment != null && pawn.apparel != null && pawn.inventory != null)
				{
					List<ThingWithComps> list = null;
					for (int j = 0; j < pawn.equipment.AllEquipmentListForReading.Count; j++)
					{
						ThingWithComps thingWithComps = pawn.equipment.AllEquipmentListForReading[j];
						if (thingWithComps.def.defName == "OrbitalTargeterBombardment" || thingWithComps.def.defName == "OrbitalTargeterPowerBeam" || thingWithComps.def.defName == "OrbitalTargeterMechCluster" || thingWithComps.def.defName == "TornadoGenerator")
						{
							list = (list ?? new List<ThingWithComps>());
							list.Add(thingWithComps);
						}
					}
					if (list != null)
					{
						foreach (ThingWithComps thingWithComps2 in list)
						{
							Apparel apparel = (Apparel)thingWithComps2;
							pawn.equipment.Remove(apparel);
							this.ResetVerbs(apparel);
							if (pawn.apparel.CanWearWithoutDroppingAnything(apparel.def))
							{
								pawn.apparel.Wear(apparel, true, false);
							}
							else
							{
								pawn.inventory.innerContainer.TryAdd(apparel, true);
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && pawn.RaceProps.IsMechanoid && pawn.Faction == Faction.OfPlayer && pawn.Name == null && Scribe.mode == LoadSaveMode.PostLoadInit)
				{
					pawn.GenerateNecessaryName();
				}
				if (ModsConfig.BiotechActive && pawn.equipment != null && pawn.def == ThingDefOf.Mech_Constructoid)
				{
					List<ThingWithComps> allEquipmentListForReading = pawn.equipment.AllEquipmentListForReading;
					for (int k = 0; k < allEquipmentListForReading.Count; k++)
					{
						if (allEquipmentListForReading[k].def.defName == "Gun_Slugthrower")
						{
							pawn.equipment.Remove(allEquipmentListForReading[k]);
							return;
						}
					}
					return;
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SaveLampColor(obj);
				Map map3 = obj as Map;
				if (map3 != null)
				{
					Map map2 = map3;
					if (map2.temporaryThingDrawer == null)
					{
						map2.temporaryThingDrawer = new TemporaryThingDrawer();
					}
					map2 = map3;
					if (map2.flecks == null)
					{
						map2.flecks = new FleckManager(map3);
					}
					map2 = map3;
					if (map2.autoSlaughterManager == null)
					{
						map2.autoSlaughterManager = new AutoSlaughterManager(map3);
					}
					map2 = map3;
					if (map2.treeDestructionTracker == null)
					{
						map2.treeDestructionTracker = new TreeDestructionTracker(map3);
					}
					map2 = map3;
					if (map2.gasGrid == null)
					{
						map2.gasGrid = new GasGrid(map3);
					}
					if (ModsConfig.BiotechActive && map3.pollutionGrid == null)
					{
						map3.pollutionGrid = new PollutionGrid(map3);
					}
					map2 = map3;
					if (map2.deferredSpawner == null)
					{
						map2.deferredSpawner = new DeferredSpawner(map3);
					}
					map2 = map3;
					if (map2.storageGroups == null)
					{
						map2.storageGroups = new StorageGroupManager(map3);
					}
					TerrainGrid terrainGrid = map3.terrainGrid;
					if (terrainGrid.colorGrid == null)
					{
						terrainGrid.colorGrid = new ColorDef[map3.cellIndices.NumGridCells];
					}
					map2 = map3;
					if (map2.tempTerrain == null)
					{
						map2.tempTerrain = new TempTerrainManager(map3);
					}
					if (ModsConfig.OdysseyActive)
					{
						map2 = map3;
						if (map2.substructureGrid == null)
						{
							map2.substructureGrid = new SubstructureGrid(map3);
						}
						map2 = map3;
						if (map2.waterBodyTracker == null)
						{
							map2.waterBodyTracker = new WaterBodyTracker(map3);
						}
						map2 = map3;
						if (map2.freezeManager == null)
						{
							map2.freezeManager = new FreezeManager(map3);
							return;
						}
					}
				}
				else
				{
					Game game = obj as Game;
					if (game != null)
					{
						Game game2 = game;
						if (game2.transportShipManager == null)
						{
							game2.transportShipManager = new TransportShipManager();
						}
						game2 = game;
						if (game2.studyManager == null)
						{
							game2.studyManager = new StudyManager();
						}
						game2 = game;
						if (game2.hiddenItemsManager == null)
						{
							game2.hiddenItemsManager = new HiddenItemsManager();
						}
						if (ModsConfig.BiotechActive && game.customXenogermDatabase == null)
						{
							game.customXenogermDatabase = new CustomXenogermDatabase();
						}
						if (ModsConfig.BiotechActive && game.customXenotypeDatabase == null)
						{
							game.customXenotypeDatabase = new CustomXenotypeDatabase();
							return;
						}
					}
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Map map = obj as Map;
				if (map != null && ScribeMetaHeaderUtility.loadedGameVersionBuild < 4208)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						if (map.listerThings.AnyThingWithDef(ThingDefOf.GravEngine))
						{
							map.wasSpawnedViaGravShipLanding = true;
						}
					});
				}
			}
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x00158B08 File Offset: 0x00156D08
		private void ResetVerbs(ThingWithComps t)
		{
			IVerbOwner verbOwner = t as IVerbOwner;
			if (verbOwner != null)
			{
				VerbTracker verbTracker = verbOwner.VerbTracker;
				if (verbTracker != null)
				{
					verbTracker.VerbsNeedReinitOnLoad();
				}
			}
			foreach (ThingComp thingComp in t.AllComps)
			{
				IVerbOwner verbOwner2 = thingComp as IVerbOwner;
				if (verbOwner2 != null)
				{
					VerbTracker verbTracker2 = verbOwner2.VerbTracker;
					if (verbTracker2 != null)
					{
						verbTracker2.VerbsNeedReinitOnLoad();
					}
				}
			}
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x00158B8C File Offset: 0x00156D8C
		public override int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			if (body == BodyDefOf.Human && ScribeMetaHeaderUtility.loadedGameVersionBuild <= 3094 && index >= 22)
			{
				return index + 1;
			}
			return index;
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x00158BAC File Offset: 0x00156DAC
		private void MealRestrictionsReworkBackCompat(object obj)
		{
			FoodRestrictionDatabase foodRestrictionDatabase = obj as FoodRestrictionDatabase;
			if (foodRestrictionDatabase != null)
			{
				foodRestrictionDatabase.CreateIdeologyFoodRestrictions();
			}
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x00158BCC File Offset: 0x00156DCC
		private void BiosculpterReworkBackCompat(object obj)
		{
			JobDriver_CarryToBiosculpterPod jobDriver_CarryToBiosculpterPod = obj as JobDriver_CarryToBiosculpterPod;
			if (jobDriver_CarryToBiosculpterPod != null)
			{
				jobDriver_CarryToBiosculpterPod.EndJobWith(JobCondition.Incompletable);
			}
			JobDriver_EnterBiosculpterPod jobDriver_EnterBiosculpterPod = obj as JobDriver_EnterBiosculpterPod;
			if (jobDriver_EnterBiosculpterPod != null)
			{
				jobDriver_EnterBiosculpterPod.EndJobWith(JobCondition.Incompletable);
			}
			Building building = obj as Building;
			if (building != null)
			{
				CompBiosculpterPod compBiosculpterPod = building.TryGetComp<CompBiosculpterPod>();
				if (compBiosculpterPod != null)
				{
					if (compBiosculpterPod.Occupant == null)
					{
						compBiosculpterPod.ClearCycle();
					}
					compBiosculpterPod.autoLoadNutrition = true;
				}
			}
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x00158C24 File Offset: 0x00156E24
		private void SaveLampColor(object obj)
		{
			Building building = obj as Building;
			if (building == null)
			{
				return;
			}
			ColorInt? colorInt = null;
			ScribeLoader loader = Scribe.loader;
			string text;
			if (loader == null)
			{
				text = null;
			}
			else
			{
				XmlNode curXmlParent = loader.curXmlParent;
				if (curXmlParent == null)
				{
					text = null;
				}
				else
				{
					XmlElement xmlElement = curXmlParent["def"];
					text = ((xmlElement != null) ? xmlElement.InnerText : null);
				}
			}
			string a = text;
			if (a == "StandingLamp_Red")
			{
				colorInt = new ColorInt?(new ColorInt(217, 80, 80));
			}
			else if (a == "StandingLamp_Green")
			{
				colorInt = new ColorInt?(new ColorInt(80, 217, 80));
			}
			else if (a == "StandingLamp_Blue")
			{
				colorInt = new ColorInt?(new ColorInt(80, 80, 217));
			}
			else if (a == "Darklamp")
			{
				colorInt = new ColorInt?(new ColorInt(78, 226, 229));
			}
			if (colorInt != null)
			{
				this.lampsToColors[building] = colorInt.Value;
			}
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x00158D20 File Offset: 0x00156F20
		private void ApplyLampColor(object obj)
		{
			Building building = obj as Building;
			if (building == null)
			{
				return;
			}
			CompGlower comp = building.GetComp<CompGlower>();
			if (comp == null)
			{
				return;
			}
			ColorInt colorInt;
			if (!this.lampsToColors.TryGetValue(building, out colorInt))
			{
				return;
			}
			float h;
			float s;
			float num;
			Color.RGBToHSV(colorInt.ToColor, out h, out s, out num);
			float num2;
			float v;
			Color.RGBToHSV(comp.GlowColor.ToColor, out num, out num2, out v);
			comp.GlowColor = new ColorInt(Color.HSVToRGB(h, s, v));
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x00158D95 File Offset: 0x00156F95
		public override void PreLoadSavegame(string loadingVersion)
		{
			this.lampsToColors.Clear();
		}

		// Token: 0x04002275 RID: 8821
		private Dictionary<Building, ColorInt> lampsToColors = new Dictionary<Building, ColorInt>(128);
	}
}
