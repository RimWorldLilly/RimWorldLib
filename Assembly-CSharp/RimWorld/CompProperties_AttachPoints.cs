using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001823 RID: 6179
	public class CompProperties_AttachPoints : CompProperties
	{
		// Token: 0x06009A94 RID: 39572 RVA: 0x0037C2A1 File Offset: 0x0037A4A1
		public CompProperties_AttachPoints()
		{
			this.compClass = typeof(CompAttachPoints);
		}

		// Token: 0x04005BE7 RID: 23527
		public List<AttachPoint> points;
	}
}
