using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005FD RID: 1533
	public abstract class FeedbackItem
	{
		// Token: 0x06003024 RID: 12324 RVA: 0x00135138 File Offset: 0x00133338
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x00135199 File Offset: 0x00133399
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x06003026 RID: 12326
		public abstract void FeedbackOnGUI();

		// Token: 0x06003027 RID: 12327 RVA: 0x001351D0 File Offset: 0x001333D0
		protected void DrawFloatingText(string str, Color TextColor)
		{
			float x = Text.CalcSize(str).x;
			Rect wordRect = new Rect(this.CurScreenPos.x - x / 2f, this.CurScreenPos.y, x, 20f);
			Find.WindowStack.ImmediateWindow(5983 * this.uniqueID + 495, wordRect, WindowLayer.Super, delegate
			{
				Rect rect = wordRect.AtZero();
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Small;
				GUI.DrawTexture(rect, TexUI.GrayTextBG);
				GUI.color = TextColor;
				Widgets.Label(rect, str);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}, false, false, 1f, null, false);
		}

		// Token: 0x04001ED0 RID: 7888
		protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

		// Token: 0x04001ED1 RID: 7889
		private int uniqueID;

		// Token: 0x04001ED2 RID: 7890
		public float TimeLeft = 2f;

		// Token: 0x04001ED3 RID: 7891
		protected Vector2 CurScreenPos;

		// Token: 0x04001ED4 RID: 7892
		private static int freeUniqueID;
	}
}
