using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000041 RID: 65
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000C93E File Offset: 0x0000AB3E
		protected virtual int HighlightedIndex
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000C941 File Offset: 0x0000AB41
		public Dialog_DebugOptionLister()
		{
			this.forcePause = true;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000C950 File Offset: 0x0000AB50
		public void NewColumn(float columnWidth)
		{
			this.curY = 0f;
			this.curX += columnWidth + 17f;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000C971 File Offset: 0x0000AB71
		protected void NewColumnIfNeeded(float columnWidth, float neededHeight)
		{
			if (this.curY + neededHeight > this.windowRect.height)
			{
				this.NewColumn(columnWidth);
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000C990 File Offset: 0x0000AB90
		public bool ButtonDebug(string label, float columnWidth, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			this.NewColumnIfNeeded(columnWidth, 22f);
			Rect rect = new Rect(this.curX, this.curY, columnWidth, 22f);
			bool result = false;
			if (base.BoundingRectCached == null || rect.Overlaps(base.BoundingRectCached.Value))
			{
				bool wordWrap = Text.WordWrap;
				Text.WordWrap = false;
				result = DevGUI.ButtonText(rect, "  " + label, new TextAnchor?(TextAnchor.MiddleLeft));
				Text.WordWrap = wordWrap;
				if (highlight)
				{
					GUI.color = Color.yellow;
					DevGUI.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
			}
			this.curY += 22f + this.verticalSpacing;
			return result;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000CA54 File Offset: 0x0000AC54
		protected void DebugLabel(string label, float columnWidth)
		{
			Text.Font = GameFont.Small;
			float num = Text.CalcHeight(label, columnWidth);
			this.NewColumnIfNeeded(columnWidth, num);
			DevGUI.Label(new Rect(this.curX, this.curY, columnWidth, num), label);
			this.curY += num + this.verticalSpacing;
			if (Event.current.type == EventType.layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000CAC4 File Offset: 0x0000ACC4
		protected bool DebugAction(string label, float columnWidth, Action action, bool highlight)
		{
			bool result = false;
			if (!base.FilterAllows(label))
			{
				return false;
			}
			if (this.ButtonDebug(label, columnWidth, highlight))
			{
				this.Close(true);
				action();
				result = true;
			}
			if (Event.current.type == EventType.layout)
			{
				this.totalOptionsHeight += 24f;
			}
			return result;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		protected void DebugToolMap(string label, float columnWidth, Action toolAction, bool highlight)
		{
			if (WorldRendererUtility.WorldSelected)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.ButtonDebug(label, columnWidth, highlight))
			{
				this.Close(true);
				DebugTools.curTool = new DebugTool(label, toolAction, null);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000CBA1 File Offset: 0x0000ADA1
		protected virtual void ChangeHighlightedOption()
		{
		}

		// Token: 0x040000D1 RID: 209
		protected int currentHighlightIndex;

		// Token: 0x040000D2 RID: 210
		protected int prioritizedHighlightedIndex;

		// Token: 0x040000D3 RID: 211
		private const float DebugOptionsGap = 7f;
	}
}
