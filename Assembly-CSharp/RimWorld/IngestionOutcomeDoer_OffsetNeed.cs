using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F03 RID: 3843
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		// Token: 0x060063EF RID: 25583 RVA: 0x0023CDBC File Offset: 0x0023AFBC
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			if (pawn.needs == null)
			{
				return;
			}
			Need need;
			if (!pawn.needs.TryGetNeed(this.need, out need))
			{
				return;
			}
			float num = this.offset * (float)ingestedCount;
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref num, false, true);
			if (this.perIngested)
			{
				num *= (float)ingested.stackCount;
			}
			need.CurLevel += num;
		}

		// Token: 0x060063F0 RID: 25584 RVA: 0x0023CE22 File Offset: 0x0023B022
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			IngestionOutcomeDoer_OffsetNeed.<SpecialDisplayStats>d__5 <SpecialDisplayStats>d__ = new IngestionOutcomeDoer_OffsetNeed.<SpecialDisplayStats>d__5(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x040038C5 RID: 14533
		public NeedDef need;

		// Token: 0x040038C6 RID: 14534
		public float offset;

		// Token: 0x040038C7 RID: 14535
		public ChemicalDef toleranceChemical;

		// Token: 0x040038C8 RID: 14536
		public bool perIngested;
	}
}
