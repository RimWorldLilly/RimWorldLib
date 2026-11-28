using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200209E RID: 8350
	public static class BookUIUtility
	{
		// Token: 0x0600D26E RID: 53870 RVA: 0x004D4D80 File Offset: 0x004D2F80
		public static void DrawTitle(Rect rect, Book book)
		{
			Rect position = rect;
			position.width = position.height;
			GUI.DrawTexture(position, book.def.uiIcon);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.Font = GameFont.Medium;
			Rect rect2 = rect;
			rect2.x += rect.height + 10f;
			rect2.width -= rect.height + 10f;
			Widgets.LabelFit(rect2, book.LabelCap);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600D26F RID: 53871 RVA: 0x004D4E0B File Offset: 0x004D300B
		public static void DrawBookDescPanel(Rect rect, Book book, ref Vector2 scroll)
		{
			Widgets.LabelScrollable(rect, book.FlavorUI, ref scroll, false, true, false);
		}

		// Token: 0x0600D270 RID: 53872 RVA: 0x004D4E20 File Offset: 0x004D3020
		public static void DrawBookInfoPanel(Rect rect, Book book)
		{
			float y = rect.y;
			BookUIUtility.DrawBenefits(rect, ref y, book);
			BookUIUtility.DrawDangers(rect, ref y, book);
			Rect rect2 = rect;
			rect2.y = y;
			rect2.yMax = rect.yMax;
			BookUIUtility.DrawHyperlinks(rect2, ref y, book);
		}

		// Token: 0x0600D271 RID: 53873 RVA: 0x004D4E68 File Offset: 0x004D3068
		private static void DrawDangers(Rect rect, ref float y, Book book)
		{
			if (book.MentalBreakChancePerHour > 0f)
			{
				BookUIUtility.DrawSubheader(rect, ref y, "Dangers".Translate());
				y += 10f;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler.AppendLiteral("- ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("BookMentalBreak".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PerHour".Translate(book.MentalBreakChancePerHour.ToStringPercent("0.0")));
				Widgets.Label(rect, ref y, defaultInterpolatedStringHandler.ToStringAndClear(), default(TipSignal));
				y += 10f;
			}
		}

		// Token: 0x0600D272 RID: 53874 RVA: 0x004D4F20 File Offset: 0x004D3120
		private static void DrawSubheader(Rect rect, ref float y, string title)
		{
			Rect position = new Rect
			{
				x = rect.x,
				y = y,
				xMax = rect.xMax,
				yMax = rect.yMax
			};
			GUI.BeginGroup(position);
			float num = 0f;
			Widgets.ListSeparator(ref num, position.width, title);
			y += num;
			GUI.EndGroup();
		}

		// Token: 0x0600D273 RID: 53875 RVA: 0x004D4F90 File Offset: 0x004D3190
		private static void DrawBenefits(Rect rect, ref float y, Book book)
		{
			bool flag = false;
			using (IEnumerator<BookOutcomeDoer> enumerator = book.BookComp.Doers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!string.IsNullOrEmpty(enumerator.Current.GetBenefitsString(null)))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			BookUIUtility.DrawSubheader(rect, ref y, "Benefits".Translate());
			y += 10f;
			foreach (BookOutcomeDoer bookOutcomeDoer in book.BookComp.Doers)
			{
				string benefitsString = bookOutcomeDoer.GetBenefitsString(null);
				if (!string.IsNullOrEmpty(benefitsString))
				{
					Widgets.Label(rect, ref y, benefitsString, default(TipSignal));
				}
			}
			y += 10f;
		}

		// Token: 0x0600D274 RID: 53876 RVA: 0x004D5074 File Offset: 0x004D3274
		private static void DrawHyperlinks(Rect rect, ref float y, Book book)
		{
			Color normalOptionColor = Widgets.NormalOptionColor;
			float num = 0f;
			foreach (Dialog_InfoCard.Hyperlink hyperlink in book.GetHyperlinks().Reverse<Dialog_InfoCard.Hyperlink>())
			{
				float num2 = Text.CalcHeight(hyperlink.Label, rect.width);
				Rect rect2 = rect;
				rect2.y = rect.yMax - num2 - num - 4f;
				rect2.height = num2;
				TaggedString taggedString = "ViewHyperlink".Translate(hyperlink.Label);
				Widgets.HyperlinkWithIcon(rect2, hyperlink, taggedString, 2f, 6f, new Color?(normalOptionColor), false, null);
				num += num2;
				y += num2;
			}
		}

		// Token: 0x0400854F RID: 34127
		private const float KeyValSplit = 140f;

		// Token: 0x04008550 RID: 34128
		private const float LineHeight = 25f;

		// Token: 0x04008551 RID: 34129
		private const float HyperlinkBaseGap = 4f;
	}
}
