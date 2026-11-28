using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C62 RID: 7266
	public class Designator_SmoothSurface : Designator_Smooth
	{
		// Token: 0x0600B95C RID: 47452 RVA: 0x00415C90 File Offset: 0x00413E90
		public Designator_SmoothSurface()
		{
			this.defaultLabel = "DesignatorSmoothSurface".Translate();
			this.defaultDesc = "DesignatorSmoothSurfaceDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SmoothSurface", true);
		}

		// Token: 0x0600B95D RID: 47453 RVA: 0x00415CDE File Offset: 0x00413EDE
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x0600B95E RID: 47454 RVA: 0x00415CEC File Offset: 0x00413EEC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor) != null || base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) != null)
			{
				return "SurfaceBeingSmoothed".Translate();
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.IsSmoothable)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (edifice != null && !SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(edifice))
			{
				return "MessageMustDesignateSmoothableSurface".Translate();
			}
			if (!c.GetAffordances(base.Map).Contains(TerrainAffordanceDefOf.SmoothableStone))
			{
				return "MessageMustDesignateSmoothableSurface".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600B95F RID: 47455 RVA: 0x00415DB7 File Offset: 0x00413FB7
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (!SmoothSurfaceDesignatorUtility.DesignateSmoothWall(base.Map, c))
			{
				SmoothSurfaceDesignatorUtility.DesignateSmoothFloor(base.Map, c);
			}
		}
	}
}
