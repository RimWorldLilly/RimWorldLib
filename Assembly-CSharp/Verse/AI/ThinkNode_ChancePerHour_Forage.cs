using System;

namespace Verse.AI
{
	// Token: 0x020008BE RID: 2238
	public class ThinkNode_ChancePerHour_Forage : ThinkNode_ChancePerHour
	{
		// Token: 0x06004599 RID: 17817 RVA: 0x001B71CC File Offset: 0x001B53CC
		protected override float MtbHours(Pawn pawn)
		{
			if (pawn.Map.Biome.forageability == 0f)
			{
				return 0f;
			}
			return 72f / pawn.Map.Biome.forageability;
		}
	}
}
