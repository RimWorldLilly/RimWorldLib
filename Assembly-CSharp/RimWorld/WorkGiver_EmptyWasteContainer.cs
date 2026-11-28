using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C1B RID: 3099
	public class WorkGiver_EmptyWasteContainer : WorkGiver_Scanner
	{
		// Token: 0x0600585D RID: 22621 RVA: 0x0020ADD9 File Offset: 0x00208FD9
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.WasteProducer);
		}

		// Token: 0x0600585E RID: 22622 RVA: 0x0020ADED File Offset: 0x00208FED
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.WasteProducer).Any<Thing>();
		}

		// Token: 0x0600585F RID: 22623 RVA: 0x0020AE0C File Offset: 0x0020900C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Empty waste container"))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, this.GetReservationLayer(pawn, t), forced))
			{
				return false;
			}
			CompWasteProducer compWasteProducer = t.TryGetComp<CompWasteProducer>();
			if (compWasteProducer == null || !compWasteProducer.CanEmptyNow || compWasteProducer.Waste.stackCount <= 0)
			{
				return false;
			}
			IntVec3 intVec;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(compWasteProducer.Waste, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out intVec, out haulDestination, false))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x06005860 RID: 22624 RVA: 0x0020AE9C File Offset: 0x0020909C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompWasteProducer compWasteProducer = t.TryGetComp<CompWasteProducer>();
			if (compWasteProducer == null || !compWasteProducer.CanEmptyNow || compWasteProducer.Waste.stackCount <= 0)
			{
				return null;
			}
			IntVec3 c;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(compWasteProducer.Waste, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out c, out haulDestination, true))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.EmptyWasteContainer, t, compWasteProducer.Waste, c);
			job.count = compWasteProducer.Waste.stackCount;
			return job;
		}

		// Token: 0x06005861 RID: 22625 RVA: 0x0020AF2A File Offset: 0x0020912A
		public override ReservationLayerDef GetReservationLayer(Pawn pawn, LocalTargetInfo t)
		{
			return ReservationLayerDefOf.Empty;
		}
	}
}
