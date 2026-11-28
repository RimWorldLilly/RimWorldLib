using System;

namespace Verse.AI.Group
{
	// Token: 0x02000991 RID: 2449
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x060049D3 RID: 18899 RVA: 0x001CB941 File Offset: 0x001C9B41
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x001CB94A File Offset: 0x001C9B4A
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return base.ActivateOn(lord, signal) && Find.TickManager.TicksGame - lord.lastPawnHarmTick >= 300;
		}

		// Token: 0x04002F55 RID: 12117
		private const int MinTicksSinceDamage = 300;
	}
}
