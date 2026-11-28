using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020004B4 RID: 1204
	public class Hediff_LaborPushing : HediffWithParents
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000C6BA4 File Offset: 0x000C4DA4
		public float Progress
		{
			get
			{
				HediffComp_Disappears hediffComp_Disappears = this.TryGetComp<HediffComp_Disappears>();
				return 1f - (float)hediffComp_Disappears.ticksToDisappear / (float)Mathf.Max(1, hediffComp_Disappears.disappearsAfterTicks + this.laborDuration);
			}
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x000C6BDA File Offset: 0x000C4DDA
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Hediff_LaborPushing.<GetGizmos>d__6 <GetGizmos>d__ = new Hediff_LaborPushing.<GetGizmos>d__6(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000C6BEC File Offset: 0x000C4DEC
		public void ForceBirth(int positivityIndex, bool dontSendLetter = false)
		{
			Precept_Ritual precept_Ritual = (Precept_Ritual)this.pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
			this.debugForceBirthOutcome = precept_Ritual.outcomeEffect.def.outcomeChances.FirstOrDefault((RitualOutcomePossibility o) => o.positivityIndex == positivityIndex);
			this.preventLetter = dontSendLetter;
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000C6C60 File Offset: 0x000C4E60
		public override void PreRemoved()
		{
			base.PreRemoved();
			Lord lord = this.pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			Precept_Ritual precept_Ritual = (Precept_Ritual)this.pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
			if (((lordJob_Ritual != null) ? lordJob_Ritual.Ritual : null) == null || lordJob_Ritual.Ritual.def != PreceptDefOf.ChildBirth || lordJob_Ritual.assignments.FirstAssignedPawn("mother") != this.pawn)
			{
				float birthQualityFor = PregnancyUtility.GetBirthQualityFor(this.pawn);
				RitualOutcomePossibility outcome = this.debugForceBirthOutcome ?? ((RitualOutcomeEffectWorker_ChildBirth)precept_Ritual.outcomeEffect).GetOutcome(birthQualityFor, null);
				RitualRoleAssignments assignments = PregnancyUtility.RitualAssignmentsForBirth(precept_Ritual, this.pawn);
				float quality = birthQualityFor;
				Precept_Ritual ritual = precept_Ritual;
				GeneSet geneSet = this.geneSet;
				PregnancyUtility.ApplyBirthOutcome(outcome, quality, ritual, (geneSet != null) ? geneSet.GenesListForReading : null, base.Mother ?? this.pawn, this.pawn, base.Father, null, null, assignments, this.preventLetter);
				return;
			}
			if (lordJob_Ritual != null)
			{
				lordJob_Ritual.ApplyOutcome(1f, true, true, false);
			}
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x000C6D6C File Offset: 0x000C4F6C
		public override void TickInterval(int delta)
		{
			if (this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.progressBar == null)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
				}
				this.progressBar.EffectTick(this.pawn, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
				if (mote != null)
				{
					mote.progress = this.Progress;
					return;
				}
			}
			else
			{
				Effecter effecter = this.progressBar;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.progressBar = null;
			}
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x000C6DFD File Offset: 0x000C4FFD
		public override void PostRemoved()
		{
			base.PostRemoved();
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x000C6E1D File Offset: 0x000C501D
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x000C6E3F File Offset: 0x000C503F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.laborDuration, "laborDuration", 0, false);
		}

		// Token: 0x04001969 RID: 6505
		public int laborDuration;

		// Token: 0x0400196A RID: 6506
		private Effecter progressBar;

		// Token: 0x0400196B RID: 6507
		private RitualOutcomePossibility debugForceBirthOutcome;

		// Token: 0x0400196C RID: 6508
		private bool preventLetter;
	}
}
