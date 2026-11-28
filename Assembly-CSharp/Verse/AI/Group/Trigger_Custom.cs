using System;

namespace Verse.AI.Group
{
	// Token: 0x02000968 RID: 2408
	public class Trigger_Custom : Trigger
	{
		// Token: 0x06004976 RID: 18806 RVA: 0x001CA79D File Offset: 0x001C899D
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x001CA7AC File Offset: 0x001C89AC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}

		// Token: 0x04002F2A RID: 12074
		private Func<TriggerSignal, bool> condition;
	}
}
