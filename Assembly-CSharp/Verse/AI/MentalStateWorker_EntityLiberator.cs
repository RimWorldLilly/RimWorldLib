using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000899 RID: 2201
	public class MentalStateWorker_EntityLiberator : MentalStateWorker
	{
		// Token: 0x0600443D RID: 17469 RVA: 0x001ABBA9 File Offset: 0x001A9DA9
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && AnomalyUtility.FindEntityOnPlatform(pawn.Map, EntityQueryType.ForRelease) != null;
		}
	}
}
