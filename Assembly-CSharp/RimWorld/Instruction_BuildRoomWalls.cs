using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E08 RID: 7688
	public class Instruction_BuildRoomWalls : Lesson_Instruction
	{
		// Token: 0x170024E2 RID: 9442
		// (get) Token: 0x0600C7C9 RID: 51145 RVA: 0x004952AD File Offset: 0x004934AD
		// (set) Token: 0x0600C7CA RID: 51146 RVA: 0x004952B9 File Offset: 0x004934B9
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
			set
			{
				Find.TutorialState.roomRect = value;
			}
		}

		// Token: 0x170024E3 RID: 9443
		// (get) Token: 0x0600C7CB RID: 51147 RVA: 0x004952C8 File Offset: 0x004934C8
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				using (IEnumerator<IntVec3> enumerator = this.RoomRect.EdgeCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, ThingDefOf.Wall))
						{
							num2++;
						}
						num++;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x0600C7CC RID: 51148 RVA: 0x00495338 File Offset: 0x00493538
		public override void OnActivated()
		{
			base.OnActivated();
			this.RoomRect = TutorUtility.FindUsableRect(12, 8, base.Map, 0f, false);
		}

		// Token: 0x0600C7CD RID: 51149 RVA: 0x0049535A File Offset: 0x0049355A
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600C7CE RID: 51150 RVA: 0x00495378 File Offset: 0x00493578
		public override void LessonUpdate()
		{
			this.cachedEdgeCells.Clear();
			this.cachedEdgeCells.AddRange((from c in this.RoomRect.EdgeCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Wall)
			select c).ToList<IntVec3>());
			GenDraw.DrawFieldEdges((from c in this.cachedEdgeCells
			where c.GetEdifice(base.Map) == null
			select c).ToList<IntVec3>(), 2900);
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0600C7CF RID: 51151 RVA: 0x00495410 File Offset: 0x00493610
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Wall")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.cachedEdgeCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x040071DA RID: 29146
		private List<IntVec3> cachedEdgeCells = new List<IntVec3>();
	}
}
