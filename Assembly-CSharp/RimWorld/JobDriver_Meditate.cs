using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000AA4 RID: 2724
	public class JobDriver_Meditate : JobDriver
	{
		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06005209 RID: 21001 RVA: 0x001E8EC1 File Offset: 0x001E70C1
		public LocalTargetInfo Focus
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C);
			}
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x0600520A RID: 21002 RVA: 0x001E8ED0 File Offset: 0x001E70D0
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).IsValid;
			}
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x001E8EF4 File Offset: 0x001E70F4
		protected string PsyfocusPerDayReport()
		{
			if (!this.pawn.HasPsylink)
			{
				return "";
			}
			Thing thing = this.Focus.Thing;
			float f = MeditationUtility.PsyfocusGainPerTick(this.pawn, thing) * 60000f;
			return "\n" + "PsyfocusPerDayOfMeditation".Translate(f.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x001E8F64 File Offset: 0x001E7164
		public override string GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return base.GetReport();
			}
			Thing thing = this.Focus.Thing;
			if (thing != null && !thing.Destroyed)
			{
				return "MeditatingAt".Translate() + ": " + thing.LabelShort.CapitalizeFirst() + "." + this.PsyfocusPerDayReport();
			}
			return base.GetReport() + this.PsyfocusPerDayReport();
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x001E8FE9 File Offset: 0x001E71E9
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x001E900D File Offset: 0x001E720D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Meditate.<MakeNewToils>d__16 <MakeNewToils>d__ = new JobDriver_Meditate.<MakeNewToils>d__16(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x001E901D File Offset: 0x001E721D
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (ModsConfig.RoyaltyActive && this.pawn.psychicEntropy != null)
			{
				this.job.psyfocusTargetLast = this.pawn.psychicEntropy.TargetPsyfocus;
			}
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x001E9054 File Offset: 0x001E7254
		protected void MeditationTick()
		{
			this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.018000001f, false, false);
			this.pawn.GainComfortFromCellIfPossible(1, false);
			if (this.pawn.needs.joy != null)
			{
				JoyUtility.JoyTickCheckEnd(this.pawn, 1, JoyTickFullJoyAction.None, 1f, null);
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Meditating, 0.42f);
			}
			if (ModsConfig.RoyaltyActive && this.pawn.psychicEntropy != null)
			{
				this.pawn.psychicEntropy.Notify_Meditated();
				if (this.pawn.HasPsylink && this.pawn.psychicEntropy.PsychicSensitivity > 1E-45f)
				{
					float yOffset = (float)(this.pawn.Position.x % 2 + this.pawn.Position.z % 2) / 10f;
					if (this.psyfocusMote == null || this.psyfocusMote.Destroyed)
					{
						this.psyfocusMote = MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_PsyfocusPulse, Vector3.zero, 1f, -1f);
						this.psyfocusMote.yOffset = yOffset;
					}
					this.psyfocusMote.Maintain();
					if (this.sustainer == null || this.sustainer.Ended)
					{
						this.sustainer = SoundDefOf.MeditationGainPsyfocus.TrySpawnSustainer(SoundInfo.InMap(this.pawn, MaintenanceType.PerTick));
					}
					this.sustainer.Maintain();
					this.pawn.psychicEntropy.GainPsyfocus_NewTemp(1, this.Focus.Thing);
				}
			}
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x001E9218 File Offset: 0x001E7418
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.faceDir, "faceDir", default(IntVec3), false);
		}

		// Token: 0x040031B2 RID: 12722
		protected IntVec3 faceDir;

		// Token: 0x040031B3 RID: 12723
		private Mote psyfocusMote;

		// Token: 0x040031B4 RID: 12724
		protected Sustainer sustainer;

		// Token: 0x040031B5 RID: 12725
		protected const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x040031B6 RID: 12726
		protected const TargetIndex BedInd = TargetIndex.B;

		// Token: 0x040031B7 RID: 12727
		protected const TargetIndex FocusInd = TargetIndex.C;

		// Token: 0x040031B8 RID: 12728
		public static float AnimaTreeSubplantProgressPerTick = 6.666667E-05f;

		// Token: 0x040031B9 RID: 12729
		private const int TicksBetweenMotesBase = 100;

		// Token: 0x040031BA RID: 12730
		private const int JobCheckIntervalTicks = 4000;
	}
}
