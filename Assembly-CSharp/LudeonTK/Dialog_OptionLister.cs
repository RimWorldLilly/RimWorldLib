using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200004C RID: 76
	public abstract class Dialog_OptionLister : Window_Dev
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000ECD1 File Offset: 0x0000CED1
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000ECE4 File Offset: 0x0000CEE4
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000ECE8 File Offset: 0x0000CEE8
		public Rect? BoundingRectCached
		{
			get
			{
				if (this.boundingRectCachedForFrame != Time.frameCount)
				{
					if (this.boundingRect != null)
					{
						Rect value = this.boundingRect.Value;
						Vector2 vector = this.scrollPosition;
						value.x += vector.x;
						value.y += vector.y;
						this.boundingRectCached = new Rect?(value);
					}
					this.boundingRectCachedForFrame = Time.frameCount;
				}
				return this.boundingRectCached;
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000ED68 File Offset: 0x0000CF68
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.focusOnFilterOnOpen)
			{
				this.focusFilter = true;
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000EDCC File Offset: 0x0000CFCC
		public override void DoWindowContents(Rect inRect)
		{
			GUI.SetNextControlName("DebugFilter");
			if (Event.current.type == EventType.keyDown && (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent || KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent))
			{
				return;
			}
			this.filter = DevGUI.TextField(new Rect(0f, 0f, 200f, 30f), this.filter);
			if ((Event.current.type == EventType.keyDown || Event.current.type == EventType.repaint) && this.focusFilter)
			{
				GUI.FocusControl("DebugFilter");
				this.filter = "";
				this.focusFilter = false;
			}
			if (Event.current.type == EventType.layout)
			{
				this.totalOptionsHeight = 0f;
			}
			Rect outRect = new Rect(inRect);
			outRect.yMin += 35f;
			int num = (int)(this.InitialSize.x / 200f);
			float num2 = (this.totalOptionsHeight + 24f * (float)(num - 1)) / (float)num;
			if (num2 < outRect.height)
			{
				num2 = outRect.height;
			}
			this.curX = 0f;
			this.curY = 0f;
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, num2);
			DevGUI.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			DevGUI.BeginGroup(inRect);
			float columnWidth = (viewRect.width - 17f * (float)(num - 1)) / (float)num;
			this.DoListingItems(inRect.AtZero(), columnWidth);
			DevGUI.EndGroup();
			DevGUI.EndScrollView();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000EF56 File Offset: 0x0000D156
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		// Token: 0x06000205 RID: 517
		protected abstract void DoListingItems(Rect inRect, float columnWidth);

		// Token: 0x06000206 RID: 518 RVA: 0x0000EF63 File Offset: 0x0000D163
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04000109 RID: 265
		protected Vector2 scrollPosition;

		// Token: 0x0400010A RID: 266
		protected string filter = "";

		// Token: 0x0400010B RID: 267
		protected float totalOptionsHeight;

		// Token: 0x0400010C RID: 268
		protected bool focusOnFilterOnOpen = true;

		// Token: 0x0400010D RID: 269
		private bool focusFilter;

		// Token: 0x0400010E RID: 270
		protected float curY;

		// Token: 0x0400010F RID: 271
		protected float curX;

		// Token: 0x04000110 RID: 272
		private int boundingRectCachedForFrame = -1;

		// Token: 0x04000111 RID: 273
		private Rect? boundingRectCached;

		// Token: 0x04000112 RID: 274
		private Rect? boundingRect;

		// Token: 0x04000113 RID: 275
		public float verticalSpacing = 2f;

		// Token: 0x04000114 RID: 276
		protected const string FilterControlName = "DebugFilter";
	}
}
