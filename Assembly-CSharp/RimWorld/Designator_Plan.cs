using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C74 RID: 7284
	public abstract class Designator_Plan : Designator_Cells
	{
		// Token: 0x170021A9 RID: 8617
		// (get) Token: 0x0600B9F2 RID: 47602 RVA: 0x0041943E File Offset: 0x0041763E
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021AA RID: 8618
		// (get) Token: 0x0600B9F3 RID: 47603 RVA: 0x00419441 File Offset: 0x00417641
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021AB RID: 8619
		// (get) Token: 0x0600B9F4 RID: 47604 RVA: 0x00419444 File Offset: 0x00417644
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Plans;
			}
		}

		// Token: 0x0600B9F5 RID: 47605 RVA: 0x0041944B File Offset: 0x0041764B
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoZoneEdgeLines();
		}
	}
}
