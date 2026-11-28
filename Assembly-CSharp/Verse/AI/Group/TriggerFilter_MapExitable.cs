using System;

namespace Verse.AI.Group
{
	// Token: 0x02000964 RID: 2404
	public class TriggerFilter_MapExitable : TriggerFilter
	{
		// Token: 0x0600496D RID: 18797 RVA: 0x001CA58B File Offset: 0x001C878B
		public override bool AllowActivation(Lord lord, TriggerSignal signal)
		{
			return lord.Map.CanEverExit;
		}
	}
}
