using System;

namespace RimWorld
{
	// Token: 0x02000A4F RID: 2639
	public class JobDriver_LayDownDeactivated : JobDriver_LayDown
	{
		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06004FA8 RID: 20392 RVA: 0x001E1819 File Offset: 0x001DFA19
		public override bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06004FA9 RID: 20393 RVA: 0x001E181C File Offset: 0x001DFA1C
		public override bool CanRest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06004FAA RID: 20394 RVA: 0x001E181F File Offset: 0x001DFA1F
		public override bool LookForOtherJobs
		{
			get
			{
				return false;
			}
		}
	}
}
