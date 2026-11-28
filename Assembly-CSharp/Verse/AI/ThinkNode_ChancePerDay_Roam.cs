using System;

namespace Verse.AI
{
	// Token: 0x020008BA RID: 2234
	public class ThinkNode_ChancePerDay_Roam : ThinkNode_ChancePerHour
	{
		// Token: 0x06004591 RID: 17809 RVA: 0x001B70EC File Offset: 0x001B52EC
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RoamMtbDays.GetValueOrDefault(0f) * 24f;
		}
	}
}
