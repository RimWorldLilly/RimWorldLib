using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C9A RID: 7322
	public class Dialog_ChooseColor : Window
	{
		// Token: 0x17002201 RID: 8705
		// (get) Token: 0x0600BB35 RID: 47925 RVA: 0x00423843 File Offset: 0x00421A43
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 410f);
			}
		}

		// Token: 0x0600BB36 RID: 47926 RVA: 0x00423854 File Offset: 0x00421A54
		public Dialog_ChooseColor(string header, Color selectedColor, List<Color> colors, Action<Color> onApply) : base(null)
		{
			this.header = header;
			this.selectedColor = selectedColor;
			this.onApply = onApply;
			this.colors = colors;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BB37 RID: 47927 RVA: 0x00423888 File Offset: 0x00421A88
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 35f), this.header);
			Text.Font = GameFont.Small;
			Widgets.ColorSelectorIcon(new Rect(inRect.x, inRect.y + 35f + 10f, 88f, 88f).ContractedBy(2f), null, this.selectedColor, true);
			Rect rect = inRect;
			rect.xMin += 105f;
			rect.yMin += 45f;
			rect.height -= Window.CloseButSize.y + 10f;
			float num = (float)this.colors.Count / (rect.width / 24f);
			Rect viewRect = rect;
			viewRect.width -= 16f;
			viewRect.height = num * 24f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, viewRect, true);
			float num2;
			Widgets.ColorSelector(rect, ref this.selectedColor, this.colors, out num2, null, 22, 2, null);
			Widgets.EndScrollView();
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.height - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "CloseButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.width - Window.CloseButSize.x, inRect.height - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "OK".Translate(), true, true, true, null))
			{
				Action<Color> action = this.onApply;
				if (action != null)
				{
					action(this.selectedColor);
				}
				this.Close(true);
			}
		}

		// Token: 0x04006A6B RID: 27243
		private string header;

		// Token: 0x04006A6C RID: 27244
		private Color selectedColor;

		// Token: 0x04006A6D RID: 27245
		private Action<Color> onApply;

		// Token: 0x04006A6E RID: 27246
		private List<Color> colors;

		// Token: 0x04006A6F RID: 27247
		private Vector2 scrollPosition;

		// Token: 0x04006A70 RID: 27248
		private const float HeaderHeight = 35f;

		// Token: 0x04006A71 RID: 27249
		private const int ColorSize = 22;

		// Token: 0x04006A72 RID: 27250
		private const int ColorPadding = 2;
	}
}
