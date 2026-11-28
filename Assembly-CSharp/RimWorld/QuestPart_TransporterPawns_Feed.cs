using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001106 RID: 4358
	public class QuestPart_TransporterPawns_Feed : QuestPart_TransporterPawns
	{
		// Token: 0x06006CC6 RID: 27846 RVA: 0x00262496 File Offset: 0x00260696
		public override void Process(Pawn pawn)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.food : null) != null)
			{
				pawn.needs.food.CurLevel = pawn.needs.food.MaxLevel;
			}
		}
	}
}
