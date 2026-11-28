using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009C1 RID: 2497
	public abstract class JobDriver_FeedBaby : JobDriver
	{
		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06004BD7 RID: 19415 RVA: 0x001D5D39 File Offset: 0x001D3F39
		protected Pawn Baby
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x001D5D46 File Offset: 0x001D3F46
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Baby, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004BD9 RID: 19417
		protected abstract IEnumerable<Toil> FeedBaby();

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06004BDA RID: 19418
		// (set) Token: 0x06004BDB RID: 19419
		protected abstract Toil FeedingToil { get; set; }

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06004BDC RID: 19420 RVA: 0x001D5D69 File Offset: 0x001D3F69
		public bool Feeding
		{
			get
			{
				return base.CurToil == this.FeedingToil;
			}
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x001D5D79 File Offset: 0x001D3F79
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FeedBaby.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_FeedBaby.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x001D5D8C File Offset: 0x001D3F8C
		private Toil GoToChair()
		{
			Toil toil = ToilMaker.MakeToil("GoToChair");
			toil.initAction = delegate()
			{
				IntVec3 breastfeedSpot = RCellFinder.SpotToStandDuringJob(this.pawn, null, null);
				Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 32f, (Thing t) => this.IsValidBreastfeedChair(t) && t.Position.GetDangerFor(this.pawn, t.Map) <= breastfeedSpot.GetDangerFor(this.pawn, this.pawn.Map), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					Toils_Ingest.TryFindFreeSittingSpotOnThing(thing, this.pawn, out breastfeedSpot);
				}
				if (!breastfeedSpot.IsValid)
				{
					this.pawn.pather.StartPath(this.pawn.Position, PathEndMode.OnCell);
					return;
				}
				this.pawn.ReserveSittableOrSpot(breastfeedSpot, toil.actor.CurJob, true);
				this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.pawn.CurJob, breastfeedSpot);
				this.pawn.pather.StartPath(breastfeedSpot, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x001D5DDF File Offset: 0x001D3FDF
		private Toil SitInChair()
		{
			Toil toil = ToilMaker.MakeToil("SitInChair");
			toil.initAction = delegate()
			{
				if (!this.pawn.Spawned)
				{
					return;
				}
				Thing thing = this.pawn.Position.GetThingList(this.pawn.Map).FirstOrDefault(new Predicate<Thing>(this.IsValidBreastfeedChair));
				if (thing != null)
				{
					this.pawn.Rotation = thing.Rotation;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x001D5E04 File Offset: 0x001D4004
		private bool IsValidBreastfeedChair(Thing t)
		{
			IntVec3 intVec;
			return t.def.building != null && t.def.building.isSittable && Toils_Ingest.TryFindFreeSittingSpotOnThing(t, this.pawn, out intVec) && !t.IsForbidden(this.pawn) && this.pawn.CanReserve(t, 1, -1, null, false) && t.IsSociallyProper(this.pawn) && !t.IsBurning() && !t.HostileTo(this.pawn);
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x001D5E98 File Offset: 0x001D4098
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.initialFoodPercentage, "initialFoodPercentage", 0f, false);
		}

		// Token: 0x04002FEC RID: 12268
		public const float BabyFedMemoryFoodNeedIncrease = 0.6f;

		// Token: 0x04002FED RID: 12269
		protected const TargetIndex BabyInd = TargetIndex.A;

		// Token: 0x04002FEE RID: 12270
		private const int ChairSearchRadius = 32;

		// Token: 0x04002FEF RID: 12271
		protected float initialFoodPercentage;
	}
}
