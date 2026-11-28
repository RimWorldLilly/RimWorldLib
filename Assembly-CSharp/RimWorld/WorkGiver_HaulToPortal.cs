using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C21 RID: 3105
	public class WorkGiver_HaulToPortal : WorkGiver_Scanner
	{
		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06005880 RID: 22656 RVA: 0x0020B7E5 File Offset: 0x002099E5
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.MapPortal);
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x06005881 RID: 22657 RVA: 0x0020B7EE File Offset: 0x002099EE
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005882 RID: 22658 RVA: 0x0020B7F1 File Offset: 0x002099F1
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x0020B7F4 File Offset: 0x002099F4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			MapPortal portal = t as MapPortal;
			return EnterPortalUtility.HasJobOnPortal(pawn, portal);
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x0020B810 File Offset: 0x00209A10
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			MapPortal portal = t as MapPortal;
			return EnterPortalUtility.JobOnPortal(pawn, portal);
		}
	}
}
