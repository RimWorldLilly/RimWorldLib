using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E33 RID: 7731
	public abstract class SpecialThingFilterWorker_AllowBookSkill : SpecialThingFilterWorker
	{
		// Token: 0x0600C8CE RID: 51406 RVA: 0x0049B689 File Offset: 0x00499889
		protected SpecialThingFilterWorker_AllowBookSkill(SkillDef skill)
		{
			this.skill = skill;
		}

		// Token: 0x0600C8CF RID: 51407 RVA: 0x0049B698 File Offset: 0x00499898
		public override bool Matches(Thing t)
		{
			Book book = t as Book;
			BookOutcomeDoerGainSkillExp bookOutcomeDoerGainSkillExp;
			return book != null && book.BookComp.TryGetDoer<BookOutcomeDoerGainSkillExp>(out bookOutcomeDoerGainSkillExp) && bookOutcomeDoerGainSkillExp.Values.ContainsKey(this.skill);
		}

		// Token: 0x0600C8D0 RID: 51408 RVA: 0x0049B6D1 File Offset: 0x004998D1
		public override bool CanEverMatch(ThingDef def)
		{
			return def.HasComp<CompBook>();
		}

		// Token: 0x04007270 RID: 29296
		private readonly SkillDef skill;
	}
}
