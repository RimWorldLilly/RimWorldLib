using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001114 RID: 4372
	public class QuestPart_GiveTechprints : QuestPart
	{
		// Token: 0x06006D57 RID: 27991 RVA: 0x00264DAC File Offset: 0x00262FAC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.amount; i++)
				{
					Find.ResearchManager.ApplyTechprint(this.project, null);
				}
			}
		}

		// Token: 0x06006D58 RID: 27992 RVA: 0x00264DF8 File Offset: 0x00262FF8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalWasGiven, "outSignalWasGiven", null, false);
		}

		// Token: 0x040045A2 RID: 17826
		public const string WasGivenSignal = "AddedTechprints";

		// Token: 0x040045A3 RID: 17827
		public string inSignal;

		// Token: 0x040045A4 RID: 17828
		public string outSignalWasGiven;

		// Token: 0x040045A5 RID: 17829
		public ResearchProjectDef project;

		// Token: 0x040045A6 RID: 17830
		public int amount;
	}
}
