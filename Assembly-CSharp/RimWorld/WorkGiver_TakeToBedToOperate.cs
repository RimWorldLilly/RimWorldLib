using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C46 RID: 3142
	public class WorkGiver_TakeToBedToOperate : WorkGiver_TakeToBed
	{
		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06005952 RID: 22866 RVA: 0x0020E723 File Offset: 0x0020C923
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06005953 RID: 22867 RVA: 0x0020E72C File Offset: 0x0020C92C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06005954 RID: 22868 RVA: 0x0020E72F File Offset: 0x0020C92F
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06005955 RID: 22869 RVA: 0x0020E734 File Offset: 0x0020C934
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (HealthAIUtility.ShouldHaveSurgeryDoneNow(allPawnsSpawned[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005956 RID: 22870 RVA: 0x0020E774 File Offset: 0x0020C974
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWhoShouldHaveSurgeryDoneNow;
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x0020E788 File Offset: 0x0020C988
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn || pawn2.InBed() || !pawn2.RaceProps.IsFlesh || !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn2) || !pawn.CanReserve(pawn2, 1, -1, null, forced) || (pawn2.InMentalState && pawn2.MentalStateDef.IsAggro))
			{
				return false;
			}
			if (!pawn2.Downed)
			{
				if (pawn2.IsColonist || pawn2.CurJobDef == JobDefOf.LayDown)
				{
					return false;
				}
				if (!pawn2.IsPrisonerOfColony && pawn2.Faction != Faction.OfPlayer)
				{
					return false;
				}
				if (pawn2.guest != null && pawn2.guest.Released)
				{
					return false;
				}
			}
			Building_Bed building_Bed = base.FindBed(pawn, pawn2);
			return building_Bed != null && pawn2.CanReserve(building_Bed, building_Bed.SleepingSlotsCount, -1, null, false);
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x0020E85C File Offset: 0x0020CA5C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Building_Bed t2 = base.FindBed(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.TakeToBedToOperate, pawn2, t2);
			job.count = 1;
			return job;
		}
	}
}
