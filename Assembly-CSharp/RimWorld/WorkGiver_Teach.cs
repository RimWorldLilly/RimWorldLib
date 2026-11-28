using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C47 RID: 3143
	public class WorkGiver_Teach : WorkGiver_Scanner
	{
		// Token: 0x0600595A RID: 22874 RVA: 0x0020E89E File Offset: 0x0020CA9E
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn teacher)
		{
			return teacher.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x0020E8B5 File Offset: 0x0020CAB5
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive || !SchoolUtility.CanTeachNow(pawn);
		}

		// Token: 0x0600595C RID: 22876 RVA: 0x0020E8CC File Offset: 0x0020CACC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn.skills == null || pawn2.skills == null)
			{
				return false;
			}
			if (SchoolUtility.NeedsTeacher(pawn2))
			{
				Thing thing = pawn2.CurJob.GetTarget(TargetIndex.A).Thing;
				if (thing != null && pawn.CanReserveSittableOrSpot(SchoolUtility.DeskSpotTeacher(thing), false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600595D RID: 22877 RVA: 0x0020E928 File Offset: 0x0020CB28
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null)
			{
				return null;
			}
			pawn2.CurJob.SetTarget(TargetIndex.B, pawn);
			return JobMaker.MakeJob(JobDefOf.Lessongiving, pawn2.CurJob.GetTarget(TargetIndex.A), pawn2);
		}
	}
}
