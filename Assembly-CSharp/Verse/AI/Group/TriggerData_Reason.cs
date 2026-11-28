using System;

namespace Verse.AI.Group
{
	// Token: 0x02000969 RID: 2409
	public class TriggerData_Reason : TriggerData
	{
		// Token: 0x06004978 RID: 18808 RVA: 0x001CA7BA File Offset: 0x001C89BA
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
		}

		// Token: 0x04002F2B RID: 12075
		public string reason;
	}
}
