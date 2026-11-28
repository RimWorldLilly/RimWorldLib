using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E17 RID: 7703
	public class Instruction_UnforbidStartingResources : Lesson_Instruction
	{
		// Token: 0x170024F1 RID: 9457
		// (get) Token: 0x0600C81A RID: 51226 RVA: 0x004963EC File Offset: 0x004945EC
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from it in Find.TutorialState.startingItems
				where !it.IsForbidden(Faction.OfPlayer) || it.Destroyed
				select it).Count<Thing>() / (float)Find.TutorialState.startingItems.Count;
			}
		}

		// Token: 0x0600C81B RID: 51227 RVA: 0x0049643E File Offset: 0x0049463E
		private IEnumerable<Thing> NeedUnforbidItems()
		{
			return from it in Find.TutorialState.startingItems
			where it.IsForbidden(Faction.OfPlayer) && !it.Destroyed
			select it;
		}

		// Token: 0x0600C81C RID: 51228 RVA: 0x0049646E File Offset: 0x0049466E
		public override void PostDeactivated()
		{
			base.PostDeactivated();
			Find.TutorialState.startingItems.RemoveAll((Thing it) => !Instruction_EquipWeapons.IsWeapon(it));
		}

		// Token: 0x0600C81D RID: 51229 RVA: 0x004964A8 File Offset: 0x004946A8
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.NeedUnforbidItems())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600C81E RID: 51230 RVA: 0x00496504 File Offset: 0x00494704
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
			foreach (Thing thing in this.NeedUnforbidItems())
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, true);
			}
		}
	}
}
