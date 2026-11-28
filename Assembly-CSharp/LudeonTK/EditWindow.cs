using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000053 RID: 83
	public abstract class EditWindow : Window_Dev
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000233 RID: 563 RVA: 0x000104C4 File Offset: 0x0000E6C4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000234 RID: 564 RVA: 0x000104D5 File Offset: 0x0000E6D5
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000104DC File Offset: 0x0000E6DC
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0001052C File Offset: 0x0000E72C
		protected void DoRowButton(ref float x, float y, string text, string tooltip, Action action)
		{
			Vector2 vector = Text.CalcSize(text);
			Rect rect = new Rect(x, y, vector.x + 10f, 24f);
			if (DevGUI.ButtonText(rect, text, null))
			{
				action();
			}
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			x += rect.width + 4f;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0001059C File Offset: 0x0000E79C
		protected void DoImageToggle(ref float x, float y, Texture2D texture, string tooltip, ref bool toggle)
		{
			Rect rect = new Rect(x, y, 24f, 24f);
			DevGUI.CheckboxImage(rect, texture, ref toggle);
			TooltipHandler.TipRegion(rect, tooltip);
			x += 28f;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000105D0 File Offset: 0x0000E7D0
		public override void PostOpen()
		{
			while (this.windowRect.x <= (float)UI.screenWidth - 200f && this.windowRect.y <= (float)UI.screenHeight - 200f)
			{
				bool flag = false;
				foreach (EditWindow editWindow in (from di in Find.WindowStack.Windows
				where di is EditWindow
				select di).Cast<EditWindow>())
				{
					if (editWindow != this && Mathf.Abs(editWindow.windowRect.x - this.windowRect.x) < 8f && Mathf.Abs(editWindow.windowRect.y - this.windowRect.y) < 8f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				this.windowRect.x = this.windowRect.x + 16f;
				this.windowRect.y = this.windowRect.y + 16f;
			}
		}

		// Token: 0x0400012B RID: 299
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x0400012C RID: 300
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x0400012D RID: 301
		private const float SuperimposeAvoidOffsetMinEdge = 200f;
	}
}
