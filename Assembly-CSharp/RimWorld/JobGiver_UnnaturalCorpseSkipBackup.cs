using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B2B RID: 2859
	public class JobGiver_UnnaturalCorpseSkipBackup : JobGiver_UnnaturalCorpseSkip
	{
		// Token: 0x0600543A RID: 21562 RVA: 0x001F5425 File Offset: 0x001F3625
		protected override bool TryGetSkipCell(Pawn pawn, Pawn victim, out IntVec3 cell)
		{
			if (base.TryGetCellAlongPath(pawn, victim, 64, out cell))
			{
				return true;
			}
			if (base.TryGetNearbySkipCell(pawn, victim.PositionHeld, victim, 48, out cell))
			{
				return true;
			}
			cell = victim.Position;
			return true;
		}
	}
}
