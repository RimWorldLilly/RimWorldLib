using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C3C RID: 3132
	public class WorkGiver_RescueDowned : WorkGiver_TakeToBed
	{
		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x0600591D RID: 22813 RVA: 0x0020DF36 File Offset: 0x0020C136
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x0600591E RID: 22814 RVA: 0x0020DF39 File Offset: 0x0020C139
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x0600591F RID: 22815 RVA: 0x0020DF3C File Offset: 0x0020C13C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06005920 RID: 22816 RVA: 0x0020DF45 File Offset: 0x0020C145
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedDownedPawns;
		}

		// Token: 0x06005921 RID: 22817 RVA: 0x0020DF58 File Offset: 0x0020C158
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Downed && !list[i].InBed())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005922 RID: 22818 RVA: 0x0020DFAC File Offset: 0x0020C1AC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !HealthAIUtility.CanRescueNow(pawn, pawn2, forced))
			{
				return false;
			}
			Thing thing = null;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (ChildcareUtility.CanSuckle(pawn2, out breastfeedFailReason))
			{
				if (!HealthAIUtility.ShouldSeekMedicalRest(pawn2))
				{
					return false;
				}
				Building_Bed building_Bed = ChildcareUtility.SafePlaceForBaby(pawn2, pawn, false).Thing as Building_Bed;
				if (building_Bed != null)
				{
					thing = building_Bed;
				}
			}
			else
			{
				thing = base.FindBed(pawn, pawn2);
			}
			return thing != null && pawn2.CanReserve(thing, 1, -1, null, false);
		}

		// Token: 0x06005923 RID: 22819 RVA: 0x0020E024 File Offset: 0x0020C224
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing t2 = base.FindBed(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.Rescue, pawn2, t2);
			job.count = 1;
			return job;
		}

		// Token: 0x04003357 RID: 13143
		private const float MinDistFromEnemy = 40f;
	}
}
