using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000052 RID: 82
	public class DrawMeasureTool : MeasureTool
	{
		// Token: 0x06000231 RID: 561 RVA: 0x00010465 File Offset: 0x0000E665
		public DrawMeasureTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00010484 File Offset: 0x0000E684
		public DrawMeasureTool(string label, Action clickAction, Vector3 firstRectCorner)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = delegate()
			{
				Vector3 v = UI.MouseMapPosition();
				Vector2 start = firstRectCorner.MapToUIPosition();
				Vector2 end = v.MapToUIPosition();
				DevGUI.DrawLine(start, end, Color.white, 0.25f);
			};
		}
	}
}
