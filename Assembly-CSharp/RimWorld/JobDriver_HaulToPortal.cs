using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A3F RID: 2623
	public class JobDriver_HaulToPortal : JobDriver_HaulToContainer
	{
		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06004F38 RID: 20280 RVA: 0x001E02F9 File Offset: 0x001DE4F9
		public MapPortal MapPortal
		{
			get
			{
				return base.Container as MapPortal;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06004F39 RID: 20281 RVA: 0x001E0306 File Offset: 0x001DE506
		protected override int Duration
		{
			get
			{
				return 90;
			}
		}

		// Token: 0x06004F3A RID: 20282 RVA: 0x001E030A File Offset: 0x001DE50A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.initialCount, "initialCount", 0, false);
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x001E0324 File Offset: 0x001DE524
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x001E0374 File Offset: 0x001DE574
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			ThingCount thingCount;
			if (this.job.targetA.IsValid)
			{
				thingCount = new ThingCount(this.job.targetA.Thing, this.job.targetA.Thing.stackCount, false);
			}
			else
			{
				thingCount = EnterPortalUtility.FindThingToLoad(this.pawn, this.MapPortal);
			}
			if (this.job.playerForced && this.pawn.carryTracker.CarriedThing != null && this.pawn.carryTracker.CarriedThing != thingCount.Thing)
			{
				Thing thing;
				this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
			}
			this.job.targetA = thingCount.Thing;
			this.job.count = thingCount.Count;
			this.initialCount = thingCount.Count;
			this.pawn.Reserve(thingCount.Thing, this.job, 1, -1, null, true, false);
		}

		// Token: 0x040030E6 RID: 12518
		private const int DepositDuration = 90;

		// Token: 0x040030E7 RID: 12519
		public int initialCount;
	}
}
