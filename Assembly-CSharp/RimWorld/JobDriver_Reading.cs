using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A62 RID: 2658
	public class JobDriver_Reading : JobDriver
	{
		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06005026 RID: 20518 RVA: 0x001E318C File Offset: 0x001E138C
		public Book Book
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Book;
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06005027 RID: 20519 RVA: 0x001E31B2 File Offset: 0x001E13B2
		public bool IsReading
		{
			get
			{
				return this.isReading;
			}
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x001E31BA File Offset: 0x001E13BA
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Book, this.job, 1, 1, null, errorOnFailed, false);
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x001E31DD File Offset: 0x001E13DD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Reading.<MakeNewToils>d__14 <MakeNewToils>d__ = new JobDriver_Reading.<MakeNewToils>d__14(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x001E31F0 File Offset: 0x001E13F0
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.job.count = 1;
			this.hasInInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(this.Book));
			Pawn pawn = this.pawn;
			this.carrying = (((pawn != null) ? pawn.carryTracker.CarriedThing : null) == this.Book);
			Pawn pawn2 = this.pawn;
			this.isLearningDesire = (((pawn2 != null) ? pawn2.learning : null) != null && this.pawn.learning.ActiveLearningDesires.Contains(LearningDesireDefOf.Reading));
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x001E3297 File Offset: 0x001E1497
		private IEnumerable<Toil> PrepareToReadBook()
		{
			JobDriver_Reading.<PrepareToReadBook>d__16 <PrepareToReadBook>d__ = new JobDriver_Reading.<PrepareToReadBook>d__16(-2);
			<PrepareToReadBook>d__.<>4__this = this;
			return <PrepareToReadBook>d__;
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x001E32A8 File Offset: 0x001E14A8
		private Toil ReadBook(int duration)
		{
			Toil toil = Toils_General.Wait(duration, TargetIndex.None);
			toil.debugName = "Reading";
			toil.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			toil.handlingFacing = true;
			toil.initAction = delegate()
			{
				this.Book.IsOpen = true;
				this.pawn.pather.StopDead();
				this.job.showCarryingInspectLine = false;
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				if (this.job.GetTarget(TargetIndex.B).IsValid)
				{
					this.pawn.rotationTracker.FaceCell(this.job.GetTarget(TargetIndex.B).Cell);
				}
				else if (this.Book.Spawned)
				{
					this.pawn.rotationTracker.FaceCell(this.Book.Position);
				}
				else if (this.pawn.Rotation == Rot4.North)
				{
					this.pawn.Rotation = new Rot4(Rand.Range(1, 4));
				}
				float readingBonus = BookUtility.GetReadingBonus(this.pawn);
				this.isReading = true;
				this.Book.OnBookReadTick(this.pawn, delta, readingBonus);
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills != null)
				{
					skills.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta, false, false);
				}
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				if (this.pawn.CurJob != null)
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					if (((needs != null) ? needs.joy : null) != null)
					{
						JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.GoToNextToil;
						if (this.pawn.CurJob.playerForced || this.pawn.learning != null)
						{
							fullJoyAction = JoyTickFullJoyAction.None;
						}
						JoyUtility.JoyTickCheckEnd(this.pawn, delta, fullJoyAction, this.Book.JoyFactor * readingBonus, null);
					}
				}
				if (this.isLearningDesire && this.job != null)
				{
					Pawn_NeedsTracker needs2 = this.pawn.needs;
					if (((needs2 != null) ? needs2.learning : null) != null)
					{
						LearningUtility.LearningTickCheckEnd(this.pawn, delta, this.job.playerForced);
					}
					else
					{
						this.pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
					}
				}
				if (this.pawn.IsHashIntervalTick(600, delta))
				{
					this.pawn.jobs.CheckForJobOverride(9.1f, true);
				}
			};
			toil.AddEndCondition(delegate
			{
				string text;
				if (!BookUtility.CanReadBook(this.Book, this.pawn, out text))
				{
					return JobCondition.InterruptForced;
				}
				return JobCondition.Ongoing;
			});
			toil.AddFinishAction(delegate
			{
				this.Book.IsOpen = false;
				TaleRecorder.RecordTale(TaleDefOf.ReadBook, new object[]
				{
					this.pawn,
					this.Book
				});
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			if (this.isLearningDesire && !this.job.playerForced)
			{
				toil.defaultCompleteMode = ToilCompleteMode.Never;
			}
			return toil;
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x001E333C File Offset: 0x001E153C
		private Toil CarryToReadingSpot()
		{
			Toil toil = ToilMaker.MakeToil("CarryToReadingSpot");
			toil.initAction = delegate()
			{
				IntVec3 intVec;
				if (!this.TryGetClosestChairFreeSittingSpot(true, out intVec) && !this.TryGetClosestChairFreeSittingSpot(false, out intVec))
				{
					intVec = RCellFinder.SpotToChewStandingNear(this.pawn, this.Book, (IntVec3 c) => !c.Fogged(this.pawn.Map) && this.pawn.CanReserveSittableOrSpot(c, false));
				}
				if (!intVec.IsValid)
				{
					this.pawn.pather.StartPath(this.pawn.Position, PathEndMode.OnCell);
					return;
				}
				this.pawn.ReserveSittableOrSpot(intVec, this.pawn.CurJob, true);
				this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.pawn.CurJob, intVec);
				this.pawn.pather.StartPath(intVec, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x001E3364 File Offset: 0x001E1564
		private bool TryGetClosestChairFreeSittingSpot(bool skipInteractionCells, out IntVec3 cell)
		{
			Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 32f, (Thing t) => JobDriver_Reading.ValidateChair(t, this.pawn, skipInteractionCells) && t.Position.GetDangerFor(this.pawn, t.Map) == Danger.None, null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				return JobDriver_Reading.TryFindFreeSittingSpotOnThing(thing, this.pawn, skipInteractionCells, out cell);
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x001E33F6 File Offset: 0x001E15F6
		private Toil FindAdjacentReadingSurface()
		{
			Toil toil = ToilMaker.MakeToil("FindAdjacentReadingSurface");
			toil.initAction = delegate()
			{
				Map map = this.pawn.Map;
				IntVec3 position = this.pawn.Position;
				Building firstThing = this.pawn.Position.GetFirstThing(this.pawn.Map);
				if (firstThing == null || firstThing.def.building == null || !firstThing.def.building.isSittable)
				{
					this.TryFaceClosestSurface(position, map);
					return;
				}
				if (this.TryFaceClosestSurface(position, map))
				{
					return;
				}
				this.job.SetTarget(TargetIndex.B, position + firstThing.Rotation.FacingCell);
				this.pawn.jobs.curDriver.rotateToFace = TargetIndex.B;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x001E341C File Offset: 0x001E161C
		private bool TryFaceClosestSurface(IntVec3 pos, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4(i);
				IntVec3 c = pos + rot.FacingCell;
				if (c.GetSurfaceType(map) == SurfaceType.Eat)
				{
					this.job.SetTarget(TargetIndex.B, c);
					this.pawn.jobs.curDriver.rotateToFace = TargetIndex.B;
					return true;
				}
			}
			for (int j = 0; j < 4; j++)
			{
				Rot4 rot2 = new Rot4(j);
				IntVec3 c2 = pos + rot2.FacingCell;
				if (c2.GetSurfaceType(map) == SurfaceType.Item)
				{
					this.job.SetTarget(TargetIndex.B, c2);
					this.pawn.jobs.curDriver.rotateToFace = TargetIndex.B;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x001E34DC File Offset: 0x001E16DC
		private static bool ValidateChair(Thing t, Pawn pawn, bool skipInteractionCells)
		{
			IntVec3 intVec;
			return t.def.building != null && t.def.building.isSittable && JobDriver_Reading.TryFindFreeSittingSpotOnThing(t, pawn, skipInteractionCells, out intVec) && !t.Fogged() && !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, false) && t.IsSociallyProper(pawn) && !t.IsBurning() && !t.HostileTo(pawn);
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x001E3564 File Offset: 0x001E1764
		private static bool TryFindFreeSittingSpotOnThing(Thing t, Pawn pawn, bool skipInteractionCells, out IntVec3 cell)
		{
			foreach (IntVec3 intVec in t.OccupiedRect())
			{
				if ((!skipInteractionCells || !intVec.IsBuildingInteractionCell(pawn.Map)) && !intVec.Fogged(pawn.Map) && pawn.CanReserveSittableOrSpot(intVec, false))
				{
					cell = intVec;
					return true;
				}
			}
			cell = default(IntVec3);
			return false;
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x001E35F4 File Offset: 0x001E17F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.carrying, "carrying", false, false);
			Scribe_Values.Look<bool>(ref this.hasInInventory, "hasInInventory", false, false);
			Scribe_Values.Look<bool>(ref this.isLearningDesire, "wasLearningDesire", false, false);
		}

		// Token: 0x0400312A RID: 12586
		private bool hasInInventory;

		// Token: 0x0400312B RID: 12587
		private bool carrying;

		// Token: 0x0400312C RID: 12588
		private bool isLearningDesire;

		// Token: 0x0400312D RID: 12589
		private bool isReading;

		// Token: 0x0400312E RID: 12590
		public const TargetIndex BookIndex = TargetIndex.A;

		// Token: 0x0400312F RID: 12591
		public const TargetIndex SurfaceIndex = TargetIndex.B;

		// Token: 0x04003130 RID: 12592
		private const int ManualReadTicks = 5000;

		// Token: 0x04003131 RID: 12593
		private const int ChairSearchRadius = 32;

		// Token: 0x04003132 RID: 12594
		private const int UrgentJobCheckIntervalTicks = 600;
	}
}
