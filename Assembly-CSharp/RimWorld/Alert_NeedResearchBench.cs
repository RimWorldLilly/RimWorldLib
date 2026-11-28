using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C12 RID: 7186
	public class Alert_NeedResearchBench : Alert
	{
		// Token: 0x1700211E RID: 8478
		// (get) Token: 0x0600B776 RID: 46966 RVA: 0x0040ACA0 File Offset: 0x00408EA0
		private bool HasRequiredResearchBench
		{
			get
			{
				ResearchProjectDef project = Find.ResearchManager.GetProject(null);
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (project.requiredResearchBuilding != null)
					{
						if (maps[i].listerBuildings.ColonistsHaveBuilding(project.requiredResearchBuilding))
						{
							return true;
						}
					}
					else if (maps[i].listerBuildings.ColonistsHaveResearchBench())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600B777 RID: 46967 RVA: 0x0040AD09 File Offset: 0x00408F09
		public Alert_NeedResearchBench()
		{
			this.defaultLabel = "NeedResearchBench".Translate();
		}

		// Token: 0x0600B778 RID: 46968 RVA: 0x0040AD26 File Offset: 0x00408F26
		public override AlertReport GetReport()
		{
			if (Find.CurrentGravship != null)
			{
				return false;
			}
			return Find.ResearchManager.GetProject(null) != null && !this.HasRequiredResearchBench;
		}

		// Token: 0x0600B779 RID: 46969 RVA: 0x0040AD54 File Offset: 0x00408F54
		protected override void OnClick()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
		}

		// Token: 0x0600B77A RID: 46970 RVA: 0x0040AD68 File Offset: 0x00408F68
		public override TaggedString GetExplanation()
		{
			ResearchProjectDef project = Find.ResearchManager.GetProject(null);
			return "NeedResearchBenchDesc".Translate(project.label, project.requiredResearchBuilding ?? ThingDefOf.SimpleResearchBench) + ("\n\n(" + "ClickToOpenResearchTab".Translate() + ")");
		}
	}
}
