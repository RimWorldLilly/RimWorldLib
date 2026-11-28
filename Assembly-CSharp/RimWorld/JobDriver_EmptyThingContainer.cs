using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A2A RID: 2602
	public class JobDriver_EmptyThingContainer : JobDriver
	{
		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06004E92 RID: 20114 RVA: 0x001DE25C File Offset: 0x001DC45C
		protected virtual PathEndMode ContainerPathEndMode
		{
			get
			{
				if (!this.job.GetTarget(TargetIndex.A).Thing.def.hasInteractionCell)
				{
					return PathEndMode.Touch;
				}
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x001DE28C File Offset: 0x001DC48C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, this.job.def.containerReservationLayer, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x001DE2F2 File Offset: 0x001DC4F2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EmptyThingContainer.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_EmptyThingContainer.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030BE RID: 12478
		protected const TargetIndex ContainerInd = TargetIndex.A;

		// Token: 0x040030BF RID: 12479
		protected const TargetIndex ContentsInd = TargetIndex.B;

		// Token: 0x040030C0 RID: 12480
		protected const TargetIndex StoreCellInd = TargetIndex.C;

		// Token: 0x040030C1 RID: 12481
		private const int OpenTicks = 120;
	}
}
