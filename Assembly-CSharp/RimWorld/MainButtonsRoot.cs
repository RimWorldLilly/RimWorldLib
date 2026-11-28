using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D8F RID: 7567
	public class MainButtonsRoot
	{
		// Token: 0x17002476 RID: 9334
		// (get) Token: 0x0600C45E RID: 50270 RVA: 0x0047DEF8 File Offset: 0x0047C0F8
		private int VisibleButtonsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.allButtonsInOrder.Count; i++)
				{
					if (this.allButtonsInOrder[i].buttonVisible)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600C45F RID: 50271 RVA: 0x0047DF38 File Offset: 0x0047C138
		public MainButtonsRoot()
		{
			this.allButtonsInOrder = (from x in DefDatabase<MainButtonDef>.AllDefs
			orderby x.order
			select x).ToList<MainButtonDef>();
		}

		// Token: 0x0600C460 RID: 50272 RVA: 0x0047DF8C File Offset: 0x0047C18C
		public void MainButtonsOnGUI()
		{
			if (Event.current.type == EventType.layout)
			{
				return;
			}
			this.DoButtons();
			for (int i = 0; i < this.allButtonsInOrder.Count; i++)
			{
				if (!this.allButtonsInOrder[i].Worker.Disabled && this.allButtonsInOrder[i].hotKey != null && this.allButtonsInOrder[i].hotKey.KeyDownEvent)
				{
					Event.current.Use();
					this.allButtonsInOrder[i].Worker.InterfaceTryActivate();
					return;
				}
			}
		}

		// Token: 0x0600C461 RID: 50273 RVA: 0x0047E028 File Offset: 0x0047C228
		public void HandleLowPriorityShortcuts()
		{
			this.tabs.HandleLowPriorityShortcuts();
			if (WorldRendererUtility.WorldSelected && Current.ProgramState == ProgramState.Playing && Find.CurrentMap != null && KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			}
		}

		// Token: 0x0600C462 RID: 50274 RVA: 0x0047E07C File Offset: 0x0047C27C
		private void DoButtons()
		{
			float num = 0f;
			for (int i = 0; i < this.allButtonsInOrder.Count; i++)
			{
				if (this.allButtonsInOrder[i].Worker.Visible)
				{
					num += (this.allButtonsInOrder[i].minimized ? 0.5f : 1f);
				}
			}
			GUI.color = Color.white;
			int num2 = (int)((float)UI.screenWidth / num);
			int num3 = num2 / 2;
			int num4 = this.allButtonsInOrder.FindLastIndex((MainButtonDef x) => x.Worker.Visible);
			int num5 = 0;
			for (int j = 0; j < this.allButtonsInOrder.Count; j++)
			{
				if (this.allButtonsInOrder[j].Worker.Visible)
				{
					int num6 = this.allButtonsInOrder[j].minimized ? num3 : num2;
					if (j == num4)
					{
						num6 = UI.screenWidth - num5;
					}
					Rect rect = new Rect((float)num5, (float)(UI.screenHeight - 35), (float)num6, 36f);
					this.allButtonsInOrder[j].Worker.DoButton(rect);
					num5 += num6;
				}
			}
		}

		// Token: 0x0400702D RID: 28717
		public MainTabsRoot tabs = new MainTabsRoot();

		// Token: 0x0400702E RID: 28718
		private List<MainButtonDef> allButtonsInOrder;
	}
}
