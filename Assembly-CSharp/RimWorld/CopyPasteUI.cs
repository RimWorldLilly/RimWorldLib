using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C2E RID: 7214
	public static class CopyPasteUI
	{
		// Token: 0x0600B80B RID: 47115 RVA: 0x0040F77C File Offset: 0x0040D97C
		public static void DoCopyPasteButtons(Rect rect, Action copyAction, Action pasteAction)
		{
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height / 2f - 12f), 18f, 24f);
			if (Widgets.ButtonImage(rect2, TexButton.Copy, true, null))
			{
				copyAction();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect2, "Copy");
			if (pasteAction != null)
			{
				Rect rect3 = rect2;
				rect3.x = rect2.xMax;
				if (Widgets.ButtonImage(rect3, TexButton.Paste, true, null))
				{
					pasteAction();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect3, "Paste");
			}
		}

		// Token: 0x0400699B RID: 27035
		public const float CopyPasteIconHeight = 24f;

		// Token: 0x0400699C RID: 27036
		public const float CopyPasteIconWidth = 18f;

		// Token: 0x0400699D RID: 27037
		public const float CopyPasteColumnWidth = 36f;
	}
}
