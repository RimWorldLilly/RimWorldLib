using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F01 RID: 3841
	public abstract class IngestionOutcomeDoer
	{
		// Token: 0x060063E8 RID: 25576 RVA: 0x0023CCE9 File Offset: 0x0023AEE9
		public void DoIngestionOutcome(Pawn pawn, Thing ingested, int ingestedCount)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested, ingestedCount);
			}
		}

		// Token: 0x060063E9 RID: 25577
		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount);

		// Token: 0x060063EA RID: 25578 RVA: 0x0023CD01 File Offset: 0x0023AF01
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			return Enumerable.Empty<StatDrawEntry>();
		}

		// Token: 0x040038BE RID: 14526
		public float chance = 1f;

		// Token: 0x040038BF RID: 14527
		public bool doToGeneratedPawnIfAddicted;
	}
}
