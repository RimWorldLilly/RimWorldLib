using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC3 RID: 3011
	public class WorkGiver_ConstructRemoveFoundation : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x060056B4 RID: 22196 RVA: 0x0020337A File Offset: 0x0020157A
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFoundation;
			}
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x00203384 File Offset: 0x00201584
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Job job = this.RemoveFloorJob(pawn, c);
			if (job != null)
			{
				return job;
			}
			Job job2 = this.RemoveFoundationJob(pawn, c);
			if (job2 != null)
			{
				return job2;
			}
			return JobMaker.MakeJob(JobDefOf.RemoveFoundation, c);
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x002033BD File Offset: 0x002015BD
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, forced) && pawn.Map.terrainGrid.CanRemoveFoundationAt(c) && !WorkGiver_ConstructRemoveFoundation.AnyBuildingBlockingFoundationRemoval(c, pawn.Map);
		}

		// Token: 0x060056B7 RID: 22199 RVA: 0x002033F4 File Offset: 0x002015F4
		public static bool AnyBuildingBlockingFoundationRemoval(IntVec3 c, Map map)
		{
			if (!map.terrainGrid.CanRemoveFoundationAt(c))
			{
				return false;
			}
			foreach (Thing thing in c.GetThingList(map))
			{
				ThingDef thingDef = thing.def;
				Blueprint_Build blueprint_Build = thing as Blueprint_Build;
				if (blueprint_Build != null)
				{
					thingDef = blueprint_Build.BuildDef;
				}
				else
				{
					Blueprint_Install blueprint_Install = thing as Blueprint_Install;
					if (blueprint_Install != null)
					{
						thingDef = blueprint_Install.MiniToInstallOrBuildingToReinstall.def;
					}
					else
					{
						Frame frame = thing as Frame;
						if (frame != null)
						{
							thingDef = frame.BuildDef;
						}
					}
				}
				if (thingDef.terrainAffordanceNeeded != null && ((map.terrainGrid.UnderTerrainAt(c) != null) ? (!map.terrainGrid.UnderTerrainAt(c).affordances.Contains(thingDef.terrainAffordanceNeeded)) : (!map.terrainGrid.TopTerrainAt(c).affordances.Contains(thingDef.terrainAffordanceNeeded))))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x00203504 File Offset: 0x00201704
		private Job RemoveFloorJob(Pawn pawn, IntVec3 c)
		{
			if (pawn.Map.terrainGrid.UnderTerrainAt(c) == null || !pawn.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return null;
			}
			if (!pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, false))
			{
				return null;
			}
			if (pawn.WorkTypeIsDisabled(WorkGiverDefOf.ConstructRemoveFloors.workType))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.RemoveFloor, c);
			job.ignoreDesignations = true;
			return job;
		}

		// Token: 0x060056B9 RID: 22201 RVA: 0x0020357C File Offset: 0x0020177C
		private Job RemoveFoundationJob(Pawn pawn, IntVec3 c)
		{
			if (!pawn.Map.terrainGrid.CanRemoveFoundationAt(c))
			{
				return null;
			}
			if (!pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, false))
			{
				return null;
			}
			if (pawn.WorkTypeIsDisabled(WorkGiverDefOf.ConstructRemoveFloors.workType))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.RemoveFoundation, c);
			job.ignoreDesignations = true;
			return job;
		}
	}
}
