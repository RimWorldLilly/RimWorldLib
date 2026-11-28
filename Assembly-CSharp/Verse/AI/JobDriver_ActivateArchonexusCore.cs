using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000801 RID: 2049
	public class JobDriver_ActivateArchonexusCore : JobDriver_Goto
	{
		// Token: 0x0600409F RID: 16543 RVA: 0x0019B9D1 File Offset: 0x00199BD1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ActivateArchonexusCore.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_ActivateArchonexusCore.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002AE7 RID: 10983
		private const int DefaultDuration = 120;
	}
}
