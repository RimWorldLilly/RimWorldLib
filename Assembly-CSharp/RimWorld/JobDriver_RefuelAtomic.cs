using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A65 RID: 2661
	public class JobDriver_RefuelAtomic : JobDriver
	{
		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x0600504B RID: 20555 RVA: 0x001E3D9C File Offset: 0x001E1F9C
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x0600504C RID: 20556 RVA: 0x001E3DBD File Offset: 0x001E1FBD
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x0600504D RID: 20557 RVA: 0x001E3DCC File Offset: 0x001E1FCC
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x001E3DF0 File Offset: 0x001E1FF0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x001E3E3E File Offset: 0x001E203E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RefuelAtomic.<MakeNewToils>d__11 <MakeNewToils>d__ = new JobDriver_RefuelAtomic.<MakeNewToils>d__11(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400313A RID: 12602
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x0400313B RID: 12603
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x0400313C RID: 12604
		private const TargetIndex FuelPlaceCellInd = TargetIndex.C;

		// Token: 0x0400313D RID: 12605
		private const int RefuelingDuration = 240;
	}
}
