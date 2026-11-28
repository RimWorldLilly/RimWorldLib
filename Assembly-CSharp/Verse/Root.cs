using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Analytics;
using Verse.AI;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000202 RID: 514
	public abstract class Root : MonoBehaviour
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0004C086 File Offset: 0x0004A286
		public static float AverageFrameTime
		{
			get
			{
				return Root.smoothedAverageDeltaTime.GetAverage();
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0004C094 File Offset: 0x0004A294
		public virtual void Start()
		{
			try
			{
				CultureInfoUtility.EnsureEnglish();
				Current.Notify_LoadedSceneChanged();
				GlobalTextureAtlasManager.FreeAllRuntimeAtlases();
				Root.CheckGlobalInit();
				Action action = delegate()
				{
					DeepProfiler.Start("Misc Init (InitializingInterface)");
					try
					{
						this.soundRoot = new SoundRoot();
						DeepProfiler.Start("Instantiate UIRoot");
						if (GenScene.InPlayScene)
						{
							this.uiRoot = new UIRoot_Play();
						}
						else if (GenScene.InEntryScene)
						{
							this.uiRoot = new UIRoot_Entry();
						}
						DeepProfiler.End();
						this.uiRoot.Init();
						Messages.Notify_LoadedLevelChanged();
						if (Current.SubcameraDriver != null)
						{
							Current.SubcameraDriver.Init();
						}
					}
					finally
					{
						DeepProfiler.End();
					}
				};
				if (!PlayDataLoader.Loaded)
				{
					Application.runInBackground = true;
					LongEventHandler.QueueLongEvent(delegate()
					{
						PlayDataLoader.LoadAllPlayData(false);
					}, null, true, null, true, false, null);
					LongEventHandler.QueueLongEvent(action, "InitializingInterface", false, null, true, false, null);
				}
				else
				{
					action();
				}
			}
			catch (Exception ex)
			{
				string str = "Critical error in root Start(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0004C144 File Offset: 0x0004A344
		private static void CheckGlobalInit()
		{
			if (Root.globalInitDone)
			{
				return;
			}
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs != null && commandLineArgs.Length > 1)
			{
				Log.Message("Command line arguments: " + GenText.ToSpaceList(commandLineArgs.Skip(1)));
			}
			PerformanceReporting.enabled = false;
			Application.targetFrameRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
			UnityDataInitializer.CopyUnityData();
			SteamManager.InitIfNeeded();
			VersionControl.LogVersionNumber();
			Prefs.Init();
			Application.logMessageReceivedThreaded += Log.Notify_MessageReceivedThreadedInternal;
			if (Prefs.DevMode)
			{
				StaticConstructorOnStartupUtility.ReportProbablyMissingAttributes();
			}
			LongEventHandler.QueueLongEvent(new Action(StaticConstructorOnStartupUtility.CallAll), null, false, null, true, false, null);
			Root.globalInitDone = true;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0004C1F8 File Offset: 0x0004A3F8
		public virtual void Update()
		{
			try
			{
				ResolutionUtility.Update();
				RealTime.Update();
				Root.rawAverageDeltaTime.AddValue(RealTime.deltaTime * 1000f);
				Root.smoothedAverageDeltaTime.AddValue(Root.rawAverageDeltaTime.GetAverage());
				bool flag;
				LongEventHandler.LongEventsUpdate(out flag);
				if (flag)
				{
					this.destroyed = true;
				}
				else if (!LongEventHandler.ShouldWaitForEvent)
				{
					Rand.EnsureStateStackEmpty();
					Widgets.EnsureMousePositionStackEmpty();
					SteamManager.Update();
					PortraitsCache.PortraitsCacheUpdate();
					AttackTargetsCache.AttackTargetsCacheStaticUpdate();
					Pawn_MeleeVerbs.PawnMeleeVerbsStaticUpdate();
					Storyteller.StorytellerStaticUpdate();
					CaravanInventoryUtility.CaravanInventoryUtilityStaticUpdate();
					this.uiRoot.UIRootUpdate();
					if (Time.frameCount > 3 && !Root.prefsApplied)
					{
						Root.prefsApplied = true;
						Prefs.Apply();
					}
					this.soundRoot.Update();
				}
			}
			catch (Exception ex)
			{
				string str = "Root level exception in Update(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0004C2E0 File Offset: 0x0004A4E0
		public void OnGUI()
		{
			try
			{
				if (!this.destroyed)
				{
					GUI.depth = 50;
					UI.ApplyUIScale();
					LongEventHandler.LongEventsOnGUI();
					if (LongEventHandler.ShouldWaitForEvent)
					{
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
					}
					else
					{
						this.uiRoot.UIRootOnGUI();
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
						if (Find.CameraDriver != null && Find.CameraDriver.isActiveAndEnabled)
						{
							Find.CameraDriver.CameraDriverOnGUI();
						}
						if (Find.WorldCameraDriver != null && Find.WorldCameraDriver.isActiveAndEnabled)
						{
							Find.WorldCameraDriver.WorldCameraDriverOnGUI();
						}
					}
				}
			}
			catch (Exception ex)
			{
				string str = "Root level exception in OnGUI(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0004C3C8 File Offset: 0x0004A5C8
		public static void Shutdown()
		{
			try
			{
				SteamManager.ShutdownSteam();
			}
			catch (Exception ex)
			{
				string str = "Error in ShutdownSteam(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			Application.Quit();
		}

		// Token: 0x04000E77 RID: 3703
		private static bool globalInitDone;

		// Token: 0x04000E78 RID: 3704
		private static bool prefsApplied;

		// Token: 0x04000E79 RID: 3705
		protected static bool checkedAutostartSaveFile;

		// Token: 0x04000E7A RID: 3706
		protected bool destroyed;

		// Token: 0x04000E7B RID: 3707
		private static readonly SimpleMovingAverage rawAverageDeltaTime = new SimpleMovingAverage(200);

		// Token: 0x04000E7C RID: 3708
		private static readonly ExponentialMovingAverage smoothedAverageDeltaTime = new ExponentialMovingAverage(0.1f);

		// Token: 0x04000E7D RID: 3709
		public SoundRoot soundRoot;

		// Token: 0x04000E7E RID: 3710
		public UIRoot uiRoot;
	}
}
