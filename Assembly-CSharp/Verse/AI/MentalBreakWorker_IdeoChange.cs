using System;

namespace Verse.AI
{
	// Token: 0x02000859 RID: 2137
	public class MentalBreakWorker_IdeoChange : MentalBreakWorker
	{
		// Token: 0x06004325 RID: 17189 RVA: 0x001A8036 File Offset: 0x001A6236
		public override bool BreakCanOccur(Pawn pawn)
		{
			return ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode && base.BreakCanOccur(pawn);
		}
	}
}
