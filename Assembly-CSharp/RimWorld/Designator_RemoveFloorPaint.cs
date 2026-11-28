using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5A RID: 7258
	public class Designator_RemoveFloorPaint : Designator_Cells
	{
		// Token: 0x17002178 RID: 8568
		// (get) Token: 0x0600B931 RID: 47409 RVA: 0x0041503F File Offset: 0x0041323F
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002179 RID: 8569
		// (get) Token: 0x0600B932 RID: 47410 RVA: 0x00415042 File Offset: 0x00413242
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Paint;
			}
		}

		// Token: 0x0600B933 RID: 47411 RVA: 0x0041504C File Offset: 0x0041324C
		public Designator_RemoveFloorPaint()
		{
			this.defaultLabel = "DesignatorRemoveFloorPaint".Translate();
			this.defaultDesc = "DesignatorRemoveFloorPaintDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemovePaint", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_RemovePaint;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "RemovePaint";
		}

		// Token: 0x0600B934 RID: 47412 RVA: 0x004150D8 File Offset: 0x004132D8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.terrainGrid.ColorAt(c) == null)
			{
				return "MessageMustDesignatePainted".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600B935 RID: 47413 RVA: 0x00415130 File Offset: 0x00413330
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.SetTerrainColor(c, null);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.RemovePaintFloor, null));
		}

		// Token: 0x0600B936 RID: 47414 RVA: 0x0041516D File Offset: 0x0041336D
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
