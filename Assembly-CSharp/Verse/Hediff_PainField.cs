using System;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004BD RID: 1213
	public class Hediff_PainField : HediffWithComps
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x000C8990 File Offset: 0x000C6B90
		public HediffComp_PainField PainComp
		{
			get
			{
				HediffComp_PainField result;
				if ((result = this.painComp) == null)
				{
					result = (this.painComp = base.GetComp<HediffComp_PainField>());
				}
				return result;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x000C89B8 File Offset: 0x000C6BB8
		public HediffComp_Link LinkComp
		{
			get
			{
				HediffComp_Link result;
				if ((result = this.linkComp) == null)
				{
					result = (this.linkComp = base.GetComp<HediffComp_Link>());
				}
				return result;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060021C9 RID: 8649 RVA: 0x000C89DE File Offset: 0x000C6BDE
		public Pawn SourcePawn
		{
			get
			{
				return this.LinkComp.OtherPawn;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x000C89EB File Offset: 0x000C6BEB
		public override float PainOffset
		{
			get
			{
				return this.Severity;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x000C89F3 File Offset: 0x000C6BF3
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent();
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x000C8A10 File Offset: 0x000C6C10
		public override bool ShouldRemove
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompShouldRemove)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060021CD RID: 8653 RVA: 0x000C8A51 File Offset: 0x000C6C51
		public override float Severity
		{
			get
			{
				return this.painSeverity;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060021CE RID: 8654 RVA: 0x000C8A5C File Offset: 0x000C6C5C
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.def.Description);
				if (this.SourcePawn == null)
				{
					return stringBuilder.ToString();
				}
				CompActivity comp = this.SourcePawn.GetComp<CompActivity>();
				if (comp != null)
				{
					float f = this.PainComp.Props.activityMultiplier.Evaluate(comp.ActivityLevel);
					StringBuilder stringBuilder2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
					defaultInterpolatedStringHandler.AppendLiteral("\n");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ActivityLevelDesc".Translate().CapitalizeFirst());
					defaultInterpolatedStringHandler.AppendLiteral(": x");
					defaultInterpolatedStringHandler.AppendFormatted(f.ToStringPercent("0"));
					stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (this.PainComp.Props.disableWhenSuppressed && (this.SourcePawn.health.hediffSet.IsPsychicallySuppressed || this.pawn.health.hediffSet.IsPsychicallySuppressed))
				{
					StringBuilder stringBuilder3 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(5, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("\n");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("PsychicallySuppressedDesc".Translate().CapitalizeFirst());
					defaultInterpolatedStringHandler2.AppendLiteral(": x0");
					stringBuilder3.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x000C8BA5 File Offset: 0x000C6DA5
		public override void PostMake()
		{
			base.PostMake();
			if (this.PainComp == null)
			{
				Log.ErrorOnce("Attempted to create a pain field hediff but missing the HediffComp_PainField component", 94756930);
			}
			if (this.LinkComp == null)
			{
				Log.ErrorOnce("Attempted to create a pain field hediff but missing the Hediff_Link component", 47836289);
			}
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000C8BDB File Offset: 0x000C6DDB
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Pain field"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000C8C04 File Offset: 0x000C6E04
		public override void PostTickInterval(int delta)
		{
			float num = this.painSeverity;
			this.goal = this.CalculateSeverity(delta);
			base.PostTickInterval(delta);
			this.painSeverity = Mathf.SmoothDamp(this.painSeverity, this.goal, ref this.vel, 0.25f, 10f, 0.016666668f);
			this.painSeverity = Mathf.Clamp01(this.painSeverity);
			if (Math.Abs(num - this.painSeverity) > Mathf.Epsilon)
			{
				this.pawn.health.hediffSet.DirtyPainCache();
				this.pawn.health.CheckForStateChange(null, this);
			}
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000C8CAC File Offset: 0x000C6EAC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.painSeverity, "painSeverity", 0f, false);
			Scribe_Values.Look<float>(ref this.goal, "goal", 0f, false);
			Scribe_Values.Look<float>(ref this.vel, "vel", 0f, false);
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000C8D04 File Offset: 0x000C6F04
		private float CalculateSeverity(int delta)
		{
			if (this.SourcePawn == null)
			{
				return 0f;
			}
			if (this.PainComp.Props.disableWhenSuppressed && (this.SourcePawn.health.hediffSet.IsPsychicallySuppressed || this.pawn.health.hediffSet.IsPsychicallySuppressed))
			{
				return 0f;
			}
			if (this.psychicSensitivityCached < 0f || this.pawn.IsHashIntervalTick(300, delta))
			{
				this.psychicSensitivityCached = this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
			}
			float num = this.pawn.PositionHeld.DistanceTo(this.SourcePawn.PositionHeld);
			float num2 = 0f;
			if (num < this.PainComp.Props.painDistance + 0.1f)
			{
				num2 = this.PainComp.Props.painInRange;
			}
			CompActivity compActivity;
			if (this.SourcePawn.TryGetComp(out compActivity))
			{
				if (compActivity.IsActive)
				{
					num2 = Mathf.Max(this.PainComp.Props.activatedMinimum, num2);
				}
				num2 *= this.PainComp.Props.activityMultiplier.Evaluate(compActivity.ActivityLevel);
			}
			num2 *= this.psychicSensitivityCached * this.PainComp.Props.psychicSensitivityMultiplier;
			return Mathf.Clamp01(num2);
		}

		// Token: 0x04001993 RID: 6547
		public float painSeverity;

		// Token: 0x04001994 RID: 6548
		public float goal;

		// Token: 0x04001995 RID: 6549
		public float vel;

		// Token: 0x04001996 RID: 6550
		private const float SeveritySmoothTime = 0.25f;

		// Token: 0x04001997 RID: 6551
		private HediffComp_PainField painComp;

		// Token: 0x04001998 RID: 6552
		private HediffComp_Link linkComp;

		// Token: 0x04001999 RID: 6553
		private float psychicSensitivityCached = -1f;
	}
}
