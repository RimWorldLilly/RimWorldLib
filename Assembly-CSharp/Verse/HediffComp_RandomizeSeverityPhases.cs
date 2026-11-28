using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000491 RID: 1169
	public class HediffComp_RandomizeSeverityPhases : HediffComp_Randomizer
	{
		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600206E RID: 8302 RVA: 0x000C2B5F File Offset: 0x000C0D5F
		public HediffCompProperties_RandomizeSeverityPhases Props
		{
			get
			{
				return (HediffCompProperties_RandomizeSeverityPhases)this.props;
			}
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x000C2B6C File Offset: 0x000C0D6C
		public HediffCompProperties_RandomizeSeverityPhases.Phase CurrentOption()
		{
			int? num = this.randomPhaseIndex;
			int num2 = 0;
			if (num.GetValueOrDefault() >= num2 & num != null)
			{
				num = this.randomPhaseIndex;
				List<HediffCompProperties_RandomizeSeverityPhases.Phase> phases = this.Props.phases;
				int? num3 = (phases != null) ? new int?(phases.Count) : null;
				if (num.GetValueOrDefault() < num3.GetValueOrDefault() & (num != null & num3 != null))
				{
					return this.Props.phases[this.randomPhaseIndex.Value];
				}
			}
			return null;
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x000C2C04 File Offset: 0x000C0E04
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.Randomize();
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x000C2C14 File Offset: 0x000C0E14
		public override void Randomize()
		{
			if (this.Props.phases == null || this.Props.phases.Count == 0)
			{
				return;
			}
			int num = Rand.Range(0, this.Props.phases.Count);
			if (this.randomPhaseIndex != null)
			{
				int num2 = num;
				int? num3 = this.randomPhaseIndex;
				if (!(num2 == num3.GetValueOrDefault() & num3 != null) && !this.Props.notifyMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
				{
					Messages.Message(this.Props.notifyMessage.Formatted(this.parent.pawn.Named("PAWN"), this.Props.phases[this.randomPhaseIndex.Value].labelPrefix, this.Props.phases[num].labelPrefix), this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			}
			this.randomPhaseIndex = new int?(num);
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000C2D44 File Offset: 0x000C0F44
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int?>(ref this.randomPhaseIndex, "randomPhaseIndex", null, false);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000C2D71 File Offset: 0x000C0F71
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			float num = severityAdjustment;
			HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
			severityAdjustment = num + ((phase != null) ? phase.severityPerDay : 0f) / 60000f;
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000C2D9C File Offset: 0x000C0F9C
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (!base.Pawn.Dead)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "severity/day: ";
				HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
				stringBuilder2.AppendLine(str + ((phase != null) ? phase.severityPerDay : 0f).ToString("F3"));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x000C2E08 File Offset: 0x000C1008
		public override string CompLabelPrefix
		{
			get
			{
				HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
				if (phase == null)
				{
					return null;
				}
				return phase.labelPrefix;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x000C2E1B File Offset: 0x000C101B
		public override string CompDescriptionExtra
		{
			get
			{
				HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
				if (phase == null)
				{
					return null;
				}
				return phase.descriptionExtra;
			}
		}

		// Token: 0x0400191D RID: 6429
		private int? randomPhaseIndex;
	}
}
