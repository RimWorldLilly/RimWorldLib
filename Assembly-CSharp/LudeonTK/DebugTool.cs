using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200003B RID: 59
	public class DebugTool
	{
		// Token: 0x0600018C RID: 396 RVA: 0x0000B59F File Offset: 0x0000979F
		public DebugTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000B5BC File Offset: 0x000097BC
		public DebugTool(string label, Action clickAction, IntVec3 firstRectCorner)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = delegate()
			{
				IntVec3 intVec = UI.MouseCell();
				Vector3 vector = firstRectCorner.ToVector3Shifted();
				Vector3 vector2 = intVec.ToVector3Shifted();
				if (vector.x < vector2.x)
				{
					vector.x -= 0.5f;
					vector2.x += 0.5f;
				}
				else
				{
					vector.x += 0.5f;
					vector2.x -= 0.5f;
				}
				if (vector.z < vector2.z)
				{
					vector.z -= 0.5f;
					vector2.z += 0.5f;
				}
				else
				{
					vector.z += 0.5f;
					vector2.z -= 0.5f;
				}
				Vector2 vector3 = vector.MapToUIPosition();
				Vector2 vector4 = vector2.MapToUIPosition();
				DevGUI.DrawBox(new Rect(vector3.x, vector3.y, vector4.x - vector3.x, vector4.y - vector3.y), 3, null);
			};
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000B5FC File Offset: 0x000097FC
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
					DebugTools.curTool = null;
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

		// Token: 0x040000AD RID: 173
		private string label;

		// Token: 0x040000AE RID: 174
		private Action clickAction;

		// Token: 0x040000AF RID: 175
		private Action onGUIAction;
	}
}
