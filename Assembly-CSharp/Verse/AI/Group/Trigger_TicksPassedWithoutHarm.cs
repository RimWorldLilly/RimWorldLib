using System;

namespace Verse.AI.Group
{
	// Token: 0x02000992 RID: 2450
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x060049D5 RID: 18901 RVA: 0x001CB971 File Offset: 0x001C9B71
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x001CB97A File Offset: 0x001C9B7A
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				base.Data.ticksPassed = 0;
			}
			return base.ActivateOn(lord, signal);
		}
	}
}
