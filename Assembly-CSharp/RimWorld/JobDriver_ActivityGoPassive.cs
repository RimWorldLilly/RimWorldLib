using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A11 RID: 2577
	public class JobDriver_ActivityGoPassive : JobDriver
	{
		// Token: 0x06004DDA RID: 19930 RVA: 0x001DBE20 File Offset: 0x001DA020
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x001DBE23 File Offset: 0x001DA023
		protected override IEnumerable<Toil> MakeNewToils()
		{
			return new JobDriver_ActivityGoPassive.<MakeNewToils>d__1(-2);
		}
	}
}
