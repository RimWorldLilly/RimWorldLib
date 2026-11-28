using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F04 RID: 3844
	public class IngestionOutcomeDoer_OffsetPsyfocus : IngestionOutcomeDoer
	{
		// Token: 0x060063F2 RID: 25586 RVA: 0x0023CE3A File Offset: 0x0023B03A
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(this.offset * (float)ingestedCount);
		}

		// Token: 0x060063F3 RID: 25587 RVA: 0x0023CE55 File Offset: 0x0023B055
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			IngestionOutcomeDoer_OffsetPsyfocus.<SpecialDisplayStats>d__2 <SpecialDisplayStats>d__ = new IngestionOutcomeDoer_OffsetPsyfocus.<SpecialDisplayStats>d__2(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x040038C9 RID: 14537
		public float offset;
	}
}
