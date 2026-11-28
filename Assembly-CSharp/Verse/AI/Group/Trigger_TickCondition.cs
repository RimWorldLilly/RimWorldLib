using System;

namespace Verse.AI.Group
{
	// Token: 0x0200096A RID: 2410
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x0600497A RID: 18810 RVA: 0x001CA7D6 File Offset: 0x001C89D6
		private TriggerData_Reason Data
		{
			get
			{
				return (TriggerData_Reason)this.data;
			}
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x001CA7E3 File Offset: 0x001C89E3
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x001CA800 File Offset: 0x001C8A00
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0 && this.condition();
		}

		// Token: 0x04002F2C RID: 12076
		private Func<bool> condition;

		// Token: 0x04002F2D RID: 12077
		private int checkEveryTicks = 1;
	}
}
