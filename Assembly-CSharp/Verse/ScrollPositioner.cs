using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000637 RID: 1591
	public class ScrollPositioner
	{
		// Token: 0x060031D6 RID: 12758 RVA: 0x0013E8D6 File Offset: 0x0013CAD6
		public void Arm(bool armed = true)
		{
			this.armed = armed;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x0013E8DF File Offset: 0x0013CADF
		public void ClearInterestRects()
		{
			this.interestRect = null;
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x0013E8ED File Offset: 0x0013CAED
		public void RegisterInterestRect(Rect rect)
		{
			if (this.interestRect != null)
			{
				this.interestRect = new Rect?(rect.Union(this.interestRect.Value));
				return;
			}
			this.interestRect = new Rect?(rect);
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x0013E925 File Offset: 0x0013CB25
		public void ScrollHorizontally(ref Vector2 scrollPos, Vector2 outRectSize)
		{
			this.Scroll(ref scrollPos, outRectSize, true, false);
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x0013E931 File Offset: 0x0013CB31
		public void ScrollVertically(ref Vector2 scrollPos, Vector2 outRectSize)
		{
			this.Scroll(ref scrollPos, outRectSize, false, true);
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x0013E940 File Offset: 0x0013CB40
		public void Scroll(ref Vector2 scrollPos, Vector2 outRectSize, bool scrollHorizontally = true, bool scrollVertically = true)
		{
			if (Event.current.type != EventType.layout)
			{
				return;
			}
			if (!this.armed)
			{
				return;
			}
			this.armed = false;
			if (this.interestRect == null)
			{
				return;
			}
			if (scrollHorizontally)
			{
				this.ScrollInDimension(ref scrollPos.x, outRectSize.x, this.interestRect.Value.xMin, this.interestRect.Value.xMax);
			}
			if (scrollVertically)
			{
				this.ScrollInDimension(ref scrollPos.y, outRectSize.y, this.interestRect.Value.yMin, this.interestRect.Value.yMax);
			}
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x0013E9F0 File Offset: 0x0013CBF0
		private void ScrollInDimension(ref float scrollPos, float scrollViewSize, float v0, float v1)
		{
			float num = v1 - v0;
			if (num <= scrollViewSize)
			{
				scrollPos = v0 + num / 2f - scrollViewSize / 2f;
				return;
			}
			scrollPos = v0;
		}

		// Token: 0x04001FED RID: 8173
		private Rect? interestRect;

		// Token: 0x04001FEE RID: 8174
		private bool armed;
	}
}
