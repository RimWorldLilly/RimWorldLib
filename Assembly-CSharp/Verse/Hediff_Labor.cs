using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B3 RID: 1203
	public class Hediff_Labor : HediffWithParents
	{
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002156 RID: 8534 RVA: 0x000C67E5 File Offset: 0x000C49E5
		public override string LabelBase
		{
			get
			{
				return "Labor".Translate();
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002157 RID: 8535 RVA: 0x000C67F8 File Offset: 0x000C49F8
		public override string LabelInBrackets
		{
			get
			{
				string labelInBrackets = base.LabelInBrackets;
				if (!labelInBrackets.NullOrEmpty())
				{
					return "LaborDilation".Translate() + ", " + labelInBrackets;
				}
				return "LaborDilation".Translate();
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002158 RID: 8536 RVA: 0x000C6844 File Offset: 0x000C4A44
		public float Progress
		{
			get
			{
				HediffComp_Disappears hediffComp_Disappears = this.TryGetComp<HediffComp_Disappears>();
				return 1f - (float)(hediffComp_Disappears.ticksToDisappear + this.laborPushingDuration) / (float)Mathf.Max(1, hediffComp_Disappears.disappearsAfterTicks + this.laborPushingDuration);
			}
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000C6881 File Offset: 0x000C4A81
		private static int GetRandomLaborPushingDuration()
		{
			return HediffDefOf.PregnancyLaborPushing.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000C6897 File Offset: 0x000C4A97
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.Severity = this.def.stages.RandomElement<HediffStage>().minSeverity;
			this.laborPushingDuration = Hediff_Labor.GetRandomLaborPushingDuration();
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000C68C6 File Offset: 0x000C4AC6
		private static TargetInfo BestBed(Pawn mother)
		{
			return mother.CurrentBed() ?? RestUtility.FindPatientBedFor(mother);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000C68DD File Offset: 0x000C4ADD
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Hediff_Labor.<GetGizmos>d__12 <GetGizmos>d__ = new Hediff_Labor.<GetGizmos>d__12(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000C68F0 File Offset: 0x000C4AF0
		public override void PreRemoved()
		{
			base.PreRemoved();
			Hediff_LaborPushing hediff_LaborPushing = (Hediff_LaborPushing)this.pawn.health.AddHediff(HediffDefOf.PregnancyLaborPushing, null, null, null);
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageColonistInFinalStagesOfLabor".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			hediff_LaborPushing.SetParents(base.Mother, base.Father, this.geneSet);
			HediffComp_Disappears hediffComp_Disappears = hediff_LaborPushing.TryGetComp<HediffComp_Disappears>();
			hediffComp_Disappears.disappearsAfterTicks = this.laborPushingDuration;
			hediffComp_Disappears.ticksToDisappear = this.laborPushingDuration;
			hediff_LaborPushing.laborDuration = this.TryGetComp<HediffComp_Disappears>().disappearsAfterTicks;
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000C69B0 File Offset: 0x000C4BB0
		private bool FoundBirthDoctor(Precept_Ritual birthRitual)
		{
			if (this.testForDoctor == null)
			{
				this.testForDoctor = birthRitual.behavior.def.roles.First((RitualRole r) => r.id == "doctor");
			}
			foreach (Pawn pawn in this.pawn.MapHeld.mapPawns.FreeColonistsSpawned)
			{
				bool flag;
				if (pawn != this.pawn && RitualRoleAssignments.PawnNotAssignableReason(pawn, this.testForDoctor, birthRitual, null, null, out flag) == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000C6A78 File Offset: 0x000C4C78
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

		// Token: 0x06002160 RID: 8544 RVA: 0x000C6B09 File Offset: 0x000C4D09
		public override void PostRemoved()
		{
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
			base.PostRemoved();
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000C6B29 File Offset: 0x000C4D29
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

		// Token: 0x06002162 RID: 8546 RVA: 0x000C6B4B File Offset: 0x000C4D4B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.laborPushingDuration, "laborPushingDuration", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.laborPushingDuration == -1)
			{
				this.laborPushingDuration = Hediff_Labor.GetRandomLaborPushingDuration();
			}
		}

		// Token: 0x04001966 RID: 6502
		private RitualRole testForDoctor;

		// Token: 0x04001967 RID: 6503
		private int laborPushingDuration;

		// Token: 0x04001968 RID: 6504
		private Effecter progressBar;
	}
}
