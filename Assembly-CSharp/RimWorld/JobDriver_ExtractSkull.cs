using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A33 RID: 2611
	public class JobDriver_ExtractSkull : JobDriver
	{
		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06004ED3 RID: 20179 RVA: 0x001DEE5D File Offset: 0x001DD05D
		protected Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x001DEE74 File Offset: 0x001DD074
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x001DEE98 File Offset: 0x001DD098
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExtractSkull.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_ExtractSkull.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030CF RID: 12495
		public const int ExtractionTimeTicks = 180;

		// Token: 0x040030D0 RID: 12496
		private const TargetIndex CorpseInd = TargetIndex.A;
	}
}
