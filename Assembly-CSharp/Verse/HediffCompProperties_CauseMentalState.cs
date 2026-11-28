using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200040E RID: 1038
	public class HediffCompProperties_CauseMentalState : HediffCompProperties
	{
		// Token: 0x06001EAC RID: 7852 RVA: 0x000BC31D File Offset: 0x000BA51D
		public HediffCompProperties_CauseMentalState()
		{
			this.compClass = typeof(HediffComp_CauseMentalState);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x000BC33C File Offset: 0x000BA53C
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties_CauseMentalState.<ConfigErrors>d__12 <ConfigErrors>d__ = new HediffCompProperties_CauseMentalState.<ConfigErrors>d__12(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04001813 RID: 6163
		public MentalStateDef animalMentalState;

		// Token: 0x04001814 RID: 6164
		public MentalStateDef animalMentalStateAlias;

		// Token: 0x04001815 RID: 6165
		public MentalStateDef humanMentalState;

		// Token: 0x04001816 RID: 6166
		[MustTranslate]
		public string overrideLetterLabel;

		// Token: 0x04001817 RID: 6167
		[MustTranslate]
		public string overrideLetterDesc;

		// Token: 0x04001818 RID: 6168
		public LetterDef letterDef;

		// Token: 0x04001819 RID: 6169
		public float mtbDaysToCauseMentalState;

		// Token: 0x0400181A RID: 6170
		public float minSeverity;

		// Token: 0x0400181B RID: 6171
		public bool endMentalStateOnCure = true;

		// Token: 0x0400181C RID: 6172
		public bool removeOnTriggered;

		// Token: 0x0400181D RID: 6173
		public bool forced;
	}
}
