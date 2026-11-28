using System;

namespace Verse.AI.Group
{
	// Token: 0x02000973 RID: 2419
	public class Trigger_DormancyWakeupOrClamor : Trigger
	{
		// Token: 0x0600498E RID: 18830 RVA: 0x001CA9FF File Offset: 0x001C8BFF
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.DormancyWakeup || signal.type == TriggerSignalType.Clamor;
		}
	}
}
