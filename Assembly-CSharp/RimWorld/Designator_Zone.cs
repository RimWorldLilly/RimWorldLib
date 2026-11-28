using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7C RID: 7292
	public abstract class Designator_Zone : Designator_Cells
	{
		// Token: 0x170021C4 RID: 8644
		// (get) Token: 0x0600BA3A RID: 47674 RVA: 0x0041A779 File Offset: 0x00418979
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021C5 RID: 8645
		// (get) Token: 0x0600BA3B RID: 47675 RVA: 0x0041A77C File Offset: 0x0041897C
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Zones;
			}
		}

		// Token: 0x0600BA3C RID: 47676 RVA: 0x0041A783 File Offset: 0x00418983
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			GenUI.RenderMouseoverBracket();
			OverlayDrawHandler.DrawZonesThisFrame();
			if (Find.Selector.SelectedZone != null)
			{
				GenDraw.DrawFieldEdges(Find.Selector.SelectedZone.Cells, 2900);
			}
			GenDraw.DrawNoZoneEdgeLines();
		}
	}
}
