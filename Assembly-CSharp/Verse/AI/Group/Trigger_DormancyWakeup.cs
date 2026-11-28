using System;

namespace Verse.AI.Group
{
	// Token: 0x02000972 RID: 2418
	public class Trigger_DormancyWakeup : Trigger
	{
		// Token: 0x0600498C RID: 18828 RVA: 0x001CA9EB File Offset: 0x001C8BEB
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.DormancyWakeup;
		}
	}
}
