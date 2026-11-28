using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D91 RID: 7569
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x17002479 RID: 9337
		// (get) Token: 0x0600C466 RID: 50278 RVA: 0x0047E2EC File Offset: 0x0047C4EC
		public override float ButtonBarPercent
		{
			get
			{
				ResearchProjectDef project = Find.ResearchManager.GetProject(null);
				if (project == null)
				{
					return 0f;
				}
				return project.ProgressPercent;
			}
		}
	}
}
