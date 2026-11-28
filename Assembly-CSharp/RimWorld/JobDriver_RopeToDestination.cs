using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009A8 RID: 2472
	public abstract class JobDriver_RopeToDestination : JobDriver
	{
		// Token: 0x06004AE3 RID: 19171
		protected abstract bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest);

		// Token: 0x06004AE4 RID: 19172
		protected abstract void ProcessArrivedRopee(Pawn ropee);

		// Token: 0x06004AE5 RID: 19173
		protected abstract bool ShouldOpportunisticallyRopeAnimal(Pawn animal);

		// Token: 0x06004AE6 RID: 19174 RVA: 0x001D2059 File Offset: 0x001D0259
		protected virtual Thing FindDistantAnimalToRope()
		{
			return null;
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x001D205C File Offset: 0x001D025C
		protected virtual bool UpdateDestination()
		{
			return false;
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x001D2060 File Offset: 0x001D0260
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.roping.Ropees.NullOrEmpty<Pawn>())
			{
				foreach (Pawn t in this.pawn.roping.Ropees)
				{
					this.pawn.Reserve(t, this.job, 1, -1, null, errorOnFailed, false);
				}
			}
			this.UpdateDestination();
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.B).Cell);
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x001D2148 File Offset: 0x001D0348
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RopeToDestination.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_RopeToDestination.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x001D2158 File Offset: 0x001D0358
		private void ProcessRopeesThatHaveArrived(bool roperWaitingAtDest)
		{
			JobDriver_RopeToDestination.tmpRopees.Clear();
			JobDriver_RopeToDestination.tmpRopees.AddRange(this.pawn.roping.Ropees);
			foreach (Pawn pawn in JobDriver_RopeToDestination.tmpRopees)
			{
				if (this.HasRopeeArrived(pawn, roperWaitingAtDest))
				{
					this.pawn.roping.DropRope(pawn);
					if (pawn.jobs != null && pawn.CurJob != null && pawn.jobs.curDriver is JobDriver_FollowRoper)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					this.ProcessArrivedRopee(pawn);
				}
			}
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x001D221C File Offset: 0x001D041C
		private void MatchLocomotionUrgency(Toil toil)
		{
			toil.AddPreInitAction(delegate
			{
				this.locomotionUrgencySameAs = this.SlowestRopee();
			});
			toil.AddFinishAction(delegate
			{
				this.locomotionUrgencySameAs = null;
			});
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x001D2244 File Offset: 0x001D0444
		private Pawn SlowestRopee()
		{
			Pawn result;
			if (!this.pawn.roping.Ropees.TryMaxBy((Pawn p) => p.TicksPerMoveCardinal, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x001D228C File Offset: 0x001D048C
		private bool FindAnotherAnimalToRope()
		{
			Pawn_MindState mindState = this.pawn.mindState;
			int? num;
			if (mindState == null)
			{
				num = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				num = ((duty != null) ? duty.ropeeLimit : null);
			}
			int? num2 = num;
			int valueOrDefault = num2.GetValueOrDefault(10);
			if (this.pawn.roping.Ropees.Count >= valueOrDefault)
			{
				return false;
			}
			Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.Touch, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 10f, new Predicate<Thing>(this.<FindAnotherAnimalToRope>g__AnimalValidator|15_0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				thing = this.FindDistantAnimalToRope();
			}
			if (thing != null)
			{
				this.job.SetTarget(TargetIndex.A, thing);
				return true;
			}
			return false;
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x001D242C File Offset: 0x001D062C
		[CompilerGenerated]
		private bool <FindAnotherAnimalToRope>g__AnimalValidator|15_0(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && this.ShouldOpportunisticallyRopeAnimal(pawn);
		}

		// Token: 0x04002FAE RID: 12206
		private const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04002FAF RID: 12207
		public const TargetIndex DestCellInd = TargetIndex.B;

		// Token: 0x04002FB0 RID: 12208
		private const int MaxAnimalsToRope = 10;

		// Token: 0x04002FB1 RID: 12209
		private const int RopeMoreAnimalsScanRadius = 10;

		// Token: 0x04002FB2 RID: 12210
		private static readonly List<Pawn> tmpRopees = new List<Pawn>();
	}
}
