using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF7 RID: 3063
	public class WorkGiver_ChangeTreeMode : WorkGiver_Scanner
	{
		// Token: 0x06005791 RID: 22417 RVA: 0x00206FB2 File Offset: 0x002051B2
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.DryadSpawner));
		}

		// Token: 0x06005792 RID: 22418 RVA: 0x00206FCC File Offset: 0x002051CC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			CompTreeConnection compTreeConnection = t.TryGetComp<CompTreeConnection>();
			return compTreeConnection != null && compTreeConnection.ConnectedPawn == pawn && compTreeConnection.Mode != compTreeConnection.desiredMode;
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x00207006 File Offset: 0x00205206
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ChangeTreeMode, t);
			job.playerForced = forced;
			return job;
		}
	}
}
