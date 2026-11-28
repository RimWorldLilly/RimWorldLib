using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC3 RID: 2755
	public class JobGiver_BingeFood : JobGiver_Binge
	{
		// Token: 0x060052B4 RID: 21172 RVA: 0x001ED005 File Offset: 0x001EB205
		protected override int IngestInterval(Pawn pawn)
		{
			return 1100;
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x001ED00C File Offset: 0x001EB20C
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			Thing result;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out result, out thingDef, false, true, false, true, true, true, false, false, false, false, false, FoodPreferability.RawTasty))
			{
				return result;
			}
			return null;
		}

		// Token: 0x040031E4 RID: 12772
		private const int BaseIngestInterval = 1100;
	}
}
