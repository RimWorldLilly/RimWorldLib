using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E07 RID: 7687
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		// Token: 0x170024E1 RID: 9441
		// (get) Token: 0x0600C7C1 RID: 51137 RVA: 0x0049511C File Offset: 0x0049331C
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		// Token: 0x0600C7C2 RID: 51138 RVA: 0x00495128 File Offset: 0x00493328
		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		// Token: 0x0600C7C3 RID: 51139 RVA: 0x0049516C File Offset: 0x0049336C
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600C7C4 RID: 51140 RVA: 0x0049518C File Offset: 0x0049338C
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		// Token: 0x0600C7C5 RID: 51141 RVA: 0x004951AD File Offset: 0x004933AD
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.allowedPlaceCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0600C7C6 RID: 51142 RVA: 0x004951DB File Offset: 0x004933DB
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x040071D9 RID: 29145
		private List<IntVec3> allowedPlaceCells;
	}
}
