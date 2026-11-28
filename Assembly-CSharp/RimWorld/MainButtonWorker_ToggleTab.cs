using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D92 RID: 7570
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x0600C468 RID: 50280 RVA: 0x0047E31C File Offset: 0x0047C51C
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
