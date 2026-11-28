using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009D8 RID: 2520
	public class JobDriver_SelfDetonate : JobDriver
	{
		// Token: 0x06004C75 RID: 19573 RVA: 0x001D70F5 File Offset: 0x001D52F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x001D70F8 File Offset: 0x001D52F8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			return new JobDriver_SelfDetonate.<MakeNewToils>d__1(-2);
		}
	}
}
