using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C04 RID: 3076
	public class WorkGiver_EnterSubcoreScanner : WorkGiver_EnterBuilding
	{
		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x060057EA RID: 22506 RVA: 0x00208FF8 File Offset: 0x002071F8
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.SubcoreScanner);
			}
		}

		// Token: 0x060057EB RID: 22507 RVA: 0x00209001 File Offset: 0x00207201
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
