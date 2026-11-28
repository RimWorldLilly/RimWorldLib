using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001046 RID: 4166
	public class QuestPart_SetQuestNotYetAccepted : QuestPart
	{
		// Token: 0x060068EE RID: 26862 RVA: 0x0025132D File Offset: 0x0024F52D
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.quest.SetNotYetAccepted();
			}
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x00251354 File Offset: 0x0024F554
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.revertOngoingQuestAfterLoad, "revertOngoingQuestAfterLoad", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.revertOngoingQuestAfterLoad && this.quest.State == QuestState.Ongoing)
			{
				this.quest.SetNotYetAccepted();
			}
		}

		// Token: 0x060068F0 RID: 26864 RVA: 0x002513B4 File Offset: 0x0024F5B4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x040042CF RID: 17103
		public string inSignal;

		// Token: 0x040042D0 RID: 17104
		public bool revertOngoingQuestAfterLoad;
	}
}
