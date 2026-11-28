using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A50 RID: 2640
	public class JobDriver_SelfShutdown : JobDriver
	{
		// Token: 0x06004FAC RID: 20396 RVA: 0x001E182A File Offset: 0x001DFA2A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return ModLister.CheckBiotech("Self-shutdown") && this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x001E1856 File Offset: 0x001DFA56
		protected override IEnumerable<Toil> MakeNewToils()
		{
			return new JobDriver_SelfShutdown.<MakeNewToils>d__2(-2);
		}

		// Token: 0x04003100 RID: 12544
		public const TargetIndex RestSpotIndex = TargetIndex.A;
	}
}
