using System;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D93 RID: 7571
	public class MainButtonWorker_ToggleWorld : MainButtonWorker
	{
		// Token: 0x0600C46A RID: 50282 RVA: 0x0047E338 File Offset: 0x0047C538
		public override void Activate()
		{
			if (Find.World.renderer.wantedMode != WorldRenderMode.Planet)
			{
				AmbientSoundManager.EnsureWorldAmbientSoundCreated();
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				if (this.resetViewNextTime)
				{
					this.resetViewNextTime = false;
					Find.World.UI.Reset();
				}
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.FormCaravan, OpportunityType.Important);
				Find.MainTabsRoot.EscapeCurrentTab(false);
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				return;
			}
			if (Find.MainTabsRoot.OpenTab != null && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				return;
			}
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			SoundDefOf.TabClose.PlayOneShotOnCamera(null);
		}

		// Token: 0x04007033 RID: 28723
		public bool resetViewNextTime = true;
	}
}
