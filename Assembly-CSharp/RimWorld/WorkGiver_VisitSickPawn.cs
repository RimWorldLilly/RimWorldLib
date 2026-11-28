using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C54 RID: 3156
	public class WorkGiver_VisitSickPawn : WorkGiver_Scanner
	{
		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06005996 RID: 22934 RVA: 0x0020EFFB File Offset: 0x0020D1FB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06005997 RID: 22935 RVA: 0x0020EFFE File Offset: 0x0020D1FE
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06005998 RID: 22936 RVA: 0x0020F007 File Offset: 0x0020D207
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
		}

		// Token: 0x06005999 RID: 22937 RVA: 0x0020F020 File Offset: 0x0020D220
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			if (!SocialInteractionUtility.CanInitiateInteraction(pawn, null))
			{
				return true;
			}
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].InBed())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600599A RID: 22938 RVA: 0x0020F070 File Offset: 0x0020D270
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && SickPawnVisitUtility.CanVisit(pawn, pawn2, JoyCategory.VeryLow);
		}

		// Token: 0x0600599B RID: 22939 RVA: 0x0020F094 File Offset: 0x0020D294
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Thing thing = SickPawnVisitUtility.FindChair(pawn, pawn2);
			if (thing != null && !pawn.CanReserveAndReach(thing, PathEndMode.OnCell, Danger.Some, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.VisitSickPawn, pawn2, thing);
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
