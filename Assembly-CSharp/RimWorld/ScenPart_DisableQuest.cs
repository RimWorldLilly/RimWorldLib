using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A7 RID: 6055
	public class ScenPart_DisableQuest : ScenPart
	{
		// Token: 0x060096EE RID: 38638 RVA: 0x0036C180 File Offset: 0x0036A380
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<QuestScriptDef>(ref this.questDef, "questDef");
		}

		// Token: 0x060096EF RID: 38639 RVA: 0x0036C198 File Offset: 0x0036A398
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

		// Token: 0x04005A77 RID: 23159
		public QuestScriptDef questDef;
	}
}
