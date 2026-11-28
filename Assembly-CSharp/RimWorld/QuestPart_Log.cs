using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001093 RID: 4243
	public class QuestPart_Log : QuestPart
	{
		// Token: 0x06006A4E RID: 27214 RVA: 0x00256459 File Offset: 0x00254659
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Log.Message(signal.args.GetFormattedText(this.message));
			}
		}

		// Token: 0x06006A4F RID: 27215 RVA: 0x00256496 File Offset: 0x00254696
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
		}

		// Token: 0x06006A50 RID: 27216 RVA: 0x002564C4 File Offset: 0x002546C4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.message = "DEV: Test";
		}

		// Token: 0x04004381 RID: 17281
		public string inSignal;

		// Token: 0x04004382 RID: 17282
		public string message;
	}
}
