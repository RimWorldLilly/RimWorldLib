using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BBF RID: 3007
	public class WorkGiver_BuildRoof : WorkGiver_Scanner
	{
		// Token: 0x0600569E RID: 22174 RVA: 0x00203062 File Offset: 0x00201262
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.BuildRoof.ActiveCells;
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x00203079 File Offset: 0x00201279
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.BuildRoof.TrueCount == 0;
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x060056A0 RID: 22176 RVA: 0x00203093 File Offset: 0x00201293
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x060056A1 RID: 22177 RVA: 0x00203096 File Offset: 0x00201296
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060056A2 RID: 22178 RVA: 0x0020309C File Offset: 0x0020129C
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (!pawn.Map.areaManager.BuildRoof[c])
			{
				return false;
			}
			if (c.Roofed(pawn.Map))
			{
				return false;
			}
			if (!pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Ceiling, forced))
			{
				return false;
			}
			if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn) && this.BuildingToTouchToBeAbleToBuildRoof(c, pawn) == null)
			{
				return false;
			}
			if (!RoofCollapseUtility.WithinRangeOfRoofHolder(c, pawn.Map, false))
			{
				return false;
			}
			if (!RoofCollapseUtility.ConnectedToRoofHolder(c, pawn.Map, true))
			{
				return false;
			}
			Thing thing = RoofUtility.FirstBlockingThing(c, pawn.Map);
			return thing == null || RoofUtility.CanHandleBlockingThing(thing, pawn, forced);
		}

		// Token: 0x060056A3 RID: 22179 RVA: 0x0020314C File Offset: 0x0020134C
		private Building BuildingToTouchToBeAbleToBuildRoof(IntVec3 c, Pawn pawn)
		{
			if (c.Standable(pawn.Map))
			{
				return null;
			}
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice == null)
			{
				return null;
			}
			if (!pawn.CanReach(edifice, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return edifice;
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x00203198 File Offset: 0x00201398
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			LocalTargetInfo targetB = c;
			Thing thing = RoofUtility.FirstBlockingThing(c, pawn.Map);
			if (thing != null)
			{
				return RoofUtility.HandleBlockingThingJob(thing, pawn, forced);
			}
			if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				targetB = this.BuildingToTouchToBeAbleToBuildRoof(c, pawn);
			}
			return JobMaker.MakeJob(JobDefOf.BuildRoof, c, targetB);
		}
	}
}
