using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C60 RID: 7264
	public class Designator_SmoothWalls : Designator_Smooth
	{
		// Token: 0x0600B954 RID: 47444 RVA: 0x00415A6C File Offset: 0x00413C6C
		public Designator_SmoothWalls()
		{
			this.defaultLabel = "DesignatorSmoothWalls".Translate();
			this.defaultDesc = "DesignatorSmoothWallsDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SmoothSurface", true);
		}

		// Token: 0x0600B955 RID: 47445 RVA: 0x00415ABC File Offset: 0x00413CBC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t != null && t.def.IsSmoothable && this.CanDesignateCell(t.Position).Accepted)
			{
				return AcceptanceReport.WasAccepted;
			}
			return false;
		}

		// Token: 0x0600B956 RID: 47446 RVA: 0x00415AFB File Offset: 0x00413CFB
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x0600B957 RID: 47447 RVA: 0x00415B09 File Offset: 0x00413D09
		public override void DesignateSingleCell(IntVec3 c)
		{
			SmoothSurfaceDesignatorUtility.DesignateSmoothWall(base.Map, c);
		}

		// Token: 0x0600B958 RID: 47448 RVA: 0x00415B18 File Offset: 0x00413D18
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) != null)
			{
				return "SurfaceBeingSmoothed".Translate();
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice == null || !edifice.def.IsSmoothable)
			{
				return "MessageMustDesignateSmoothableWall".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
