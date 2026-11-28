using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000898 RID: 2200
	public class MentalStateWorker_EntityKiller : MentalStateWorker
	{
		// Token: 0x0600443B RID: 17467 RVA: 0x001ABB85 File Offset: 0x001A9D85
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && AnomalyUtility.FindEntityOnPlatform(pawn.Map, EntityQueryType.ForSlaughter) != null;
		}
	}
}
