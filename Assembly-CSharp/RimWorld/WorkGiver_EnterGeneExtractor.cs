using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C02 RID: 3074
	public class WorkGiver_EnterGeneExtractor : WorkGiver_EnterBuilding
	{
		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x060057E4 RID: 22500 RVA: 0x00208FBC File Offset: 0x002071BC
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GeneExtractor);
			}
		}

		// Token: 0x060057E5 RID: 22501 RVA: 0x00208FC8 File Offset: 0x002071C8
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
