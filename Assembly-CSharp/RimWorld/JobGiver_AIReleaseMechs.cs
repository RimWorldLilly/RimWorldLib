using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ADE RID: 2782
	public class JobGiver_AIReleaseMechs : ThinkNode_JobGiver
	{
		// Token: 0x06005343 RID: 21315 RVA: 0x001EFD44 File Offset: 0x001EDF44
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompMechCarrier compMechCarrier = pawn.TryGetComp<CompMechCarrier>();
			if (compMechCarrier != null && compMechCarrier.CanSpawn)
			{
				return JobMaker.MakeJob(JobDefOf.ReleaseMechs);
			}
			return null;
		}
	}
}
