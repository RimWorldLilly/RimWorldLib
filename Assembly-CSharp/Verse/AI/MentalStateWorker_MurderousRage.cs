using System;

namespace Verse.AI
{
	// Token: 0x0200089A RID: 2202
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x0600443F RID: 17471 RVA: 0x001ABBCD File Offset: 0x001A9DCD
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
