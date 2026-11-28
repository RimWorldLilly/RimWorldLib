using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Profile;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005B8 RID: 1464
	public static class DebugActionsMisc
	{
		// Token: 0x06002CB6 RID: 11446 RVA: 0x0010F2B0 File Offset: 0x0010D4B0
		[DebugAction("Other", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnWorld, hideInSubMenu = true, requiresOdyssey = true)]
		private static void AddTestPlanetLayer()
		{
			if (DebugActionsMisc.TestLayer != null)
			{
				Find.WorldGrid.RemovePlanetLayer(DebugActionsMisc.TestLayer);
			}
			PlanetLayer orbit = Find.WorldGrid.Orbit;
			DebugActionsMisc.TestLayer = Find.WorldGrid.RegisterPlanetLayer(PlanetLayerDefOf.Orbit, null, 200f, 180f, 0f, 10, 0f, 0f, null);
			DebugActionsMisc.TestLayer.AddConnection(orbit, 0f);
			orbit.AddConnection(DebugActionsMisc.TestLayer, 0f);
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x0010F33F File Offset: 0x0010D53F
		[DebugAction("Other", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnWorld, hideInSubMenu = true, requiresOdyssey = true)]
		private static void RemoveTestPlanetLayer()
		{
			if (DebugActionsMisc.TestLayer != null)
			{
				Find.WorldGrid.RemovePlanetLayer(DebugActionsMisc.TestLayer);
			}
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x0010F358 File Offset: 0x0010D558
		[DebugAction("Other", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnWorld, hideInSubMenu = true)]
		private static void LogPlanetLayerConnections()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(planetLayer2.LayerID);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(planetLayer2.Def.label);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				foreach (KeyValuePair<PlanetLayer, PlanetLayerConnection> keyValuePair2 in planetLayer2.Connections)
				{
					PlanetLayerConnection planetLayerConnection;
					keyValuePair2.Deconstruct(out planetLayer, out planetLayerConnection);
					PlanetLayer planetLayer3 = planetLayer;
					PlanetLayerConnection planetLayerConnection2 = planetLayerConnection;
					StringBuilder stringBuilder3 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(14, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("   -> ");
					defaultInterpolatedStringHandler2.AppendFormatted(planetLayer3.Def.label);
					defaultInterpolatedStringHandler2.AppendLiteral(", cost: ");
					defaultInterpolatedStringHandler2.AppendFormatted<float>(planetLayerConnection2.fuelCost);
					stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x0010F4D4 File Offset: 0x0010D6D4
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllPlants()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				if (thing is Plant)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x0010F540 File Offset: 0x0010D740
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllThings()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x0010F5A0 File Offset: 0x0010D7A0
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyClutter()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Chunk).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
			foreach (Thing thing2 in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Filth).ToList<Thing>())
			{
				thing2.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x0010F654 File Offset: 0x0010D854
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllHats()
		{
			foreach (Pawn pawn in PawnsFinder.AllMaps)
			{
				if (pawn.RaceProps.Humanlike)
				{
					for (int i = pawn.apparel.WornApparel.Count - 1; i >= 0; i--)
					{
						Apparel apparel = pawn.apparel.WornApparel[i];
						if (apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
						{
							apparel.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x0010F720 File Offset: 0x0010D920
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DestroyAllCorpses()
		{
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x0010F75E File Offset: 0x0010D95E
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FinishAllResearch()
		{
			Find.ResearchManager.DebugSetAllProjectsFinished();
			Find.EntityCodex.debug_UnhideAllResearch = true;
			Messages.Message("All research finished.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x0010F788 File Offset: 0x0010D988
		[DebugAction("General", "Add techprint to project", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					Find.ResearchManager.AddTechprints(localProject, localProject.TechprintCount - Find.ResearchManager.GetTechprints(localProject));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x0010F840 File Offset: 0x0010DA40
		[DebugAction("General", "Apply techprint on project", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ApplyTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				Action <>9__2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DebugMenuOption> list3 = list2;
					string label = "None";
					DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
					Action method;
					if ((method = <>9__2) == null)
					{
						method = (<>9__2 = delegate()
						{
							Find.ResearchManager.ApplyTechprint(localProject, null);
						});
					}
					list3.Add(new DebugMenuOption(label, mode, method));
					foreach (Pawn localColonist2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
					{
						Pawn localColonist = localColonist2;
						list2.Add(new DebugMenuOption(localColonist.LabelCap, DebugMenuOptionMode.Action, delegate()
						{
							Find.ResearchManager.ApplyTechprint(localProject, localColonist);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x0010F8F8 File Offset: 0x0010DAF8
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> AddTradeShipOfKind()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<TraderKindDef> enumerator = (from t in DefDatabase<TraderKindDef>.AllDefs
			where t.orbital
			select t).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TraderKindDef traderKind = enumerator.Current;
					list.Add(new DebugActionNode(traderKind.label, DebugActionType.Action, delegate()
					{
						Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.traderKind = traderKind;
						IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x0010F998 File Offset: 0x0010DB98
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ReplaceAllTradeShips()
		{
			Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
			for (int i = 0; i < 5; i++)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.CurrentMap;
				IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
			}
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x0010F9E4 File Offset: 0x0010DBE4
		[DebugAction("General", "Change weather...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> ChangeWeather()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (WeatherDef localWeather2 in DefDatabase<WeatherDef>.AllDefs)
			{
				WeatherDef localWeather = localWeather2;
				list.Add(new DebugActionNode(localWeather.LabelCap, DebugActionType.Action, delegate()
				{
					Find.CurrentMap.weatherManager.TransitionTo(localWeather);
				}, null));
			}
			return list;
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x0010FA68 File Offset: 0x0010DC68
		[DebugAction("General", "Celestial debugger", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OpenCelestialDebugger()
		{
			Dialog_DevCelestial dialog_DevCelestial;
			if (Find.WindowStack.TryGetWindow<Dialog_DevCelestial>(out dialog_DevCelestial))
			{
				dialog_DevCelestial.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevCelestial());
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x0010FA9C File Offset: 0x0010DC9C
		[DebugAction("Sound", "Music debugger", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OpenMusicDebugger()
		{
			Dialog_DevMusic dialog_DevMusic;
			if (Find.WindowStack.TryGetWindow<Dialog_DevMusic>(out dialog_DevMusic))
			{
				dialog_DevMusic.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevMusic());
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x0010FAD0 File Offset: 0x0010DCD0
		[DebugAction("General", "World noise visualizer", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.WorldRenderedNow)]
		private static void OpenWorldNoiseDebugger()
		{
			Dialog_DevNoiseWorld dialog_DevNoiseWorld;
			if (Find.WindowStack.TryGetWindow<Dialog_DevNoiseWorld>(out dialog_DevNoiseWorld))
			{
				dialog_DevNoiseWorld.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevNoiseWorld());
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x0010FB04 File Offset: 0x0010DD04
		[DebugAction("General", "Map noise visualizer", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		private static void OpenMapNoiseDebugger()
		{
			Dialog_DevNoiseMap dialog_DevNoiseMap;
			if (Find.WindowStack.TryGetWindow<Dialog_DevNoiseMap>(out dialog_DevNoiseMap))
			{
				dialog_DevNoiseMap.Close(true);
			}
			Find.WindowStack.Add(new Dialog_DevNoiseMap());
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x0010FB35 File Offset: 0x0010DD35
		[DebugAction("Sound", "Test music fadeout and silence", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestFadeoutAndSilence()
		{
			Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(120f, 5f, true);
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x0010FB4C File Offset: 0x0010DD4C
		[DebugAction("Sound", "Play song...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> PlaySong()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SongDef localSong2 in DefDatabase<SongDef>.AllDefs)
			{
				SongDef localSong = localSong2;
				list.Add(new DebugActionNode(localSong.defName, DebugActionType.Action, delegate()
				{
					Find.MusicManagerPlay.ForcePlaySong(localSong, false);
				}, null));
			}
			return list;
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x0010FBCC File Offset: 0x0010DDCC
		[DebugAction("Sound", "Trigger transition...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TriggerTransition()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (MusicTransitionDef local2 in DefDatabase<MusicTransitionDef>.AllDefs)
			{
				MusicTransitionDef local = local2;
				list.Add(new DebugActionNode(local.defName, DebugActionType.Action, delegate()
				{
					Find.MusicManagerPlay.ForceTriggerTransition(local);
				}, null));
			}
			return list;
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x0010FC4C File Offset: 0x0010DE4C
		[DebugAction("Sound", "Play sound...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> PlaySound()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (SoundDef localSd2 in from s in DefDatabase<SoundDef>.AllDefs
			where !s.sustain
			select s)
			{
				SoundDef localSd = localSd2;
				list.Add(new DebugActionNode(localSd.defName, DebugActionType.Action, delegate()
				{
					if (localSd.subSounds.Any((SubSoundDef sub) => sub.onCamera))
					{
						localSd.PlayOneShotOnCamera(null);
						return;
					}
					localSd.PlayOneShot(SoundInfo.InMap(new TargetInfo(Find.CameraDriver.MapPosition, Find.CurrentMap, false), MaintenanceType.None));
				}, null));
			}
			return list;
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x0010FCF0 File Offset: 0x0010DEF0
		[DebugAction("General", "End game condition...", false, false, false, false, false, 0, false, allowedGameStates = (AllowedGameStates.Playing | AllowedGameStates.IsCurrentlyOnMap | AllowedGameStates.HasGameCondition))]
		private static void EndGameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameCondition localMc2 in Find.CurrentMap.gameConditionManager.ActiveConditions)
			{
				GameCondition localMc = localMc2;
				list.Add(new DebugMenuOption(localMc.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					localMc.End();
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x0010FD8C File Offset: 0x0010DF8C
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresBiotech = true)]
		private static List<DebugActionNode> SimulateSanguophageMeeting()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 1; i <= 8; i++)
			{
				int num = i;
				list.Add(new DebugActionNode(num.ToString() + " sanguophages", DebugActionType.ToolMap, delegate()
				{
					List<FactionRelation> list2 = new List<FactionRelation>();
					foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
					{
						if (!faction.def.PermanentlyHostileTo(FactionDefOf.Sanguophages))
						{
							list2.Add(new FactionRelation(faction, FactionRelationKind.Neutral));
						}
					}
					Faction faction2 = FactionGenerator.NewGeneratedFactionWithRelations(FactionDefOf.Sanguophages, list2, true);
					faction2.temporary = true;
					Find.FactionManager.Add(faction2);
					List<Pawn> list3 = new List<Pawn>();
					PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Sanguophage, faction2, PawnGenerationContext.NonPlayer, null, true, false, false, true, true, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
					for (int j = 0; j < num; j++)
					{
						list3.Add(PawnGenerator.GeneratePawn(request));
					}
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					PawnsArrivalModeDef edgeWalkIn = PawnsArrivalModeDefOf.EdgeWalkIn;
					edgeWalkIn.Worker.TryResolveRaidSpawnCenter(incidentParms);
					edgeWalkIn.Worker.Arrive(list3, incidentParms);
					LordMaker.MakeNewLord(faction2, new LordJob_SanguophageMeeting(UI.MouseCell(), new List<Thing>(), 60000, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty), Find.CurrentMap, list3);
				}, null));
			}
			return list;
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x0010FDE8 File Offset: 0x0010DFE8
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyAssault()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				LordToil_Stage lordToil_Stage = lord.CurLordToil as LordToil_Stage;
				if (lordToil_Stage != null)
				{
					foreach (Transition transition in lord.Graph.transitions)
					{
						if (transition.sources.Contains(lordToil_Stage) && (transition.target is LordToil_AssaultColony || transition.target is LordToil_AssaultColonyBreaching || transition.target is LordToil_AssaultColonyPrisoners || transition.target is LordToil_AssaultColonySappers || transition.target is LordToil_AssaultColonyBossgroup || transition.target is LordToil_MoveInBossgroup))
						{
							string str = "Debug forcing to assault toil: ";
							Faction faction = lord.faction;
							Messages.Message(str + ((faction != null) ? faction.ToString() : null), MessageTypeDefOf.TaskCompletion, false);
							lord.GotoToil(transition.target);
							return;
						}
					}
				}
			}
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (quest.State == QuestState.Ongoing)
				{
					foreach (QuestPart questPart in quest.PartsListForReading)
					{
						QuestPart_BossgroupArrives questPart_BossgroupArrives = questPart as QuestPart_BossgroupArrives;
						if (questPart_BossgroupArrives != null && questPart_BossgroupArrives.State == QuestPartState.Enabled)
						{
							questPart_BossgroupArrives.DebugForceComplete();
							Messages.Message("Debug forcing bossgroup assault.", MessageTypeDefOf.TaskCompletion, false);
						}
					}
				}
			}
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x00110020 File Offset: 0x0010E220
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyFlee()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				if (lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer) && lord.faction.def.autoFlee)
				{
					LordToil lordToil = lord.Graph.lordToils.FirstOrDefault((LordToil st) => st is LordToil_PanicFlee);
					if (lordToil != null)
					{
						lord.GotoToil(lordToil);
					}
				}
			}
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x001100DC File Offset: 0x0010E2DC
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AdaptionProgress10Days()
		{
			Find.StoryWatcher.watcherAdaptation.Debug_OffsetAdaptDays(10f);
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x001100F2 File Offset: 0x0010E2F2
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void UnloadUnusedAssets()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x001100FC File Offset: 0x0010E2FC
		[DebugAction("General", "Name settlement...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void NameSettlement()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Faction", DebugMenuOptionMode.Action, delegate()
			{
				Find.WindowStack.Add(new Dialog_NamePlayerFaction());
			}));
			if (Find.CurrentMap != null && Find.CurrentMap.IsPlayerHome && Find.CurrentMap.Parent is Settlement)
			{
				Settlement factionBase = (Settlement)Find.CurrentMap.Parent;
				list.Add(new DebugMenuOption("Faction base", DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_NamePlayerSettlement(factionBase));
				}));
				list.Add(new DebugMenuOption("Faction and faction base", DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(factionBase));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x001101C9 File Offset: 0x0010E3C9
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void NextLesson()
		{
			LessonAutoActivator.DebugForceInitiateBestLessonNow();
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x001101D0 File Offset: 0x0010E3D0
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 900)]
		private static List<DebugActionNode> ChangeCameraConfig()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			list.Add(new DebugActionNode("Open editor", DebugActionType.Action, delegate()
			{
				Find.WindowStack.Add(new Dialog_CameraConfig());
			}, null));
			List<DebugActionNode> list2 = list;
			foreach (Type localType2 in typeof(CameraMapConfig).AllSubclasses())
			{
				Type localType = localType2;
				string text = localType.Name;
				if (text.StartsWith("CameraMapConfig_"))
				{
					text = text.Substring("CameraMapConfig_".Length);
				}
				list2.Add(new DebugActionNode(text, DebugActionType.Action, delegate()
				{
					Find.CameraDriver.config = (CameraMapConfig)Activator.CreateInstance(localType);
				}, null));
			}
			return list2;
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x001102B4 File Offset: 0x0010E4B4
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ForceShipCountdown()
		{
			ShipCountdown.InitiateCountdown(null);
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x001102BC File Offset: 0x0010E4BC
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void ForceStartShip()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return;
			}
			Building_ShipComputerCore building_ShipComputerCore = (Building_ShipComputerCore)currentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Ship_ComputerCore).FirstOrDefault<Building>();
			if (building_ShipComputerCore == null)
			{
				Messages.Message("Could not find any compute core on current map!", MessageTypeDefOf.NeutralEvent, true);
			}
			building_ShipComputerCore.ForceLaunch();
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x00110308 File Offset: 0x0010E508
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashTradeDropSpot()
		{
			IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.CurrentMap);
			Find.CurrentMap.debugDrawer.FlashCell(intVec, 0f, null, 50);
			string str = "trade drop spot: ";
			IntVec3 intVec2 = intVec;
			Log.Message(str + intVec2.ToString());
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x00110358 File Offset: 0x0010E558
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeFactionLeader(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactionsVisible.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction faction = enumerator.Current;
					list.Add(new DebugMenuOption(faction.Name, DebugMenuOptionMode.Action, delegate()
					{
						if (faction.leader != p)
						{
							faction.leader = p;
							if (ModsConfig.IdeologyActive)
							{
								foreach (Precept precept in faction.ideos.PrimaryIdeo.PreceptsListForReading)
								{
									Precept_Role precept_Role = precept as Precept_Role;
									if (precept_Role != null && precept_Role.def.leaderRole)
									{
										precept_Role.Assign(p, false);
										break;
									}
								}
							}
							DebugActionsUtility.DustPuffFrom(p);
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x001103FC File Offset: 0x0010E5FC
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillFactionLeader()
		{
			Pawn leader = (from x in Find.FactionManager.AllFactions
			where x.leader != null
			select x).RandomElement<Faction>().leader;
			int num = 0;
			while (!leader.Dead)
			{
				if (++num > 1000)
				{
					Log.Warning("Could not kill faction leader.");
					return;
				}
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Bullet, 30f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetIgnoreInstantKillProtection(true);
				leader.TakeDamage(dinfo);
			}
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x0011049C File Offset: 0x0010E69C
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillKidnappedPawn()
		{
			IEnumerable<Pawn> pawnsBySituation = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.Kidnapped);
			if (pawnsBySituation.Any<Pawn>())
			{
				Pawn pawn = pawnsBySituation.RandomElement<Pawn>();
				pawn.Kill(null, null);
				Messages.Message("Killed " + pawn.LabelCap, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x001104F0 File Offset: 0x0010E6F0
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				Pawn pLocal = pawn;
				list.Add(new DebugMenuOption(pawn.LabelShort + "(" + pawn.kindDef.label + ")", DebugMenuOptionMode.Action, delegate()
				{
					pLocal.Kill(null, null);
					Messages.Message("Killed " + pLocal.LabelCap, MessageTypeDefOf.NeutralEvent, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x0011059C File Offset: 0x0010E79C
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetFactionRelations()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac3 in Find.FactionManager.AllFactionsVisibleInViewOrder)
			{
				Faction localFac = localFac3;
				foreach (object obj in Enum.GetValues(typeof(FactionRelationKind)))
				{
					FactionRelationKind localRk2 = (FactionRelationKind)obj;
					FactionRelationKind localRk = localRk2;
					Faction localFac2 = localFac;
					FloatMenuOption item = new FloatMenuOption(((localFac2 != null) ? localFac2.ToString() : null) + " - " + localRk.ToString(), delegate()
					{
						if (localRk == FactionRelationKind.Hostile)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(localFac, -100, true, true, HistoryEventDefOf.DebugGoodwill, null);
							return;
						}
						if (localRk == FactionRelationKind.Ally)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(localFac, 100, true, true, HistoryEventDefOf.DebugGoodwill, null);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x001106DC File Offset: 0x0010E8DC
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void VisitorGift()
		{
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.AllPawnsSpawned)
			{
				if (pawn.Faction != null && !pawn.Faction.IsPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					list.Add(pawn);
					break;
				}
			}
			VisitorGiftForPlayerUtility.GiveRandomGift(list, list[0].Faction);
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x00110774 File Offset: 0x0010E974
		[DebugAction("General", "Increment time", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 500)]
		private static List<DebugActionNode> IncrementTime()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < DebugActionsMisc.TimeIncreases.Length; i++)
			{
				int durationLocal = DebugActionsMisc.TimeIncreases[i];
				list.Add(new DebugActionNode(durationLocal.ToStringTicksToPeriod(true, false, true, true, false), DebugActionType.Action, delegate()
				{
					Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + durationLocal);
				}, null));
			}
			return list;
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x001107D8 File Offset: 0x0010E9D8
		[DebugAction("General", "Storywatcher tick 1 day", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void StorywatcherTick1Day()
		{
			for (int i = 0; i < 60000; i++)
			{
				Find.StoryWatcher.StoryWatcherTick();
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x00110818 File Offset: 0x0010EA18
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void KillRandomLentColonist()
		{
			if (QuestUtility.TotalBorrowedColonistCount() > 0)
			{
				DebugActionsMisc.tmpLentColonists.Clear();
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (questsListForReading[i].State == QuestState.Ongoing)
					{
						List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
						for (int j = 0; j < partsListForReading.Count; j++)
						{
							QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = partsListForReading[j] as QuestPart_LendColonistsToFaction;
							if (questPart_LendColonistsToFaction != null)
							{
								List<Thing> lentColonistsListForReading = questPart_LendColonistsToFaction.LentColonistsListForReading;
								for (int k = 0; k < lentColonistsListForReading.Count; k++)
								{
									Pawn pawn = lentColonistsListForReading[k] as Pawn;
									if (pawn != null && !pawn.Dead)
									{
										DebugActionsMisc.tmpLentColonists.Add(pawn);
									}
								}
							}
						}
					}
				}
				Pawn pawn2 = DebugActionsMisc.tmpLentColonists.RandomElement<Pawn>();
				bool flag = pawn2.health.hediffSet.hediffs.Any((Hediff x) => x.def.isBad);
				pawn2.Kill(null, flag ? pawn2.health.hediffSet.hediffs.RandomElement<Hediff>() : null);
			}
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x00110959 File Offset: 0x0010EB59
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ClearPrisonerInteractionSchedule(Pawn p)
		{
			if (p.IsPrisonerOfColony)
			{
				p.mindState.lastAssignedInteractTime = -1;
				p.mindState.interactionsToday = 0;
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x00110984 File Offset: 0x0010EB84
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -100)]
		private static void GlowAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (c.InBounds(currentMap))
				{
					float num = Find.CurrentMap.glowGrid.GroundGlowAt(c, false, false);
					currentMap.debugDrawer.FlashCell(c, 0f, num.ToString("F1"), 100);
				}
			}
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x00110A18 File Offset: 0x0010EC18
		[DebugAction("General", "HSV At Position", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -100)]
		private static void HSVAtPosition()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(UI.MouseCell(), 10f, true))
			{
				if (intVec.InBounds(currentMap))
				{
					float value;
					float value2;
					float value3;
					Color.RGBToHSV(Find.CurrentMap.glowGrid.VisualGlowAt(intVec), out value, out value2, out value3);
					DebugCellDrawer debugDrawer = currentMap.debugDrawer;
					IntVec3 c = intVec;
					float colorPct = 0.5f;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
					defaultInterpolatedStringHandler.AppendLiteral("HSV(");
					defaultInterpolatedStringHandler.AppendFormatted<float>(value, ".0#");
					defaultInterpolatedStringHandler.AppendLiteral(",");
					defaultInterpolatedStringHandler.AppendFormatted<float>(value2, ".0#");
					defaultInterpolatedStringHandler.AppendLiteral(",");
					defaultInterpolatedStringHandler.AppendFormatted<float>(value3, ".0#");
					defaultInterpolatedStringHandler.AppendLiteral(")");
					debugDrawer.FlashCell(c, colorPct, defaultInterpolatedStringHandler.ToStringAndClear(), 100);
				}
			}
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x00110B1C File Offset: 0x0010ED1C
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void FlashBlockedLandingCells()
		{
			Map currentMap = Find.CurrentMap;
			foreach (IntVec3 c in currentMap.AllCells)
			{
				if (!c.Fogged(currentMap) && !DropCellFinder.IsGoodDropSpot(c, currentMap, false, false, false))
				{
					currentMap.debugDrawer.FlashCell(c, 0f, "bl", 50);
				}
			}
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x00110B98 File Offset: 0x0010ED98
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> PawnKindApparelCheck()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef3 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.race == ThingDefOf.Human
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef3;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.Action, delegate()
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
					bool flag = false;
					for (int i = 0; i < 100; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, faction, null);
						if (pawn.royalty != null)
						{
							RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
							if (mostSeniorTitle != null && !mostSeniorTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
							{
								for (int j = 0; j < mostSeniorTitle.def.requiredApparel.Count; j++)
								{
									ApparelRequirement apparelRequirement = mostSeniorTitle.def.requiredApparel[j];
									if (apparelRequirement.IsActive(pawn) && !apparelRequirement.IsMet(pawn))
									{
										string[] array = new string[6];
										int num = 0;
										PawnKindDef localKindDef2 = localKindDef;
										array[num] = ((localKindDef2 != null) ? localKindDef2.ToString() : null);
										array[1] = " (";
										array[2] = mostSeniorTitle.def.label;
										array[3] = ")  does not have its title requirements met. index=";
										array[4] = j.ToString();
										array[5] = DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|53_0(pawn);
										Log.Error(string.Concat(array));
										flag = true;
									}
								}
							}
						}
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int k = 0; k < wornApparel.Count; k++)
						{
							string text = DebugActionsMisc.<PawnKindApparelCheck>g__apparelOkayToWear|53_1(pawn, wornApparel[k]);
							if (text != "OK")
							{
								Log.Error(text + " - " + wornApparel[k].Label + DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|53_0(pawn));
								flag = true;
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (!flag)
					{
						Log.Message("No errors for " + localKindDef.defName);
					}
				}, null));
			}
			return list;
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x00110C60 File Offset: 0x0010EE60
		[DebugAction("General", "Edit effecter...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> EditEffecter()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (EffecterDef localDef2 in DefDatabase<EffecterDef>.AllDefs)
			{
				EffecterDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, delegate()
				{
					if (!Find.WindowStack.TryRemove(typeof(EditWindow_DefEditor), true))
					{
						Find.WindowStack.Add(new EditWindow_DefEditor(localDef));
					}
				}, null));
			}
			return list;
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x00110CE0 File Offset: 0x0010EEE0
		[DebugAction("General", "Edit Animation...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> EditAnimation()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (AnimationDef localDef2 in DefDatabase<AnimationDef>.AllDefs)
			{
				AnimationDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, delegate()
				{
					if (!Find.WindowStack.TryRemove(typeof(EditWindow_DefEditor), true))
					{
						Find.WindowStack.Add(new EditWindow_DefEditor(localDef));
					}
				}, null));
			}
			return list;
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x00110D60 File Offset: 0x0010EF60
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static List<DebugActionNode> PawnKindAbilityCheck()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (PawnKindDef localKindDef3 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.titleRequired != null || !kd.titleSelectOne.NullOrEmpty<RoyalTitleDef>()
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef3;
				list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.Action, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionDef);
					for (int i = 0; i < 100; i++)
					{
						RoyalTitleDef royalTitleDef = null;
						if (localKindDef.titleRequired != null)
						{
							royalTitleDef = localKindDef.titleRequired;
						}
						else if (!localKindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() && Rand.Chance(localKindDef.royalTitleChance))
						{
							royalTitleDef = localKindDef.titleSelectOne.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
						}
						PawnKindDef localKindDef2 = localKindDef;
						Faction faction2 = faction;
						PawnGenerationContext context = PawnGenerationContext.NonPlayer;
						RoyalTitleDef fixedTitle = royalTitleDef;
						Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(localKindDef2, faction2, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, fixedTitle, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
						RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
						if (mostSeniorTitle != null)
						{
							Hediff_Psylink mainPsylinkSource = pawn.GetMainPsylinkSource();
							if (mainPsylinkSource == null)
							{
								if (mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def) > 0)
								{
									string text = mostSeniorTitle.def.LabelCap + " - No psylink.";
									if (pawn.abilities.abilities.Any((Ability x) => x.def.IsPsycast && x.def.level > 0))
									{
										text += " Has psycasts without psylink.";
									}
									stringBuilder.AppendLine(text);
								}
							}
							else if (mainPsylinkSource.level < mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								stringBuilder.AppendLine("Psylink at level " + mainPsylinkSource.level.ToString() + ", but requires " + mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def).ToString());
							}
							else if (mainPsylinkSource.level > mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								stringBuilder.AppendLine("Psylink at level " + mainPsylinkSource.level.ToString() + ". Max is " + mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def).ToString());
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (stringBuilder.Length == 0)
					{
						Log.Message("No errors for " + localKindDef.defName);
						return;
					}
					string str = "Errors:\n";
					StringBuilder stringBuilder2 = stringBuilder;
					Log.Error(str + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null));
				}, null));
			}
			return list;
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x00110E28 File Offset: 0x0010F028
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		public static void AtlasRebuild()
		{
			GlobalTextureAtlasManager.rebakeAtlas = true;
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x00110E30 File Offset: 0x0010F030
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DumpPawnAtlases()
		{
			string text = Application.dataPath + "\\atlasDump_Pawn";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			GlobalTextureAtlasManager.DumpPawnAtlases(text);
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x00110E64 File Offset: 0x0010F064
		[DebugAction("General", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void DumpStaticAtlases()
		{
			string text = Application.dataPath + "\\atlasDump_Static";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			GlobalTextureAtlasManager.DumpStaticAtlases(text);
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x00110E98 File Offset: 0x0010F098
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, name = "Set Anomaly level...", allowedGameStates = AllowedGameStates.Playing, requiresAnomaly = true)]
		private static List<DebugActionNode> SetMonolithLevel()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<MonolithLevelDef> enumerator = DefDatabase<MonolithLevelDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MonolithLevelDef def = enumerator.Current;
					list.Add(new DebugActionNode(def.defName, DebugActionType.Action, delegate()
					{
						Find.Anomaly.SetLevel(def, false);
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x00110F14 File Offset: 0x0010F114
		[DebugAction("General", null, false, false, false, false, false, 0, false, name = "Hot reload Defs", allowedGameStates = AllowedGameStates.Playing, displayPriority = 9999)]
		private static void HotReloadDefs()
		{
			PlayDataLoader.HotReloadDefs();
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x00110F1B File Offset: 0x0010F11B
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void SetGraphicsDirty(Pawn p)
		{
			p.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x00110F30 File Offset: 0x0010F130
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ToggleMovement(Pawn p)
		{
			p.pather.debugDisabled = !p.pather.debugDisabled;
			MoteMaker.ThrowText(p.Position.ToVector3(), Find.CurrentMap, "Movement " + (p.pather.debugDisabled ? "Disabled" : "Enabled"), -1f);
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x00110F98 File Offset: 0x0010F198
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void ToggleMaxMoveSpeed(Pawn p)
		{
			p.debugMaxMoveSpeed = !p.debugMaxMoveSpeed;
			MoteMaker.ThrowText(p.Position.ToVector3(), Find.CurrentMap, "Max MoveSpeed " + (p.debugMaxMoveSpeed ? "Enabled" : "Disabled"), -1f);
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x00110FF0 File Offset: 0x0010F1F0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static List<DebugActionNode> LockRotation()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Rot4 lRot2 in Rot4.AllRotations)
			{
				Rot4 lRot = lRot2;
				list.Add(new DebugActionNode(lRot2.ToStringHuman() ?? "", DebugActionType.ToolMapForPawns, null, null)
				{
					pawnAction = delegate(Pawn pawn)
					{
						if (pawn.debugRotLocked)
						{
							pawn.debugRotLocked = false;
						}
						pawn.Rotation = lRot;
						pawn.debugRotLocked = true;
						MoteMaker.ThrowText(pawn.Position.ToVector3(), Find.CurrentMap, "Rot Locked", -1f);
					}
				});
			}
			return list;
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x0011107C File Offset: 0x0010F27C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000)]
		private static void UnlockRotation(Pawn p)
		{
			if (!p.debugRotLocked)
			{
				return;
			}
			p.debugRotLocked = false;
			MoteMaker.ThrowText(p.Position.ToVector3(), Find.CurrentMap, "Rot Unlocked", -1f);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x001110BB File Offset: 0x0010F2BB
		[DebugAction("General", "Reveal Hidden Defs", false, false, false, false, false, 0, false)]
		private static void RevealHiddenDefs()
		{
			Find.HiddenItemsManager.ClearHiddenDefs();
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x001110C8 File Offset: 0x0010F2C8
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, name = "End revenant hypnosis", requiresAnomaly = true)]
		private static void EndRevenantHypnosis(Pawn p)
		{
			Hediff hediff = p.health.hediffSet.hediffs.Find((Hediff hd) => hd.def == HediffDefOf.RevenantHypnosis);
			if (hediff != null)
			{
				p.health.RemoveHediff(hediff);
			}
			p.stances.stunner.StopStun();
			Find.Anomaly.EndHypnotize(p);
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x00111134 File Offset: 0x0010F334
		[DebugAction("General", "Layer pathfinder...", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void LayerPathfinder()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer origin2 = planetLayer;
				PlanetLayer origin = origin2;
				list.Add(new DebugMenuOption(origin.Def.label, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (KeyValuePair<int, PlanetLayer> keyValuePair2 in Find.WorldGrid.PlanetLayers)
					{
						int num2;
						PlanetLayer planetLayer2;
						keyValuePair2.Deconstruct(out num2, out planetLayer2);
						PlanetLayer dest2 = planetLayer2;
						PlanetLayer dest = dest2;
						if (dest != origin)
						{
							list2.Add(new DebugMenuOption(dest.Def.label, DebugMenuOptionMode.Action, delegate()
							{
								List<PlanetLayerConnection> list3 = new List<PlanetLayerConnection>();
								float value;
								string text;
								if (origin.TryGetPath(dest, list3, out value, 2147483647))
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 3);
									defaultInterpolatedStringHandler.AppendLiteral("Found path between ");
									defaultInterpolatedStringHandler.AppendFormatted(origin.Def.label);
									defaultInterpolatedStringHandler.AppendLiteral(" and ");
									defaultInterpolatedStringHandler.AppendFormatted(dest.Def.label);
									defaultInterpolatedStringHandler.AppendLiteral(", cost: ");
									defaultInterpolatedStringHandler.AppendFormatted<float>(value);
									text = defaultInterpolatedStringHandler.ToStringAndClear();
									using (List<PlanetLayerConnection>.Enumerator enumerator3 = list3.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											PlanetLayerConnection planetLayerConnection = enumerator3.Current;
											string str = text;
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(9, 3);
											defaultInterpolatedStringHandler2.AppendLiteral("\n ");
											defaultInterpolatedStringHandler2.AppendFormatted(planetLayerConnection.origin.Def.label);
											defaultInterpolatedStringHandler2.AppendLiteral(" (");
											defaultInterpolatedStringHandler2.AppendFormatted<float>(planetLayerConnection.fuelCost);
											defaultInterpolatedStringHandler2.AppendLiteral(") -> ");
											defaultInterpolatedStringHandler2.AppendFormatted(planetLayerConnection.target.Def.label);
											text = str + defaultInterpolatedStringHandler2.ToStringAndClear();
										}
										goto IL_17B;
									}
								}
								text = "No path between " + origin.Def.label + " and " + dest.Def.label;
								IL_17B:
								Log.Message(text);
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, "Select destination"));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, "Select origin"));
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x001111E0 File Offset: 0x0010F3E0
		[DebugAction("Other", "Clear cached materials", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Invalid, hideInSubMenu = true)]
		private static void ClearCachedMaterials()
		{
			MatLoader.ClearCache();
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x001111E7 File Offset: 0x0010F3E7
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void LogCellMapData()
		{
			Find.CurrentMap.pathFinder.MapData.LogCell(UI.MouseCell());
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x00111202 File Offset: 0x0010F402
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		private static void LogPathfinderState()
		{
			Find.CurrentMap.pathFinder.LogPathfinderState();
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x00111213 File Offset: 0x0010F413
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void LogCellGridResult()
		{
			Find.CurrentMap.pathFinder.LogGridCellResult(UI.MouseCell());
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x0011122C File Offset: 0x0010F42C
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Breach()
		{
			IntVec3 start;
			Action <>9__1;
			DebugTools.curTool = new DebugTool("start...", delegate()
			{
				start = UI.MouseCell();
				string label = "end...";
				Action clickAction;
				if ((clickAction = <>9__1) == null)
				{
					clickAction = (<>9__1 = delegate()
					{
						IntVec3 end = UI.MouseCell();
						new BreachingGrid(Find.CurrentMap, null).CreateBreachPath(start, end, 1, 3, true);
						DebugTools.curTool = null;
					});
				}
				DebugTools.curTool = new DebugTool(label, clickAction, null);
			}, null);
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x0011125C File Offset: 0x0010F45C
		[DebugAction("Pathing", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Goto(Pawn pawn)
		{
			DebugTools.curTool = new DebugTool("Select destination...", delegate()
			{
				IntVec3 c = UI.MouseCell();
				if (!pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					MoteMaker.ThrowText(c.ToVector3(), Find.CurrentMap, "Cannot reach cell", -1f);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
				job.locomotionUrgency = LocomotionUrgency.Jog;
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				DebugTools.curTool = null;
			}, null);
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x00111294 File Offset: 0x0010F494
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.Playing, requiresOdyssey = true)]
		private static void RetroactivelyAddLandmarksToWorld()
		{
			new WorldGenStep_Landmarks().GenerateFresh(Find.World.info.seedString, Find.WorldGrid.Surface);
			Find.World.renderer.GetLayer<WorldDrawLayer_Terrain>(Find.WorldGrid.Surface).RegenerateNow();
			Find.World.renderer.GetLayer<WorldDrawLayer_Landmarks>(Find.WorldGrid.Surface).RegenerateNow();
			Find.World.renderer.GetLayer<WorldDrawLayer_Hills>(Find.WorldGrid.Surface).RegenerateNow();
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x00111320 File Offset: 0x0010F520
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.Playing, requiresOdyssey = true)]
		private static void RegenerateMapFeatures()
		{
			foreach (Tile tile in Find.WorldGrid.Surface.Tiles)
			{
				List<TileMutatorDef> mutatorsNullable = tile.mutatorsNullable;
				if (mutatorsNullable != null)
				{
					mutatorsNullable.Clear();
				}
			}
			WorldGenStep_Mutators.AddMutatorsFromTile(Find.WorldGrid.Surface);
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x001113B8 File Offset: 0x0010F5B8
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__logApparel|53_0(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Apparel of " + p.LabelShort + ":");
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				stringBuilder.AppendLine("  - " + wornApparel[i].Label);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x00111430 File Offset: 0x0010F630
		[CompilerGenerated]
		internal static string <PawnKindApparelCheck>g__apparelOkayToWear|53_1(Pawn pawn, Apparel apparel)
		{
			ApparelProperties app = apparel.def.apparel;
			if (!pawn.kindDef.apparelRequired.NullOrEmpty<ThingDef>() && pawn.kindDef.apparelRequired.Contains(apparel.def))
			{
				return "OK";
			}
			if (!app.PawnCanWear(pawn, false))
			{
				return "Pawn cannot wear.";
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirements[i], apparel.def) && PawnApparelGenerator.ApparelRequirementTagsMatch(specificApparelRequirements[i], apparel.def))
					{
						return "OK";
					}
				}
			}
			if (!pawn.kindDef.apparelTags.NullOrEmpty<string>())
			{
				if (!app.tags.Any((string tag) => pawn.kindDef.apparelTags.Contains(tag)))
				{
					return "Required tag missing.";
				}
				if ((pawn.royalty == null || pawn.royalty.MostSeniorTitle == null) && app.tags.Contains("Royal") && !pawn.kindDef.apparelTags.Any((string tag) => app.tags.Contains(tag)))
				{
					return "Royal apparel on non-royal pawn.";
				}
			}
			if (!pawn.kindDef.apparelDisallowTags.NullOrEmpty<string>() && pawn.kindDef.apparelDisallowTags.Any((string t) => app.tags.Contains(t)))
			{
				return "Has a disallowed tag.";
			}
			if (pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
			{
				RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
				QualityCategory qualityCategory;
				if (apparel.TryGetQuality(out qualityCategory) && qualityCategory < mostSeniorTitle.def.requiredMinimumApparelQuality)
				{
					return "Quality too low.";
				}
			}
			return "OK";
		}

		// Token: 0x04001E27 RID: 7719
		private static PlanetLayer TestLayer;

		// Token: 0x04001E28 RID: 7720
		private static readonly int[] TimeIncreases = new int[]
		{
			2500,
			15000,
			30000,
			60000,
			900000
		};

		// Token: 0x04001E29 RID: 7721
		private static List<Pawn> tmpLentColonists = new List<Pawn>();

		// Token: 0x04001E2A RID: 7722
		private const string NoErrorString = "OK";

		// Token: 0x04001E2B RID: 7723
		private const string RoyalApparelTag = "Royal";

		// Token: 0x04001E2C RID: 7724
		private const int PawnsToGenerate = 100;
	}
}
