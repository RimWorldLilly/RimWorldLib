using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A1A RID: 2586
	public class JobDriver_CarryToEntityHolder : JobDriver
	{
		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06004E22 RID: 20002 RVA: 0x001DCD4C File Offset: 0x001DAF4C
		private Thing Takee
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06004E23 RID: 20003 RVA: 0x001DCD70 File Offset: 0x001DAF70
		private CompEntityHolder DestHolder
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x001DCD98 File Offset: 0x001DAF98
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DestHolder.parent, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x001DCDF0 File Offset: 0x001DAFF0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CarryToEntityHolder.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_CarryToEntityHolder.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x001DCE00 File Offset: 0x001DB000
		public static IEnumerable<Toil> ChainTakeeToPlatformToils(Pawn taker, Thing takee, CompEntityHolder platform, TargetIndex platformIndex)
		{
			JobDriver_CarryToEntityHolder.<ChainTakeeToPlatformToils>d__9 <ChainTakeeToPlatformToils>d__ = new JobDriver_CarryToEntityHolder.<ChainTakeeToPlatformToils>d__9(-2);
			<ChainTakeeToPlatformToils>d__.<>3__taker = taker;
			<ChainTakeeToPlatformToils>d__.<>3__takee = takee;
			<ChainTakeeToPlatformToils>d__.<>3__platform = platform;
			<ChainTakeeToPlatformToils>d__.<>3__platformIndex = platformIndex;
			return <ChainTakeeToPlatformToils>d__;
		}

		// Token: 0x0400309C RID: 12444
		private const TargetIndex DestHolderIndex = TargetIndex.A;

		// Token: 0x0400309D RID: 12445
		private const TargetIndex TakeeIndex = TargetIndex.B;

		// Token: 0x0400309E RID: 12446
		private const int EnterDelayTicks = 300;
	}
}
