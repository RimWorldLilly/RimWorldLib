using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E15 RID: 7701
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		// Token: 0x170024EF RID: 9455
		// (get) Token: 0x0600C812 RID: 51218 RVA: 0x0049629D File Offset: 0x0049449D
		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		// Token: 0x0600C813 RID: 51219 RVA: 0x004962D8 File Offset: 0x004944D8
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600C814 RID: 51220 RVA: 0x00496300 File Offset: 0x00494500
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}
	}
}
