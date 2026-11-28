using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110C RID: 4364
	public abstract class QuestPartCanDisable : QuestPart
	{
		// Token: 0x06006D0D RID: 27917 RVA: 0x00262C54 File Offset: 0x00260E54
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.state != QuestPartState.Enabled)
			{
				return;
			}
			if (signal.tag == this.inSignalDisable)
			{
				this.Disable();
				return;
			}
			this.ProcessQuestSignal(signal);
		}

		// Token: 0x06006D0E RID: 27918 RVA: 0x00262C81 File Offset: 0x00260E81
		protected virtual void ProcessQuestSignal(Signal signal)
		{
		}

		// Token: 0x06006D0F RID: 27919 RVA: 0x00262C83 File Offset: 0x00260E83
		public void Disable()
		{
			this.state = QuestPartState.Disabled;
		}

		// Token: 0x06006D10 RID: 27920 RVA: 0x00262C8C File Offset: 0x00260E8C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalDisable, "inSignalDisable", null, false);
			Scribe_Values.Look<QuestPartState>(ref this.state, "state", QuestPartState.NeverEnabled, false);
		}

		// Token: 0x04004557 RID: 17751
		public string inSignalDisable;

		// Token: 0x04004558 RID: 17752
		public QuestPartState state = QuestPartState.Enabled;
	}
}
