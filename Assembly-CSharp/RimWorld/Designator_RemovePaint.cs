using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5C RID: 7260
	public class Designator_RemovePaint : Designator_Cells
	{
		// Token: 0x1700217A RID: 8570
		// (get) Token: 0x0600B93A RID: 47418 RVA: 0x00415300 File Offset: 0x00413500
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700217B RID: 8571
		// (get) Token: 0x0600B93B RID: 47419 RVA: 0x00415303 File Offset: 0x00413503
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Paint;
			}
		}

		// Token: 0x0600B93C RID: 47420 RVA: 0x0041530C File Offset: 0x0041350C
		public Designator_RemovePaint()
		{
			this.defaultLabel = "DesignatorRemovePaint".Translate();
			this.defaultDesc = "DesignatorRemovePaintDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemovePaint", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_RemovePaint;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "RemovePaint";
		}

		// Token: 0x0600B93D RID: 47421 RVA: 0x00415398 File Offset: 0x00413598
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if ((from t in c.GetThingList(base.Map)
			where this.CanDesignateThing(t).Accepted
			select t).Any<Thing>())
			{
				return AcceptanceReport.WasAccepted;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
			{
				return "MessageMustDesignatePainted".Translate();
			}
			if (base.Map.terrainGrid.ColorAt(c) == null)
			{
				return "MessageMustDesignatePainted".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600B93E RID: 47422 RVA: 0x00415458 File Offset: 0x00413658
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
					return;
				}
			}
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.SetTerrainColor(c, null);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.RemovePaintFloor, null));
		}

		// Token: 0x0600B93F RID: 47423 RVA: 0x004154E4 File Offset: 0x004136E4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.Faction != Faction.OfPlayer)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
			{
				return false;
			}
			Building building = t as Building;
			return building != null && building.PaintColorDef != null;
		}

		// Token: 0x0600B940 RID: 47424 RVA: 0x00415540 File Offset: 0x00413740
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.TryRemoveDesignationOn(t, DesignationDefOf.PaintBuilding);
			base.Map.designationManager.TryRemoveDesignationOn(t, DesignationDefOf.RemovePaintBuilding);
			if (DebugSettings.godMode)
			{
				((Building)t).ChangePaint(null);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.RemovePaintBuilding, null));
		}

		// Token: 0x0600B941 RID: 47425 RVA: 0x004155AE File Offset: 0x004137AE
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
