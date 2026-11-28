using System;

namespace Verse
{
	// Token: 0x02000496 RID: 1174
	public class HediffComp_SeverityPerSecond : HediffComp
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x000C317B File Offset: 0x000C137B
		private HediffCompProperties_SeverityPerSecond Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerSecond)this.props;
			}
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000C3188 File Offset: 0x000C1388
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerSecond = this.Props.CalculateSeverityPerSecond();
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000C31A2 File Offset: 0x000C13A2
		public override void CompPostTick(ref float severityAdjustment)
		{
			float num = severityAdjustment;
			float num2 = this.severityPerSecond / 60f;
			HediffStage curStage = this.parent.CurStage;
			severityAdjustment = num + num2 * ((curStage != null) ? curStage.severityGainFactor : 1f);
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x000C31D4 File Offset: 0x000C13D4
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerSecond, "severityPerDay", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.severityPerSecond == 0f && this.Props.severityPerSecond != 0f && this.Props.severityPerSecondRange == FloatRange.Zero)
			{
				this.severityPerSecond = this.Props.CalculateSeverityPerSecond();
				Log.Warning("Hediff " + this.parent.Label + " had severityPerSecond not matching parent.");
			}
		}

		// Token: 0x04001929 RID: 6441
		protected float severityPerSecond;
	}
}
