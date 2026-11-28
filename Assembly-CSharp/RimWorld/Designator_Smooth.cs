using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5F RID: 7263
	public abstract class Designator_Smooth : Designator_Cells
	{
		// Token: 0x1700217E RID: 8574
		// (get) Token: 0x0600B94F RID: 47439 RVA: 0x004159CC File Offset: 0x00413BCC
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700217F RID: 8575
		// (get) Token: 0x0600B950 RID: 47440 RVA: 0x004159CF File Offset: 0x00413BCF
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Orders;
			}
		}

		// Token: 0x0600B951 RID: 47441 RVA: 0x004159D6 File Offset: 0x00413BD6
		protected Designator_Smooth()
		{
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_SmoothSurface;
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x0600B952 RID: 47442 RVA: 0x00415A11 File Offset: 0x00413C11
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x0600B953 RID: 47443 RVA: 0x00415A18 File Offset: 0x00413C18
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (c.InNoBuildEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
