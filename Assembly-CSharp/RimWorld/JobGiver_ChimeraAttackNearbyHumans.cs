using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B34 RID: 2868
	public class JobGiver_ChimeraAttackNearbyHumans : ThinkNode_JobGiver
	{
		// Token: 0x06005457 RID: 21591 RVA: 0x001F59B8 File Offset: 0x001F3BB8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			DutyDef dutyDef;
			if (mindState == null)
			{
				dutyDef = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				dutyDef = ((duty != null) ? duty.def : null);
			}
			if (dutyDef == DutyDefOf.ChimeraStalkFlee)
			{
				return null;
			}
			List<Pawn> allHumanlikeSpawned = pawn.Map.mapPawns.AllHumanlikeSpawned;
			for (int i = 0; i < allHumanlikeSpawned.Count; i++)
			{
				if (!allHumanlikeSpawned[i].DeadOrDowned && pawn.Position.InHorDistOf(allHumanlikeSpawned[i].Position, this.attackRadius))
				{
					return JobMaker.MakeJob(JobDefOf.ChimeraSwitchToAttackMode);
				}
			}
			return null;
		}

		// Token: 0x0400326E RID: 12910
		public float attackRadius = 10f;
	}
}
