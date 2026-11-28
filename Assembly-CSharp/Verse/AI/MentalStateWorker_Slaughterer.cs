using System;

namespace Verse.AI
{
	// Token: 0x02000897 RID: 2199
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		// Token: 0x06004439 RID: 17465 RVA: 0x001ABB67 File Offset: 0x001A9D67
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
