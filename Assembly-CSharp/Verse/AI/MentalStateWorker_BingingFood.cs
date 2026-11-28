using System;

namespace Verse.AI
{
	// Token: 0x0200088D RID: 2189
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		// Token: 0x0600441F RID: 17439 RVA: 0x001AB81A File Offset: 0x001A9A1A
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.needs.food != null && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
