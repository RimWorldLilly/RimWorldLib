using System;
using System.Linq;
using System.Text;

namespace Verse.Sound
{
	// Token: 0x020007E7 RID: 2023
	public static class DebugSoundEventsLog
	{
		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x0600400B RID: 16395 RVA: 0x001988FC File Offset: 0x00196AFC
		public static string EventsListingDebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LogMessage logMessage in DebugSoundEventsLog.queue.Messages.Reverse<LogMessage>())
				{
					stringBuilder.AppendLine(logMessage.ToString());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x00198964 File Offset: 0x00196B64
		public static void Notify_SoundEvent(SoundDef def, SoundInfo info)
		{
			if (!DebugViewSettings.writeSoundEventsRecord)
			{
				return;
			}
			string str;
			if (def == null)
			{
				str = "null: ";
			}
			else if (def.isUndefined)
			{
				str = "Undefined: ";
			}
			else
			{
				str = (def.sustain ? "SustainerSpawn: " : "OneShot: ");
			}
			string str2 = (def != null) ? def.defName : "null";
			DebugSoundEventsLog.CreateRecord(str + str2 + " - " + info.ToString());
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x001989D8 File Offset: 0x00196BD8
		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			DebugSoundEventsLog.CreateRecord("SustainerEnd: " + sustainer.def.defName + " - " + info.ToString());
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x00198A06 File Offset: 0x00196C06
		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}

		// Token: 0x04002A98 RID: 10904
		private static LogMessageQueue queue = new LogMessageQueue();
	}
}
