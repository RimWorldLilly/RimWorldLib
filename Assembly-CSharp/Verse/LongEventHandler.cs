using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RimWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000098 RID: 152
	public static class LongEventHandler
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00026515 File Offset: 0x00024715
		public static bool ShouldWaitForEvent
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting && ((LongEventHandler.currentEvent != null && !LongEventHandler.currentEvent.UseStandardWindow) || (Find.UIRoot == null || Find.WindowStack == null));
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x00026546 File Offset: 0x00024746
		public static bool AnyEventNowOrWaiting
		{
			get
			{
				return LongEventHandler.currentEvent != null || LongEventHandler.eventQueue.Count > 0;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x00026560 File Offset: 0x00024760
		public static bool AnyEventWhichDoesntUseStandardWindowNowOrWaiting
		{
			get
			{
				LongEventHandler.QueuedLongEvent queuedLongEvent = LongEventHandler.currentEvent;
				if (queuedLongEvent != null && !queuedLongEvent.UseStandardWindow)
				{
					return true;
				}
				return LongEventHandler.eventQueue.Any((LongEventHandler.QueuedLongEvent x) => !x.UseStandardWindow);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000265A9 File Offset: 0x000247A9
		public static bool ForcePause
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting;
			}
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000265B0 File Offset: 0x000247B0
		public static void QueueLongEvent(Action action, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true, bool forceHideUI = false, Action callback = null)
		{
			LongEventHandler.QueuedLongEvent item = new LongEventHandler.QueuedLongEvent
			{
				eventAction = action,
				eventTextKey = textKey,
				doAsynchronously = doAsynchronously,
				exceptionHandler = exceptionHandler,
				canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting,
				showExtraUIInfo = showExtraUIInfo,
				callback = callback,
				forceHideUI = forceHideUI
			};
			LongEventHandler.eventQueue.Enqueue(item);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00026610 File Offset: 0x00024810
		public static void QueueLongEvent(IEnumerable action, string textKey, Action<Exception> exceptionHandler = null, bool showExtraUIInfo = true, bool forceHideUI = false)
		{
			LongEventHandler.QueuedLongEvent item = new LongEventHandler.QueuedLongEvent
			{
				eventActionEnumerator = action.GetEnumerator(),
				eventTextKey = textKey,
				doAsynchronously = false,
				exceptionHandler = exceptionHandler,
				canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting,
				showExtraUIInfo = showExtraUIInfo,
				forceHideUI = forceHideUI
			};
			LongEventHandler.eventQueue.Enqueue(item);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0002666C File Offset: 0x0002486C
		public static void QueueLongEvent(Action preLoadLevelAction, string levelToLoad, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true, bool forceHideUI = false)
		{
			LongEventHandler.QueuedLongEvent item = new LongEventHandler.QueuedLongEvent
			{
				eventAction = preLoadLevelAction,
				levelToLoad = levelToLoad,
				eventTextKey = textKey,
				doAsynchronously = doAsynchronously,
				exceptionHandler = exceptionHandler,
				canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting,
				showExtraUIInfo = showExtraUIInfo,
				forceHideUI = forceHideUI
			};
			LongEventHandler.eventQueue.Enqueue(item);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000266CC File Offset: 0x000248CC
		public static void ClearQueuedEvents()
		{
			LongEventHandler.eventQueue.Clear();
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000266D8 File Offset: 0x000248D8
		public static void LongEventsOnGUI()
		{
			if (LongEventHandler.currentEvent == null)
			{
				GameplayTipWindow.ResetTipTimer();
				return;
			}
			if (LongEventHandler.currentEvent.forceHideUI)
			{
				return;
			}
			float num = LongEventHandler.StatusRectSize.x;
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				Text.Font = GameFont.Small;
				num = Mathf.Max(num, Text.CalcSize(LongEventHandler.currentEvent.eventText + "...").x + 40f);
			}
			bool flag2 = Find.UIRoot != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			bool flag3 = Find.UIRoot != null && Current.Game != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			Vector2 vector = flag3 ? ModSummaryWindow.GetEffectiveSize() : Vector2.zero;
			float num2 = LongEventHandler.StatusRectSize.y;
			if (flag3)
			{
				num2 += 17f + vector.y;
			}
			if (flag2)
			{
				num2 += 17f + GameplayTipWindow.WindowSize.y;
			}
			float num3 = ((float)UI.screenHeight - num2) / 2f;
			Vector2 vector2 = new Vector2(((float)UI.screenWidth - GameplayTipWindow.WindowSize.x) / 2f, num3 + LongEventHandler.StatusRectSize.y + 17f);
			Vector2 offset = new Vector2(((float)UI.screenWidth - vector.x) / 2f, vector2.y + GameplayTipWindow.WindowSize.y + 17f);
			Rect rect = new Rect(((float)UI.screenWidth - num) / 2f, num3, num, LongEventHandler.StatusRectSize.y);
			rect = rect.Rounded();
			if (!LongEventHandler.currentEvent.UseStandardWindow || Find.UIRoot == null || Find.WindowStack == null)
			{
				if (UIMenuBackgroundManager.background == null)
				{
					UIMenuBackgroundManager.background = new UI_BackgroundMain();
				}
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				Widgets.DrawShadowAround(rect);
				Widgets.DrawWindowBackground(rect);
				LongEventHandler.DrawLongEventWindowContents(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, false);
				}
				if (flag3)
				{
					ModSummaryWindow.DrawWindow(offset, false);
					TooltipHandler.DoTooltipGUI();
					return;
				}
			}
			else
			{
				LongEventHandler.DrawLongEventWindow(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, true);
				}
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00026914 File Offset: 0x00024B14
		private static void DrawLongEventWindow(Rect statusRect)
		{
			Find.WindowStack.ImmediateWindow(62893994, statusRect, WindowLayer.Super, delegate
			{
				LongEventHandler.DrawLongEventWindowContents(statusRect.AtZero());
			}, true, false, 1f, null, true);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0002695C File Offset: 0x00024B5C
		public static void LongEventsUpdate(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent != null)
			{
				if (LongEventHandler.currentEvent.eventActionEnumerator != null)
				{
					LongEventHandler.UpdateCurrentEnumeratorEvent();
				}
				else if (LongEventHandler.currentEvent.doAsynchronously)
				{
					LongEventHandler.UpdateCurrentAsynchronousEvent();
				}
				else
				{
					LongEventHandler.UpdateCurrentSynchronousEvent(out sceneChanged);
				}
			}
			if (LongEventHandler.currentEvent == null && LongEventHandler.eventQueue.Count > 0)
			{
				LongEventHandler.currentEvent = LongEventHandler.eventQueue.Dequeue();
				if (LongEventHandler.currentEvent.eventTextKey == null)
				{
					LongEventHandler.currentEvent.eventText = "";
					return;
				}
				LongEventHandler.currentEvent.eventText = LongEventHandler.currentEvent.eventTextKey.Translate();
			}
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000269FC File Offset: 0x00024BFC
		public static void ExecuteWhenFinished(Action action)
		{
			LongEventHandler.toExecuteWhenFinished.Add(action);
			if ((LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.ShouldWaitUntilDisplayed) && !LongEventHandler.executingToExecuteWhenFinished)
			{
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00026A28 File Offset: 0x00024C28
		public static void ForceExecuteToExecuteWhenFinished()
		{
			LongEventHandler.ExecuteToExecuteWhenFinished();
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00026A30 File Offset: 0x00024C30
		public static void SetCurrentEventText(string newText)
		{
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				if (LongEventHandler.currentEvent != null)
				{
					LongEventHandler.currentEvent.eventText = newText;
				}
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00026A80 File Offset: 0x00024C80
		private static void UpdateCurrentEnumeratorEvent()
		{
			try
			{
				float num = Time.realtimeSinceStartup + 0.1f;
				while (LongEventHandler.currentEvent.eventActionEnumerator.MoveNext())
				{
					if (num <= Time.realtimeSinceStartup)
					{
						return;
					}
				}
				IDisposable disposable = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				Action callback = LongEventHandler.currentEvent.callback;
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
				if (callback != null)
				{
					callback();
				}
			}
			catch (Exception ex)
			{
				string str = "Exception from long event: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				if (LongEventHandler.currentEvent != null)
				{
					IDisposable disposable2 = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
					Action<Exception> exceptionHandler = LongEventHandler.currentEvent.exceptionHandler;
					if (exceptionHandler != null)
					{
						exceptionHandler(ex);
					}
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00026B74 File Offset: 0x00024D74
		private static void UpdateCurrentAsynchronousEvent()
		{
			if (LongEventHandler.eventThread == null)
			{
				LongEventHandler.eventThread = new Thread(delegate()
				{
					LongEventHandler.RunEventFromAnotherThread(LongEventHandler.currentEvent.eventAction);
				});
				LongEventHandler.eventThread.Start();
				return;
			}
			if (!LongEventHandler.eventThread.IsAlive)
			{
				bool flag = false;
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					if (LongEventHandler.levelLoadOp == null)
					{
						LongEventHandler.levelLoadOp = SceneManager.LoadSceneAsync(LongEventHandler.currentEvent.levelToLoad);
					}
					else if (LongEventHandler.levelLoadOp.isDone)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					LongEventHandler.ExecuteToExecuteWhenFinished();
					Action callback = LongEventHandler.currentEvent.callback;
					if (callback != null)
					{
						callback();
					}
					LongEventHandler.currentEvent = null;
					LongEventHandler.eventThread = null;
					LongEventHandler.levelLoadOp = null;
				}
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00026C3C File Offset: 0x00024E3C
		private static void UpdateCurrentSynchronousEvent(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent.ShouldWaitUntilDisplayed)
			{
				return;
			}
			try
			{
				if (LongEventHandler.currentEvent.eventAction != null)
				{
					LongEventHandler.currentEvent.eventAction();
				}
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					SceneManager.LoadScene(LongEventHandler.currentEvent.levelToLoad);
					sceneChanged = true;
				}
				Action callback = LongEventHandler.currentEvent.callback;
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
				if (callback != null)
				{
					callback();
				}
			}
			catch (Exception ex)
			{
				string str = "Exception from long event: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
				{
					LongEventHandler.currentEvent.exceptionHandler(ex);
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00026D2C File Offset: 0x00024F2C
		private static void RunEventFromAnotherThread(Action action)
		{
			CultureInfoUtility.EnsureEnglish();
			try
			{
				if (action != null)
				{
					action();
				}
			}
			catch (Exception ex)
			{
				string str = "Exception from asynchronous event: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				try
				{
					if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				catch (Exception ex3)
				{
					string str2 = "Exception was thrown while trying to handle exception. Exception: ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00026DC8 File Offset: 0x00024FC8
		private static void ExecuteToExecuteWhenFinished()
		{
			if (LongEventHandler.executingToExecuteWhenFinished)
			{
				Log.Warning("Already executing.");
				return;
			}
			LongEventHandler.executingToExecuteWhenFinished = true;
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.Start("ExecuteToExecuteWhenFinished()");
			}
			for (int i = 0; i < LongEventHandler.toExecuteWhenFinished.Count; i++)
			{
				DeepProfiler.Start(LongEventHandler.toExecuteWhenFinished[i].Method.DeclaringType.ToString() + " -> " + LongEventHandler.toExecuteWhenFinished[i].Method.ToString());
				try
				{
					LongEventHandler.toExecuteWhenFinished[i]();
				}
				catch (Exception ex)
				{
					string str = "Could not execute post-long-event action. Exception: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.End();
			}
			LongEventHandler.toExecuteWhenFinished.Clear();
			LongEventHandler.executingToExecuteWhenFinished = false;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00026ED0 File Offset: 0x000250D0
		private static void DrawLongEventWindowContents(Rect rect)
		{
			if (LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.forceHideUI)
			{
				return;
			}
			if (Event.current.type == EventType.repaint)
			{
				LongEventHandler.currentEvent.alreadyDisplayed = true;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			float num = 0f;
			if (LongEventHandler.levelLoadOp != null)
			{
				float f = 1f;
				if (!LongEventHandler.levelLoadOp.isDone)
				{
					f = LongEventHandler.levelLoadOp.progress;
				}
				TaggedString taggedString = "LoadingAssets".Translate() + " " + f.ToStringPercent();
				num = Text.CalcSize(taggedString).x;
				Widgets.Label(rect, taggedString);
			}
			else
			{
				object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
				lock (currentEventTextLock)
				{
					num = Text.CalcSize(LongEventHandler.currentEvent.eventText).x;
					Widgets.Label(rect, LongEventHandler.currentEvent.eventText);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			rect.xMin = rect.center.x + num / 2f;
			Widgets.Label(rect, (!LongEventHandler.currentEvent.UseAnimatedDots) ? "..." : GenText.MarchingEllipsis(0f));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0400024B RID: 587
		private static Queue<LongEventHandler.QueuedLongEvent> eventQueue = new Queue<LongEventHandler.QueuedLongEvent>();

		// Token: 0x0400024C RID: 588
		internal static LongEventHandler.QueuedLongEvent currentEvent = null;

		// Token: 0x0400024D RID: 589
		private static Thread eventThread = null;

		// Token: 0x0400024E RID: 590
		private static AsyncOperation levelLoadOp = null;

		// Token: 0x0400024F RID: 591
		private static List<Action> toExecuteWhenFinished = new List<Action>();

		// Token: 0x04000250 RID: 592
		private static bool executingToExecuteWhenFinished = false;

		// Token: 0x04000251 RID: 593
		private static readonly object CurrentEventTextLock = new object();

		// Token: 0x04000252 RID: 594
		private static readonly Vector2 StatusRectSize = new Vector2(240f, 75f);

		// Token: 0x020024D3 RID: 9427
		internal class QueuedLongEvent
		{
			// Token: 0x170029B0 RID: 10672
			// (get) Token: 0x0600EBA0 RID: 60320 RVA: 0x005707AC File Offset: 0x0056E9AC
			public bool UseAnimatedDots
			{
				get
				{
					return this.doAsynchronously || this.eventActionEnumerator != null;
				}
			}

			// Token: 0x170029B1 RID: 10673
			// (get) Token: 0x0600EBA1 RID: 60321 RVA: 0x005707C1 File Offset: 0x0056E9C1
			public bool ShouldWaitUntilDisplayed
			{
				get
				{
					return !this.alreadyDisplayed && this.UseStandardWindow && !this.eventText.NullOrEmpty();
				}
			}

			// Token: 0x170029B2 RID: 10674
			// (get) Token: 0x0600EBA2 RID: 60322 RVA: 0x005707E3 File Offset: 0x0056E9E3
			public bool UseStandardWindow
			{
				get
				{
					return this.canEverUseStandardWindow && !this.doAsynchronously && this.eventActionEnumerator == null;
				}
			}

			// Token: 0x040096B6 RID: 38582
			public Action eventAction;

			// Token: 0x040096B7 RID: 38583
			public Action callback;

			// Token: 0x040096B8 RID: 38584
			public IEnumerator eventActionEnumerator;

			// Token: 0x040096B9 RID: 38585
			public string levelToLoad;

			// Token: 0x040096BA RID: 38586
			public string eventTextKey = "";

			// Token: 0x040096BB RID: 38587
			public string eventText = "";

			// Token: 0x040096BC RID: 38588
			public bool doAsynchronously;

			// Token: 0x040096BD RID: 38589
			public Action<Exception> exceptionHandler;

			// Token: 0x040096BE RID: 38590
			public bool alreadyDisplayed;

			// Token: 0x040096BF RID: 38591
			public bool canEverUseStandardWindow = true;

			// Token: 0x040096C0 RID: 38592
			public bool showExtraUIInfo = true;

			// Token: 0x040096C1 RID: 38593
			public bool forceHideUI;
		}
	}
}
