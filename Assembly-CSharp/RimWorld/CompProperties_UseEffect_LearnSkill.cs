using System;

namespace RimWorld
{
	// Token: 0x02001A40 RID: 6720
	public class CompProperties_UseEffect_LearnSkill : CompProperties_UseEffect
	{
		// Token: 0x0600A87D RID: 43133 RVA: 0x003BDC17 File Offset: 0x003BBE17
		public CompProperties_UseEffect_LearnSkill()
		{
			this.compClass = typeof(CompUseEffect_LearnSkill);
		}

		// Token: 0x040062FC RID: 25340
		public SkillDef skill;

		// Token: 0x040062FD RID: 25341
		public float xpGainAmount = 50000f;
	}
}
