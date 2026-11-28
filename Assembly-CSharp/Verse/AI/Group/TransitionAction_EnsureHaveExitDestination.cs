using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200095A RID: 2394
	public class TransitionAction_EnsureHaveExitDestination : TransitionAction
	{
		// Token: 0x06004957 RID: 18775 RVA: 0x001CA200 File Offset: 0x001C8400
		public override void DoAction(Transition trans)
		{
			LordToil_Travel lordToil_Travel = (LordToil_Travel)trans.target;
			if (lordToil_Travel.HasDestination())
			{
				return;
			}
			Pawn pawn;
			if ((from x in lordToil_Travel.lord.ownedPawns
			where x.Spawned
			select x).TryRandomElement(out pawn))
			{
				IntVec3 destination;
				if (!CellFinder.TryFindRandomPawnExitCell(pawn, out destination))
				{
					RCellFinder.TryFindRandomPawnEntryCell(out destination, pawn.Map, 0f, false, null);
				}
				lordToil_Travel.SetDestination(destination);
			}
		}
	}
}
