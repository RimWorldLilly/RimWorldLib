using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000891 RID: 2193
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		// Token: 0x06004429 RID: 17449 RVA: 0x001AB934 File Offset: 0x001A9B34
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TantrumAll.tmpThings, null, 0, 40);
			bool result = MentalStateWorker_TantrumAll.tmpThings.Count >= 2;
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			return result;
		}

		// Token: 0x04002C3E RID: 11326
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
