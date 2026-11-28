using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C34 RID: 3124
	public class WorkGiver_PruneGauranlenTree : WorkGiver_Scanner
	{
		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x060058ED RID: 22765 RVA: 0x0020D6C8 File Offset: 0x0020B8C8
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060058EE RID: 22766 RVA: 0x0020D6CC File Offset: 0x0020B8CC
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			if (!t.HasThing)
			{
				return 0f;
			}
			CompTreeConnection compTreeConnection = t.Thing.TryGetComp<CompTreeConnection>();
			if (compTreeConnection == null)
			{
				return 0f;
			}
			return compTreeConnection.DesiredConnectionStrength - compTreeConnection.ConnectionStrength;
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x0020D70B File Offset: 0x0020B90B
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.DryadSpawner));
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x0020D724 File Offset: 0x0020B924
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			CompTreeConnection compTreeConnection = t.TryGetComp<CompTreeConnection>();
			return compTreeConnection != null && compTreeConnection.ConnectedPawn == pawn && compTreeConnection.ShouldBePrunedNow(forced) && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x060058F1 RID: 22769 RVA: 0x0020D771 File Offset: 0x0020B971
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.PruneGauranlenTree, t);
			job.playerForced = forced;
			return job;
		}

		// Token: 0x04003356 RID: 13142
		public const float MaxConnectionStrengthForAutoPruning = 0.99f;
	}
}
