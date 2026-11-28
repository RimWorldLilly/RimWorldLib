using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000893 RID: 2195
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x0600442F RID: 17455 RVA: 0x001ABA28 File Offset: 0x001A9C28
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_TargetedTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalStateWorker_TargetedTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04002C40 RID: 11328
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
