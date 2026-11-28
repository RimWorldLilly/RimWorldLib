using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B11 RID: 2833
	public class JobGiver_SpectateInCircleDuty : JobGiver_SpectateDutySpectateRect
	{
		// Token: 0x060053E9 RID: 21481 RVA: 0x001F3B20 File Offset: 0x001F1D20
		protected override bool TryFindSpot(Pawn pawn, PawnDuty duty, out IntVec3 spot)
		{
			if (!SpectatorCellFinder.TryFindCircleSpectatorCellFor(pawn, duty.spectateRect, (float)duty.spectateDistance.min, (float)duty.spectateDistance.max, pawn.Map, out spot, null, new Func<IntVec3, Pawn, Map, bool>(RitualUtility.GoodSpectateCellForRitual)))
			{
				return base.TryFindSpot(pawn, duty, out spot);
			}
			return spot.IsValid;
		}
	}
}
