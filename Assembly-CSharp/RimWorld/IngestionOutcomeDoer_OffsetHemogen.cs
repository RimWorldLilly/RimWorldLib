using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F05 RID: 3845
	public class IngestionOutcomeDoer_OffsetHemogen : IngestionOutcomeDoer
	{
		// Token: 0x060063F5 RID: 25589 RVA: 0x0023CE6D File Offset: 0x0023B06D
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			GeneUtility.OffsetHemogen(pawn, this.offset * (float)ingestedCount, true);
		}

		// Token: 0x060063F6 RID: 25590 RVA: 0x0023CE7F File Offset: 0x0023B07F
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			IngestionOutcomeDoer_OffsetHemogen.<SpecialDisplayStats>d__2 <SpecialDisplayStats>d__ = new IngestionOutcomeDoer_OffsetHemogen.<SpecialDisplayStats>d__2(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x040038CA RID: 14538
		public float offset;
	}
}
