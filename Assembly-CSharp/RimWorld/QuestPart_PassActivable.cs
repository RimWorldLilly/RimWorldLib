using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104A RID: 4170
	public class QuestPart_PassActivable : QuestPartActivable
	{
		// Token: 0x0600690B RID: 26891 RVA: 0x0025195D File Offset: 0x0024FB5D
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (signal.tag == this.inSignal)
			{
				this.Complete(signal.args);
			}
		}

		// Token: 0x0600690C RID: 26892 RVA: 0x00251985 File Offset: 0x0024FB85
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x0600690D RID: 26893 RVA: 0x002519A0 File Offset: 0x0024FBA0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x040042E0 RID: 17120
		public string inSignal;
	}
}
