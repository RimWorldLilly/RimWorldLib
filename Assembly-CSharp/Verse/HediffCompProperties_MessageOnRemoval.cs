using System;

namespace Verse
{
	// Token: 0x0200045D RID: 1117
	public class HediffCompProperties_MessageOnRemoval : HediffCompProperties_MessageBase
	{
		// Token: 0x06001FBF RID: 8127 RVA: 0x000C0619 File Offset: 0x000BE819
		public HediffCompProperties_MessageOnRemoval()
		{
			this.compClass = typeof(HediffComp_MessageOnRemoval);
		}

		// Token: 0x040018B7 RID: 6327
		public bool messageOnZeroSeverity = true;

		// Token: 0x040018B8 RID: 6328
		public bool messageOnNonZeroSeverity = true;
	}
}
