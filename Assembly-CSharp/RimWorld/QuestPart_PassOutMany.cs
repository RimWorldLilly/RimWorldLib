using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001054 RID: 4180
	public class QuestPart_PassOutMany : QuestPartCanDisable
	{
		// Token: 0x06006938 RID: 26936 RVA: 0x00252544 File Offset: 0x00250744
		protected override void ProcessQuestSignal(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.outSignals.Count; i++)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignals[i], signal.args, false));
				}
			}
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x0025259C File Offset: 0x0025079C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x002525CC File Offset: 0x002507CC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			for (int i = 0; i < 3; i++)
			{
				this.outSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x040042F7 RID: 17143
		public string inSignal;

		// Token: 0x040042F8 RID: 17144
		public List<string> outSignals = new List<string>();
	}
}
