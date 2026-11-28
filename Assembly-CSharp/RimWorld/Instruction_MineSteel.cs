using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E12 RID: 7698
	public class Instruction_MineSteel : Lesson_Instruction
	{
		// Token: 0x170024EE RID: 9454
		// (get) Token: 0x0600C7FC RID: 51196 RVA: 0x00495EB0 File Offset: 0x004940B0
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.mineCells.Count; i++)
				{
					IntVec3 c = this.mineCells[i];
					if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.Mine) != null || c.GetEdifice(base.Map) == null || c.GetEdifice(base.Map).def != ThingDefOf.MineableSteel)
					{
						num++;
					}
				}
				return (float)num / (float)this.mineCells.Count;
			}
		}

		// Token: 0x0600C7FD RID: 51197 RVA: 0x00495F33 File Offset: 0x00494133
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.mineCells, "mineCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x0600C7FE RID: 51198 RVA: 0x00495F54 File Offset: 0x00494154
		public override void OnActivated()
		{
			base.OnActivated();
			CellRect cellRect = TutorUtility.FindUsableRect(10, 10, base.Map, 0f, true);
			new GenStep_ScatterLumpsMineable
			{
				forcedDefToScatter = ThingDefOf.MineableSteel
			}.ForceScatterAt(cellRect.CenterCell, base.Map);
			this.mineCells = new List<IntVec3>();
			foreach (IntVec3 intVec in cellRect)
			{
				Building edifice = intVec.GetEdifice(base.Map);
				if (edifice != null && edifice.def == ThingDefOf.MineableSteel)
				{
					this.mineCells.Add(intVec);
				}
			}
		}

		// Token: 0x0600C7FF RID: 51199 RVA: 0x00496010 File Offset: 0x00494210
		public override void LessonOnGUI()
		{
			if (!this.mineCells.NullOrEmpty<IntVec3>())
			{
				TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.mineCells), this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600C800 RID: 51200 RVA: 0x00496040 File Offset: 0x00494240
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.mineCells), false);
		}

		// Token: 0x0600C801 RID: 51201 RVA: 0x00496053 File Offset: 0x00494253
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.mineCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0600C802 RID: 51202 RVA: 0x00496081 File Offset: 0x00494281
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine" && this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x040071DF RID: 29151
		private List<IntVec3> mineCells;
	}
}
