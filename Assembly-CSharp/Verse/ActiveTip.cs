using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200063D RID: 1597
	[StaticConstructorOnStartup]
	public class ActiveTip
	{
		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06003217 RID: 12823 RVA: 0x0013F6C4 File Offset: 0x0013D8C4
		private string FinalText
		{
			get
			{
				string text;
				if (this.signal.textGetter != null)
				{
					try
					{
						text = this.signal.textGetter();
						goto IL_3E;
					}
					catch (Exception ex)
					{
						Log.Error(ex.ToString());
						text = "Error getting tip text.";
						goto IL_3E;
					}
				}
				text = this.signal.text;
				IL_3E:
				return text.TrimEnd();
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x0013F728 File Offset: 0x0013D928
		public Rect TipRect
		{
			get
			{
				string finalText = this.FinalText;
				Vector2 vector = Text.CalcSize(finalText);
				if (vector.x > 260f)
				{
					vector.x = 260f;
					vector.y = Text.CalcHeight(finalText, vector.x);
				}
				return new Rect(0f, 0f, vector.x, vector.y).ContractedBy(-4f).RoundedCeil();
			}
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x0013F799 File Offset: 0x0013D999
		public ActiveTip(TipSignal signal)
		{
			this.signal = signal;
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x0013F7A8 File Offset: 0x0013D9A8
		public ActiveTip(ActiveTip cloneSource)
		{
			this.signal = cloneSource.signal;
			this.firstTriggerTime = cloneSource.firstTriggerTime;
			this.lastTriggerFrame = cloneSource.lastTriggerFrame;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x0013F7D4 File Offset: 0x0013D9D4
		public float DrawTooltip(Vector2 pos)
		{
			Text.Font = GameFont.Small;
			string finalText = this.FinalText;
			Rect bgRect = this.TipRect;
			bgRect.position = pos;
			if (!LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting)
			{
				Find.WindowStack.ImmediateWindow(153 * this.signal.uniqueId + 62346, bgRect, WindowLayer.Super, delegate
				{
					this.DrawInner(bgRect.AtZero(), finalText);
				}, false, false, 1f, null, false);
			}
			else
			{
				Widgets.DrawShadowAround(bgRect);
				Widgets.DrawWindowBackground(bgRect);
				this.DrawInner(bgRect, finalText);
			}
			return bgRect.height;
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x0013F891 File Offset: 0x0013DA91
		private void DrawInner(Rect bgRect, string label)
		{
			Widgets.DrawAtlas(bgRect, ActiveTip.TooltipBGAtlas);
			Text.Font = GameFont.Small;
			Widgets.Label(bgRect.ContractedBy(4f), label);
		}

		// Token: 0x04002006 RID: 8198
		public TipSignal signal;

		// Token: 0x04002007 RID: 8199
		public double firstTriggerTime;

		// Token: 0x04002008 RID: 8200
		public int lastTriggerFrame;

		// Token: 0x04002009 RID: 8201
		private const int TipMargin = 4;

		// Token: 0x0400200A RID: 8202
		private const float MaxWidth = 260f;

		// Token: 0x0400200B RID: 8203
		public static readonly Texture2D TooltipBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TooltipBG", true);
	}
}
