using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200080A RID: 2058
	public abstract class JobDriver_BabyPlay : JobDriver
	{
		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x0019BEAD File Offset: 0x0019A0AD
		protected Pawn Baby
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x060040C5 RID: 16581
		protected abstract JobDriver_BabyPlay.StartingConditions StartingCondition { get; }

		// Token: 0x060040C6 RID: 16582 RVA: 0x0019BEBA File Offset: 0x0019A0BA
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x0019BEDE File Offset: 0x0019A0DE
		public Toil SetPlayPercentage()
		{
			return Toils_General.Do(delegate
			{
				if (this.initialPlayPercentage == -1f)
				{
					Need_Play play = this.Baby.needs.play;
					this.initialPlayPercentage = ((play != null) ? play.CurLevelPercentage : 1f);
				}
			});
		}

		// Token: 0x060040C8 RID: 16584 RVA: 0x0019BEF4 File Offset: 0x0019A0F4
		public void AddPlayThoughtIfAppropriate()
		{
			if (this.Baby.DestroyedOrNull())
			{
				return;
			}
			Need_Play play = this.Baby.needs.play;
			float num = (play != null) ? play.CurLevelPercentage : -1f;
			if (this.initialPlayPercentage >= 0f && num - this.initialPlayPercentage > 0.6f)
			{
				this.Baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.PlayedWithMe, this.pawn, null);
			}
		}

		// Token: 0x060040C9 RID: 16585
		protected abstract IEnumerable<Toil> Play();

		// Token: 0x060040CA RID: 16586 RVA: 0x0019BF77 File Offset: 0x0019A177
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_BabyPlay.<MakeNewToils>d__14 <MakeNewToils>d__ = new JobDriver_BabyPlay.<MakeNewToils>d__14(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x0019BF87 File Offset: 0x0019A187
		private IEnumerable<Toil> CreateStartingCondition()
		{
			JobDriver_BabyPlay.<CreateStartingCondition>d__15 <CreateStartingCondition>d__ = new JobDriver_BabyPlay.<CreateStartingCondition>d__15(-2);
			<CreateStartingCondition>d__.<>4__this = this;
			return <CreateStartingCondition>d__;
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x0019BF98 File Offset: 0x0019A198
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.initialPlayPercentage, "initialPlayPercentage", 0f, false);
			Scribe_Values.Look<float>(ref this.roomPlayGainFactor, "roomPlayGainFactor", 0f, false);
			Scribe_Values.Look<bool>(ref this.finishedSetup, "finishedSetup", false, false);
		}

		// Token: 0x04002AF7 RID: 10999
		protected const float BabyPlayMemoryNeedIncrease = 0.6f;

		// Token: 0x04002AF8 RID: 11000
		protected float roomPlayGainFactor = -1f;

		// Token: 0x04002AF9 RID: 11001
		protected float initialPlayPercentage = -1f;

		// Token: 0x04002AFA RID: 11002
		protected bool finishedSetup;

		// Token: 0x04002AFB RID: 11003
		protected const TargetIndex BabyInd = TargetIndex.A;

		// Token: 0x02002ADA RID: 10970
		public enum StartingConditions
		{
			// Token: 0x0400AFD2 RID: 45010
			Invalid,
			// Token: 0x0400AFD3 RID: 45011
			GotoBaby,
			// Token: 0x0400AFD4 RID: 45012
			PickupBaby
		}
	}
}
