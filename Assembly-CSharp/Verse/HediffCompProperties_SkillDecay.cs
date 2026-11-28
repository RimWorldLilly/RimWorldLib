using System;

namespace Verse
{
	// Token: 0x02000480 RID: 1152
	public class HediffCompProperties_SkillDecay : HediffCompProperties
	{
		// Token: 0x0600201D RID: 8221 RVA: 0x000C184F File Offset: 0x000BFA4F
		public HediffCompProperties_SkillDecay()
		{
			this.compClass = typeof(HediffComp_SkillDecay);
		}

		// Token: 0x040018F4 RID: 6388
		public SimpleCurve decayPerDayPercentageLevelCurve;
	}
}
