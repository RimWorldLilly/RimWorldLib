using System;

namespace Verse.AI
{
	// Token: 0x020008BB RID: 2235
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		// Token: 0x06004593 RID: 17811 RVA: 0x001B711A File Offset: 0x001B531A
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
