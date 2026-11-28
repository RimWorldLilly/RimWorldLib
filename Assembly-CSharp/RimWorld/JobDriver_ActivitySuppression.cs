using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A12 RID: 2578
	public class JobDriver_ActivitySuppression : JobDriver
	{
		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06004DDD RID: 19933 RVA: 0x001DBE34 File Offset: 0x001DA034
		private Building_HoldingPlatform Platform
		{
			get
			{
				return base.TargetThingA as Building_HoldingPlatform;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06004DDE RID: 19934 RVA: 0x001DBE41 File Offset: 0x001DA041
		private Thing ThingToSuppress
		{
			get
			{
				Building_HoldingPlatform platform = this.Platform;
				return ((platform != null) ? platform.HeldPawn : null) ?? base.TargetThingA;
			}
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x001DBE5F File Offset: 0x001DA05F
		protected override string ReportStringProcessed(string str)
		{
			return JobUtility.GetResolvedJobReport(str, LocalTargetInfo.Invalid, LocalTargetInfo.Invalid, this.ThingToSuppress);
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x001DBE7C File Offset: 0x001DA07C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Platform != null)
			{
				return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.ThingToSuppress, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.ThingToSuppress, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x001DBEF9 File Offset: 0x001DA0F9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ActivitySuppression.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_ActivitySuppression.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x001DBF0C File Offset: 0x001DA10C
		private Toil TrySuppress(Thing thingToSuppress)
		{
			Toil toil = ToilMaker.MakeToil("TrySuppress");
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = toil.actor;
				if (thingToSuppress == null)
				{
					Log.Error("Tried to execute activity suppression toil but thingToSuppress was null");
					actor.jobs.EndCurrentJob(JobCondition.Errored, true, true);
					return;
				}
				CompActivity compActivity = thingToSuppress.TryGetComp<CompActivity>();
				float num = actor.GetStatValue(StatDefOf.ActivitySuppressionRate, true, -1) / 2500f * (float)delta;
				compActivity.AdjustActivity(-num);
				this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f, false, false);
				if (compActivity.ActivityLevel < thingToSuppress.TryGetComp<CompActivity>().suppressIfAbove - 0.1f && actor.IsHashIntervalTick(4000, delta))
				{
					actor.jobs.CheckForJobOverride(0f, true);
				}
				CompObelisk compObelisk = this.ThingToSuppress.TryGetComp<CompObelisk>();
				if (compObelisk == null)
				{
					return;
				}
				compObelisk.Notify_InteractedTick(this.pawn, delta);
			};
			toil.WithProgressBar(TargetIndex.A, delegate
			{
				if (thingToSuppress == null)
				{
					return 1f;
				}
				return 1f - thingToSuppress.TryGetComp<CompActivity>().ActivityLevel;
			}, false, -0.5f, false);
			toil.WithEffect(EffecterDefOf.ActivitySuppression, () => this.ThingToSuppress.SpawnedParentOrMe, null);
			toil.AddEndCondition(delegate
			{
				if (thingToSuppress == null)
				{
					Log.Error("Tried to execute activity suppression toil but thingToSuppress was null");
					return JobCondition.Errored;
				}
				if (thingToSuppress.TryGetComp<CompActivity>().ActivityLevel < 0.01f)
				{
					return JobCondition.Succeeded;
				}
				return JobCondition.Ongoing;
			});
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.activeSkill = (() => SkillDefOf.Intellectual);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x04003084 RID: 12420
		private const TargetIndex ThingToSuppressOrPlatformIndex = TargetIndex.A;

		// Token: 0x04003085 RID: 12421
		private const TargetIndex AdjacentCellIndex = TargetIndex.B;
	}
}
