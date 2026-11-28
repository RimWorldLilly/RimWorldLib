using System;

namespace Verse.AI.Group
{
	// Token: 0x02000990 RID: 2448
	public class TriggerData_TicksPassedRitual : TriggerData
	{
		// Token: 0x060049D1 RID: 18897 RVA: 0x001CB921 File Offset: 0x001C9B21
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.ticksPassed, "ticksPassed", 0f, false);
		}

		// Token: 0x04002F54 RID: 12116
		public float ticksPassed;
	}
}
