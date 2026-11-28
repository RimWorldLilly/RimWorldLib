using System;

namespace Verse.AI.Group
{
	// Token: 0x02000970 RID: 2416
	public class Trigger_ChanceOnTickInterval : Trigger
	{
		// Token: 0x06004987 RID: 18823 RVA: 0x001CA933 File Offset: 0x001C8B33
		public Trigger_ChanceOnTickInterval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x001CA949 File Offset: 0x001C8B49
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}

		// Token: 0x04002F34 RID: 12084
		private float chancePerInterval;

		// Token: 0x04002F35 RID: 12085
		private int interval;
	}
}
