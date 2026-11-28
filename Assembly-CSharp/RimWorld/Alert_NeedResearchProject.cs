using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BED RID: 7149
	public class Alert_NeedResearchProject : Alert
	{
		// Token: 0x0600B6E0 RID: 46816 RVA: 0x00407112 File Offset: 0x00405312
		public Alert_NeedResearchProject()
		{
			this.defaultLabel = "NeedResearchProject".Translate();
			this.defaultExplanation = "NeedResearchProjectDesc".Translate();
		}

		// Token: 0x0600B6E1 RID: 46817 RVA: 0x00407144 File Offset: 0x00405344
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			if (Find.ResearchManager.GetProject(null) != null)
			{
				return false;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].listerBuildings.ColonistsHaveResearchBench())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (!Find.ResearchManager.AnyProjectIsAvailable)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B6E2 RID: 46818 RVA: 0x004071D4 File Offset: 0x004053D4
		protected override void OnClick()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
			((MainTabWindow_Research)MainButtonDefOf.Research.TabWindow).CurTab = ResearchTabDefOf.Main;
		}
	}
}
