using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000635 RID: 1589
	public struct RectDivider
	{
		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060031CA RID: 12746 RVA: 0x0013E493 File Offset: 0x0013C693
		private int ErrorHash
		{
			get
			{
				return this.contextHash ^ 1006562692;
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060031CB RID: 12747 RVA: 0x0013E4A1 File Offset: 0x0013C6A1
		public Rect Rect
		{
			get
			{
				return this.currentRect;
			}
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x0013E4AC File Offset: 0x0013C6AC
		public RectDivider(Rect parent, int contextHash, Vector2? margin = null)
		{
			this.currentRect = parent;
			this.margin = (margin ?? new Vector2(17f, 4f));
			this.contextHash = contextHash;
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x0013E4F0 File Offset: 0x0013C6F0
		public static implicit operator Rect(RectDivider rect)
		{
			return rect.currentRect;
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x0013E4F8 File Offset: 0x0013C6F8
		public RectDivider NewRow(float height, VerticalJustification justification = VerticalJustification.Top, float? marginOverride = null)
		{
			if (justification == VerticalJustification.Top)
			{
				Rect rect = this.currentRect;
				float? num = marginOverride;
				float compressibleMargin = num ?? this.margin.y;
				float? topHeight = new float?(height);
				num = null;
				Rect parent;
				Rect rect2;
				float value;
				if (!rect.SplitHorizontallyWithMargin(out parent, out rect2, out value, compressibleMargin, topHeight, num))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Rect height was too small by ");
					defaultInterpolatedStringHandler.AppendFormatted<float>(value);
					defaultInterpolatedStringHandler.AppendLiteral(" for the requested row height.");
					Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), this.ErrorHash);
				}
				this.currentRect = rect2;
				return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
			}
			if (justification == VerticalJustification.Bottom)
			{
				Rect rect3 = this.currentRect;
				float compressibleMargin2 = marginOverride ?? this.margin.y;
				float? num = new float?(height);
				Rect rect4;
				Rect parent2;
				float value2;
				if (!rect3.SplitHorizontallyWithMargin(out rect4, out parent2, out value2, compressibleMargin2, null, num))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(60, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("Rect height was too small by ");
					defaultInterpolatedStringHandler2.AppendFormatted<float>(value2);
					defaultInterpolatedStringHandler2.AppendLiteral(" for the requested rows height.");
					Log.ErrorOnce(defaultInterpolatedStringHandler2.ToStringAndClear(), this.ErrorHash);
				}
				this.currentRect = rect4;
				return new RectDivider(parent2, this.contextHash, new Vector2?(this.margin));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x0013E658 File Offset: 0x0013C858
		public RectDivider NewCol(float width, HorizontalJustification justification = HorizontalJustification.Left, float? marginOverride = null)
		{
			if (justification == HorizontalJustification.Left)
			{
				Rect rect = this.currentRect;
				float? num = marginOverride;
				float compressibleMargin = num ?? this.margin.x;
				float? leftWidth = new float?(width);
				num = null;
				Rect parent;
				Rect rect2;
				float value;
				if (!rect.SplitVerticallyWithMargin(out parent, out rect2, out value, compressibleMargin, leftWidth, num))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Rect width was too small by ");
					defaultInterpolatedStringHandler.AppendFormatted<float>(value);
					defaultInterpolatedStringHandler.AppendLiteral(" for the requested column width.");
					Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), this.ErrorHash);
				}
				this.currentRect = rect2;
				return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
			}
			if (justification == HorizontalJustification.Right)
			{
				Rect rect3 = this.currentRect;
				float compressibleMargin2 = marginOverride ?? this.margin.x;
				float? num = new float?(width);
				Rect rect4;
				Rect parent2;
				float value2;
				if (!rect3.SplitVerticallyWithMargin(out rect4, out parent2, out value2, compressibleMargin2, null, num))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(60, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("Rect width was too small by ");
					defaultInterpolatedStringHandler2.AppendFormatted<float>(value2);
					defaultInterpolatedStringHandler2.AppendLiteral(" for the requested column width.");
					Log.ErrorOnce(defaultInterpolatedStringHandler2.ToStringAndClear(), this.ErrorHash);
				}
				this.currentRect = rect4;
				return new RectDivider(parent2, this.contextHash, new Vector2?(this.margin));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x0013E7B8 File Offset: 0x0013C9B8
		public RectDivider CreateViewRect(int count, float rowHeight)
		{
			float num = 0f;
			if (count > 0)
			{
				num = (float)count * rowHeight + this.margin.y * (float)(count - 1);
			}
			float num2 = this.Rect.width;
			if (num > this.Rect.height)
			{
				num2 -= 16f;
			}
			return new RectDivider(new Rect(0f, 0f, num2, num), this.contextHash, null);
		}

		// Token: 0x04001FE9 RID: 8169
		private Rect currentRect;

		// Token: 0x04001FEA RID: 8170
		private Vector2 margin;

		// Token: 0x04001FEB RID: 8171
		private int contextHash;
	}
}
