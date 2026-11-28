using System;
using System.Collections.Generic;
using System.Diagnostics;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001FB RID: 507
	public sealed class TickManager : IExposable
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0004B399 File Offset: 0x00049599
		public int TicksGame
		{
			get
			{
				return this.ticksGameInt;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0004B3A1 File Offset: 0x000495A1
		public int TicksAbs
		{
			get
			{
				if (this.gameStartAbsTick == 0)
				{
					Log.ErrorOnce("Accessing TicksAbs but gameStartAbsTick is not set yet (you most likely want to use GenTicks.TicksAbs instead).", 1049580013);
					return this.ticksGameInt;
				}
				return this.ticksGameInt + this.gameStartAbsTick;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0004B3CE File Offset: 0x000495CE
		public int TicksSinceSettle
		{
			get
			{
				return this.ticksGameInt - this.lastSettleTicksInt;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0004B3DD File Offset: 0x000495DD
		public int SettleTick
		{
			get
			{
				return this.lastSettleTicksInt;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0004B3E5 File Offset: 0x000495E5
		public int StartingYear
		{
			get
			{
				return this.startingYearInt;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0004B3F0 File Offset: 0x000495F0
		public float TickRateMultiplier
		{
			get
			{
				if (this.slower.ForcedNormalSpeed)
				{
					if (this.curTimeSpeed == TimeSpeed.Paused)
					{
						return 0f;
					}
					return 1f;
				}
				else
				{
					switch (this.curTimeSpeed)
					{
					case TimeSpeed.Paused:
						return 0f;
					case TimeSpeed.Normal:
						return 1f;
					case TimeSpeed.Fast:
						return 3f;
					case TimeSpeed.Superfast:
						if (Find.Maps.Count == 0)
						{
							return 18f;
						}
						if (this.NothingHappeningInGame())
						{
							return 12f;
						}
						return 6f;
					case TimeSpeed.Ultrafast:
						if (TickManager.UltraSpeedBoost)
						{
							return 20000f;
						}
						if (Find.Maps.Count == 0)
						{
							return 150f;
						}
						return 15f;
					default:
						return -1f;
					}
				}
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0004B4A3 File Offset: 0x000496A3
		public int TicksThisFrame
		{
			get
			{
				return this.ticksThisFrame;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0004B4AB File Offset: 0x000496AB
		private float CurTimePerTick
		{
			get
			{
				if (this.TickRateMultiplier == 0f)
				{
					return 0f;
				}
				return 1f / (60f * this.TickRateMultiplier);
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0004B4D2 File Offset: 0x000496D2
		public bool Paused
		{
			get
			{
				return this.curTimeSpeed == TimeSpeed.Paused || this.ForcePaused;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x0004B4E4 File Offset: 0x000496E4
		public bool ForcePaused
		{
			get
			{
				if ((Find.WindowStack == null || !Find.WindowStack.WindowsForcePause) && !LongEventHandler.ForcePause && !Find.TilePicker.Active && !WorldComponent_GravshipController.CutsceneInProgress)
				{
					WorldComponent_GravshipController gravshipController = Find.GravshipController;
					if (gravshipController == null || !gravshipController.LandingAreaConfirmationInProgress)
					{
						return MapGenerator.debugMode;
					}
				}
				return true;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0004B538 File Offset: 0x00049738
		private AcceptanceReport PlayerCanControl
		{
			get
			{
				WorldComponent_GravshipController gravshipController = Find.GravshipController;
				if (gravshipController != null && gravshipController.LandingAreaConfirmationInProgress)
				{
					return "MessageConfirmLandingAreaFirst".Translate();
				}
				if (!Current.Game.PlayerHasControl)
				{
					return false;
				}
				return true;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0004B576 File Offset: 0x00049776
		public bool NotPlaying
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0004B58C File Offset: 0x0004978C
		// (set) Token: 0x06000E91 RID: 3729 RVA: 0x0004B594 File Offset: 0x00049794
		public TimeSpeed CurTimeSpeed
		{
			get
			{
				return this.curTimeSpeed;
			}
			set
			{
				if (!this.PlayerCanControl)
				{
					if (!this.PlayerCanControl.Reason.NullOrEmpty())
					{
						Messages.Message(this.PlayerCanControl.Reason, MessageTypeDefOf.RejectInput, false);
					}
					return;
				}
				this.curTimeSpeed = value;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0004B5E4 File Offset: 0x000497E4
		public bool HasSettledNewColony
		{
			get
			{
				return this.lastSettleTicksInt > 0;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x0004B5EF File Offset: 0x000497EF
		public float MeanTickTime
		{
			get
			{
				return this.smoothedTickTimeAverage.GetAverage();
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0004B5FC File Offset: 0x000497FC
		public void TogglePaused()
		{
			if (!this.PlayerCanControl)
			{
				if (!this.PlayerCanControl.Reason.NullOrEmpty())
				{
					Messages.Message(this.PlayerCanControl.Reason, MessageTypeDefOf.RejectInput, false);
				}
				return;
			}
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.prePauseTimeSpeed = this.curTimeSpeed;
				this.curTimeSpeed = TimeSpeed.Paused;
				return;
			}
			if (this.prePauseTimeSpeed != this.curTimeSpeed)
			{
				this.curTimeSpeed = this.prePauseTimeSpeed;
				return;
			}
			this.curTimeSpeed = TimeSpeed.Normal;
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0004B683 File Offset: 0x00049883
		public void Pause()
		{
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.TogglePaused();
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0004B694 File Offset: 0x00049894
		private bool NothingHappeningInGame()
		{
			if (this.lastNothingHappeningCheckTick != this.TicksGame)
			{
				this.nothingHappeningCached = true;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
					for (int j = 0; j < list.Count; j++)
					{
						Pawn pawn = list[j];
						if (pawn.HostFaction == null && pawn.RaceProps.Humanlike && pawn.Awake() && !pawn.IsGhoul)
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
					if (!this.nothingHappeningCached)
					{
						break;
					}
				}
				if (this.nothingHappeningCached)
				{
					for (int k = 0; k < maps.Count; k++)
					{
						if (maps[k].IsPlayerHome && maps[k].dangerWatcher.DangerRating >= StoryDanger.Low)
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
				}
				this.lastNothingHappeningCheckTick = this.TicksGame;
			}
			return this.nothingHappeningCached;
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0004B798 File Offset: 0x00049998
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksGameInt, "ticksGame", 0, false);
			Scribe_Values.Look<int>(ref this.gameStartAbsTick, "gameStartAbsTick", 0, false);
			Scribe_Values.Look<int>(ref this.startingYearInt, "startingYear", 0, false);
			Scribe_Values.Look<int>(ref this.lastSettleTicksInt, "lastSettleTicks", 0, false);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0004B7F0 File Offset: 0x000499F0
		public void RegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.RegisterThing(t);
			}
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0004B810 File Offset: 0x00049A10
		public void DeRegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.DeregisterThing(t);
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0004B830 File Offset: 0x00049A30
		private TickList TickListFor(Thing t)
		{
			if (t is IThingHolder)
			{
				return this.tickListNormal;
			}
			TickList result;
			switch (t.def.tickerType)
			{
			case TickerType.Never:
				result = null;
				break;
			case TickerType.Normal:
				result = this.tickListNormal;
				break;
			case TickerType.Rare:
				result = this.tickListRare;
				break;
			case TickerType.Long:
				result = this.tickListLong;
				break;
			default:
				throw new InvalidOperationException();
			}
			return result;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0004B898 File Offset: 0x00049A98
		public void TickManagerUpdate()
		{
			this.ticksThisFrame = 0;
			if (!this.Paused)
			{
				float curTimePerTick = this.CurTimePerTick;
				if (Mathf.Abs(Time.deltaTime - curTimePerTick) < curTimePerTick * 0.1f)
				{
					this.realTimeToTickThrough += curTimePerTick;
				}
				else
				{
					this.realTimeToTickThrough += Time.deltaTime;
				}
				float tickRateMultiplier = this.TickRateMultiplier;
				this.clock.Reset();
				this.clock.Start();
				while (this.realTimeToTickThrough > 0f && (float)this.ticksThisFrame < tickRateMultiplier * 2f)
				{
					double totalMilliseconds = this.clock.Elapsed.TotalMilliseconds;
					this.DoSingleTick();
					double totalMilliseconds2 = this.clock.Elapsed.TotalMilliseconds;
					this.realTimeToTickThrough -= curTimePerTick;
					this.ticksThisFrame++;
					this.rawTickTimeAverage.AddValue((float)(totalMilliseconds2 - totalMilliseconds));
					this.smoothedTickTimeAverage.AddValue(this.rawTickTimeAverage.GetAverage());
					if (this.Paused || (float)this.clock.ElapsedMilliseconds > 45.454544f)
					{
						break;
					}
				}
				if (this.realTimeToTickThrough > 0f)
				{
					this.realTimeToTickThrough = 0f;
				}
			}
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0004B9DC File Offset: 0x00049BDC
		public void DoSingleTick()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].MapPreTick();
			}
			if (!DebugSettings.fastEcology)
			{
				this.ticksGameInt++;
			}
			else
			{
				this.ticksGameInt += 2000;
			}
			Shader.SetGlobalFloat(ShaderPropertyIDs.GameSeconds, this.TicksGame.TicksToSeconds());
			this.tickListNormal.Tick();
			this.tickListRare.Tick();
			this.tickListLong.Tick();
			try
			{
				Find.DateNotifier.DateNotifierTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			try
			{
				Find.Scenario.TickScenario();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			try
			{
				Find.World.WorldTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
			try
			{
				Find.StoryWatcher.StoryWatcherTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString());
			}
			try
			{
				Find.GameEnder.GameEndTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString());
			}
			try
			{
				Find.Storyteller.StorytellerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString());
			}
			try
			{
				Find.TaleManager.TaleManagerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString());
			}
			try
			{
				Find.QuestManager.QuestManagerTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString());
			}
			try
			{
				Find.World.WorldPostTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString());
			}
			for (int j = 0; j < maps.Count; j++)
			{
				maps[j].MapPostTick();
			}
			try
			{
				Find.History.HistoryTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString());
			}
			GameComponentUtility.GameComponentTick();
			try
			{
				Find.LetterStack.LetterStackTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString());
			}
			try
			{
				Find.Autosaver.AutosaverTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString());
			}
			if (DebugViewSettings.logHourlyScreenshot && Find.TickManager.TicksGame >= this.lastAutoScreenshot + 2500)
			{
				ScreenshotTaker.QueueSilentScreenshot();
				this.lastAutoScreenshot = Find.TickManager.TicksGame / 2500 * 2500;
			}
			try
			{
				FilthMonitor.FilthMonitorTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString());
			}
			try
			{
				Find.TransportShipManager.ShipObjectsTick();
			}
			catch (Exception ex14)
			{
				Log.Error(ex14.ToString());
			}
			Debug.developerConsoleVisible = false;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0004BCD8 File Offset: 0x00049ED8
		public void RemoveAllFromMap(Map map)
		{
			this.tickListNormal.RemoveWhere((Thing x) => x.Map == map);
			this.tickListRare.RemoveWhere((Thing x) => x.Map == map);
			this.tickListLong.RemoveWhere((Thing x) => x.Map == map);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0004BD37 File Offset: 0x00049F37
		public void DebugSetTicksGame(int newTicksGame)
		{
			this.ticksGameInt = newTicksGame;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0004BD40 File Offset: 0x00049F40
		public void Notify_GeneratedPotentiallyHostileMap()
		{
			this.Pause();
			this.slower.SignalForceNormalSpeedShort();
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0004BD53 File Offset: 0x00049F53
		public void ResetSettlementTicks()
		{
			this.lastSettleTicksInt = this.TicksGame;
		}

		// Token: 0x04000E4F RID: 3663
		[TweakValue("Gameplay", 0f, 100f)]
		private static bool UltraSpeedBoost;

		// Token: 0x04000E50 RID: 3664
		private int ticksGameInt;

		// Token: 0x04000E51 RID: 3665
		public int gameStartAbsTick;

		// Token: 0x04000E52 RID: 3666
		private float realTimeToTickThrough;

		// Token: 0x04000E53 RID: 3667
		private TimeSpeed curTimeSpeed = TimeSpeed.Normal;

		// Token: 0x04000E54 RID: 3668
		public TimeSpeed prePauseTimeSpeed;

		// Token: 0x04000E55 RID: 3669
		private int startingYearInt = 5500;

		// Token: 0x04000E56 RID: 3670
		private Stopwatch clock = new Stopwatch();

		// Token: 0x04000E57 RID: 3671
		private int lastSettleTicksInt;

		// Token: 0x04000E58 RID: 3672
		private TickList tickListNormal = new TickList(TickerType.Normal);

		// Token: 0x04000E59 RID: 3673
		private TickList tickListRare = new TickList(TickerType.Rare);

		// Token: 0x04000E5A RID: 3674
		private TickList tickListLong = new TickList(TickerType.Long);

		// Token: 0x04000E5B RID: 3675
		public TimeSlower slower = new TimeSlower();

		// Token: 0x04000E5C RID: 3676
		private int lastAutoScreenshot;

		// Token: 0x04000E5D RID: 3677
		private int ticksThisFrame;

		// Token: 0x04000E5E RID: 3678
		private SimpleMovingAverage rawTickTimeAverage = new SimpleMovingAverage(720);

		// Token: 0x04000E5F RID: 3679
		private ExponentialMovingAverage smoothedTickTimeAverage = new ExponentialMovingAverage(0.1f);

		// Token: 0x04000E60 RID: 3680
		public const float WorstAllowedFPS = 22f;

		// Token: 0x04000E61 RID: 3681
		private int lastNothingHappeningCheckTick = -1;

		// Token: 0x04000E62 RID: 3682
		private bool nothingHappeningCached;
	}
}
