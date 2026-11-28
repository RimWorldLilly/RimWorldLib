using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A36 RID: 2614
	public class JobDriver_FishAnimal : JobDriver
	{
		// Token: 0x06004EEB RID: 20203 RVA: 0x001DF5F4 File Offset: 0x001DD7F4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x001DF649 File Offset: 0x001DD849
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FishAnimal.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_FishAnimal.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x001DF659 File Offset: 0x001DD859
		private Toil CompleteFishingToil()
		{
			Toil toil = ToilMaker.MakeToil("CompleteFishingToil");
			toil.initAction = delegate()
			{
				IntVec3 cell = this.job.GetTarget(TargetIndex.A).Cell;
				bool flag;
				List<Thing> catchesFor = FishingUtility.GetCatchesFor(this.pawn, cell, true, out flag);
				if (!catchesFor.Any<Thing>())
				{
					return;
				}
				bool flag2 = false;
				int num = catchesFor.Sum((Thing x) => x.stackCount);
				foreach (Thing thing in catchesFor)
				{
					flag2 |= GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
				if (flag2)
				{
					this.pawn.Map.waterBodyTracker.Notify_Fished(cell, (float)num);
				}
			};
			return toil;
		}

		// Token: 0x040030D6 RID: 12502
		private const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x040030D7 RID: 12503
		private const TargetIndex StandInd = TargetIndex.B;
	}
}
