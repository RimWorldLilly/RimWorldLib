using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E14 RID: 7700
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x0600C80B RID: 51211 RVA: 0x004961AC File Offset: 0x004943AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x0600C80C RID: 51212 RVA: 0x004961E7 File Offset: 0x004943E7
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x0600C80D RID: 51213 RVA: 0x004961FF File Offset: 0x004943FF
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x0600C80E RID: 51214 RVA: 0x00496226 File Offset: 0x00494426
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600C80F RID: 51215 RVA: 0x00496244 File Offset: 0x00494444
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells, 2900);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x0600C810 RID: 51216 RVA: 0x00496267 File Offset: 0x00494467
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAddStockpile_Resources")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x040071E2 RID: 29154
		private CellRect stockpileRect;

		// Token: 0x040071E3 RID: 29155
		private List<IntVec3> cachedCells;
	}
}
