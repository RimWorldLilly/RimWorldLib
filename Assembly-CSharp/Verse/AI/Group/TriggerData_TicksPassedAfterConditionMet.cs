using System;

namespace Verse.AI.Group
{
	// Token: 0x02000995 RID: 2453
	public class TriggerData_TicksPassedAfterConditionMet : TriggerData_TicksPassed
	{
		// Token: 0x060049DC RID: 18908 RVA: 0x001CBA73 File Offset: 0x001C9C73
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.conditionMet, "conditionMet", false, false);
		}

		// Token: 0x04002F59 RID: 12121
		public bool conditionMet;
	}
}
