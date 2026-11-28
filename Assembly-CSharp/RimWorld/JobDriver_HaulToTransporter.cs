using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A40 RID: 2624
	public class JobDriver_HaulToTransporter : JobDriver_HaulToContainer
	{
		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06004F3E RID: 20286 RVA: 0x001E0493 File Offset: 0x001DE693
		public CompTransporter Transporter
		{
			get
			{
				Thing container = base.Container;
				if (container == null)
				{
					return null;
				}
				return container.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x001E04A6 File Offset: 0x001DE6A6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.initialCount, "initialCount", 0, false);
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x001E04C0 File Offset: 0x001DE6C0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x001E0510 File Offset: 0x001DE710
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
				thingCount = LoadTransportersJobUtility.FindThingToLoad(this.pawn, base.Container.TryGetComp<CompTransporter>());
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

		// Token: 0x040030E8 RID: 12520
		public int initialCount;
	}
}
