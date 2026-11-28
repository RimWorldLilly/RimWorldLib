using System;

namespace Verse.AI.Group
{
	// Token: 0x02000994 RID: 2452
	public class Trigger_TicksPassedAfterConditionMet : Trigger_TicksPassed
	{
		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x060049D9 RID: 18905 RVA: 0x001CB9DE File Offset: 0x001C9BDE
		protected new TriggerData_TicksPassedAfterConditionMet Data
		{
			get
			{
				return (TriggerData_TicksPassedAfterConditionMet)this.data;
			}
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x001CB9EB File Offset: 0x001C9BEB
		public Trigger_TicksPassedAfterConditionMet(int tickLimit, Func<bool> condition, int checkEveryTicks = 1) : base(tickLimit)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
			this.data = new TriggerData_TicksPassedAfterConditionMet();
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x001CBA10 File Offset: 0x001C9C10
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (!this.Data.conditionMet && signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0)
			{
				this.Data.conditionMet = this.condition();
			}
			return this.Data.conditionMet && base.ActivateOn(lord, signal);
		}

		// Token: 0x04002F57 RID: 12119
		private Func<bool> condition;

		// Token: 0x04002F58 RID: 12120
		private int checkEveryTicks;
	}
}
