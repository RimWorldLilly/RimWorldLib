using System;

namespace RimWorld
{
	// Token: 0x02001A3B RID: 6715
	public class CompProperties_UseEffectGiveQuest : CompProperties_UseEffect
	{
		// Token: 0x0600A870 RID: 43120 RVA: 0x003BD792 File Offset: 0x003BB992
		public CompProperties_UseEffectGiveQuest()
		{
			this.compClass = typeof(CompUseEffect_GiveQuest);
		}

		// Token: 0x040062F0 RID: 25328
		public QuestScriptDef quest;

		// Token: 0x040062F1 RID: 25329
		public bool sendLetterQuestAvailable = true;

		// Token: 0x040062F2 RID: 25330
		public bool skipIfMechlinkAlreadySentMechs;

		// Token: 0x040062F3 RID: 25331
		public bool? discovered;
	}
}
