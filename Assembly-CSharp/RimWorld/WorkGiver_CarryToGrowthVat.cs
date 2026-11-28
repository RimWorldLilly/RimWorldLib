using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF4 RID: 3060
	public class WorkGiver_CarryToGrowthVat : WorkGiver_CarryToBuilding
	{
		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06005788 RID: 22408 RVA: 0x00206F5B File Offset: 0x0020515B
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GrowthVat);
			}
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x00206F67 File Offset: 0x00205167
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
