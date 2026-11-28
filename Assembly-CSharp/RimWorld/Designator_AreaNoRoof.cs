using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C39 RID: 7225
	public class Designator_AreaNoRoof : Designator_Cells
	{
		// Token: 0x17002141 RID: 8513
		// (get) Token: 0x0600B83A RID: 47162 RVA: 0x004102E6 File Offset: 0x0040E4E6
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002142 RID: 8514
		// (get) Token: 0x0600B83B RID: 47163 RVA: 0x004102E9 File Offset: 0x0040E4E9
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x0600B83C RID: 47164 RVA: 0x004102F0 File Offset: 0x0040E4F0
		public Designator_AreaNoRoof()
		{
			this.defaultLabel = "DesignatorAreaNoRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaNoRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/NoRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc5;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
		}

		// Token: 0x0600B83D RID: 47165 RVA: 0x00410370 File Offset: 0x0040E570
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			RoofDef roofDef = base.Map.roofGrid.RoofAt(c);
			if (roofDef != null && roofDef.isThickRoof)
			{
				return "MessageNothingCanRemoveThickRoofs".Translate();
			}
			return !base.Map.areaManager.NoRoof[c];
		}

		// Token: 0x0600B83E RID: 47166 RVA: 0x004103F2 File Offset: 0x0040E5F2
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.NoRoof[c] = true;
			Designator_AreaNoRoof.justAddedCells.Add(c);
		}

		// Token: 0x0600B83F RID: 47167 RVA: 0x00410418 File Offset: 0x0040E618
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < Designator_AreaNoRoof.justAddedCells.Count; i++)
			{
				base.Map.areaManager.BuildRoof[Designator_AreaNoRoof.justAddedCells[i]] = false;
			}
			Designator_AreaNoRoof.justAddedCells.Clear();
		}

		// Token: 0x0600B840 RID: 47168 RVA: 0x0041046B File Offset: 0x0040E66B
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}

		// Token: 0x040069A5 RID: 27045
		private static readonly List<IntVec3> justAddedCells = new List<IntVec3>();
	}
}
