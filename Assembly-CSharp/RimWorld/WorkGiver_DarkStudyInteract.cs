using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C41 RID: 3137
	public class WorkGiver_DarkStudyInteract : WorkGiver_StudyBase
	{
		// Token: 0x0600593A RID: 22842 RVA: 0x0020E322 File Offset: 0x0020C522
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x0600593B RID: 22843 RVA: 0x0020E32C File Offset: 0x0020C52C
		public override string PostProcessedGerund(Job job)
		{
			if (job.targetC == null)
			{
				return base.PostProcessedGerund(job);
			}
			return "DoWorkAtThing".Translate(this.def.gerund.Named("GERUND"), job.targetC.Label.Named("TARGETLABEL"));
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x0020E390 File Offset: 0x0020C590
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			Thing thing = t;
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = t as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					thing = building_HoldingPlatform.HeldPawn;
					if (thing == null || !pawn.CanReserve(thing, 1, -1, null, forced))
					{
						return false;
					}
				}
			}
			if (thing == null)
			{
				return false;
			}
			if (thing == pawn)
			{
				return false;
			}
			CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
			if (compStudiable.KnowledgeCategory == null)
			{
				return false;
			}
			if (!compStudiable.EverStudiable())
			{
				JobFailReason.IsSilent();
				return false;
			}
			if (!compStudiable.CurrentlyStudiable())
			{
				if (compStudiable.Props.frequencyTicks > 0 && compStudiable.TicksTilNextStudy > 0)
				{
					JobFailReason.Is("CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst(), null);
				}
				else
				{
					JobFailReason.IsSilent();
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600593D RID: 22845 RVA: 0x0020E468 File Offset: 0x0020C668
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			JobDef studyInteract = JobDefOf.StudyInteract;
			LocalTargetInfo targetA = t;
			LocalTargetInfo targetB = null;
			Building_HoldingPlatform building_HoldingPlatform = t as Building_HoldingPlatform;
			return JobMaker.MakeJob(studyInteract, targetA, targetB, (building_HoldingPlatform != null) ? building_HoldingPlatform.HeldPawn : null);
		}
	}
}
