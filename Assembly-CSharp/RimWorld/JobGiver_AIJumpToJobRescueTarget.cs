using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE9 RID: 2793
	public class JobGiver_AIJumpToJobRescueTarget : JobGiver_AIJumpToJobTarget
	{
		// Token: 0x0600536E RID: 21358 RVA: 0x001F11B0 File Offset: 0x001EF3B0
		public override bool CanJumpToTarget(Pawn pawn, LocalTargetInfo target)
		{
			if (!base.CanJumpToTarget(pawn, target))
			{
				return false;
			}
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing is Pawn)
				{
					return true;
				}
				if (thing is Building_Bed && pawn.carryTracker != null && pawn.carryTracker.CarriedThing is Pawn)
				{
					return true;
				}
			}
			return false;
		}
	}
}
