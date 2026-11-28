using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C44 RID: 3140
	public class WorkGiver_TakeBioferriteOutOfHarvester : WorkGiver_Scanner
	{
		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x0600594A RID: 22858 RVA: 0x0020E68E File Offset: 0x0020C88E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.BioferriteHarvester);
			}
		}

		// Token: 0x0600594B RID: 22859 RVA: 0x0020E69A File Offset: 0x0020C89A
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x0600594C RID: 22860 RVA: 0x0020E6A4 File Offset: 0x0020C8A4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x0020E6A8 File Offset: 0x0020C8A8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_BioferriteHarvester building_BioferriteHarvester = t as Building_BioferriteHarvester;
			return building_BioferriteHarvester != null && building_BioferriteHarvester.unloadingEnabled && building_BioferriteHarvester.ReadyForHauling && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x0600594E RID: 22862 RVA: 0x0020E6F0 File Offset: 0x0020C8F0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.TakeBioferriteOutOfHarvester, t);
		}
	}
}
