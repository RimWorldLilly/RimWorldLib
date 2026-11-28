using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000894 RID: 2196
	public class MentalStateWorker_SadisticRageTantrum : MentalStateWorker
	{
		// Token: 0x06004432 RID: 17458 RVA: 0x001ABA8C File Offset: 0x001A9C8C
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_SadisticRageTantrum.tmpThings, (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(pawn, x), 0, 40);
			bool result = MentalStateWorker_SadisticRageTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04002C41 RID: 11329
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
