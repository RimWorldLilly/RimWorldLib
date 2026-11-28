using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001D94 RID: 7572
	public class MainTabsRoot
	{
		// Token: 0x1700247A RID: 9338
		// (get) Token: 0x0600C46C RID: 50284 RVA: 0x0047E40C File Offset: 0x0047C60C
		public MainButtonDef OpenTab
		{
			get
			{
				MainTabWindow mainTabWindow = Find.WindowStack.WindowOfType<MainTabWindow>();
				if (mainTabWindow == null)
				{
					return null;
				}
				return mainTabWindow.def;
			}
		}

		// Token: 0x0600C46D RID: 50285 RVA: 0x0047E430 File Offset: 0x0047C630
		public void HandleLowPriorityShortcuts()
		{
			this.AutoCloseInspectionTabIfNothingSelected(true);
			if (Find.Selector.NumSelected == 0 && Event.current.type == EventType.mouseDown && Event.current.button == 1 && !WorldRendererUtility.WorldSelected && (!SteamDeck.IsSteamDeck || !Input.GetMouseButton(2)))
			{
				Event.current.Use();
				MainButtonDefOf.Architect.Worker.InterfaceTryActivate();
			}
			if (this.OpenTab != null && this.OpenTab != MainButtonDefOf.Inspect && Event.current.type == EventType.mouseDown && Event.current.button != 2)
			{
				this.EscapeCurrentTab(true);
				if (Event.current.button == 0)
				{
					Find.Selector.ClearSelection();
					Find.WorldSelector.ClearSelection();
				}
			}
		}

		// Token: 0x0600C46E RID: 50286 RVA: 0x0047E4ED File Offset: 0x0047C6ED
		public void EscapeCurrentTab(bool playSound = true)
		{
			this.SetCurrentTab(null, playSound);
		}

		// Token: 0x0600C46F RID: 50287 RVA: 0x0047E4F7 File Offset: 0x0047C6F7
		public void SetCurrentTab(MainButtonDef tab, bool playSound = true)
		{
			if (tab == this.OpenTab)
			{
				return;
			}
			this.ToggleTab(tab, playSound);
		}

		// Token: 0x0600C470 RID: 50288 RVA: 0x0047E50C File Offset: 0x0047C70C
		public void ToggleTab(MainButtonDef newTab, bool playSound = true)
		{
			if (this.OpenTab == null && newTab == null)
			{
				return;
			}
			if (this.OpenTab == newTab)
			{
				Find.WindowStack.TryRemove(this.OpenTab.TabWindow, true);
				if (playSound)
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					return;
				}
			}
			else
			{
				if (this.OpenTab != null)
				{
					Find.WindowStack.TryRemove(this.OpenTab.TabWindow, false);
				}
				if (newTab != null)
				{
					Find.WindowStack.Add(newTab.TabWindow);
				}
				if (playSound)
				{
					if (newTab == null)
					{
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
					}
				}
				if (TutorSystem.TutorialMode && newTab != null)
				{
					TutorSystem.Notify_Event("MainTab-" + newTab.defName + "-Open");
				}
			}
		}

		// Token: 0x0600C471 RID: 50289 RVA: 0x0047E5D0 File Offset: 0x0047C7D0
		public void Notify_SelectedObjectDespawned()
		{
			this.AutoCloseInspectionTabIfNothingSelected(false);
		}

		// Token: 0x0600C472 RID: 50290 RVA: 0x0047E5D9 File Offset: 0x0047C7D9
		private void AutoCloseInspectionTabIfNothingSelected(bool playSound)
		{
			if (this.OpenTab == MainButtonDefOf.Inspect && (Find.Selector.NumSelected == 0 || WorldRendererUtility.WorldSelected))
			{
				this.EscapeCurrentTab(playSound);
			}
		}
	}
}
