using System;

namespace Verse.AI.Group
{
	// Token: 0x02000962 RID: 2402
	public abstract class TriggerFilter
	{
		// Token: 0x06004969 RID: 18793
		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}
