using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF5 RID: 3061
	public class WorkGiver_CarryToGeneExtractor : WorkGiver_CarryToBuilding
	{
		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x0600578B RID: 22411 RVA: 0x00206F79 File Offset: 0x00205179
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GeneExtractor);
			}
		}

		// Token: 0x0600578C RID: 22412 RVA: 0x00206F85 File Offset: 0x00205185
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
