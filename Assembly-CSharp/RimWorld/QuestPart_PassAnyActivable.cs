using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001051 RID: 4177
	public class QuestPart_PassAnyActivable : QuestPartActivable
	{
		// Token: 0x0600692B RID: 26923 RVA: 0x002521A8 File Offset: 0x002503A8
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				base.Complete();
			}
		}

		// Token: 0x0600692C RID: 26924 RVA: 0x002521CA File Offset: 0x002503CA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0600692D RID: 26925 RVA: 0x002521E8 File Offset: 0x002503E8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x040042F0 RID: 17136
		public List<string> inSignals = new List<string>();
	}
}
