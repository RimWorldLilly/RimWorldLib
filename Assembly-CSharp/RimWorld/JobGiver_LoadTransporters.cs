using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B1E RID: 2846
	public class JobGiver_LoadTransporters : ThinkNode_JobGiver
	{
		// Token: 0x0600540E RID: 21518 RVA: 0x001F4650 File Offset: 0x001F2850
		protected override Job TryGiveJob(Pawn pawn)
		{
			TransporterUtility.GetTransportersInGroup(pawn.mindState.duty.transportersGroup, pawn.Map, JobGiver_LoadTransporters.tmpTransporters);
			for (int i = 0; i < JobGiver_LoadTransporters.tmpTransporters.Count; i++)
			{
				CompTransporter transporter = JobGiver_LoadTransporters.tmpTransporters[i];
				if (LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter))
				{
					return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
				}
			}
			return null;
		}

		// Token: 0x04003256 RID: 12886
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();
	}
}
