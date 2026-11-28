using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000664 RID: 1636
	public class Dialog_NodeTree : Window
	{
		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060033E4 RID: 13284 RVA: 0x0014E09C File Offset: 0x0014C29C
		public override Vector2 InitialSize
		{
			get
			{
				int num = 480;
				if (this.curNode.options.Count > 5)
				{
					Text.Font = GameFont.Small;
					num += (this.curNode.options.Count - 5) * (int)(Text.LineHeight + 7f);
				}
				return new Vector2(620f, (float)Mathf.Min(num, UI.screenHeight));
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060033E5 RID: 13285 RVA: 0x0014E100 File Offset: 0x0014C300
		private bool InteractiveNow
		{
			get
			{
				return Time.realtimeSinceStartup >= this.makeInteractiveAtTime;
			}
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x0014E114 File Offset: 0x0014C314
		public Dialog_NodeTree(DiaNode nodeRoot, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(null)
		{
			this.title = title;
			this.GotoNode(nodeRoot);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			if (delayInteractivity)
			{
				this.makeInteractiveAtTime = RealTime.LastRealTime + 1f;
			}
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (radioMode)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x0014E196 File Offset: 0x0014C396
		public override void PreClose()
		{
			base.PreClose();
			this.curNode.PreClose();
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0014E1A9 File Offset: 0x0014C3A9
		public override void PostClose()
		{
			base.PostClose();
			if (this.closeAction != null)
			{
				this.closeAction();
			}
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x0014E1C4 File Offset: 0x0014C3C4
		public override void WindowOnGUI()
		{
			if (this.screenFillColor != Color.clear)
			{
				GUI.color = this.screenFillColor;
				GUI.DrawTexture(new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), BaseContent.WhiteTex);
				GUI.color = Color.white;
			}
			base.WindowOnGUI();
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x0014E224 File Offset: 0x0014C424
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect.AtZero();
			if (this.title != null)
			{
				Text.Font = GameFont.Small;
				Rect rect2 = rect;
				rect2.height = 36f;
				rect.yMin += 53f;
				Widgets.DrawTitleBG(rect2);
				rect2.xMin += 9f;
				rect2.yMin += 5f;
				Widgets.Label(rect2, this.title);
			}
			this.DrawNode(rect);
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x0014E2A8 File Offset: 0x0014C4A8
		protected void DrawNode(Rect rect)
		{
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			float num = Mathf.Min(this.optTotalHeight, rect.height - 100f - this.Margin * 2f);
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - Mathf.Max(num, this.minOptionsAreaHeight));
			float width = rect.width - 16f;
			Rect rect2 = new Rect(0f, 0f, width, Text.CalcHeight(this.curNode.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Widgets.Label(rect2, this.curNode.text.Resolve());
			Widgets.EndScrollView();
			Widgets.BeginScrollView(new Rect(0f, rect.height - num, rect.width, num), ref this.optsScrollPosition, new Rect(0f, 0f, rect.width - 16f, this.optTotalHeight), true);
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this.curNode.options.Count; i++)
			{
				Rect rect3 = new Rect(15f, num2, rect.width - 30f, 999f);
				float num4 = this.curNode.options[i].OptOnGUI(rect3, this.InteractiveNow);
				num2 += num4 + 7f;
				num3 += num4 + 7f;
			}
			if (Event.current.type == EventType.layout)
			{
				this.optTotalHeight = num3;
			}
			Widgets.EndScrollView();
			Widgets.EndGroup();
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x0014E45C File Offset: 0x0014C65C
		public void GotoNode(DiaNode node)
		{
			foreach (DiaOption diaOption in node.options)
			{
				diaOption.dialog = this;
			}
			this.curNode = node;
		}

		// Token: 0x040021B4 RID: 8628
		private Vector2 scrollPosition;

		// Token: 0x040021B5 RID: 8629
		private Vector2 optsScrollPosition;

		// Token: 0x040021B6 RID: 8630
		protected string title;

		// Token: 0x040021B7 RID: 8631
		protected DiaNode curNode;

		// Token: 0x040021B8 RID: 8632
		public Action closeAction;

		// Token: 0x040021B9 RID: 8633
		private float makeInteractiveAtTime;

		// Token: 0x040021BA RID: 8634
		public Color screenFillColor = Color.clear;

		// Token: 0x040021BB RID: 8635
		protected float minOptionsAreaHeight;

		// Token: 0x040021BC RID: 8636
		private const float InteractivityDelay = 1f;

		// Token: 0x040021BD RID: 8637
		private const float TitleHeight = 36f;

		// Token: 0x040021BE RID: 8638
		protected const float OptHorMargin = 15f;

		// Token: 0x040021BF RID: 8639
		protected const float OptVerticalSpace = 7f;

		// Token: 0x040021C0 RID: 8640
		private const int ResizeIfMoreOptionsThan = 5;

		// Token: 0x040021C1 RID: 8641
		private const float MinSpaceLeftForTextAfterOptionsResizing = 100f;

		// Token: 0x040021C2 RID: 8642
		private float optTotalHeight;
	}
}
