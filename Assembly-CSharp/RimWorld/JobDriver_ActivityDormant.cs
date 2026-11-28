using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A51 RID: 2641
	public class JobDriver_ActivityDormant : JobDriver
	{
		// Token: 0x06004FAF RID: 20399 RVA: 0x001E1867 File Offset: 0x001DFA67
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return ModLister.AnomalyInstalled;
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x001E186E File Offset: 0x001DFA6E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			return new JobDriver_ActivityDormant.<MakeNewToils>d__1(-2);
		}
	}
}
