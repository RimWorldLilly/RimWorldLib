using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000628 RID: 1576
	public class Listing_Tree : Listing_Lines
	{
		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06003166 RID: 12646 RVA: 0x0013BF9E File Offset: 0x0013A19E
		protected virtual float LabelWidth
		{
			get
			{
				return base.ColumnWidth - 26f;
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06003167 RID: 12647 RVA: 0x0013BFAC File Offset: 0x0013A1AC
		protected float EditAreaWidth
		{
			get
			{
				return base.ColumnWidth - this.LabelWidth;
			}
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x0013BFBB File Offset: 0x0013A1BB
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x0013BFD0 File Offset: 0x0013A1D0
		public override void End()
		{
			base.End();
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x0013BFE4 File Offset: 0x0013A1E4
		protected float XAtIndentLevel(int indentLevel)
		{
			return (float)indentLevel * this.nestIndentWidth;
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x0013BFF0 File Offset: 0x0013A1F0
		protected void LabelLeft(string label, string tipText, int indentLevel, float widthOffset = 0f, Color? textColor = null, float leftOffset = 0f)
		{
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight)
			{
				xMin = this.XAtIndentLevel(indentLevel) + 18f + leftOffset
			};
			Widgets.DrawHighlightIfMouseover(rect);
			if (!tipText.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					GUI.DrawTexture(rect, TexUI.HighlightTex);
				}
				TooltipHandler.TipRegion(rect, tipText);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = (textColor ?? Color.white);
			rect.width = this.LabelWidth - rect.xMin + widthOffset;
			rect.yMax += 5f;
			rect.yMin -= 5f;
			Widgets.Label(rect, label.Truncate(rect.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x0013C0E8 File Offset: 0x0013A2E8
		protected bool OpenCloseWidget(TreeNode node, int indentLevel, int openMask)
		{
			if (!node.Openable)
			{
				return false;
			}
			float x = this.XAtIndentLevel(indentLevel);
			float y = this.curY + this.lineHeight / 2f - 9f;
			Rect butRect = new Rect(x, y, 18f, 18f);
			bool flag = this.IsOpen(node, openMask);
			Texture2D tex = flag ? TexButton.Collapse : TexButton.Reveal;
			if (Widgets.ButtonImage(butRect, tex, true, null))
			{
				if (flag)
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
				node.SetOpen(openMask, !flag);
				return true;
			}
			return false;
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x0013C17E File Offset: 0x0013A37E
		public virtual bool IsOpen(TreeNode node, int openMask)
		{
			return node.IsOpen(openMask);
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x0013C18C File Offset: 0x0013A38C
		public void InfoText(string text, int indentLevel)
		{
			Text.WordWrap = true;
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, 50f);
			rect.xMin = this.LabelWidth;
			rect.height = Text.CalcHeight(text, rect.width);
			Widgets.Label(rect, text);
			this.curY += rect.height;
			Text.WordWrap = false;
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x0013C200 File Offset: 0x0013A400
		public bool ButtonText(string label)
		{
			Text.WordWrap = true;
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool result = Widgets.ButtonText(new Rect(0f, this.curY, base.ColumnWidth, num), label, true, true, true, null);
			this.curY += num + 0f;
			Text.WordWrap = false;
			return result;
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x0013C263 File Offset: 0x0013A463
		public WidgetRow StartWidgetsRow(int indentLevel)
		{
			WidgetRow result = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
			this.curY += 24f;
			return result;
		}

		// Token: 0x04001FA6 RID: 8102
		public float nestIndentWidth = 11f;

		// Token: 0x04001FA7 RID: 8103
		protected const float OpenCloseWidgetSize = 18f;
	}
}
