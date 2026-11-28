using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009BF RID: 2495
	public class JobDriver_Breastfeed : JobDriver_FeedBaby
	{
		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06004BBC RID: 19388 RVA: 0x001D5783 File Offset: 0x001D3983
		// (set) Token: 0x06004BBD RID: 19389 RVA: 0x001D578B File Offset: 0x001D398B
		protected override Toil FeedingToil { get; set; }

		// Token: 0x06004BBE RID: 19390 RVA: 0x001D5794 File Offset: 0x001D3994
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return base.TryMakePreToilReservations(errorOnFailed) && this.pawn.Reserve(this.pawn, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x001D57C2 File Offset: 0x001D39C2
		public override void SetInitialPosture()
		{
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x001D57C4 File Offset: 0x001D39C4
		public override bool CanBeginNowWhileLyingDown()
		{
			return this.pawn.Downed;
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x001D57D1 File Offset: 0x001D39D1
		protected override IEnumerable<Toil> FeedBaby()
		{
			JobDriver_Breastfeed.<FeedBaby>d__7 <FeedBaby>d__ = new JobDriver_Breastfeed.<FeedBaby>d__7(-2);
			<FeedBaby>d__.<>4__this = this;
			return <FeedBaby>d__;
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x001D57E4 File Offset: 0x001D39E4
		private Toil Breastfeed()
		{
			this.FeedingToil = ToilMaker.MakeToil("Breastfeed");
			this.FeedingToil.initAction = delegate()
			{
				base.Baby.jobs.StartJob(ChildcareUtility.MakeBabySuckleJob(this.pawn), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
				this.initialFoodPercentage = base.Baby.needs.food.CurLevelPercentage;
			};
			this.FeedingToil.tickIntervalAction = delegate(int delta)
			{
				bool flag = ChildcareUtility.SuckleFromLactatingPawn(base.Baby, this.pawn, delta);
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				if (!this.pawn.Downed && this.pawn.Rotation == Rot4.North)
				{
					this.pawn.Rotation = Rot4.East;
				}
				if (!flag)
				{
					base.ReadyForNextToil();
				}
			};
			this.FeedingToil.AddFinishAction(delegate
			{
				if (base.Baby.Dead)
				{
					return;
				}
				if (base.Baby.needs.food.CurLevelPercentage - this.initialFoodPercentage > 0.6f)
				{
					base.Baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedMe, this.pawn, null);
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedBaby, base.Baby, null);
				}
				if (base.Baby.CurJobDef == JobDefOf.BabySuckle)
				{
					base.Baby.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
				if (this.pawn.Downed)
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
				}
			});
			this.FeedingToil.AddFailCondition(delegate
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return !ChildcareUtility.CanBreastfeed(this.pawn, out breastfeedFailReason);
			});
			this.FeedingToil.handlingFacing = true;
			this.FeedingToil.WithProgressBar(TargetIndex.A, () => base.Baby.needs.food.CurLevelPercentage, false, -0.5f, false);
			this.FeedingToil.defaultCompleteMode = ToilCompleteMode.Never;
			this.FeedingToil.WithEffect(EffecterDefOf.Breastfeeding, TargetIndex.A, null);
			return this.FeedingToil;
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x001D58B6 File Offset: 0x001D3AB6
		private Toil TuckMomInIfDowned()
		{
			Toil toil = ToilMaker.MakeToil("TuckMomInIfDowned");
			toil.initAction = delegate()
			{
				if (HealthAIUtility.ShouldSeekMedicalRest(this.pawn))
				{
					Building_Bed building_Bed = this.pawn.CurrentBed();
					if (building_Bed != null && building_Bed == RestUtility.FindPatientBedFor(this.pawn))
					{
						this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, building_Bed), JobCondition.Succeeded, null, false, true, null, new JobTag?(JobTag.RestingForMedicalReasons), false, false, null, false, true, false);
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}
	}
}
