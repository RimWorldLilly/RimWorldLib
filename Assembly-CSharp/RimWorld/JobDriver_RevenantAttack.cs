using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A70 RID: 2672
	public class JobDriver_RevenantAttack : JobDriver
	{
		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06005098 RID: 20632 RVA: 0x001E49C8 File Offset: 0x001E2BC8
		private int HypnotizeDurationTicks
		{
			get
			{
				return RevenantUtility.HypnotizeDurationSecondsFromNumColonistsCurve.Evaluate((float)RevenantUtility.NumSpawnedUnhypnotizedColonists(this.pawn.Map)).SecondsToTicks();
			}
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06005099 RID: 20633 RVA: 0x001E49EA File Offset: 0x001E2BEA
		private float RevealRange
		{
			get
			{
				return RevenantUtility.RevealRangeFromNumColonistsCurve.Evaluate((float)RevenantUtility.NumSpawnedUnhypnotizedColonists(this.pawn.Map));
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x0600509A RID: 20634 RVA: 0x001E4A07 File Offset: 0x001E2C07
		private CompRevenant Comp
		{
			get
			{
				return this.pawn.TryGetComp<CompRevenant>();
			}
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x001E4A14 File Offset: 0x001E2C14
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hypnotizeEndTick, "hypnotizeEndTick", 0, false);
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x001E4A2E File Offset: 0x001E2C2E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x001E4A31 File Offset: 0x001E2C31
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.pawn.mindState.enemyTarget != null)
			{
				this.job.SetTarget(TargetIndex.A, this.pawn.mindState.enemyTarget);
			}
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x001E4A6C File Offset: 0x001E2C6C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RevenantAttack.<MakeNewToils>d__15 <MakeNewToils>d__ = new JobDriver_RevenantAttack.<MakeNewToils>d__15(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x001E4A7C File Offset: 0x001E2C7C
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x001E4A98 File Offset: 0x001E2C98
		public override void Notify_PatherFailed()
		{
			Pawn pawn = this.job.GetTarget(TargetIndex.A).Pawn;
			if (pawn != null)
			{
				Find.Anomaly.EndHypnotize(pawn);
			}
			this.job.SetTarget(TargetIndex.A, RevenantUtility.GetClosestTargetInRadius(this.pawn, 20f));
			this.pawn.mindState.enemyTarget = this.job.GetTarget(TargetIndex.A).Pawn;
			if (this.pawn.mindState.enemyTarget == null)
			{
				this.Comp.revenantState = RevenantState.Wander;
			}
			base.Notify_PatherFailed();
		}

		// Token: 0x0400314F RID: 12623
		private const TargetIndex VictimInd = TargetIndex.A;

		// Token: 0x04003150 RID: 12624
		private const float NewTargetScanRadius = 20f;

		// Token: 0x04003151 RID: 12625
		private const int SmearMTBTicks = 60;

		// Token: 0x04003152 RID: 12626
		private const int CheckForCloserTargetMTB = 180;

		// Token: 0x04003153 RID: 12627
		private static readonly int LongStunTicks = 2500;

		// Token: 0x04003154 RID: 12628
		private int hypnotizeEndTick;
	}
}
