using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009C0 RID: 2496
	public class JobDriver_BringBabyToSafety : JobDriver
	{
		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06004BCC RID: 19404 RVA: 0x001D5B59 File Offset: 0x001D3D59
		private Pawn Baby
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x001D5B66 File Offset: 0x001D3D66
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Baby, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x001D5B89 File Offset: 0x001D3D89
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_BringBabyToSafety.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_BringBabyToSafety.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x001D5B9C File Offset: 0x001D3D9C
		private Toil FindBedForBaby()
		{
			Toil toil = ToilMaker.MakeToil("FindBedForBaby");
			toil.initAction = delegate()
			{
				LocalTargetInfo pack = ChildcareUtility.SafePlaceForBaby(this.Baby, this.pawn, false);
				LocalTargetInfo pack2 = LocalTargetInfo.Invalid;
				if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(this.Baby))
				{
					pack2 = JobGiver_PrepareCaravan_GatherDownedPawns.FindRandomDropCell(this.pawn, this.Baby);
				}
				if (pack.IsValid)
				{
					if (pack2.IsValid && pack2.Cell.DistanceTo(this.pawn.Position) < pack.Cell.DistanceTo(this.pawn.Position))
					{
						toil.GetActor().CurJob.SetTarget(TargetIndex.B, pack2);
						return;
					}
					toil.GetActor().CurJob.SetTarget(TargetIndex.B, pack);
					return;
				}
				else
				{
					if (pack2.IsValid)
					{
						toil.GetActor().CurJob.SetTarget(TargetIndex.B, pack2);
						return;
					}
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					return;
				}
			};
			toil.FailOn(() => !this.pawn.IsCarryingPawn(this.Baby) || this.pawn.Downed || this.pawn.Drafted);
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x04002FEA RID: 12266
		private const TargetIndex BabyInd = TargetIndex.A;

		// Token: 0x04002FEB RID: 12267
		private const TargetIndex BabyBedInd = TargetIndex.B;
	}
}
