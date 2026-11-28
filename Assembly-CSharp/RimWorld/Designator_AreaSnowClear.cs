using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3D RID: 7229
	public abstract class Designator_AreaSnowClear : Designator_Cells
	{
		// Token: 0x17002145 RID: 8517
		// (get) Token: 0x0600B84A RID: 47178 RVA: 0x00410670 File Offset: 0x0040E870
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002146 RID: 8518
		// (get) Token: 0x0600B84B RID: 47179 RVA: 0x00410673 File Offset: 0x0040E873
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x0600B84C RID: 47180 RVA: 0x0041067C File Offset: 0x0040E87C
		public Designator_AreaSnowClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "AreaSnowClear";
		}

		// Token: 0x0600B84D RID: 47181 RVA: 0x004106CC File Offset: 0x0040E8CC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.SnowOrSandClear[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x0600B84E RID: 47182 RVA: 0x0041071D File Offset: 0x0040E91D
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.SnowOrSandClear[c] = true;
				return;
			}
			base.Map.areaManager.SnowOrSandClear[c] = false;
		}

		// Token: 0x0600B84F RID: 47183 RVA: 0x00410756 File Offset: 0x0040E956
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowOrSandClear.MarkForDraw();
		}

		// Token: 0x040069A7 RID: 27047
		private DesignateMode mode;
	}
}
