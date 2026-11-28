using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BCB RID: 3019
	public class WorkGiver_DeconstructForBlueprint : WorkGiver_Scanner
	{
		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x060056E8 RID: 22248 RVA: 0x0020478C File Offset: 0x0020298C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060056E9 RID: 22249 RVA: 0x0020478F File Offset: 0x0020298F
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_DeconstructForBlueprint.<PotentialWorkThingsGlobal>d__2 <PotentialWorkThingsGlobal>d__ = new WorkGiver_DeconstructForBlueprint.<PotentialWorkThingsGlobal>d__2(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x002047A0 File Offset: 0x002029A0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			IEnumerable<Blueprint> enumerable = WorkGiver_DeconstructForBlueprint.BuildableBlueprintsAt(pawn, t.Position);
			if (enumerable.EnumerableNullOrEmpty<Blueprint>())
			{
				return false;
			}
			CompExplosive compExplosive;
			if (t.TryGetComp(out compExplosive) && compExplosive.wickStarted)
			{
				JobFailReason.Is("AboutToExplode".Translate(), null);
				return false;
			}
			Building building = t as Building;
			if (building != null)
			{
				AcceptanceReport acceptanceReport = building.DeconstructibleBy(pawn.Faction);
				if (!acceptanceReport.Accepted)
				{
					JobFailReason.Is(acceptanceReport.Reason, null);
					return false;
				}
				if (building.IsForbidden(pawn))
				{
					JobFailReason.Is("ForbiddenLower".Translate(), null);
					return false;
				}
			}
			foreach (Blueprint blueprint in enumerable)
			{
				Blueprint_Install blueprint_Install = blueprint as Blueprint_Install;
				if (blueprint_Install != null && blueprint_Install.ThingToInstall == t)
				{
					return false;
				}
				if (blueprint.IsForbidden(pawn))
				{
					JobFailReason.Is("ForbiddenLower".Translate(), null);
					return false;
				}
				if (GenConstruct.CanReplace(blueprint.EntityToBuild(), t.def, blueprint.Stuff, t.Stuff))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x002048F4 File Offset: 0x00202AF4
		private static IEnumerable<Blueprint> BuildableBlueprintsAt(Pawn pawn, IntVec3 cell)
		{
			WorkGiver_DeconstructForBlueprint.<BuildableBlueprintsAt>d__4 <BuildableBlueprintsAt>d__ = new WorkGiver_DeconstructForBlueprint.<BuildableBlueprintsAt>d__4(-2);
			<BuildableBlueprintsAt>d__.<>3__pawn = pawn;
			<BuildableBlueprintsAt>d__.<>3__cell = cell;
			return <BuildableBlueprintsAt>d__;
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x0020490B File Offset: 0x00202B0B
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.DeconstructForBlueprint, t);
		}
	}
}
