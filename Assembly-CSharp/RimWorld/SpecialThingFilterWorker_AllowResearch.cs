using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E30 RID: 7728
	public abstract class SpecialThingFilterWorker_AllowResearch : SpecialThingFilterWorker
	{
		// Token: 0x0600C8C9 RID: 51401 RVA: 0x0049B61A File Offset: 0x0049981A
		protected SpecialThingFilterWorker_AllowResearch(ResearchTabDef tab)
		{
			this.tab = tab;
		}

		// Token: 0x0600C8CA RID: 51402 RVA: 0x0049B62C File Offset: 0x0049982C
		public override bool Matches(Thing t)
		{
			Book book = t as Book;
			ReadingOutcomeDoerGainResearch readingOutcomeDoerGainResearch;
			return book != null && book.BookComp.TryGetDoer<ReadingOutcomeDoerGainResearch>(out readingOutcomeDoerGainResearch) && readingOutcomeDoerGainResearch.Props.tab == this.tab;
		}

		// Token: 0x0600C8CB RID: 51403 RVA: 0x0049B667 File Offset: 0x00499867
		public override bool CanEverMatch(ThingDef def)
		{
			return def.HasComp<CompBook>();
		}

		// Token: 0x0400726F RID: 29295
		private readonly ResearchTabDef tab;
	}
}
