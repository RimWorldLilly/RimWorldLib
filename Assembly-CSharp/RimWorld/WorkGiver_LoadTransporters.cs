using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C24 RID: 3108
	public class WorkGiver_LoadTransporters : WorkGiver_Scanner
	{
		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06005893 RID: 22675 RVA: 0x0020BBA8 File Offset: 0x00209DA8
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Transporter);
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06005894 RID: 22676 RVA: 0x0020BBB1 File Offset: 0x00209DB1
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005895 RID: 22677 RVA: 0x0020BBB4 File Offset: 0x00209DB4
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x0020BBB8 File Offset: 0x00209DB8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter);
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x0020BBD4 File Offset: 0x00209DD4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
		}
	}
}
