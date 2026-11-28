using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200066B RID: 1643
	public class Dialog_Confirm : Window
	{
		// Token: 0x06003400 RID: 13312 RVA: 0x0014EB77 File Offset: 0x0014CD77
		public Dialog_Confirm(string title, Action onConfirm) : this(title, "Confirm".Translate(), onConfirm)
		{
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x0014EB90 File Offset: 0x0014CD90
		public Dialog_Confirm(string title, string confirm, Action onConfirm) : base(null)
		{
			this.title = title;
			this.confirm = confirm;
			this.onConfirm = onConfirm;
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003402 RID: 13314 RVA: 0x0014EBCA File Offset: 0x0014CDCA
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 150f);
			}
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x0014EBDC File Offset: 0x0014CDDC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.keyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				flag = true;
				Event.current.Use();
			}
			Rect rect = inRect;
			rect.width = inRect.width / 2f - 5f;
			rect.yMin = inRect.yMax - Dialog_Confirm.ButtonSize.y - 10f;
			Rect rect2 = inRect;
			rect2.xMin = rect.xMax + 10f;
			rect2.yMin = inRect.yMax - Dialog_Confirm.ButtonSize.y - 10f;
			Rect rect3 = inRect;
			rect3.y += 4f;
			rect3.yMax = rect2.y - 10f;
			using (new TextBlock(TextAnchor.UpperCenter))
			{
				Widgets.Label(rect3, this.title);
			}
			if (Widgets.ButtonText(rect, "Cancel".Translate(), true, true, true, null))
			{
				Find.WindowStack.TryRemove(this, true);
			}
			if (Widgets.ButtonText(rect2, this.confirm, true, true, true, null) || flag)
			{
				Action action = this.onConfirm;
				if (action != null)
				{
					action();
				}
				Find.WindowStack.TryRemove(this, true);
			}
		}

		// Token: 0x040021E3 RID: 8675
		private string title;

		// Token: 0x040021E4 RID: 8676
		private string confirm;

		// Token: 0x040021E5 RID: 8677
		private Action onConfirm;

		// Token: 0x040021E6 RID: 8678
		private static readonly Vector2 ButtonSize = new Vector2(120f, 32f);
	}
}
