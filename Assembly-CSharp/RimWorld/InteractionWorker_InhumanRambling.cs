using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001519 RID: 5401
	public class InteractionWorker_InhumanRambling : InteractionWorker
	{
		// Token: 0x0600844D RID: 33869 RVA: 0x002F99B9 File Offset: 0x002F7BB9
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return 0f;
			}
			if (initiator.MentalStateDef != MentalStateDefOf.HumanityBreak)
			{
				return 0f;
			}
			return 999f;
		}

		// Token: 0x040051B4 RID: 20916
		private const float SelectionWeight_HumanityBreak = 999f;
	}
}
