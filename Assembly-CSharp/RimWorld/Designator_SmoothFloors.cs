using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C61 RID: 7265
	public class Designator_SmoothFloors : Designator_Smooth
	{
		// Token: 0x0600B959 RID: 47449 RVA: 0x00415B94 File Offset: 0x00413D94
		public Designator_SmoothFloors()
		{
			this.defaultLabel = "DesignatorSmoothFloors".Translate();
			this.defaultDesc = "DesignatorSmoothFloorsDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SmoothSurface", true);
		}

		// Token: 0x0600B95A RID: 47450 RVA: 0x00415BE2 File Offset: 0x00413DE2
		public override void DesignateSingleCell(IntVec3 c)
		{
			SmoothSurfaceDesignatorUtility.DesignateSmoothFloor(base.Map, c);
		}

		// Token: 0x0600B95B RID: 47451 RVA: 0x00415BF0 File Offset: 0x00413DF0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor) != null)
			{
				return "SurfaceBeingSmoothed".Translate();
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && !SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(edifice))
			{
				return "MessageMustDesignateSmoothableFloor".Translate();
			}
			if (!c.GetAffordances(base.Map).Contains(TerrainAffordanceDefOf.SmoothableStone))
			{
				return "MessageMustDesignateSmoothableFloor".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
