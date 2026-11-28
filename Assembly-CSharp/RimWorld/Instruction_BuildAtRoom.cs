using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E04 RID: 7684
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		// Token: 0x170024DD RID: 9437
		// (get) Token: 0x0600C7B3 RID: 51123
		protected abstract CellRect BuildableRect { get; }

		// Token: 0x170024DE RID: 9438
		// (get) Token: 0x0600C7B4 RID: 51124 RVA: 0x00494F41 File Offset: 0x00493141
		protected override float ProgressPercent
		{
			get
			{
				if (this.def.targetCount <= 1)
				{
					return -1f;
				}
				return (float)this.NumPlaced() / (float)this.def.targetCount;
			}
		}

		// Token: 0x0600C7B5 RID: 51125 RVA: 0x00494F6C File Offset: 0x0049316C
		protected int NumPlaced()
		{
			int num = 0;
			using (CellRect.Enumerator enumerator = this.BuildableRect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, this.def.thingDef))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600C7B6 RID: 51126 RVA: 0x00494FDC File Offset: 0x004931DC
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600C7B7 RID: 51127 RVA: 0x00495010 File Offset: 0x00493210
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		// Token: 0x0600C7B8 RID: 51128 RVA: 0x00495034 File Offset: 0x00493234
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-" + this.def.thingDef.defName)
			{
				return this.AllowBuildAt(ep.Cell);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0600C7B9 RID: 51129 RVA: 0x00495084 File Offset: 0x00493284
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		// Token: 0x0600C7BA RID: 51130 RVA: 0x004950A0 File Offset: 0x004932A0
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
