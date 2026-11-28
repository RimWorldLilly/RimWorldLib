using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000059 RID: 89
	public class MeasureTool
	{
		// Token: 0x06000267 RID: 615 RVA: 0x00013424 File Offset: 0x00011624
		public void DebugToolOnGUI()
		{
			if (Event.current.type == EventType.mouseDown)
			{
				if (Event.current.button == 0)
				{
					this.clickAction();
				}
				if (Event.current.button == 1)
				{
					DebugTools.curMeasureTool = null;
				}
				Event.current.Use();
			}
			Vector2 vector = Event.current.mousePosition + new Vector2(15f, 15f);
			Rect rect = new Rect(vector.x, vector.y, 999f, 999f);
			Text.Font = GameFont.Small;
			DevGUI.Label(rect, this.label);
			if (this.onGUIAction != null)
			{
				this.onGUIAction();
			}
		}

		// Token: 0x0400015B RID: 347
		public string label;

		// Token: 0x0400015C RID: 348
		public Action clickAction;

		// Token: 0x0400015D RID: 349
		public Action onGUIAction;
	}
}
