using System;

namespace Verse.AI
{
	// Token: 0x020008BD RID: 2237
	public class ThinkNode_ChancePerHour_InsectDigChance : ThinkNode_ChancePerHour
	{
		// Token: 0x06004597 RID: 17815 RVA: 0x001B716C File Offset: 0x001B536C
		protected override float MtbHours(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return 18f;
			}
			int num = room.IsHuge ? 9999 : room.CellCount;
			float num2 = GenMath.LerpDoubleClamped(2f, 25f, 6f, 1f, (float)num);
			return 18f / num2;
		}

		// Token: 0x04002D4E RID: 11598
		private const float BaseMtbHours = 18f;
	}
}
