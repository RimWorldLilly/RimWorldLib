using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000481 RID: 1153
	public class HediffComp_SkillDecay : HediffComp
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x000C1867 File Offset: 0x000BFA67
		public HediffCompProperties_SkillDecay Props
		{
			get
			{
				return (HediffCompProperties_SkillDecay)this.props;
			}
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x000C1874 File Offset: 0x000BFA74
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			Pawn_SkillTracker skills = base.Pawn.skills;
			if (skills == null)
			{
				return;
			}
			for (int i = 0; i < skills.skills.Count; i++)
			{
				SkillRecord skillRecord = skills.skills[i];
				float num = this.parent.Severity * this.Props.decayPerDayPercentageLevelCurve.Evaluate((float)skillRecord.GetLevel(false));
				float num2 = skillRecord.XpRequiredForLevelUp * num / 60000f * (float)delta;
				skillRecord.Learn(-num2, false, false);
			}
		}
	}
}
