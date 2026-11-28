using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000679 RID: 1657
	public class Dialog_Slider : Window
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06003435 RID: 13365 RVA: 0x001508B0 File Offset: 0x0014EAB0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 130f + this.extraBottomSpace);
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06003436 RID: 13366 RVA: 0x001508C8 File Offset: 0x0014EAC8
		protected override float Margin
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x001508D0 File Offset: 0x0014EAD0
		public Dialog_Slider(Func<int, string> textGetter, int from, int to, Action<int> confirmAction, int startingValue = -2147483648, float roundTo = 1f) : base(null)
		{
			this.textGetter = textGetter;
			this.from = from;
			this.to = to;
			this.confirmAction = confirmAction;
			this.roundTo = roundTo;
			this.forcePause = true;
			this.closeOnClickedOutside = true;
			if (startingValue == -2147483648)
			{
				this.curValue = from;
				return;
			}
			this.curValue = startingValue;
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x0015093C File Offset: 0x0014EB3C
		public Dialog_Slider(string text, int from, int to, Action<int> confirmAction, int startingValue = -2147483648, float roundTo = 1f) : this((int val) => string.Format(text, val), from, to, confirmAction, startingValue, roundTo)
		{
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x00150970 File Offset: 0x0014EB70
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			string text = this.textGetter(this.curValue);
			float height = Text.CalcHeight(text, inRect.width);
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, height);
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(rect, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(inRect.x, inRect.y + rect.height + 10f, inRect.width, 30f);
			this.curValue = (int)Widgets.HorizontalSlider(rect2, (float)this.curValue, (float)this.from, (float)this.to, true, null, null, null, this.roundTo);
			GUI.color = ColoredText.SubtleGrayColor;
			Text.Font = GameFont.Tiny;
			Widgets.Label(new Rect(inRect.x, rect2.yMax - 10f, inRect.width / 2f, Text.LineHeight), this.from.ToString());
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(new Rect(inRect.x + inRect.width / 2f, rect2.yMax - 10f, inRect.width / 2f, Text.LineHeight), this.to.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			float num = (inRect.width - 10f) / 2f;
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - 30f, num, 30f), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.x + num + 10f, inRect.yMax - 30f, num, 30f), "OK".Translate(), true, true, true, null))
			{
				this.Close(true);
				this.confirmAction(this.curValue);
			}
		}

		// Token: 0x0400221B RID: 8731
		public Func<int, string> textGetter;

		// Token: 0x0400221C RID: 8732
		public int from;

		// Token: 0x0400221D RID: 8733
		public int to;

		// Token: 0x0400221E RID: 8734
		public float roundTo = 1f;

		// Token: 0x0400221F RID: 8735
		public float extraBottomSpace;

		// Token: 0x04002220 RID: 8736
		private Action<int> confirmAction;

		// Token: 0x04002221 RID: 8737
		private int curValue;

		// Token: 0x04002222 RID: 8738
		private const float BotAreaHeight = 30f;

		// Token: 0x04002223 RID: 8739
		private const float NumberYOffset = 10f;
	}
}
