using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A4 RID: 6052
	public class ScenPart_CreateQuest : ScenPart
	{
		// Token: 0x060096E6 RID: 38630 RVA: 0x0036BFF6 File Offset: 0x0036A1F6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<QuestScriptDef>(ref this.questDef, "questDef");
		}

		// Token: 0x060096E7 RID: 38631 RVA: 0x0036C010 File Offset: 0x0036A210
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			string label;
			if (this.questDef != null)
			{
				label = ((!string.IsNullOrEmpty(this.questDef.label)) ? this.questDef.LabelCap : this.questDef.defName);
			}
			else
			{
				label = "ScenPart_SelectQuestDef".Translate();
			}
			if (Widgets.ButtonText(scenPartRect, label, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (QuestScriptDef questScriptDef in DefDatabase<QuestScriptDef>.AllDefs)
				{
					QuestScriptDef localFd = questScriptDef;
					string label2 = (!string.IsNullOrEmpty(questScriptDef.label)) ? questScriptDef.LabelCap : questScriptDef.defName;
					list.Add(new FloatMenuOption(label2, delegate()
					{
						this.questDef = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x060096E8 RID: 38632 RVA: 0x0036C13C File Offset: 0x0036A33C
		public override void PostGameStart()
		{
			if (this.questDef == null)
			{
				return;
			}
			QuestUtility.GenerateQuestAndMakeAvailable(this.questDef, 0f);
		}

		// Token: 0x04005A76 RID: 23158
		private QuestScriptDef questDef;
	}
}
