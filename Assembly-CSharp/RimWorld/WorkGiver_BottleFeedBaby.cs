using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BEF RID: 3055
	public class WorkGiver_BottleFeedBaby : WorkGiver_FeedBabyManually
	{
		// Token: 0x0600577A RID: 22394 RVA: 0x00206C7C File Offset: 0x00204E7C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.CanCreateManualFeedingJob(pawn, t, forced))
			{
				return null;
			}
			Pawn baby = (Pawn)t;
			Thing thing = ChildcareUtility.FindBabyFoodForBaby(pawn, baby);
			if (thing != null)
			{
				return ChildcareUtility.MakeBottlefeedJob(baby, thing);
			}
			JobFailReason.Is("NoBabyFood".Translate(), null);
			return null;
		}
	}
}
