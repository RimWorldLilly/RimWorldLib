using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A52 RID: 2642
	public class JobDriver_LinkPsylinkable : JobDriver
	{
		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06004FB2 RID: 20402 RVA: 0x001E1880 File Offset: 0x001DFA80
		private Thing PsylinkableThing
		{
			get
			{
				return base.TargetA.Thing;
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06004FB3 RID: 20403 RVA: 0x001E189B File Offset: 0x001DFA9B
		private CompPsylinkable Psylinkable
		{
			get
			{
				return this.PsylinkableThing.TryGetComp<CompPsylinkable>();
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06004FB4 RID: 20404 RVA: 0x001E18A8 File Offset: 0x001DFAA8
		private LocalTargetInfo LinkSpot
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x001E18B8 File Offset: 0x001DFAB8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.PsylinkableThing, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.LinkSpot, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x001E1906 File Offset: 0x001DFB06
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_LinkPsylinkable.<MakeNewToils>d__11 <MakeNewToils>d__ = new JobDriver_LinkPsylinkable.<MakeNewToils>d__11(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003101 RID: 12545
		public const int LinkTimeTicks = 15000;

		// Token: 0x04003102 RID: 12546
		public const int EffectsTickInterval = 720;

		// Token: 0x04003103 RID: 12547
		protected const TargetIndex PsylinkableInd = TargetIndex.A;

		// Token: 0x04003104 RID: 12548
		protected const TargetIndex LinkSpotInd = TargetIndex.B;
	}
}
