using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C31 RID: 3121
	public class WorkGiver_PlantsCut : WorkGiver_Scanner
	{
		// Token: 0x060058D9 RID: 22745 RVA: 0x0020D17D File Offset: 0x0020B37D
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060058DA RID: 22746 RVA: 0x0020D180 File Offset: 0x0020B380
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_PlantsCut.<PotentialWorkThingsGlobal>d__1 <PotentialWorkThingsGlobal>d__ = new WorkGiver_PlantsCut.<PotentialWorkThingsGlobal>d__1(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x0020D190 File Offset: 0x0020B390
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.CutPlant) && !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.HarvestPlant);
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x060058DC RID: 22748 RVA: 0x0020D1C3 File Offset: 0x0020B3C3
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x0020D1C8 File Offset: 0x0020B3C8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.category != ThingCategory.Plant)
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			if (t.IsBurning())
			{
				return null;
			}
			if (!PlantUtility.PawnWillingToCutPlant_Job(t, pawn))
			{
				return null;
			}
			CompPlantPreventCutting compPlantPreventCutting;
			if (!forced && t.TryGetComp(out compPlantPreventCutting) && compPlantPreventCutting.PreventCutting)
			{
				return null;
			}
			foreach (Designation designation in pawn.Map.designationManager.AllDesignationsOn(t))
			{
				if (designation.def == DesignationDefOf.HarvestPlant)
				{
					if (!((Plant)t).HarvestableNow)
					{
						return null;
					}
					return JobMaker.MakeJob(JobDefOf.HarvestDesignated, t);
				}
				else if (designation.def == DesignationDefOf.CutPlant)
				{
					return JobMaker.MakeJob(JobDefOf.CutPlantDesignated, t);
				}
			}
			return null;
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x0020D2C4 File Offset: 0x0020B4C4
		public override string PostProcessedGerund(Job job)
		{
			if (job.def == JobDefOf.HarvestDesignated)
			{
				return "HarvestGerund".Translate();
			}
			if (job.def == JobDefOf.CutPlantDesignated)
			{
				return "CutGerund".Translate();
			}
			return this.def.gerund;
		}
	}
}
