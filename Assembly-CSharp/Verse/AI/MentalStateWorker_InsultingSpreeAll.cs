using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200088F RID: 2191
	public class MentalStateWorker_InsultingSpreeAll : MentalStateWorker
	{
		// Token: 0x06004423 RID: 17443 RVA: 0x001AB8AB File Offset: 0x001A9AAB
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_InsultingSpreeAll.candidates, true);
			bool result = MentalStateWorker_InsultingSpreeAll.candidates.Count >= 2;
			MentalStateWorker_InsultingSpreeAll.candidates.Clear();
			return result;
		}

		// Token: 0x04002C3C RID: 11324
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
