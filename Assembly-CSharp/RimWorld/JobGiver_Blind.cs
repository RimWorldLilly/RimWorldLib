using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AF7 RID: 2807
	public class JobGiver_Blind : ThinkNode_JobGiver
	{
		// Token: 0x060053A2 RID: 21410 RVA: 0x001F21D4 File Offset: 0x001F03D4
		protected override Job TryGiveJob(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation = lord.LordJob as LordJob_Ritual_Mutilation;
				if (lordJob_Ritual_Mutilation != null)
				{
					Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
					if (lordJob_Ritual_Mutilation.mutilatedPawns.Contains(pawn2) || !pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false))
					{
						return null;
					}
					return JobMaker.MakeJob(JobDefOf.Blind, pawn2, pawn.mindState.duty.focus);
				}
			}
			return null;
		}
	}
}
