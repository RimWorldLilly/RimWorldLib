using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008B9 RID: 2233
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x0600458F RID: 17807 RVA: 0x001B70D9 File Offset: 0x001B52D9
		protected override float MtbHours(Pawn pawn)
		{
			return NuzzleUtility.GetNuzzleMTBHours(pawn);
		}
	}
}
