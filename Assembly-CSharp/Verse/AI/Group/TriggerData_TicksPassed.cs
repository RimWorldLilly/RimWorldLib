using System;

namespace Verse.AI.Group
{
	// Token: 0x0200098E RID: 2446
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x060049CB RID: 18891 RVA: 0x001CB85C File Offset: 0x001C9A5C
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x04002F51 RID: 12113
		public int ticksPassed;
	}
}
