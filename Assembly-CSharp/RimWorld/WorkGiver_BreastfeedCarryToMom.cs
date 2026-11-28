using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF1 RID: 3057
	public class WorkGiver_BreastfeedCarryToMom : WorkGiver_FeedBabyManually
	{
		// Token: 0x0600577F RID: 22399 RVA: 0x00206D28 File Offset: 0x00204F28
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> freeColonistsAndPrisonersSpawned = pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < freeColonistsAndPrisonersSpawned.Count; i++)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if ((freeColonistsAndPrisonersSpawned[i].Downed || freeColonistsAndPrisonersSpawned[i].IsPrisoner) && ChildcareUtility.CanBreastfeedNow(freeColonistsAndPrisonersSpawned[i], out breastfeedFailReason))
				{
					flag2 = true;
				}
				if (ChildcareUtility.CanSuckle(freeColonistsAndPrisonersSpawned[i], out breastfeedFailReason))
				{
					flag = true;
				}
				if (flag2 && flag)
				{
					return false;
				}
				if (flag && forced)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005780 RID: 22400 RVA: 0x00206DAC File Offset: 0x00204FAC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.CanCreateManualFeedingJob(pawn, t, forced))
			{
				return null;
			}
			Pawn baby = (Pawn)t;
			Pawn pawn2;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.ImmobileBreastfeederAvailable(pawn, baby, forced, out pawn2, out breastfeedFailReason))
			{
				if (forced && pawn2 != pawn)
				{
					JobFailReason.Is(breastfeedFailReason.Value.Translate(pawn, pawn2, baby), null);
				}
				return null;
			}
			return ChildcareUtility.MakeBreastfeedCarryToMomJob(baby, pawn2);
		}
	}
}
