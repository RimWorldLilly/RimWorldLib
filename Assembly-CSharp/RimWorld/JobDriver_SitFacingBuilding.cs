using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009FD RID: 2557
	public class JobDriver_SitFacingBuilding : JobDriver
	{
		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06004D5D RID: 19805 RVA: 0x001DA456 File Offset: 0x001D8656
		private Building Building
		{
			get
			{
				return (Building)base.TargetThingA;
			}
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x001DA464 File Offset: 0x001D8664
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.job.targetB.Cell, this.job, errorOnFailed);
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x001DA4C7 File Offset: 0x001D86C7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_SitFacingBuilding.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_SitFacingBuilding.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x001DA4D7 File Offset: 0x001D86D7
		protected virtual void ModifyPlayToil(Toil toil)
		{
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x001DA4DC File Offset: 0x001D86DC
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}

		// Token: 0x04003065 RID: 12389
		private readonly TargetIndex BuildingIndex = TargetIndex.A;

		// Token: 0x04003066 RID: 12390
		private readonly TargetIndex SeatIndex = TargetIndex.B;
	}
}
