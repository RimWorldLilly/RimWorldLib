using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000642 RID: 1602
	[StaticConstructorOnStartup]
	public static class UIHighlighter
	{
		// Token: 0x06003237 RID: 12855 RVA: 0x001401A8 File Offset: 0x0013E3A8
		public static void HighlightTag(string tag)
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			if (tag.NullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				if (UIHighlighter.liveTags[i].First == tag && UIHighlighter.liveTags[i].Second == Time.frameCount)
				{
					return;
				}
			}
			UIHighlighter.liveTags.Add(new Pair<string, int>(tag, Time.frameCount));
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x0014022C File Offset: 0x0013E42C
		public static void HighlightOpportunity(Rect rect, string tag)
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				Pair<string, int> pair = UIHighlighter.liveTags[i];
				if (tag == pair.First && Time.frameCount == pair.Second + 1)
				{
					Widgets.DrawTextHighlight(rect, 0f, new Color?(Color.white.ToTransparent(Pulser.PulseBrightness(1.2f, 0.7f))));
				}
			}
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x001402B0 File Offset: 0x0013E4B0
		public static void UIHighlighterUpdate()
		{
			UIHighlighter.liveTags.RemoveAll((Pair<string, int> pair) => Time.frameCount > pair.Second + 1);
		}

		// Token: 0x0400201C RID: 8220
		private static List<Pair<string, int>> liveTags = new List<Pair<string, int>>();

		// Token: 0x0400201D RID: 8221
		private const float PulseFrequency = 1.2f;

		// Token: 0x0400201E RID: 8222
		private const float PulseAmplitude = 0.7f;
	}
}
