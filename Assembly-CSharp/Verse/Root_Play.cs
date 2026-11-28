using System;
using System.IO;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000204 RID: 516
	public class Root_Play : Root
	{
		// Token: 0x06000ED1 RID: 3793 RVA: 0x0004C5D0 File Offset: 0x0004A7D0
		public override void Start()
		{
			base.Start();
			try
			{
				this.musicManagerPlay = new MusicManagerPlay();
				FileInfo autostart = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
				Root.checkedAutostartSaveFile = true;
				if (autostart != null)
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Path.GetFileNameWithoutExtension(autostart.Name));
					}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true, false, null);
				}
				else if (Find.GameInitData != null && !Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Find.GameInitData.gameToLoad);
					}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true, false, null);
				}
				else
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						if (Current.Game == null)
						{
							Root_Play.SetupForQuickTestPlay();
							Find.GameInitData.PrepForMapGen();
							Find.Scenario.PreMapGenerate();
						}
						Current.Game.InitNewGame();
					}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
				}
				LongEventHandler.QueueLongEvent(delegate()
				{
					ScreenFader.SetColor(Color.black);
					ScreenFader.StartFade(Color.clear, 0.5f);
				}, null, false, null, true, false, null);
			}
			catch (Exception ex)
			{
				string str = "Critical error in root Start(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0004C738 File Offset: 0x0004A938
		public override void Update()
		{
			base.Update();
			if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
			{
				return;
			}
			try
			{
				ShipCountdown.ShipCountdownUpdate();
				if (ModsConfig.IdeologyActive)
				{
					ArchonexusCountdown.ArchonexusCountdownUpdate();
				}
				TargetHighlighter.TargetHighlighterUpdate();
				Current.Game.UpdatePlay();
				this.musicManagerPlay.MusicUpdate();
				PerformanceBenchmarkUtility.CheckBenchmark();
			}
			catch (Exception ex)
			{
				string str = "Root level exception in Update(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0004C7C0 File Offset: 0x0004A9C0
		public static void SetupForQuickTestPlay()
		{
			Current.ProgramState = ProgramState.Entry;
			Game.ClearCaches();
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = ScenarioDefOf.Crashlanded.scenario;
			Find.Scenario.PreConfigure();
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.Rough);
			Current.Game.World = WorldGenerator.GenerateWorld(0.3f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal, OverallPopulation.Normal, LandmarkDensity.Normal, null, 0f);
			Find.GameInitData.ChooseRandomStartingTile();
			Find.GameInitData.mapSize = 250;
			Find.Scenario.PostIdeoChosen();
		}

		// Token: 0x04000E80 RID: 3712
		public MusicManagerPlay musicManagerPlay;
	}
}
