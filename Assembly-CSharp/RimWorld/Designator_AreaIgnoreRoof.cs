using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C38 RID: 7224
	public class Designator_AreaIgnoreRoof : Designator_Cells
	{
		// Token: 0x1700213F RID: 8511
		// (get) Token: 0x0600B834 RID: 47156 RVA: 0x0041018B File Offset: 0x0040E38B
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002140 RID: 8512
		// (get) Token: 0x0600B835 RID: 47157 RVA: 0x0041018E File Offset: 0x0040E38E
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x0600B836 RID: 47158 RVA: 0x00410198 File Offset: 0x0040E398
		public Designator_AreaIgnoreRoof()
		{
			this.defaultLabel = "DesignatorAreaIgnoreRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaIgnoreRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/IgnoreRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc11;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
		}

		// Token: 0x0600B837 RID: 47159 RVA: 0x00410218 File Offset: 0x0040E418
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
			return base.Map.areaManager.BuildRoof[c] || base.Map.areaManager.NoRoof[c];
		}

		// Token: 0x0600B838 RID: 47160 RVA: 0x00410285 File Offset: 0x0040E485
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = false;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x0600B839 RID: 47161 RVA: 0x004102B5 File Offset: 0x0040E4B5
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
