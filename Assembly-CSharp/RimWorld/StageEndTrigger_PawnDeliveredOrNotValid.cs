using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001662 RID: 5730
	public class StageEndTrigger_PawnDeliveredOrNotValid : StageEndTrigger
	{
		// Token: 0x06009003 RID: 36867 RVA: 0x0034EB35 File Offset: 0x0034CD35
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_TickCondition(delegate()
			{
				foreach (TargetInfo targetInfo in foci)
				{
					Pawn pawn = targetInfo.Thing as Pawn;
					IntVec3 c = (spot.Thing != null) ? spot.Thing.OccupiedRect().CenterCell : spot.Cell;
					if (!pawn.CanReachImmediate(c, PathEndMode.Touch) && !pawn.Dead)
					{
						return false;
					}
				}
				return true;
			}, 1);
		}

		// Token: 0x06009004 RID: 36868 RVA: 0x0034EB5B File Offset: 0x0034CD5B
		public override void ExposeData()
		{
		}
	}
}
