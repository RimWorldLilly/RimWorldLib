using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002158 RID: 8536
	public static class QuestGen_Debug
	{
		// Token: 0x0600D585 RID: 54661 RVA: 0x004F3680 File Offset: 0x004F1880
		public static void Log(this Quest quest, string message, string inSignal = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			quest.AddPart(new QuestPart_Log
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				signalListenMode = signalListenMode,
				message = message
			});
		}
	}
}
