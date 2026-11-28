using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005F0 RID: 1520
	public class FloatMenu : Window
	{
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002FB5 RID: 12213 RVA: 0x00131B13 File Offset: 0x0012FD13
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06002FB6 RID: 12214 RVA: 0x00131B1A File Offset: 0x0012FD1A
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalWindowHeight);
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x00131B2D File Offset: 0x0012FD2D
		private float MaxWindowHeight
		{
			get
			{
				return (float)UI.screenHeight * 0.9f;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x00131B3B File Offset: 0x0012FD3B
		private float TotalWindowHeight
		{
			get
			{
				return Mathf.Min(this.TotalViewHeight, this.MaxWindowHeight) + 1f;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06002FB9 RID: 12217 RVA: 0x00131B54 File Offset: 0x0012FD54
		private float MaxViewHeight
		{
			get
			{
				if (this.UsingScrollbar)
				{
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < this.options.Count; i++)
					{
						float requiredHeight = this.options[i].RequiredHeight;
						if (requiredHeight > num)
						{
							num = requiredHeight;
						}
						num2 += requiredHeight + -1f;
					}
					int columnCount = this.ColumnCount;
					num2 += (float)columnCount * num;
					return num2 / (float)columnCount;
				}
				return this.MaxWindowHeight;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x00131BCC File Offset: 0x0012FDCC
		private float TotalViewHeight
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				float maxViewHeight = this.MaxViewHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxViewHeight)
					{
						if (num2 > num)
						{
							num = num2;
						}
						num2 = requiredHeight;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return Mathf.Max(num, num2);
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06002FBB RID: 12219 RVA: 0x00131C40 File Offset: 0x0012FE40
		private float TotalWidth
		{
			get
			{
				float num = (float)this.ColumnCount * this.ColumnWidth;
				if (this.UsingScrollbar)
				{
					num += 16f;
				}
				return num;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06002FBC RID: 12220 RVA: 0x00131C70 File Offset: 0x0012FE70
		private float ColumnWidth
		{
			get
			{
				float num = 70f;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredWidth = this.options[i].RequiredWidth;
					if (requiredWidth >= 300f)
					{
						return 300f;
					}
					if (requiredWidth > num)
					{
						num = requiredWidth;
					}
				}
				return Mathf.Round(num);
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06002FBD RID: 12221 RVA: 0x00131CC5 File Offset: 0x0012FEC5
		private int MaxColumns
		{
			get
			{
				return Mathf.FloorToInt(((float)UI.screenWidth - 16f) / this.ColumnWidth);
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06002FBE RID: 12222 RVA: 0x00131CDF File Offset: 0x0012FEDF
		private bool UsingScrollbar
		{
			get
			{
				return this.ColumnCountIfNoScrollbar > this.MaxColumns;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06002FBF RID: 12223 RVA: 0x00131CEF File Offset: 0x0012FEEF
		private int ColumnCount
		{
			get
			{
				return Mathf.Min(this.ColumnCountIfNoScrollbar, this.MaxColumns);
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x00131D04 File Offset: 0x0012FF04
		private int ColumnCountIfNoScrollbar
		{
			get
			{
				if (this.options == null)
				{
					return 1;
				}
				Text.Font = GameFont.Small;
				int num = 1;
				float num2 = 0f;
				float maxWindowHeight = this.MaxWindowHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxWindowHeight)
					{
						num2 = requiredHeight;
						num++;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return num;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06002FC1 RID: 12225 RVA: 0x00131D7B File Offset: 0x0012FF7B
		public FloatMenuSizeMode SizeMode
		{
			get
			{
				if (this.options.Count > 60)
				{
					return FloatMenuSizeMode.Tiny;
				}
				return FloatMenuSizeMode.Normal;
			}
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x00131D90 File Offset: 0x0012FF90
		public FloatMenu(List<FloatMenuOption> options) : base(null)
		{
			if (options.NullOrEmpty<FloatMenuOption>())
			{
				Log.Error("Created FloatMenu with no options. Closing.");
				this.Close(true);
			}
			this.options = (from op in options
			orderby op.Priority descending, op.orderInPriority descending
			select op).ToList<FloatMenuOption>();
			for (int i = 0; i < options.Count; i++)
			{
				options[i].SetSizeMode(this.SizeMode);
			}
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x00131E74 File Offset: 0x00130074
		public FloatMenu(List<FloatMenuOption> options, string title, bool needSelection = false) : this(options)
		{
			this.title = title;
			this.needSelection = needSelection;
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x00131E8C File Offset: 0x0013008C
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + FloatMenu.InitialPositionShift;
			if (vector.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				vector.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (vector.y + this.InitialSize.y > (float)UI.screenHeight)
			{
				vector.y = (float)UI.screenHeight - this.InitialSize.y;
			}
			this.windowRect = new Rect(vector.x, vector.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x00131F3C File Offset: 0x0013013C
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (!this.title.NullOrEmpty())
			{
				Vector2 vector = new Vector2(this.windowRect.x, this.windowRect.y);
				Text.Font = GameFont.Small;
				float width = Mathf.Max(150f, 15f + Text.CalcSize(this.title).x);
				Rect titleRect = new Rect(vector.x + FloatMenu.TitleOffset.x, vector.y + FloatMenu.TitleOffset.y, width, 23f);
				Find.WindowStack.ImmediateWindow(6830963, titleRect, WindowLayer.Super, delegate
				{
					GUI.color = this.baseColor;
					Text.Font = GameFont.Small;
					Rect position = titleRect.AtZero();
					position.width = 150f;
					GUI.DrawTexture(position, TexUI.TextBGBlack);
					Rect rect = titleRect.AtZero();
					rect.x += 15f;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, this.title);
					Text.Anchor = TextAnchor.UpperLeft;
				}, false, false, 0f, null, false);
			}
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x00132010 File Offset: 0x00130210
		public override void DoWindowContents(Rect rect)
		{
			if (this.needSelection && Find.Selector.SingleSelectedThing == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			this.UpdateBaseColor();
			bool usingScrollbar = this.UsingScrollbar;
			GUI.color = this.baseColor;
			Text.Font = GameFont.Small;
			Vector2 zero = Vector2.zero;
			float maxViewHeight = this.MaxViewHeight;
			float columnWidth = this.ColumnWidth;
			if (usingScrollbar)
			{
				rect.width -= 10f;
				Widgets.BeginScrollView(rect, ref this.scrollPosition, new Rect(0f, 0f, this.TotalWidth - 16f, this.TotalViewHeight), true);
			}
			for (int i = 0; i < this.options.Count; i++)
			{
				FloatMenuOption floatMenuOption = this.options[i];
				float requiredHeight = floatMenuOption.RequiredHeight;
				if (zero.y + requiredHeight + -1f > maxViewHeight)
				{
					zero.y = 0f;
					zero.x += columnWidth + -1f;
				}
				Rect rect2 = new Rect(zero.x, zero.y, columnWidth, requiredHeight);
				zero.y += requiredHeight + -1f;
				if (floatMenuOption.DoGUI(rect2, this.givesColonistOrders, this))
				{
					Find.WindowStack.TryRemove(this, true);
					break;
				}
			}
			if (usingScrollbar)
			{
				Widgets.EndScrollView();
			}
			if (Event.current.type == EventType.mouseDown)
			{
				Event.current.Use();
				this.Close(true);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x00132191 File Offset: 0x00130391
		public override void PostClose()
		{
			base.PostClose();
			if (this.onCloseCallback != null)
			{
				this.onCloseCallback();
			}
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x001321AC File Offset: 0x001303AC
		public void Cancel()
		{
			SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
			Find.WindowStack.TryRemove(this, true);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x001321C6 File Offset: 0x001303C6
		public virtual void PreOptionChosen(FloatMenuOption opt)
		{
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x001321C8 File Offset: 0x001303C8
		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			if (this.vanishIfMouseDistant)
			{
				Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalWindowHeight).ContractedBy(-5f);
				if (!r.Contains(Event.current.mousePosition))
				{
					float num = GenUI.DistFromRect(r, Event.current.mousePosition);
					this.baseColor = new Color(1f, 1f, 1f, 1f - num / 95f);
					if (num > 95f)
					{
						this.Close(false);
						this.Cancel();
						return;
					}
				}
			}
		}

		// Token: 0x04001E5B RID: 7771
		public bool givesColonistOrders;

		// Token: 0x04001E5C RID: 7772
		public bool vanishIfMouseDistant = true;

		// Token: 0x04001E5D RID: 7773
		public Action onCloseCallback;

		// Token: 0x04001E5E RID: 7774
		protected List<FloatMenuOption> options;

		// Token: 0x04001E5F RID: 7775
		private string title;

		// Token: 0x04001E60 RID: 7776
		private bool needSelection;

		// Token: 0x04001E61 RID: 7777
		private Color baseColor = Color.white;

		// Token: 0x04001E62 RID: 7778
		private Vector2 scrollPosition;

		// Token: 0x04001E63 RID: 7779
		private static readonly Vector2 TitleOffset = new Vector2(30f, -25f);

		// Token: 0x04001E64 RID: 7780
		private const float OptionSpacing = -1f;

		// Token: 0x04001E65 RID: 7781
		private const float MaxScreenHeightPercent = 0.9f;

		// Token: 0x04001E66 RID: 7782
		private const float MinimumColumnWidth = 70f;

		// Token: 0x04001E67 RID: 7783
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		// Token: 0x04001E68 RID: 7784
		public const float FadeStartMouseDist = 5f;

		// Token: 0x04001E69 RID: 7785
		private const float FadeFinishMouseDist = 100f;

		// Token: 0x04001E6A RID: 7786
		public const float FinishDistFromStartDist = 95f;
	}
}
