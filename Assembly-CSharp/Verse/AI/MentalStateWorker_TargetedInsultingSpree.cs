using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000890 RID: 2192
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		// Token: 0x06004426 RID: 17446 RVA: 0x001AB8F2 File Offset: 0x001A9AF2
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_TargetedInsultingSpree.candidates, false);
			bool result = MentalStateWorker_TargetedInsultingSpree.candidates.Any<Pawn>();
			MentalStateWorker_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x04002C3D RID: 11325
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
