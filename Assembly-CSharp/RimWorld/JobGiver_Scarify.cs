using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B0F RID: 2831
	public class JobGiver_Scarify : ThinkNode_JobGiver
	{
		// Token: 0x060053E4 RID: 21476 RVA: 0x001F38A0 File Offset: 0x001F1AA0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation = lord.LordJob as LordJob_Ritual_Mutilation;
				if (lordJob_Ritual_Mutilation != null)
				{
					Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
					if (lordJob_Ritual_Mutilation.mutilatedPawns.Contains(pawn2) || !pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false) || !JobDriver_Scarify.AvailableOnNow(pawn2, null))
					{
						return null;
					}
					return JobMaker.MakeJob(JobDefOf.Scarify, pawn2, pawn.mindState.duty.focus);
				}
			}
			return null;
		}
	}
}
