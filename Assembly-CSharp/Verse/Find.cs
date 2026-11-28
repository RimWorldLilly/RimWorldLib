using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020006DA RID: 1754
	public static class Find
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x0600373C RID: 14140 RVA: 0x001620C4 File Offset: 0x001602C4
		public static Root Root
		{
			get
			{
				return Current.Root;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x001620CB File Offset: 0x001602CB
		public static SoundRoot SoundRoot
		{
			get
			{
				return Current.Root.soundRoot;
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x0600373E RID: 14142 RVA: 0x001620D7 File Offset: 0x001602D7
		public static UIRoot UIRoot
		{
			get
			{
				if (!(Current.Root != null))
				{
					return null;
				}
				return Current.Root.uiRoot;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x0600373F RID: 14143 RVA: 0x001620F2 File Offset: 0x001602F2
		public static MusicManagerEntry MusicManagerEntry
		{
			get
			{
				return ((Root_Entry)Current.Root).musicManagerEntry;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06003740 RID: 14144 RVA: 0x00162103 File Offset: 0x00160303
		public static MusicManagerPlay MusicManagerPlay
		{
			get
			{
				return ((Root_Play)Current.Root).musicManagerPlay;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06003741 RID: 14145 RVA: 0x00162114 File Offset: 0x00160314
		public static LanguageWorker ActiveLanguageWorker
		{
			get
			{
				return LanguageDatabase.activeLanguage.Worker;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06003742 RID: 14146 RVA: 0x00162120 File Offset: 0x00160320
		public static Camera Camera
		{
			get
			{
				return Current.Camera;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06003743 RID: 14147 RVA: 0x00162127 File Offset: 0x00160327
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.CameraDriver;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06003744 RID: 14148 RVA: 0x0016212E File Offset: 0x0016032E
		public static ColorCorrectionCurves CameraColor
		{
			get
			{
				return Current.ColorCorrectionCurves;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06003745 RID: 14149 RVA: 0x00162135 File Offset: 0x00160335
		public static Camera PawnCacheCamera
		{
			get
			{
				return PawnCacheCameraManager.PawnCacheCamera;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06003746 RID: 14150 RVA: 0x0016213C File Offset: 0x0016033C
		public static PawnCacheRenderer PawnCacheRenderer
		{
			get
			{
				return PawnCacheCameraManager.PawnCacheRenderer;
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06003747 RID: 14151 RVA: 0x00162143 File Offset: 0x00160343
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.WorldCamera;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06003748 RID: 14152 RVA: 0x0016214A File Offset: 0x0016034A
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.WorldCameraDriver;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06003749 RID: 14153 RVA: 0x00162151 File Offset: 0x00160351
		public static WindowStack WindowStack
		{
			get
			{
				UIRoot uiroot = Find.UIRoot;
				if (uiroot == null)
				{
					return null;
				}
				return uiroot.windows;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x0600374A RID: 14154 RVA: 0x00162163 File Offset: 0x00160363
		public static ScreenshotModeHandler ScreenshotModeHandler
		{
			get
			{
				UIRoot uiroot = Find.UIRoot;
				if (uiroot == null)
				{
					return null;
				}
				return uiroot.screenshotMode;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x0600374B RID: 14155 RVA: 0x00162175 File Offset: 0x00160375
		public static MainButtonsRoot MainButtonsRoot
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mainButtonsRoot;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x0600374C RID: 14156 RVA: 0x00162186 File Offset: 0x00160386
		public static MainTabsRoot MainTabsRoot
		{
			get
			{
				return Find.MainButtonsRoot.tabs;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x0600374D RID: 14157 RVA: 0x00162192 File Offset: 0x00160392
		public static MapInterface MapUI
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mapUI;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x001621A3 File Offset: 0x001603A3
		public static AlertsReadout Alerts
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).alerts;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x0600374F RID: 14159 RVA: 0x001621B4 File Offset: 0x001603B4
		public static Selector Selector
		{
			get
			{
				return Find.MapUI.selector;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06003750 RID: 14160 RVA: 0x001621C0 File Offset: 0x001603C0
		public static Targeter Targeter
		{
			get
			{
				return Find.MapUI.targeter;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06003751 RID: 14161 RVA: 0x001621CC File Offset: 0x001603CC
		public static ColonistBar ColonistBar
		{
			get
			{
				return Find.MapUI.colonistBar;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003752 RID: 14162 RVA: 0x001621D8 File Offset: 0x001603D8
		public static DesignatorManager DesignatorManager
		{
			get
			{
				return Find.MapUI.designatorManager;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06003753 RID: 14163 RVA: 0x001621E4 File Offset: 0x001603E4
		public static ReverseDesignatorDatabase ReverseDesignatorDatabase
		{
			get
			{
				return Find.MapUI.reverseDesignatorDatabase;
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06003754 RID: 14164 RVA: 0x001621F0 File Offset: 0x001603F0
		public static GameInitData GameInitData
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.InitData;
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003755 RID: 14165 RVA: 0x00162202 File Offset: 0x00160402
		public static GameInfo GameInfo
		{
			get
			{
				return Current.Game.Info;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003756 RID: 14166 RVA: 0x00162210 File Offset: 0x00160410
		public static Scenario Scenario
		{
			get
			{
				if (Current.Game != null && Current.Game.Scenario != null)
				{
					return Current.Game.Scenario;
				}
				if (ScenarioMaker.GeneratingScenario != null)
				{
					return ScenarioMaker.GeneratingScenario;
				}
				if (Find.UIRoot != null)
				{
					Page_ScenarioEditor page_ScenarioEditor = Find.WindowStack.WindowOfType<Page_ScenarioEditor>();
					if (page_ScenarioEditor != null)
					{
						return page_ScenarioEditor.EditingScenario;
					}
				}
				return null;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003757 RID: 14167 RVA: 0x00162265 File Offset: 0x00160465
		public static World World
		{
			get
			{
				if (Current.Game == null || Current.Game.World == null)
				{
					return Current.CreatingWorld;
				}
				return Current.Game.World;
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06003758 RID: 14168 RVA: 0x0016228A File Offset: 0x0016048A
		public static List<Map> Maps
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.Maps;
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003759 RID: 14169 RVA: 0x0016229C File Offset: 0x0016049C
		public static Map CurrentMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.CurrentMap;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x0600375A RID: 14170 RVA: 0x001622AE File Offset: 0x001604AE
		public static Map AnyPlayerHomeMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.AnyPlayerHomeMap;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x001622C0 File Offset: 0x001604C0
		public static Map RandomPlayerHomeMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.RandomPlayerHomeMap;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x0600375C RID: 14172 RVA: 0x001622D2 File Offset: 0x001604D2
		public static Map RandomRootSurfacePlayerHomeMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.RandomRootSurfacePlayerHomeMap;
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x0600375D RID: 14173 RVA: 0x001622E4 File Offset: 0x001604E4
		public static Map RandomSurfacePlayerHomeMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.RandomRootSurfacePlayerHomeMap;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x0600375E RID: 14174 RVA: 0x001622F6 File Offset: 0x001604F6
		public static StoryWatcher StoryWatcher
		{
			get
			{
				return Current.Game.storyWatcher;
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x00162302 File Offset: 0x00160502
		public static ResearchManager ResearchManager
		{
			get
			{
				return Current.Game.researchManager;
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003760 RID: 14176 RVA: 0x0016230E File Offset: 0x0016050E
		public static AnalysisManager AnalysisManager
		{
			get
			{
				return Current.Game.analysisManager;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003761 RID: 14177 RVA: 0x0016231A File Offset: 0x0016051A
		public static Storyteller Storyteller
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.storyteller;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x0016232C File Offset: 0x0016052C
		public static GameEnder GameEnder
		{
			get
			{
				return Current.Game.gameEnder;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003763 RID: 14179 RVA: 0x00162338 File Offset: 0x00160538
		public static LetterStack LetterStack
		{
			get
			{
				return Current.Game.letterStack;
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003764 RID: 14180 RVA: 0x00162344 File Offset: 0x00160544
		public static Archive Archive
		{
			get
			{
				History history = Find.History;
				if (history == null)
				{
					return null;
				}
				return history.archive;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003765 RID: 14181 RVA: 0x00162356 File Offset: 0x00160556
		public static PlaySettings PlaySettings
		{
			get
			{
				return Current.Game.playSettings;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003766 RID: 14182 RVA: 0x00162362 File Offset: 0x00160562
		public static History History
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.history;
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x00162374 File Offset: 0x00160574
		public static TaleManager TaleManager
		{
			get
			{
				return Current.Game.taleManager;
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003768 RID: 14184 RVA: 0x00162380 File Offset: 0x00160580
		public static PlayLog PlayLog
		{
			get
			{
				return Current.Game.playLog;
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x0016238C File Offset: 0x0016058C
		public static BattleLog BattleLog
		{
			get
			{
				return Current.Game.battleLog;
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x0600376A RID: 14186 RVA: 0x00162398 File Offset: 0x00160598
		public static TickManager TickManager
		{
			get
			{
				return Current.Game.tickManager;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x001623A4 File Offset: 0x001605A4
		public static Tutor Tutor
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.tutor;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600376C RID: 14188 RVA: 0x001623B6 File Offset: 0x001605B6
		public static TutorialState TutorialState
		{
			get
			{
				return Current.Game.tutor.tutorialState;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x0600376D RID: 14189 RVA: 0x001623C7 File Offset: 0x001605C7
		public static ActiveLessonHandler ActiveLesson
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.tutor.activeLesson;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x0600376E RID: 14190 RVA: 0x001623DE File Offset: 0x001605DE
		public static Autosaver Autosaver
		{
			get
			{
				return Current.Game.autosaver;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x0600376F RID: 14191 RVA: 0x001623EA File Offset: 0x001605EA
		public static DateNotifier DateNotifier
		{
			get
			{
				return Current.Game.dateNotifier;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003770 RID: 14192 RVA: 0x001623F6 File Offset: 0x001605F6
		public static SignalManager SignalManager
		{
			get
			{
				return Current.Game.signalManager;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06003771 RID: 14193 RVA: 0x00162402 File Offset: 0x00160602
		public static UniqueIDsManager UniqueIDsManager
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.uniqueIDsManager;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06003772 RID: 14194 RVA: 0x00162414 File Offset: 0x00160614
		public static QuestManager QuestManager
		{
			get
			{
				return Current.Game.questManager;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003773 RID: 14195 RVA: 0x00162420 File Offset: 0x00160620
		public static RelationshipRecords RelationshipRecords
		{
			get
			{
				return Current.Game.relationshipRecords;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003774 RID: 14196 RVA: 0x0016242C File Offset: 0x0016062C
		public static TransportShipManager TransportShipManager
		{
			get
			{
				return Current.Game.transportShipManager;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003775 RID: 14197 RVA: 0x00162438 File Offset: 0x00160638
		public static GameComponent_Bossgroup BossgroupManager
		{
			get
			{
				return Current.Game.GetComponent<GameComponent_Bossgroup>();
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003776 RID: 14198 RVA: 0x00162444 File Offset: 0x00160644
		public static GameComponent_Anomaly Anomaly
		{
			get
			{
				GameComponent_Anomaly result;
				if ((result = Find.cachedAnomaly) == null)
				{
					result = (Find.cachedAnomaly = Current.Game.GetComponent<GameComponent_Anomaly>());
				}
				return result;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003777 RID: 14199 RVA: 0x0016245F File Offset: 0x0016065F
		public static StudyManager StudyManager
		{
			get
			{
				return Current.Game.studyManager;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003778 RID: 14200 RVA: 0x0016246B File Offset: 0x0016066B
		public static CustomXenogermDatabase CustomXenogermDatabase
		{
			get
			{
				return Current.Game.customXenogermDatabase;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003779 RID: 14201 RVA: 0x00162477 File Offset: 0x00160677
		public static GameComponent_PsychicRitualManager PsychicRitualManager
		{
			get
			{
				return Current.Game.GetComponent<GameComponent_PsychicRitualManager>();
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x0600377A RID: 14202 RVA: 0x00162483 File Offset: 0x00160683
		public static GameComponent_PawnDuplicator PawnDuplicator
		{
			get
			{
				return Current.Game.GetComponent<GameComponent_PawnDuplicator>();
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x0600377B RID: 14203 RVA: 0x0016248F File Offset: 0x0016068F
		public static HiddenItemsManager HiddenItemsManager
		{
			get
			{
				return Current.Game.hiddenItemsManager;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x0600377C RID: 14204 RVA: 0x0016249B File Offset: 0x0016069B
		public static EntityCodex EntityCodex
		{
			get
			{
				return Current.Game.entityCodex;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x0600377D RID: 14205 RVA: 0x001624A7 File Offset: 0x001606A7
		public static Gravship CurrentGravship
		{
			get
			{
				return Current.Game.Gravship;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x0600377E RID: 14206 RVA: 0x001624B3 File Offset: 0x001606B3
		public static FactionManager FactionManager
		{
			get
			{
				return Find.World.factionManager;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x0600377F RID: 14207 RVA: 0x001624BF File Offset: 0x001606BF
		public static IdeoManager IdeoManager
		{
			get
			{
				return Find.World.ideoManager;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003780 RID: 14208 RVA: 0x001624CB File Offset: 0x001606CB
		public static WorldPawns WorldPawns
		{
			get
			{
				return Find.World.worldPawns;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06003781 RID: 14209 RVA: 0x001624D7 File Offset: 0x001606D7
		public static WorldObjectsHolder WorldObjects
		{
			get
			{
				return Find.World.worldObjects;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x001624E3 File Offset: 0x001606E3
		public static WorldGrid WorldGrid
		{
			get
			{
				return Find.World.grid;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06003783 RID: 14211 RVA: 0x001624EF File Offset: 0x001606EF
		public static WorldDebugDrawer WorldDebugDrawer
		{
			get
			{
				return Find.World.debugDrawer;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003784 RID: 14212 RVA: 0x001624FB File Offset: 0x001606FB
		public static WorldPathGrid WorldPathGrid
		{
			get
			{
				return Find.World.pathGrid;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06003785 RID: 14213 RVA: 0x00162507 File Offset: 0x00160707
		public static WorldDynamicDrawManager WorldDynamicDrawManager
		{
			get
			{
				return Find.World.dynamicDrawManager;
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06003786 RID: 14214 RVA: 0x00162513 File Offset: 0x00160713
		public static WorldPathPool WorldPathPool
		{
			get
			{
				return Find.World.pathPool;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06003787 RID: 14215 RVA: 0x0016251F File Offset: 0x0016071F
		public static WorldReachability WorldReachability
		{
			get
			{
				return Find.World.reachability;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06003788 RID: 14216 RVA: 0x0016252B File Offset: 0x0016072B
		public static WorldFeatures WorldFeatures
		{
			get
			{
				return Find.World.features;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06003789 RID: 14217 RVA: 0x00162537 File Offset: 0x00160737
		public static WorldComponent_GravshipController GravshipController
		{
			get
			{
				return Find.World.GetComponent<WorldComponent_GravshipController>();
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x0600378A RID: 14218 RVA: 0x00162543 File Offset: 0x00160743
		public static WorldInterface WorldInterface
		{
			get
			{
				return Find.World.UI;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x0600378B RID: 14219 RVA: 0x0016254F File Offset: 0x0016074F
		public static WorldSelector WorldSelector
		{
			get
			{
				return Find.WorldInterface.selector;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x0600378C RID: 14220 RVA: 0x0016255B File Offset: 0x0016075B
		public static WorldTargeter WorldTargeter
		{
			get
			{
				return Find.WorldInterface.targeter;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x0600378D RID: 14221 RVA: 0x00162567 File Offset: 0x00160767
		public static WorldRoutePlanner WorldRoutePlanner
		{
			get
			{
				return Find.WorldInterface.routePlanner;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x0600378E RID: 14222 RVA: 0x00162573 File Offset: 0x00160773
		public static TilePicker TilePicker
		{
			get
			{
				return Find.WorldInterface.tilePicker;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x0600378F RID: 14223 RVA: 0x0016257F File Offset: 0x0016077F
		public static HistoryEventsManager HistoryEventsManager
		{
			get
			{
				return Find.History.historyEventsManager;
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06003790 RID: 14224 RVA: 0x0016258B File Offset: 0x0016078B
		public static GoodwillSituationManager GoodwillSituationManager
		{
			get
			{
				return Find.FactionManager.goodwillSituationManager;
			}
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x00162597 File Offset: 0x00160797
		public static void ClearCache()
		{
			Find.cachedAnomaly = null;
		}

		// Token: 0x04002448 RID: 9288
		private static GameComponent_Anomaly cachedAnomaly;
	}
}
