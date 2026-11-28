using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF6 RID: 3062
	public class WorkGiver_CarryToSubcoreScanner : WorkGiver_CarryToBuilding
	{
		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x0600578E RID: 22414 RVA: 0x00206F97 File Offset: 0x00205197
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.SubcoreScanner);
			}
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x00206FA0 File Offset: 0x002051A0
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
