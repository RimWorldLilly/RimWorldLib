using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A48 RID: 2632
	public class JobDriver_InteractThing : JobDriver
	{
		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06004F73 RID: 20339 RVA: 0x001E0E34 File Offset: 0x001DF034
		private Thing InteractableThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06004F74 RID: 20340 RVA: 0x001E0E58 File Offset: 0x001DF058
		private Thing OptionalThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06004F75 RID: 20341 RVA: 0x001E0E79 File Offset: 0x001DF079
		private bool HasOptionalThing
		{
			get
			{
				return this.OptionalThing != null;
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06004F76 RID: 20342 RVA: 0x001E0E84 File Offset: 0x001DF084
		private CompInteractable Interactable
		{
			get
			{
				if (this.job.interactableIndex == -1)
				{
					return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompInteractable>();
				}
				ThingWithComps thingWithComps = this.job.GetTarget(TargetIndex.A).Thing as ThingWithComps;
				if (thingWithComps == null)
				{
					return null;
				}
				return thingWithComps.GetComps<CompInteractable>().ToList<CompInteractable>()[this.job.interactableIndex];
			}
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x001E0EF4 File Offset: 0x001DF0F4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (base.TargetThingB != null)
			{
				this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			}
			return (base.TargetThingB == null || this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false)) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x001E0F79 File Offset: 0x001DF179
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_InteractThing.<MakeNewToils>d__12 <MakeNewToils>d__ = new JobDriver_InteractThing.<MakeNewToils>d__12(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x001E0F8C File Offset: 0x001DF18C
		public override string GetReport()
		{
			if (base.CurToilIndex != 1 || this.ticksLeftThisToil <= 0)
			{
				if (string.IsNullOrEmpty(this.Interactable.Props.activatingStringPending))
				{
					return base.GetReport();
				}
				return this.Interactable.Props.activatingStringPending.Formatted(this.Interactable.parent.LabelShort);
			}
			else
			{
				int ticksToActivate = this.Interactable.TicksToActivate;
				int num = (this.ticksLeftThisToil < 0) ? ticksToActivate : this.ticksLeftThisToil;
				float value = (float)(ticksToActivate - (ticksToActivate - num)) / 60f;
				if (string.IsNullOrEmpty(this.Interactable.Props.activatingString))
				{
					return base.GetReport();
				}
				return this.Interactable.Props.activatingString.Formatted(this.Interactable.parent.LabelShort, value);
			}
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x001E1078 File Offset: 0x001DF278
		private Toil WaitForActivate(int remainingTicks, int totalTicks)
		{
			Toil toil = ToilMaker.MakeToil("WaitForActivate").FailOn(() => !this.Interactable.CanInteract(this.pawn, true));
			if (this.Interactable.Props.maintainProgress)
			{
				toil.WithProgressBar(TargetIndex.A, () => this.Interactable.progress, false, -0.5f, false);
			}
			else
			{
				toil.WithProgressBarToilDelay(TargetIndex.A, remainingTicks, false, -0.5f);
			}
			Toil toil5 = toil;
			toil5.initAction = (Action)Delegate.Combine(toil5.initAction, new Action(delegate()
			{
				this.Interactable.Notify_InteractionStarted();
				toil.actor.pather.StopDead();
			}));
			Toil toil2 = toil;
			toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(this.TargetA);
			}));
			if (this.Interactable.Props.maintainProgress)
			{
				Toil toil3 = toil;
				toil3.tickIntervalAction = (Action<int>)Delegate.Combine(toil3.tickIntervalAction, new Action<int>(delegate(int delta)
				{
					this.Interactable.progress = 1f - (float)this.ticksLeftThisToil / (float)totalTicks;
				}));
			}
			if (this.Interactable.Props.forceNormalSpeedOnInteracting)
			{
				Toil toil4 = toil;
				toil4.tickAction = (Action)Delegate.Combine(toil4.tickAction, new Action(delegate()
				{
					Find.TickManager.slower.SignalForceNormalSpeed();
				}));
			}
			toil.AddFinishAction(delegate
			{
				this.Interactable.Notify_InteractionEnded();
			});
			toil.handlingFacing = true;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultDuration = remainingTicks;
			return toil;
		}

		// Token: 0x040030F9 RID: 12537
		private const TargetIndex InteractableIndex = TargetIndex.A;

		// Token: 0x040030FA RID: 12538
		private const TargetIndex OptionalItemIndex = TargetIndex.B;

		// Token: 0x040030FB RID: 12539
		private const TargetIndex SpawnedOrParentIndex = TargetIndex.C;
	}
}
