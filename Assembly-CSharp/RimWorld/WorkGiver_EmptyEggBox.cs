using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C19 RID: 3097
	public class WorkGiver_EmptyEggBox : WorkGiver_Scanner
	{
		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06005854 RID: 22612 RVA: 0x0020AB69 File Offset: 0x00208D69
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.EggBox);
			}
		}

		// Token: 0x06005855 RID: 22613 RVA: 0x0020AB78 File Offset: 0x00208D78
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!t.Spawned)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			CompEggContainer compEggContainer = t.TryGetComp<CompEggContainer>();
			if (((compEggContainer != null) ? compEggContainer.ContainedThing : null) == null)
			{
				return false;
			}
			if (!compEggContainer.CanEmpty && !forced)
			{
				return false;
			}
			IntVec3 intVec;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(compEggContainer.ContainedThing, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out intVec, out haulDestination, false))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x06005856 RID: 22614 RVA: 0x0020ABF8 File Offset: 0x00208DF8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompEggContainer compEggContainer = t.TryGetComp<CompEggContainer>();
			if (((compEggContainer != null) ? compEggContainer.ContainedThing : null) == null)
			{
				return null;
			}
			if (!compEggContainer.CanEmpty && !forced)
			{
				return null;
			}
			IntVec3 c;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(compEggContainer.ContainedThing, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out c, out haulDestination, true))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.EmptyThingContainer, t, compEggContainer.ContainedThing, c);
			job.count = compEggContainer.ContainedThing.stackCount;
			return job;
		}
	}
}
