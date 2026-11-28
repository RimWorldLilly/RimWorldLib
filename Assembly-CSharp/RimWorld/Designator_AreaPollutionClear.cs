using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3A RID: 7226
	public abstract class Designator_AreaPollutionClear : Designator_Cells
	{
		// Token: 0x17002143 RID: 8515
		// (get) Token: 0x0600B842 RID: 47170 RVA: 0x004104A8 File Offset: 0x0040E6A8
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002144 RID: 8516
		// (get) Token: 0x0600B843 RID: 47171 RVA: 0x004104AB File Offset: 0x0040E6AB
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x0600B844 RID: 47172 RVA: 0x004104B2 File Offset: 0x0040E6B2
		public Designator_AreaPollutionClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x0600B845 RID: 47173 RVA: 0x004104E0 File Offset: 0x0040E6E0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.PollutionClear[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x0600B846 RID: 47174 RVA: 0x00410531 File Offset: 0x0040E731
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.PollutionClear[c] = true;
				return;
			}
			base.Map.areaManager.PollutionClear[c] = false;
		}

		// Token: 0x0600B847 RID: 47175 RVA: 0x0041056A File Offset: 0x0040E76A
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.PollutionClear.MarkForDraw();
		}

		// Token: 0x040069A6 RID: 27046
		private DesignateMode mode;
	}
}
