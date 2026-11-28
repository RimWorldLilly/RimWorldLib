using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000623 RID: 1571
	public abstract class Listing
	{
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x0600312B RID: 12587 RVA: 0x0013A86A File Offset: 0x00138A6A
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x0013A872 File Offset: 0x00138A72
		public float MaxColumnHeightSeen
		{
			get
			{
				return Math.Max(this.CurHeight, this.maxHeightColumnSeen);
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x0013A895 File Offset: 0x00138A95
		// (set) Token: 0x0600312D RID: 12589 RVA: 0x0013A885 File Offset: 0x00138A85
		public float ColumnWidth
		{
			get
			{
				return this.columnWidthInt;
			}
			set
			{
				this.columnWidthInt = value;
				this.hasCustomColumnWidth = true;
			}
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x0013A89D File Offset: 0x00138A9D
		public void NewColumn()
		{
			this.maxHeightColumnSeen = Math.Max(this.curY, this.maxHeightColumnSeen);
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x0013A8DA File Offset: 0x00138ADA
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.maxOneColumn)
			{
				return;
			}
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x0013A900 File Offset: 0x00138B00
		public Rect GetRect(float height, float widthPct = 1f)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth * widthPct, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x0013A931 File Offset: 0x00138B31
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x0013A944 File Offset: 0x00138B44
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x0013A9AE File Offset: 0x00138BAE
		public void Indent(float gapWidth = 12f)
		{
			this.curX += gapWidth;
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x0013A9BE File Offset: 0x00138BBE
		public void Outdent(float gapWidth = 12f)
		{
			this.curX -= gapWidth;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x0013A9D0 File Offset: 0x00138BD0
		public virtual void Begin(Rect rect)
		{
			this.listingRect = rect;
			if (this.hasCustomColumnWidth)
			{
				if (this.columnWidthInt > this.listingRect.width)
				{
					Log.Error(string.Concat(new string[]
					{
						"Listing set ColumnWith to ",
						this.columnWidthInt.ToString(),
						" which is more than the whole listing rect width of ",
						this.listingRect.width.ToString(),
						". Clamping."
					}));
					this.columnWidthInt = this.listingRect.width;
				}
			}
			else
			{
				this.columnWidthInt = this.listingRect.width;
			}
			this.curX = 0f;
			this.curY = 0f;
			this.maxHeightColumnSeen = 0f;
			Widgets.BeginGroup(rect);
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x0013AA96 File Offset: 0x00138C96
		public virtual void End()
		{
			Widgets.EndGroup();
		}

		// Token: 0x04001F8B RID: 8075
		public float verticalSpacing = 2f;

		// Token: 0x04001F8C RID: 8076
		protected Rect listingRect;

		// Token: 0x04001F8D RID: 8077
		protected float curY;

		// Token: 0x04001F8E RID: 8078
		protected float curX;

		// Token: 0x04001F8F RID: 8079
		private float columnWidthInt;

		// Token: 0x04001F90 RID: 8080
		private bool hasCustomColumnWidth;

		// Token: 0x04001F91 RID: 8081
		private float maxHeightColumnSeen;

		// Token: 0x04001F92 RID: 8082
		public bool maxOneColumn;

		// Token: 0x04001F93 RID: 8083
		public const float ColumnSpacing = 17f;

		// Token: 0x04001F94 RID: 8084
		public const float DefaultGap = 12f;

		// Token: 0x04001F95 RID: 8085
		public const float DefaultIndent = 12f;
	}
}
