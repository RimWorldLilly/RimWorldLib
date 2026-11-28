using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C35 RID: 7221
	public abstract class Designator_AreaHome : Designator_Cells
	{
		// Token: 0x1700213D RID: 8509
		// (get) Token: 0x0600B82B RID: 47147 RVA: 0x0040FF8D File Offset: 0x0040E18D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700213E RID: 8510
		// (get) Token: 0x0600B82C RID: 47148 RVA: 0x0040FF90 File Offset: 0x0040E190
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x0600B82D RID: 47149 RVA: 0x0040FF97 File Offset: 0x0040E197
		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x0600B82E RID: 47150 RVA: 0x0040FFD0 File Offset: 0x0040E1D0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.Home[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x0600B82F RID: 47151 RVA: 0x00410021 File Offset: 0x0040E221
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.Home[c] = true;
				return;
			}
			base.Map.areaManager.Home[c] = false;
		}

		// Token: 0x0600B830 RID: 47152 RVA: 0x0041005A File Offset: 0x0040E25A
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		// Token: 0x0600B831 RID: 47153 RVA: 0x0041006D File Offset: 0x0040E26D
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}

		// Token: 0x040069A4 RID: 27044
		private DesignateMode mode;
	}
}
