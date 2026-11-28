using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E09 RID: 7689
	public class Instruction_BuildSandbags : Lesson_Instruction
	{
		// Token: 0x170024E4 RID: 9444
		// (get) Token: 0x0600C7D3 RID: 51155 RVA: 0x00495478 File Offset: 0x00493678
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				using (List<IntVec3>.Enumerator enumerator = this.sandbagCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, ThingDefOf.Sandbags))
						{
							num2++;
						}
						num++;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x0600C7D4 RID: 51156 RVA: 0x004954E8 File Offset: 0x004936E8
		public override void OnActivated()
		{
			base.OnActivated();
			Find.TutorialState.sandbagsRect = TutorUtility.FindUsableRect(7, 7, base.Map, 0f, false);
			this.sandbagCells = new List<IntVec3>();
			foreach (IntVec3 intVec in Find.TutorialState.sandbagsRect.EdgeCells)
			{
				if (intVec.x != Find.TutorialState.sandbagsRect.CenterCell.x && intVec.z != Find.TutorialState.sandbagsRect.CenterCell.z)
				{
					this.sandbagCells.Add(intVec);
				}
			}
			foreach (IntVec3 c in Find.TutorialState.sandbagsRect.ContractedBy(1))
			{
				if (!Find.TutorialState.sandbagsRect.ContractedBy(2).Contains(c))
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Thing thing = thingList[i];
						if (thing.def.passability != Traversability.Standable && (thing.def.category == ThingCategory.Plant || thing.def.category == ThingCategory.Item))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x0600C7D5 RID: 51157 RVA: 0x0049567C File Offset: 0x0049387C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.sandbagCells, "sandbagCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x0600C7D6 RID: 51158 RVA: 0x0049569A File Offset: 0x0049389A
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.sandbagCells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600C7D7 RID: 51159 RVA: 0x004956C0 File Offset: 0x004938C0
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges((from c in this.sandbagCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Sandbags)
			select c).ToList<IntVec3>(), 2900);
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.sandbagCells), false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0600C7D8 RID: 51160 RVA: 0x0049571B File Offset: 0x0049391B
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Sandbags")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.sandbagCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x040071DB RID: 29147
		private List<IntVec3> sandbagCells;
	}
}
