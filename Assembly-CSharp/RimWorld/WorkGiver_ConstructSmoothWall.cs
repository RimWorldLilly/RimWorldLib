using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC9 RID: 3017
	public class WorkGiver_ConstructSmoothWall : WorkGiver_Scanner
	{
		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x060056DE RID: 22238 RVA: 0x00204652 File Offset: 0x00202852
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060056DF RID: 22239 RVA: 0x00204655 File Offset: 0x00202855
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			WorkGiver_ConstructSmoothWall.<PotentialWorkCellsGlobal>d__2 <PotentialWorkCellsGlobal>d__ = new WorkGiver_ConstructSmoothWall.<PotentialWorkCellsGlobal>d__2(-2);
			<PotentialWorkCellsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkCellsGlobal>d__;
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x00204665 File Offset: 0x00202865
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.SmoothWall);
		}

		// Token: 0x060056E1 RID: 22241 RVA: 0x00204680 File Offset: 0x00202880
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) == null)
			{
				return false;
			}
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice == null || !edifice.def.IsSmoothable)
			{
				Log.ErrorOnce("Failed to find valid edifice when trying to smooth a wall", 58988176);
				pawn.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.SmoothWall);
				return false;
			}
			return pawn.CanReserve(edifice, 1, -1, null, forced) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x00204712 File Offset: 0x00202912
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SmoothWall, c.GetEdifice(pawn.Map));
		}
	}
}
