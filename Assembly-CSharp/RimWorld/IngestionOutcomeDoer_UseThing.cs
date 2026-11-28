using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F06 RID: 3846
	public class IngestionOutcomeDoer_UseThing : IngestionOutcomeDoer
	{
		// Token: 0x060063F8 RID: 25592 RVA: 0x0023CE97 File Offset: 0x0023B097
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			ingested.TryGetComp<CompUsable>().UsedBy(pawn);
		}

		// Token: 0x060063F9 RID: 25593 RVA: 0x0023CEA5 File Offset: 0x0023B0A5
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			return new IngestionOutcomeDoer_UseThing.<SpecialDisplayStats>d__1(-2);
		}
	}
}
