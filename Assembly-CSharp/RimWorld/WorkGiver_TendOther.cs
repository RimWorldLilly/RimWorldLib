using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4B RID: 3147
	public class WorkGiver_TendOther : WorkGiver_Tend
	{
		// Token: 0x06005971 RID: 22897 RVA: 0x0020EBF0 File Offset: 0x0020CDF0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && pawn != t;
		}
	}
}
