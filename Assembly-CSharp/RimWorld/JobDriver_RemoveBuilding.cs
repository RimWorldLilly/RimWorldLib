using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009D0 RID: 2512
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06004C3D RID: 19517 RVA: 0x001D6AC6 File Offset: 0x001D4CC6
		protected Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06004C3E RID: 19518 RVA: 0x001D6AD8 File Offset: 0x001D4CD8
		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06004C3F RID: 19519
		protected abstract DesignationDef Designation { get; }

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06004C40 RID: 19520
		protected abstract float TotalNeededWork { get; }

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06004C41 RID: 19521
		protected abstract EffecterDef WorkEffecter { get; }

		// Token: 0x06004C42 RID: 19522 RVA: 0x001D6AEA File Offset: 0x001D4CEA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x001D6B1E File Offset: 0x001D4D1E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x001D6B41 File Offset: 0x001D4D41
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RemoveBuilding.<MakeNewToils>d__14 <MakeNewToils>d__ = new JobDriver_RemoveBuilding.<MakeNewToils>d__14(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x001D6B51 File Offset: 0x001D4D51
		protected virtual void FinishedRemoving()
		{
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x001D6B53 File Offset: 0x001D4D53
		protected virtual void TickActionInterval(int delta)
		{
		}

		// Token: 0x04003004 RID: 12292
		private float workLeft;

		// Token: 0x04003005 RID: 12293
		private float totalNeededWork;
	}
}
