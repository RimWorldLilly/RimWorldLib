using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C03 RID: 3075
	public class WorkGiver_EnterGrowthVat : WorkGiver_EnterBuilding
	{
		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x060057E7 RID: 22503 RVA: 0x00208FDA File Offset: 0x002071DA
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GrowthVat);
			}
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x00208FE6 File Offset: 0x002071E6
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
