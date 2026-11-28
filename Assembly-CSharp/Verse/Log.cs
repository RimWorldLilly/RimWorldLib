using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Brutal.Logging;
using LudeonTK;
using UnityEngine;
using Waylith.UnityCompat;

namespace Verse
{
	// Token: 0x02000094 RID: 148
	public static class Log
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00025D64 File Offset: 0x00023F64
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				if (Log.IsMainThread)
				{
					object obj = Log.logLock;
					lock (obj)
					{
						Log.DrainOffThreadMessages();
					}
				}
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x00025DB4 File Offset: 0x00023FB4
		private static bool IsMainThread
		{
			get
			{
				UnityRuntime unityRuntime = UnityRuntime.Current;
				int? num = (unityRuntime != null) ? new int?(unityRuntime.App.MainThreadId) : null;
				int currentManagedThreadId = Environment.CurrentManagedThreadId;
				return num.GetValueOrDefault() == currentManagedThreadId & num != null;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00025DFD File Offset: 0x00023FFD
		private static bool PreventLogging
		{
			get
			{
				return Log.reachedMaxMessagesLimit || Log.logDisablers > 0;
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00025E10 File Offset: 0x00024010
		public static Log.LogLock LockMessages()
		{
			return new Log.LogLock();
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00025E18 File Offset: 0x00024018
		static Log()
		{
			Application.logMessageReceivedThreaded += Log.Notify_LogMessageReceived;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00025E74 File Offset: 0x00024074
		public static void ResetMessageCount()
		{
			object obj = Log.logLock;
			lock (obj)
			{
				Log.messageCount = 0;
				if (Log.reachedMaxMessagesLimit)
				{
					Debug.unityLogger.logEnabled = true;
					Log.reachedMaxMessagesLimit = false;
					Log.Message("Message logging is now once again on.");
				}
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00025ED8 File Offset: 0x000240D8
		public static void Message(string text)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					Log.logCategory.Info(text, "Message", "D:\\Workspaces\\rimworld-builds\\Assets\\Scripts\\LudeonTK\\Utility\\Log\\Log.cs", 91);
					Log.PostMessage();
				}
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00025F3C File Offset: 0x0002413C
		public static void Message(object obj)
		{
			Log.Message(obj.ToString());
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00025F4C File Offset: 0x0002414C
		public static void Warning(string text)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					Log.logCategory.Warning(text, "Warning", "D:\\Workspaces\\rimworld-builds\\Assets\\Scripts\\LudeonTK\\Utility\\Log\\Log.cs", 108);
					Log.PostMessage();
					if (Prefs.OpenLogOnWarnings)
					{
						Log.TryOpenLogWindow();
					}
				}
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00025FBC File Offset: 0x000241BC
		public static void WarningOnce(string text, int key)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					if (Log.usedKeys.Add(key))
					{
						Log.Warning(text);
					}
				}
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00026014 File Offset: 0x00024214
		public static void Error(string text)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					Log.logCategory.Error(text, "Error", "D:\\Workspaces\\rimworld-builds\\Assets\\Scripts\\LudeonTK\\Utility\\Log\\Log.cs", 137);
					if (!Log.currentlyLoggingError)
					{
						Log.currentlyLoggingError = true;
						try
						{
							if (DebugSettings.pauseOnError && Current.ProgramState == ProgramState.Playing)
							{
								Find.TickManager.Pause();
							}
							Log.PostMessage();
						}
						catch (Exception ex)
						{
							LogCategory logCategory = Log.logCategory;
							string str = "An error occurred while logging an error: ";
							Exception ex2 = ex;
							logCategory.Error(str + ((ex2 != null) ? ex2.ToString() : null), "Error", "D:\\Workspaces\\rimworld-builds\\Assets\\Scripts\\LudeonTK\\Utility\\Log\\Log.cs", 151);
						}
						finally
						{
							Log.currentlyLoggingError = false;
						}
					}
				}
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x000260FC File Offset: 0x000242FC
		public static void ErrorOnce(string text, int key)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					if (Log.usedKeys.Add(key))
					{
						Log.Error(text);
					}
				}
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00026154 File Offset: 0x00024354
		public static void Clear()
		{
			object obj = Log.logLock;
			lock (obj)
			{
				EditWindow_Log.ClearSelectedMessage();
				Log.messageQueue.Clear();
				Log.offThreadMessages.Clear();
				Log.ResetMessageCount();
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000261AC File Offset: 0x000243AC
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000261C1 File Offset: 0x000243C1
		private static void PostMessage()
		{
			if (Log.openOnMessage)
			{
				Log.TryOpenLogWindow();
				EditWindow_Log.SelectLastMessage(true);
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000261D8 File Offset: 0x000243D8
		private static void Notify_LogMessageReceived(string text, string stackTrace, LogType type)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					LogMessageType logMessageType;
					if (type != LogType.Warning)
					{
						if (type == LogType.Log)
						{
							logMessageType = LogMessageType.Message;
						}
						else
						{
							logMessageType = LogMessageType.Error;
						}
					}
					else
					{
						logMessageType = LogMessageType.Warning;
					}
					LogMessageType logMessageType2 = logMessageType;
					LogMessage logMessage = new LogMessage(logMessageType2, text, stackTrace);
					if (Log.IsMainThread)
					{
						Log.DrainOffThreadMessages();
						Log.messageQueue.Enqueue(logMessage);
					}
					else
					{
						Log.offThreadMessages.Enqueue(logMessage);
					}
					if (logMessageType2 == LogMessageType.Error && (!PlayDataLoader.Loaded || Prefs.DevMode))
					{
						Log.TryOpenLogWindow();
					}
				}
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00026270 File Offset: 0x00024470
		private static void DrainOffThreadMessages()
		{
			LogMessage msg;
			while (Log.offThreadMessages.TryDequeue(out msg))
			{
				Log.messageQueue.Enqueue(msg);
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00026298 File Offset: 0x00024498
		public static void Notify_MessageReceivedThreadedInternal(string msg, string stackTrace, LogType type)
		{
			if (++Log.messageCount == 10000)
			{
				Log.Warning("Reached max messages limit. Stopping logging to avoid spam.");
				Log.Notify_LogMessageReceived("Reached max messages limit. Stopping logging to avoid spam.", StackTraceUtility.ExtractStackTrace(), LogType.Warning);
				Log.reachedMaxMessagesLimit = true;
				Debug.unityLogger.logEnabled = false;
			}
		}

		// Token: 0x04000234 RID: 564
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x04000235 RID: 565
		private static ConcurrentQueue<LogMessage> offThreadMessages = new ConcurrentQueue<LogMessage>();

		// Token: 0x04000236 RID: 566
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x04000237 RID: 567
		public static bool openOnMessage = false;

		// Token: 0x04000238 RID: 568
		private static bool currentlyLoggingError;

		// Token: 0x04000239 RID: 569
		private static int messageCount;

		// Token: 0x0400023A RID: 570
		private static bool reachedMaxMessagesLimit;

		// Token: 0x0400023B RID: 571
		private static object logLock = new object();

		// Token: 0x0400023C RID: 572
		private static int logDisablers;

		// Token: 0x0400023D RID: 573
		private static readonly LogCategory logCategory = LogCategory.Make("Verse");

		// Token: 0x0400023E RID: 574
		private const int StopLoggingAtMessageCount = 10000;

		// Token: 0x020024D2 RID: 9426
		public class LogLock : IDisposable
		{
			// Token: 0x0600EB9E RID: 60318 RVA: 0x00570784 File Offset: 0x0056E984
			public LogLock()
			{
				Log.logDisablers = Interlocked.Increment(ref Log.logDisablers);
			}

			// Token: 0x0600EB9F RID: 60319 RVA: 0x0057079B File Offset: 0x0056E99B
			public void Dispose()
			{
				Log.logDisablers = Interlocked.Decrement(ref Log.logDisablers);
			}
		}
	}
}
