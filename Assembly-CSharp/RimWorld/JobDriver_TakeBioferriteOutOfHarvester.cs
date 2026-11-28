using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A7F RID: 2687
	public class JobDriver_TakeBioferriteOutOfHarvester : JobDriver
	{
		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06005104 RID: 20740 RVA: 0x001E5BDC File Offset: 0x001E3DDC
		protected Building_BioferriteHarvester Harvester
		{
			get
			{
				return (Building_BioferriteHarvester)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06005105 RID: 20741 RVA: 0x001E5C04 File Offset: 0x001E3E04
		protected Thing Bioferrite
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x001E5C25 File Offset: 0x001E3E25
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Harvester, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x001E5C48 File Offset: 0x001E3E48
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeBioferriteOutOfHarvester.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_TakeBioferriteOutOfHarvester.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003173 RID: 12659
		private const TargetIndex HarvesterInd = TargetIndex.A;

		// Token: 0x04003174 RID: 12660
		private const TargetIndex BioferriteToHaulInd = TargetIndex.B;

		// Token: 0x04003175 RID: 12661
		private const TargetIndex StorageCellInd = TargetIndex.C;

		// Token: 0x04003176 RID: 12662
		private const int Duration = 200;
	}
}
