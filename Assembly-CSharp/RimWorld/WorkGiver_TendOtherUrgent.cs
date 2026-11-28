using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4E RID: 3150
	public class WorkGiver_TendOtherUrgent : WorkGiver_TendOther
	{
		// Token: 0x06005979 RID: 22905 RVA: 0x0020EC9E File Offset: 0x0020CE9E
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent((Pawn)t);
		}
	}
}
