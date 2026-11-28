using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	// Token: 0x02000993 RID: 2451
	public class Trigger_TicksPassedWithoutHarmOrMemos : Trigger_TicksPassed
	{
		// Token: 0x060049D7 RID: 18903 RVA: 0x001CB998 File Offset: 0x001C9B98
		public Trigger_TicksPassedWithoutHarmOrMemos(int tickLimit, params string[] memos) : base(tickLimit)
		{
			this.memos = memos.ToList<string>();
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x001CB9AD File Offset: 0x001C9BAD
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal) || this.memos.Contains(signal.memo))
			{
				base.Data.ticksPassed = 0;
			}
			return base.ActivateOn(lord, signal);
		}

		// Token: 0x04002F56 RID: 12118
		private List<string> memos;
	}
}
