using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B18 RID: 2840
	public class JobGiver_GotoTravelDestination : ThinkNode_JobGiver
	{
		// Token: 0x060053FE RID: 21502 RVA: 0x001F3FF4 File Offset: 0x001F21F4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GotoTravelDestination jobGiver_GotoTravelDestination = (JobGiver_GotoTravelDestination)base.DeepCopy(resolve);
			jobGiver_GotoTravelDestination.locomotionUrgency = this.locomotionUrgency;
			jobGiver_GotoTravelDestination.maxDanger = this.maxDanger;
			jobGiver_GotoTravelDestination.jobMaxDuration = this.jobMaxDuration;
			jobGiver_GotoTravelDestination.exactCell = this.exactCell;
			jobGiver_GotoTravelDestination.ritualTagOnArrival = this.ritualTagOnArrival;
			jobGiver_GotoTravelDestination.wanderOnArrival = this.wanderOnArrival;
			jobGiver_GotoTravelDestination.jobDef = this.jobDef;
			jobGiver_GotoTravelDestination.destinationFocusIndex = this.destinationFocusIndex;
			jobGiver_GotoTravelDestination.allowZeroLengthPaths = this.allowZeroLengthPaths;
			return jobGiver_GotoTravelDestination;
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x001F407C File Offset: 0x001F227C
		protected virtual LocalTargetInfo GetDestination(Pawn pawn)
		{
			switch (this.destinationFocusIndex)
			{
			case 1:
				return pawn.mindState.duty.focus;
			case 2:
				return pawn.mindState.duty.focusSecond;
			case 3:
				return pawn.mindState.duty.focusThird;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid focus index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.destinationFocusIndex);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return LocalTargetInfo.Invalid;
			}
			}
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x001F411C File Offset: 0x001F231C
		protected override Job TryGiveJob(Pawn pawn)
		{
			pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			Pawn pawn2 = this.CaravanBabyToCarry(pawn);
			if (pawn2 != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.PickupToHold, pawn2);
				job.count = 1;
				return job;
			}
			if (pawn.mindState.nextMoveOrderIsWait && !this.exactCell)
			{
				if (!this.wanderOnArrival)
				{
					return null;
				}
				return this.GetWaitJob();
			}
			else
			{
				LocalTargetInfo destination = this.GetDestination(pawn);
				if (!pawn.CanReach(destination, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
				{
					return null;
				}
				if (pawn.Downed)
				{
					return null;
				}
				if (!this.exactCell || !(pawn.Position == destination))
				{
					LocalTargetInfo targetA = destination;
					if (!this.exactCell)
					{
						targetA = CellFinder.RandomClosewalkCellNear(destination.Cell, pawn.Map, 6, null);
					}
					Job job2 = JobMaker.MakeJob(this.jobDef ?? JobDefOf.Goto, targetA);
					job2.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
					job2.expiryInterval = this.jobMaxDuration;
					job2.ritualTag = this.ritualTagOnArrival;
					return job2;
				}
				if (!this.ritualTagOnArrival.NullOrEmpty())
				{
					Lord lord = pawn.GetLord();
					LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(pawn, this.ritualTagOnArrival);
					}
				}
				if (this.allowZeroLengthPaths)
				{
					return this.GetWaitJob();
				}
				return null;
			}
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x001F427C File Offset: 0x001F247C
		private Job GetWaitJob()
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
			job.expiryInterval = this.WaitTicks.RandomInRange;
			return job;
		}

		// Token: 0x06005402 RID: 21506 RVA: 0x001F429C File Offset: 0x001F249C
		private Pawn CaravanBabyToCarry(Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike || pawn.Downed || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || (pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				return null;
			}
			LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lord.LordJob as LordJob_FormAndSendCaravan;
			List<Pawn> list = (lordJob_FormAndSendCaravan != null) ? lordJob_FormAndSendCaravan.downedPawns : null;
			if (list.NullOrEmpty<Pawn>())
			{
				return null;
			}
			foreach (Pawn pawn2 in list)
			{
				if (pawn2.Downed && pawn2 != pawn && pawn2.DevelopmentalStage.Baby() && pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false) && pawn2.Spawned)
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x0400324A RID: 12874
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x0400324B RID: 12875
		protected Danger maxDanger = Danger.Some;

		// Token: 0x0400324C RID: 12876
		protected int jobMaxDuration = 999999;

		// Token: 0x0400324D RID: 12877
		protected bool exactCell;

		// Token: 0x0400324E RID: 12878
		protected string ritualTagOnArrival;

		// Token: 0x0400324F RID: 12879
		protected bool wanderOnArrival = true;

		// Token: 0x04003250 RID: 12880
		protected JobDef jobDef;

		// Token: 0x04003251 RID: 12881
		protected int destinationFocusIndex = 1;

		// Token: 0x04003252 RID: 12882
		protected bool allowZeroLengthPaths;

		// Token: 0x04003253 RID: 12883
		private IntRange WaitTicks = new IntRange(30, 80);
	}
}
