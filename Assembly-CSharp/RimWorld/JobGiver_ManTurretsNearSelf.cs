using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B21 RID: 2849
	public class JobGiver_ManTurretsNearSelf : JobGiver_ManTurrets
	{
		// Token: 0x06005417 RID: 21527 RVA: 0x001F479E File Offset: 0x001F299E
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
