using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C56 RID: 7254
	[StaticConstructorOnStartup]
	public class Designator_PaintFloor : Designator_Paint
	{
		// Token: 0x17002174 RID: 8564
		// (get) Token: 0x0600B91C RID: 47388 RVA: 0x00414910 File Offset: 0x00412B10
		protected override Texture2D IconTopTex
		{
			get
			{
				return ContentFinder<Texture2D>.Get("UI/Designators/PaintFloor_Top", true);
			}
		}

		// Token: 0x17002175 RID: 8565
		// (get) Token: 0x0600B91D RID: 47389 RVA: 0x0041491D File Offset: 0x00412B1D
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.PaintFloor;
			}
		}

		// Token: 0x0600B91E RID: 47390 RVA: 0x00414924 File Offset: 0x00412B24
		public Designator_PaintFloor()
		{
			this.defaultLabel = "DesignatorPaintFloor".Translate();
			this.defaultDesc = "DesignatorPaintFloorDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PaintFloor_Bottom", true);
		}

		// Token: 0x0600B91F RID: 47391 RVA: 0x00414974 File Offset: 0x00412B74
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				return this.eyedropper.CanDesignateCell(c);
			}
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
			{
				return false;
			}
			if (!base.Map.terrainGrid.TerrainAt(c).isPaintable)
			{
				return "MessageMustDesignatePaintableFloors".Translate(this.colorDef);
			}
			if (base.Map.terrainGrid.ColorAt(c) == this.colorDef)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B920 RID: 47392 RVA: 0x00414A64 File Offset: 0x00412C64
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				this.eyedropper.DesignateSingleCell(c);
				return;
			}
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.SetTerrainColor(c, this.colorDef);
				return;
			}
			base.Map.designationManager.TryRemoveDesignation(c, this.Designation);
			base.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.RemovePaintFloor);
			base.Map.designationManager.AddDesignation(new Designation(c, this.Designation, this.colorDef));
		}

		// Token: 0x0600B921 RID: 47393 RVA: 0x00414AF9 File Offset: 0x00412CF9
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x0600B922 RID: 47394 RVA: 0x00414B00 File Offset: 0x00412D00
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x0600B923 RID: 47395 RVA: 0x00414B09 File Offset: 0x00412D09
		protected override int NumHighlightedCells()
		{
			return Find.DesignatorManager.Dragger.DragCells.Count;
		}
	}
}
