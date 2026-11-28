using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF2 RID: 3826
	public class CompProperties_Schedule : CompProperties
	{
		// Token: 0x060063D0 RID: 25552 RVA: 0x0023C6E0 File Offset: 0x0023A8E0
		public CompProperties_Schedule()
		{
			this.compClass = typeof(CompSchedule);
		}

		// Token: 0x0400384C RID: 14412
		public float startTime;

		// Token: 0x0400384D RID: 14413
		public float endTime = 1f;

		// Token: 0x0400384E RID: 14414
		[MustTranslate]
		public string offMessage;
	}
}
