using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000636 RID: 1590
	public class ScreenshotModeHandler
	{
		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060031D1 RID: 12753 RVA: 0x0013E832 File Offset: 0x0013CA32
		// (set) Token: 0x060031D2 RID: 12754 RVA: 0x0013E83A File Offset: 0x0013CA3A
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060031D3 RID: 12755 RVA: 0x0013E844 File Offset: 0x0013CA44
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.repaint || Event.current.type == EventType.layout || (Event.current.type == EventType.mouseDown || Event.current.type == EventType.mouseUp || Event.current.type == EventType.mouseDrag));
			}
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x0013E8A0 File Offset: 0x0013CAA0
		public void ScreenshotModesOnGUI()
		{
			if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent && !WorldComponent_GravshipController.CutsceneInProgress)
			{
				this.active = !this.active;
				Event.current.Use();
			}
		}

		// Token: 0x04001FEC RID: 8172
		private bool active;
	}
}
