using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A3C RID: 2620
	public class JobDriver_GiveToPawn : JobDriver
	{
		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06004F0C RID: 20236 RVA: 0x001DFB90 File Offset: 0x001DDD90
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06004F0D RID: 20237 RVA: 0x001DFBB4 File Offset: 0x001DDDB4
		private Pawn Receiver
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Pawn;
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06004F0E RID: 20238 RVA: 0x001DFBD5 File Offset: 0x001DDDD5
		public int CountBeingHauled
		{
			get
			{
				if (this.pawn.carryTracker.CarriedThing == null)
				{
					return 0;
				}
				return this.pawn.carryTracker.CarriedThing.stackCount;
			}
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x001DFC00 File Offset: 0x001DDE00
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x001DFC23 File Offset: 0x001DDE23
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GiveToPawn.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_GiveToPawn.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x001DFC33 File Offset: 0x001DDE33
		private Toil DetermineNumToHaul()
		{
			Toil toil = ToilMaker.MakeToil("DetermineNumToHaul");
			toil.initAction = delegate()
			{
				int num = GiveItemsToPawnUtility.ItemCountLeftToCollect(this.Receiver);
				if (num <= 0)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
				this.job.count = num;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x001DFC5F File Offset: 0x001DDE5F
		private Toil SetItemTarget()
		{
			Toil toil = ToilMaker.MakeToil("SetItemTarget");
			toil.initAction = delegate()
			{
				Thing thing = GiveItemsToPawnUtility.FindItemToGive(this.pawn, this.Item.def);
				if (thing == null)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				this.job.SetTarget(TargetIndex.A, thing);
			};
			return toil;
		}

		// Token: 0x040030E0 RID: 12512
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x040030E1 RID: 12513
		private const TargetIndex PawnInd = TargetIndex.B;
	}
}
