using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064F RID: 1615
	public static class OptionListingUtility
	{
		// Token: 0x060032AF RID: 12975 RVA: 0x001433E0 File Offset: 0x001415E0
		public static float DrawOptionListing(Rect rect, List<ListableOption> optList)
		{
			float num = 0f;
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			foreach (ListableOption listableOption in optList)
			{
				num += listableOption.DrawOption(new Vector2(0f, num), rect.width) + 7f;
			}
			Widgets.EndGroup();
			return num;
		}

		// Token: 0x04002064 RID: 8292
		private const float OptionSpacing = 7f;
	}
}
