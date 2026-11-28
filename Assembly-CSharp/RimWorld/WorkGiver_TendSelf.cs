using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C4F RID: 3151
	public class WorkGiver_TendSelf : WorkGiver_Tend
	{
		// Token: 0x0600597B RID: 22907 RVA: 0x0020ECC0 File Offset: 0x0020CEC0
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_TendSelf.<PotentialWorkThingsGlobal>d__0 <PotentialWorkThingsGlobal>d__ = new WorkGiver_TendSelf.<PotentialWorkThingsGlobal>d__0(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x0600597C RID: 22908 RVA: 0x0020ECD0 File Offset: 0x0020CED0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x0600597D RID: 22909 RVA: 0x0020ECD8 File Offset: 0x0020CED8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool flag = pawn == t && pawn.playerSettings != null && base.HasJobOnThing(pawn, t, forced);
			if (flag && !pawn.playerSettings.selfTend)
			{
				JobFailReason.Is("SelfTendDisabled".Translate(), null);
			}
			return flag && pawn.playerSettings.selfTend;
		}
	}
}
