using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A7A RID: 2682
	public class JobDriver_StudyInteract : JobDriver
	{
		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x060050D6 RID: 20694 RVA: 0x001E53F8 File Offset: 0x001E35F8
		private Building_HoldingPlatform Platform
		{
			get
			{
				return base.TargetThingA as Building_HoldingPlatform;
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x060050D7 RID: 20695 RVA: 0x001E5405 File Offset: 0x001E3605
		private Thing ThingToStudy
		{
			get
			{
				Building_HoldingPlatform platform = this.Platform;
				return ((platform != null) ? platform.HeldPawn : null) ?? base.TargetThingA;
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x060050D8 RID: 20696 RVA: 0x001E5423 File Offset: 0x001E3623
		private CompStudiable StudyComp
		{
			get
			{
				Thing thingToStudy = this.ThingToStudy;
				if (thingToStudy == null)
				{
					return null;
				}
				return thingToStudy.TryGetComp<CompStudiable>();
			}
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x001E5438 File Offset: 0x001E3638
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Platform != null)
			{
				return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.ThingToStudy, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(base.TargetThingA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x001E54B5 File Offset: 0x001E36B5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_StudyInteract.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_StudyInteract.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x001E54C8 File Offset: 0x001E36C8
		private Toil StudyPawn(TargetIndex pawnIndex, int duration)
		{
			Toil study = ToilMaker.MakeToil("Study pawn");
			study.initAction = delegate()
			{
				Pawn actor = study.actor;
				actor.pather.StopDead();
				Pawn pawn = actor.CurJob.GetTarget(pawnIndex).Thing as Pawn;
				if (pawn != null)
				{
					PawnUtility.ForceWait(pawn, duration, study.actor, false, false);
					actor.interactions.TryInteractWith(pawn, InteractionDefOf.PrisonerStudyAnomaly);
				}
			};
			study.tickIntervalAction = delegate(int delta)
			{
				study.actor.rotationTracker.FaceTarget(study.actor.CurJob.GetTarget(pawnIndex));
			};
			study.handlingFacing = true;
			study.defaultCompleteMode = ToilCompleteMode.Delay;
			study.defaultDuration = duration;
			study.WithProgressBarToilDelay(pawnIndex, false, -0.5f);
			return study;
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x001E5570 File Offset: 0x001E3770
		private void DoStudyInteraction()
		{
			float anomalyKnowledgeAmount = 0f;
			if (ModsConfig.AnomalyActive)
			{
				anomalyKnowledgeAmount = this.StudyComp.AdjustedAnomalyKnowledgePerStudy * this.pawn.GetStatValue(StatDefOf.StudyEfficiency, true, -1);
			}
			this.StudyComp.Study(this.pawn, 0.87f, anomalyKnowledgeAmount);
			this.studyInteractions++;
			if (ModsConfig.AnomalyActive)
			{
				Pawn pawn = this.ThingToStudy as Pawn;
				if (pawn != null)
				{
					pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
					pawn.mindState.interactionsToday++;
				}
			}
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x001E560C File Offset: 0x001E380C
		public override bool? IsSameJobAs(Job j)
		{
			return new bool?(j.targetA == base.TargetThingA);
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x001E5629 File Offset: 0x001E3829
		protected override string ReportStringProcessed(string str)
		{
			return JobUtility.GetResolvedJobReport(str, this.ThingToStudy);
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x001E563C File Offset: 0x001E383C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.studyInteractions, "studyInteractions", 0, false);
		}

		// Token: 0x04003162 RID: 12642
		private const TargetIndex ThingToStudyIndex = TargetIndex.A;

		// Token: 0x04003163 RID: 12643
		private const TargetIndex AdjacentCellIndex = TargetIndex.B;

		// Token: 0x04003164 RID: 12644
		private const int BaseDurationTicks = 600;

		// Token: 0x04003165 RID: 12645
		private const int PawnTargetStudyDurationFactor = 2;

		// Token: 0x04003166 RID: 12646
		public const float ElectroharvesterFactor = 0.5f;

		// Token: 0x04003167 RID: 12647
		private int studyInteractions;
	}
}
