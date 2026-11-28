using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BB9 RID: 3001
	public class WorkGiver_Slaughter : WorkGiver_Scanner
	{
		// Token: 0x0600568A RID: 22154 RVA: 0x002028BF File Offset: 0x00200ABF
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_Slaughter.<PotentialWorkThingsGlobal>d__0 <PotentialWorkThingsGlobal>d__ = new WorkGiver_Slaughter.<PotentialWorkThingsGlobal>d__0(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x0600568B RID: 22155 RVA: 0x002028CF File Offset: 0x00200ACF
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x002028D2 File Offset: 0x00200AD2
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Slaughter) && pawn.Map.autoSlaughterManager.AnimalsToSlaughter.Count == 0;
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x00202908 File Offset: 0x00200B08
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.IsAnimal)
			{
				return false;
			}
			if (!pawn2.ShouldBeSlaughtered())
			{
				return false;
			}
			if (pawn.Faction != t.Faction)
			{
				return false;
			}
			if (pawn2.InAggroMentalState)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is("IsIncapableOfViolenceShort".Translate(pawn), null);
				return false;
			}
			return (!ModsConfig.IdeologyActive || new HistoryEvent(HistoryEventDefOf.SlaughteredAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (!HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn2) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (pawn.Ideo == null || !pawn.Ideo.IsVeneratedAnimal(pawn2) || new HistoryEvent(HistoryEventDefOf.SlaughteredVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job());
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x00202A08 File Offset: 0x00200C08
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Slaughter, t);
		}
	}
}
