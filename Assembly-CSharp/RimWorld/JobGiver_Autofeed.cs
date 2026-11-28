using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B31 RID: 2865
	public class JobGiver_Autofeed : ThinkNode_JobGiver
	{
		// Token: 0x06005450 RID: 21584 RVA: 0x001F58CC File Offset: 0x001F3ACC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.CanReserve(pawn, 1, -1, null, false))
			{
				return null;
			}
			Thing thing;
			Pawn pawn2 = ChildcareUtility.FindAutofeedBaby(pawn, AutofeedMode.Urgent, out thing);
			if (pawn2 == null || !pawn2.Spawned)
			{
				return null;
			}
			Pawn pawn3;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (thing != pawn && ChildcareUtility.ImmobileBreastfeederAvailable(pawn, pawn2, false, out pawn3, out breastfeedFailReason))
			{
				thing = pawn3;
			}
			if (thing == null)
			{
				return null;
			}
			return ChildcareUtility.MakeAutofeedBabyJob(pawn, pawn2, thing);
		}

		// Token: 0x06005451 RID: 21585 RVA: 0x001F5925 File Offset: 0x001F3B25
		public override float GetPriority(Pawn pawn)
		{
			return 9.5f;
		}
	}
}
