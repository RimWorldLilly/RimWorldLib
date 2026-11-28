using System;

namespace RimWorld
{
	// Token: 0x02000A4E RID: 2638
	public class JobDriver_LayDownDormant : JobDriver_LayDown
	{
		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06004FA5 RID: 20389 RVA: 0x001E180B File Offset: 0x001DFA0B
		public override bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06004FA6 RID: 20390 RVA: 0x001E180E File Offset: 0x001DFA0E
		public override bool LookForOtherJobs
		{
			get
			{
				return false;
			}
		}
	}
}
