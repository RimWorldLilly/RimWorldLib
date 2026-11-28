using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DDC RID: 7644
	public abstract class PawnColumnWorker_Text : PawnColumnWorker
	{
		// Token: 0x170024BF RID: 9407
		// (get) Token: 0x0600C6B8 RID: 50872 RVA: 0x0048C905 File Offset: 0x0048AB05
		protected virtual int Width
		{
			get
			{
				return this.def.width;
			}
		}

		// Token: 0x170024C0 RID: 9408
		// (get) Token: 0x0600C6B9 RID: 50873 RVA: 0x0048C912 File Offset: 0x0048AB12
		protected virtual TextAnchor Anchor
		{
			get
			{
				return TextAnchor.MiddleLeft;
			}
		}

		// Token: 0x0600C6BA RID: 50874 RVA: 0x0048C915 File Offset: 0x0048AB15
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x0600C6BB RID: 50875 RVA: 0x0048C928 File Offset: 0x0048AB28
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, Mathf.Min(rect.height, 30f));
			string textFor = this.GetTextFor(pawn);
			if (textFor != null)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = this.Anchor;
				Text.WordWrap = false;
				Widgets.Label(rect2, textFor);
				Text.WordWrap = true;
				Text.Anchor = TextAnchor.UpperLeft;
				if (Mouse.IsOver(rect2))
				{
					string tip = this.GetTip(pawn);
					if (!tip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, tip);
					}
				}
			}
		}

		// Token: 0x0600C6BC RID: 50876 RVA: 0x0048C9BB File Offset: 0x0048ABBB
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x0600C6BD RID: 50877 RVA: 0x0048C9CF File Offset: 0x0048ABCF
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnColumnWorker_Text.comparer.Compare(this.GetTextFor(a), this.GetTextFor(b));
		}

		// Token: 0x0600C6BE RID: 50878
		protected abstract string GetTextFor(Pawn pawn);

		// Token: 0x0600C6BF RID: 50879 RVA: 0x0048C9E9 File Offset: 0x0048ABE9
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x04007104 RID: 28932
		private static NumericStringComparer comparer = new NumericStringComparer();
	}
}
