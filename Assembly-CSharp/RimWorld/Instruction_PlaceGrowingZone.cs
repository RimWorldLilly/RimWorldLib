using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E13 RID: 7699
	public class Instruction_PlaceGrowingZone : Lesson_Instruction
	{
		// Token: 0x0600C804 RID: 51204 RVA: 0x004960B8 File Offset: 0x004942B8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.growingZoneRect, "growingZoneRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x0600C805 RID: 51205 RVA: 0x004960F3 File Offset: 0x004942F3
		private void RecacheCells()
		{
			this.cachedCells = this.growingZoneRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x0600C806 RID: 51206 RVA: 0x0049610B File Offset: 0x0049430B
		public override void OnActivated()
		{
			base.OnActivated();
			this.growingZoneRect = TutorUtility.FindUsableRect(10, 8, base.Map, 0.5f, false);
			this.RecacheCells();
		}

		// Token: 0x0600C807 RID: 51207 RVA: 0x00496133 File Offset: 0x00494333
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.growingZoneRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600C808 RID: 51208 RVA: 0x00496151 File Offset: 0x00494351
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells, 2900);
			GenDraw.DrawArrowPointingAt(this.growingZoneRect.CenterVector3, false);
		}

		// Token: 0x0600C809 RID: 51209 RVA: 0x00496174 File Offset: 0x00494374
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAdd_Growing")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x040071E0 RID: 29152
		private CellRect growingZoneRect;

		// Token: 0x040071E1 RID: 29153
		private List<IntVec3> cachedCells;
	}
}
