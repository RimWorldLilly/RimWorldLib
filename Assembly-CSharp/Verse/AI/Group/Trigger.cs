using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000960 RID: 2400
	public abstract class Trigger
	{
		// Token: 0x06004963 RID: 18787
		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		// Token: 0x06004964 RID: 18788 RVA: 0x001CA489 File Offset: 0x001C8689
		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x001CA48B File Offset: 0x001C868B
		public override string ToString()
		{
			return base.GetType().ToString();
		}

		// Token: 0x04002F0E RID: 12046
		public TriggerData data;

		// Token: 0x04002F0F RID: 12047
		public List<TriggerFilter> filters;
	}
}
