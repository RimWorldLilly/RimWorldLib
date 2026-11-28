using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200067F RID: 1663
	public class WindowResizer
	{
		// Token: 0x0600346B RID: 13419 RVA: 0x00151444 File Offset: 0x0014F644
		public Rect DoResizeControl(Rect winRect)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect rect = new Rect(winRect.width - 24f, winRect.height - 24f, 24f, 24f);
			if (Event.current.type == EventType.mouseDown && Mouse.IsOver(rect))
			{
				this.isResizing = true;
				this.resizeStart = new Rect(mousePosition.x, mousePosition.y, winRect.width, winRect.height);
			}
			if (this.isResizing)
			{
				winRect.width = this.resizeStart.width + (mousePosition.x - this.resizeStart.x);
				winRect.height = this.resizeStart.height + (mousePosition.y - this.resizeStart.y);
				if (winRect.width < this.minWindowSize.x)
				{
					winRect.width = this.minWindowSize.x;
				}
				if (winRect.height < this.minWindowSize.y)
				{
					winRect.height = this.minWindowSize.y;
				}
				winRect.xMax = Mathf.Min((float)UI.screenWidth, winRect.xMax);
				winRect.yMax = Mathf.Min((float)UI.screenHeight, winRect.yMax);
				if (Event.current.type == EventType.mouseUp)
				{
					this.isResizing = false;
				}
			}
			Widgets.ButtonImage(rect, TexUI.WinExpandWidget, true, null);
			return new Rect(winRect.x, winRect.y, (float)((int)winRect.width), (float)((int)winRect.height));
		}

		// Token: 0x04002258 RID: 8792
		public Vector2 minWindowSize = new Vector2(150f, 150f);

		// Token: 0x04002259 RID: 8793
		private bool isResizing;

		// Token: 0x0400225A RID: 8794
		private Rect resizeStart;

		// Token: 0x0400225B RID: 8795
		private const float ResizeButtonSize = 24f;
	}
}
