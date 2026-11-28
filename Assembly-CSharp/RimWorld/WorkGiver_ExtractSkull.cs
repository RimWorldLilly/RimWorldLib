using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C07 RID: 3079
	public class WorkGiver_ExtractSkull : WorkGiver_Scanner
	{
		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x060057F5 RID: 22517 RVA: 0x00209165 File Offset: 0x00207365
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060057F6 RID: 22518 RVA: 0x00209168 File Offset: 0x00207368
		public static bool CanExtractSkull(Ideo ideo)
		{
			return ideo.classicMode || ideo.HasPrecept(PreceptDefOf.Skullspike_Desired) || (ModsConfig.AnomalyActive && ResearchProjectDefOf.AdvancedPsychicRituals.IsFinished);
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x00209194 File Offset: 0x00207394
		public static bool CanPlayerExtractSkull()
		{
			if (Find.IdeoManager.classicMode)
			{
				return true;
			}
			if (WorkGiver_ExtractSkull.CanExtractSkull(Faction.OfPlayer.ideos.PrimaryIdeo))
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = Faction.OfPlayer.ideos.IdeosMinorListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (WorkGiver_ExtractSkull.CanExtractSkull(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060057F8 RID: 22520 RVA: 0x0020921C File Offset: 0x0020741C
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_ExtractSkull.<PotentialWorkThingsGlobal>d__4 <PotentialWorkThingsGlobal>d__ = new WorkGiver_ExtractSkull.<PotentialWorkThingsGlobal>d__4(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x0020922C File Offset: 0x0020742C
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.ExtractSkull);
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x00209248 File Offset: 0x00207448
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Corpse corpse = t as Corpse;
			if (corpse == null || corpse.Destroyed)
			{
				return false;
			}
			if (corpse.Map.designationManager.DesignationOn(t, DesignationDefOf.ExtractSkull) == null)
			{
				return false;
			}
			if (!corpse.InnerPawn.health.hediffSet.HasHead)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (ModsConfig.IdeologyActive && (pawn.Ideo == null || !WorkGiver_ExtractSkull.CanExtractSkull(pawn.Ideo)))
			{
				JobFailReason.Is("CannotExtractSkull".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x060057FB RID: 22523 RVA: 0x002092E4 File Offset: 0x002074E4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ExtractSkull, t);
			job.count = 1;
			return job;
		}
	}
}
