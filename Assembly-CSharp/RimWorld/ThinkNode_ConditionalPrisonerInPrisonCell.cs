using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D40 RID: 3392
	public class ThinkNode_ConditionalPrisonerInPrisonCell : ThinkNode_Conditional
	{
		// Token: 0x06005EFD RID: 24317 RVA: 0x0022B780 File Offset: 0x00229980
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.IsPrisoner)
			{
				return false;
			}
			Room room = pawn.GetRoom(RegionType.Set_All);
			return room != null && room.IsPrisonCell;
		}
	}
}
