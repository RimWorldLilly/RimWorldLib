using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E02 RID: 7682
	public class Instruction_AddBill : Lesson_Instruction
	{
		// Token: 0x170024DC RID: 9436
		// (get) Token: 0x0600C7AB RID: 51115 RVA: 0x00494D80 File Offset: 0x00492F80
		protected override float ProgressPercent
		{
			get
			{
				int num = this.def.recipeTargetCount + 1;
				int num2 = 0;
				Bill_Production bill_Production = this.RelevantBill();
				if (bill_Production != null)
				{
					num2++;
					if (bill_Production.repeatMode == BillRepeatModeDefOf.RepeatCount)
					{
						num2 += bill_Production.repeatCount;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x0600C7AC RID: 51116 RVA: 0x00494DC8 File Offset: 0x00492FC8
		private Bill_Production RelevantBill()
		{
			if (Find.Selector.SingleSelectedThing != null && Find.Selector.SingleSelectedThing.def == this.def.thingDef)
			{
				IBillGiver billGiver = Find.Selector.SingleSelectedThing as IBillGiver;
				if (billGiver != null)
				{
					return (Bill_Production)billGiver.BillStack.Bills.FirstOrDefault((Bill b) => b.recipe == this.def.recipeDef);
				}
			}
			return null;
		}

		// Token: 0x0600C7AD RID: 51117 RVA: 0x00494E33 File Offset: 0x00493033
		private IEnumerable<Thing> ThingsToSelect()
		{
			Instruction_AddBill.<ThingsToSelect>d__3 <ThingsToSelect>d__ = new Instruction_AddBill.<ThingsToSelect>d__3(-2);
			<ThingsToSelect>d__.<>4__this = this;
			return <ThingsToSelect>d__;
		}

		// Token: 0x0600C7AE RID: 51118 RVA: 0x00494E44 File Offset: 0x00493044
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.ThingsToSelect())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			if (this.RelevantBill() == null)
			{
				UIHighlighter.HighlightTag("AddBill");
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600C7AF RID: 51119 RVA: 0x00494EB4 File Offset: 0x004930B4
		public override void LessonUpdate()
		{
			foreach (Thing thing in this.ThingsToSelect())
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, false);
			}
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
