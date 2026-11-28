using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000825 RID: 2085
	public class JobDriver_CastAbility : JobDriver_CastVerbOnce
	{
		// Token: 0x0600417C RID: 16764 RVA: 0x0019F753 File Offset: 0x0019D953
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CastAbility.<MakeNewToils>d__0 <MakeNewToils>d__ = new JobDriver_CastAbility.<MakeNewToils>d__0(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x0019F763 File Offset: 0x0019D963
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			Ability ability = this.job.ability;
			if (ability == null)
			{
				return;
			}
			ability.Notify_StartedCasting();
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x0019F780 File Offset: 0x0019D980
		public override string GetReport()
		{
			string text;
			if (this.job.ability != null && !this.job.ability.def.targetRequired)
			{
				text = "UsingVerbNoTarget".Translate(this.job.verbToUse.ReportLabel);
			}
			else
			{
				text = base.GetReport();
			}
			if (this.job.ability != null && this.job.ability.def.showCastingProgressBar)
			{
				text += " " + "DurationLeft".Translate(this.job.verbToUse.WarmupTicksLeft.ToStringSecondsFromTicks()) + ".";
			}
			return text;
		}
	}
}
