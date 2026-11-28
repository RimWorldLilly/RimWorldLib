using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A8F RID: 2703
	public class JobDriver_ExtractTree : JobDriver
	{
		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06005165 RID: 20837 RVA: 0x001E7080 File Offset: 0x001E5280
		protected Thing Target
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06005166 RID: 20838 RVA: 0x001E70A1 File Offset: 0x001E52A1
		protected Plant Tree
		{
			get
			{
				return (Plant)this.Target.GetInnerIfMinified();
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06005167 RID: 20839 RVA: 0x001E70B3 File Offset: 0x001E52B3
		protected DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.ExtractTree;
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06005168 RID: 20840 RVA: 0x001E70BA File Offset: 0x001E52BA
		protected float TotalNeededWork
		{
			get
			{
				return this.Tree.def.plant.harvestWork;
			}
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x001E70D1 File Offset: 0x001E52D1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x001E7105 File Offset: 0x001E5305
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x001E7128 File Offset: 0x001E5328
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExtractTree.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_ExtractTree.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400318B RID: 12683
		private float workLeft;

		// Token: 0x0400318C RID: 12684
		private float totalNeededWork;

		// Token: 0x0400318D RID: 12685
		public const TargetIndex TreeInd = TargetIndex.A;
	}
}
