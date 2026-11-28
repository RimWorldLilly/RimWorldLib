using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000698 RID: 1688
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x0600352D RID: 13613 RVA: 0x0015A489 File Offset: 0x00158689
		public CreditRecord_Text()
		{
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x0015A491 File Offset: 0x00158691
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x0015A4A7 File Offset: 0x001586A7
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x0015A4B5 File Offset: 0x001586B5
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Text.Font = GameFont.Medium;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x040022D7 RID: 8919
		public string text;

		// Token: 0x040022D8 RID: 8920
		public TextAnchor anchor;
	}
}
