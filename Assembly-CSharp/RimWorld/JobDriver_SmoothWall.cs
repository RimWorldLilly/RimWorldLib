using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009D4 RID: 2516
	public class JobDriver_SmoothWall : JobDriver
	{
		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06004C62 RID: 19554 RVA: 0x001D6E58 File Offset: 0x001D5058
		protected int BaseWorkAmount
		{
			get
			{
				return 6500;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06004C63 RID: 19555 RVA: 0x001D6E5F File Offset: 0x001D505F
		protected DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothWall;
			}
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x001D6E68 File Offset: 0x001D5068
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x001D6EC5 File Offset: 0x001D50C5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_SmoothWall.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_SmoothWall.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x001D6ED8 File Offset: 0x001D50D8
		protected void DoEffect()
		{
			SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(base.TargetA.Thing, this.pawn), this.pawn);
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x001D6F09 File Offset: 0x001D5109
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x0400300A RID: 12298
		private float workLeft = -1000f;
	}
}
