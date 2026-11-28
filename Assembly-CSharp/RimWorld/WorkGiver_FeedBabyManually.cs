using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C09 RID: 3081
	public abstract class WorkGiver_FeedBabyManually : WorkGiver_Scanner
	{
		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06005800 RID: 22528 RVA: 0x0020931B File Offset: 0x0020751B
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06005801 RID: 22529 RVA: 0x00209324 File Offset: 0x00207524
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x00209327 File Offset: 0x00207527
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x0020932A File Offset: 0x0020752A
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHungryPawns;
		}

		// Token: 0x06005804 RID: 22532 RVA: 0x0020933C File Offset: 0x0020753C
		protected bool CanCreateManualFeedingJob(Pawn pawn, Thing t, bool forced)
		{
			Pawn pawn2 = t as Pawn;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(pawn2, out breastfeedFailReason))
			{
				return false;
			}
			if (ChildcareUtility.CanMomAutoBreastfeedBabyNow(pawn, pawn2, false, out breastfeedFailReason))
			{
				return false;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2 = null;
			if (!ChildcareUtility.CanSuckleNow(pawn2, out breastfeedFailReason2))
			{
				if (forced)
				{
					JobFailReason.Is(breastfeedFailReason2.Value.Translate(null, null, pawn2), null);
				}
				return false;
			}
			if (!ChildcareUtility.CanHaulBaby(pawn, pawn2, out breastfeedFailReason))
			{
				return false;
			}
			if (!ChildcareUtility.CanHaulBabyNow(pawn, pawn2, forced, out breastfeedFailReason2))
			{
				if (forced)
				{
					JobFailReason.Is(breastfeedFailReason2.Value.Translate(pawn, null, pawn2), null);
				}
				return false;
			}
			if (!forced && !ChildcareUtility.WantsSuckle(pawn2, out breastfeedFailReason))
			{
				return false;
			}
			Pawn pawn3;
			if (!forced && this.AutoBreastfeederAvailable(pawn2, forced, out pawn3))
			{
				return false;
			}
			bool result;
			if (pawn2 == null)
			{
				result = (null != null);
			}
			else
			{
				Pawn_NeedsTracker needs = pawn2.needs;
				result = (((needs != null) ? needs.food : null) != null);
			}
			return result;
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x00209410 File Offset: 0x00207610
		private bool AutoBreastfeederAvailable(Pawn baby, bool forced, out Pawn feeder)
		{
			Pawn pawn = WorkGiver_FeedBabyManually.cachedBabyAutoBreastfeeder.TryGetValue(baby, null);
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (ChildcareUtility.CanMomAutoBreastfeedBabyNow(pawn, baby, forced, out breastfeedFailReason))
			{
				feeder = pawn;
				return true;
			}
			WorkGiver_FeedBabyManually.cachedBabyAutoBreastfeeder[baby] = null;
			foreach (Pawn pawn2 in ChildcareUtility.CanBreastfeedMothers(baby, false))
			{
				if (ChildcareUtility.CanMomAutoBreastfeedBabyNow(pawn2, baby, forced, out breastfeedFailReason))
				{
					WorkGiver_FeedBabyManually.cachedBabyAutoBreastfeeder[baby] = pawn2;
					feeder = pawn2;
					return true;
				}
			}
			feeder = null;
			return false;
		}

		// Token: 0x04003342 RID: 13122
		private static Dictionary<Pawn, Pawn> cachedBabyAutoBreastfeeder = new Dictionary<Pawn, Pawn>();
	}
}
