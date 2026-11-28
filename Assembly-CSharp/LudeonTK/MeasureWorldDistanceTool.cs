using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200005A RID: 90
	public class MeasureWorldDistanceTool : MeasureTool
	{
		// Token: 0x06000269 RID: 617 RVA: 0x000134D7 File Offset: 0x000116D7
		public MeasureWorldDistanceTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000134F4 File Offset: 0x000116F4
		public MeasureWorldDistanceTool(string label, Action clickAction, Vector3 firstRectCorner)
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
