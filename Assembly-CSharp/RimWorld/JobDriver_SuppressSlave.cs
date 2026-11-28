using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AAB RID: 2731
	public class JobDriver_SuppressSlave : JobDriver
	{
		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06005244 RID: 21060 RVA: 0x001EAB00 File Offset: 0x001E8D00
		protected Pawn Slave
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x001EAB17 File Offset: 0x001E8D17
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x001EAB3A File Offset: 0x001E8D3A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_SuppressSlave.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_SuppressSlave.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x001EAB4A File Offset: 0x001E8D4A
		private Toil TrySuppress(TargetIndex slaveInd)
		{
			Toil toil = ToilMaker.MakeToil("TrySuppress");
			toil.initAction = delegate()
			{
				this.pawn.interactions.TryInteractWith(this.Slave, InteractionDefOf.Suppress);
				PawnUtility.ForceWait(this.Slave, this.SuppressDuration, this.pawn, false, false);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.SuppressDuration;
			return toil;
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x001EAB84 File Offset: 0x001E8D84
		private Toil SetLastSuppressionTime(TargetIndex targetInd)
		{
			Toil toil = ToilMaker.MakeToil("SetLastSuppressionTime");
			toil.initAction = delegate()
			{
				((Pawn)toil.actor.jobs.curJob.GetTarget(targetInd).Thing).mindState.lastSlaveSuppressedTick = Find.TickManager.TicksGame;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x040031C7 RID: 12743
		private int SuppressDuration = 180;
	}
}
