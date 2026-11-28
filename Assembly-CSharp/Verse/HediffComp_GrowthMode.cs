using System;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000448 RID: 1096
	public class HediffComp_GrowthMode : HediffComp_SeverityModifierBase
	{
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x000BED3E File Offset: 0x000BCF3E
		public HediffCompProperties_GrowthMode Props
		{
			get
			{
				return (HediffCompProperties_GrowthMode)this.props;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001F63 RID: 8035 RVA: 0x000BED4B File Offset: 0x000BCF4B
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.growthMode.GetLabel();
			}
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000BED58 File Offset: 0x000BCF58
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<HediffGrowthMode>(ref this.growthMode, "growthMode", HediffGrowthMode.Growing, false);
			Scribe_Values.Look<float>(ref this.severityPerDayGrowingRandomFactor, "severityPerDayGrowingRandomFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.severityPerDayRemissionRandomFactor, "severityPerDayRemissionRandomFactor", 1f, false);
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000BEDAC File Offset: 0x000BCFAC
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.growthMode = ((HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))).RandomElement<HediffGrowthMode>();
			this.severityPerDayGrowingRandomFactor = this.Props.severityPerDayGrowingRandomFactor.RandomInRange;
			this.severityPerDayRemissionRandomFactor = this.Props.severityPerDayRemissionRandomFactor.RandomInRange;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000BEE0B File Offset: 0x000BD00B
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			base.CompPostTickInterval(ref severityAdjustment, delta);
			if (base.Pawn.IsHashIntervalTick(5000, delta) && Rand.MTBEventOccurs(100f, 60000f, 5000f))
			{
				this.ChangeGrowthMode();
			}
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000BEE44 File Offset: 0x000BD044
		public override float SeverityChangePerDay()
		{
			switch (this.growthMode)
			{
			case HediffGrowthMode.Growing:
			{
				float num = this.Props.severityPerDayGrowing * this.severityPerDayGrowingRandomFactor;
				if (ModsConfig.BiotechActive && this.parent.def == HediffDefOf.Carcinoma)
				{
					num *= base.Pawn.GetStatValue(StatDefOf.CancerRate, true, -1);
				}
				return num;
			}
			case HediffGrowthMode.Stable:
				return 0f;
			case HediffGrowthMode.Remission:
				return this.Props.severityPerDayRemission * this.severityPerDayRemissionRandomFactor;
			default:
				throw new NotImplementedException("GrowthMode");
			}
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x000BEED4 File Offset: 0x000BD0D4
		private void ChangeGrowthMode()
		{
			this.growthMode = (from x in (HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))
			where x != this.growthMode
			select x).RandomElement<HediffGrowthMode>();
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				switch (this.growthMode)
				{
				case HediffGrowthMode.Growing:
					Messages.Message("DiseaseGrowthModeChanged_Growing".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					return;
				case HediffGrowthMode.Stable:
					Messages.Message("DiseaseGrowthModeChanged_Stable".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NeutralEvent, true);
					return;
				case HediffGrowthMode.Remission:
					Messages.Message("DiseaseGrowthModeChanged_Remission".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000BF03C File Offset: 0x000BD23C
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			stringBuilder.AppendLine("severity: " + this.parent.Severity.ToString("F3") + ((this.parent.Severity >= base.Def.maxSeverity) ? " (reached max)" : ""));
			stringBuilder.AppendLine("severityPerDayGrowingRandomFactor: " + this.severityPerDayGrowingRandomFactor.ToString("0.##"));
			stringBuilder.AppendLine("severityPerDayRemissionRandomFactor: " + this.severityPerDayRemissionRandomFactor.ToString("0.##"));
			return stringBuilder.ToString();
		}

		// Token: 0x04001883 RID: 6275
		private const int CheckGrowthModeChangeInterval = 5000;

		// Token: 0x04001884 RID: 6276
		private const float GrowthModeChangeMtbDays = 100f;

		// Token: 0x04001885 RID: 6277
		public HediffGrowthMode growthMode;

		// Token: 0x04001886 RID: 6278
		private float severityPerDayGrowingRandomFactor = 1f;

		// Token: 0x04001887 RID: 6279
		private float severityPerDayRemissionRandomFactor = 1f;
	}
}
