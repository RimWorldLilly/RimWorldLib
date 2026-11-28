using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C43 RID: 3139
	public class WorkGiver_TakeBeerOutOfFermentingBarrel : WorkGiver_Scanner
	{
		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06005944 RID: 22852 RVA: 0x0020E5D7 File Offset: 0x0020C7D7
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		// Token: 0x06005945 RID: 22853 RVA: 0x0020E5E4 File Offset: 0x0020C7E4
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.FermentingBarrel);
			for (int i = 0; i < list.Count; i++)
			{
				if (((Building_FermentingBarrel)list[i]).Fermented)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06005946 RID: 22854 RVA: 0x0020E62E File Offset: 0x0020C82E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005947 RID: 22855 RVA: 0x0020E634 File Offset: 0x0020C834
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			return building_FermentingBarrel != null && building_FermentingBarrel.Fermented && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06005948 RID: 22856 RVA: 0x0020E674 File Offset: 0x0020C874
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.TakeBeerOutOfFermentingBarrel, t);
		}
	}
}
