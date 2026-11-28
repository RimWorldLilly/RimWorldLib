using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A0C RID: 2572
	public class JobDriver_Skydreaming : JobDriver
	{
		// Token: 0x06004DBA RID: 19898 RVA: 0x001DB76F File Offset: 0x001D996F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x001DB772 File Offset: 0x001D9972
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Skydreaming.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_Skydreaming.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
