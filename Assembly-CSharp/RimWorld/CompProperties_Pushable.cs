using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001949 RID: 6473
	public class CompProperties_Pushable : CompProperties
	{
		// Token: 0x0600A266 RID: 41574 RVA: 0x003A0C7D File Offset: 0x0039EE7D
		public CompProperties_Pushable()
		{
			this.compClass = typeof(CompPushable);
		}

		// Token: 0x04005FEF RID: 24559
		public float offsetDistance;

		// Token: 0x04005FF0 RID: 24560
		public float smoothTime = 0.3f;

		// Token: 0x04005FF1 RID: 24561
		public bool givePushOption;
	}
}
