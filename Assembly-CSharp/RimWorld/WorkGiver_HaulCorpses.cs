using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C17 RID: 3095
	public class WorkGiver_HaulCorpses : WorkGiver_Haul
	{
		// Token: 0x0600584C RID: 22604 RVA: 0x0020A990 File Offset: 0x00208B90
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Corpse))
			{
				return null;
			}
			Pawn pawn2 = pawn.Map.physicalInteractionReservationManager.FirstReserverOf(t);
			if (pawn2 != null && pawn2.RaceProps.Animal && pawn2.Faction != Faction.OfPlayer)
			{
				return null;
			}
			return base.JobOnThing(pawn, t, forced);
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x0020A9E8 File Offset: 0x00208BE8
		public override string PostProcessedGerund(Job job)
		{
			if (job.GetTarget(TargetIndex.B).Thing is Building_Grave)
			{
				return "Burying".Translate();
			}
			return base.PostProcessedGerund(job);
		}
	}
}
