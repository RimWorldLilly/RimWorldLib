using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000614 RID: 1556
	public class GridLayout
	{
		// Token: 0x060030AE RID: 12462 RVA: 0x00137EBC File Offset: 0x001360BC
		public GridLayout(Rect container, int cols = 1, int rows = 1, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.container = new Rect(container);
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			float num = container.width - outerPadding * 2f - (float)(cols - 1) * innerPadding;
			float num2 = container.height - outerPadding * 2f - (float)(rows - 1) * innerPadding;
			this.colWidth = num / (float)cols;
			this.rowHeight = num2 / (float)rows;
			this.colStride = this.colWidth + innerPadding;
			this.rowStride = this.rowHeight + innerPadding;
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x00137F58 File Offset: 0x00136158
		public GridLayout(float colWidth, float rowHeight, int cols, int rows, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.colWidth = colWidth;
			this.rowHeight = rowHeight;
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			this.colStride = colWidth + innerPadding;
			this.rowStride = rowHeight + innerPadding;
			this.container = new Rect(0f, 0f, outerPadding * 2f + colWidth * (float)cols + innerPadding * (float)cols - 1f, outerPadding * 2f + rowHeight * (float)rows + innerPadding * (float)rows - 1f);
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x00137FF0 File Offset: 0x001361F0
		public Rect GetCellRectByIndex(int index, int colspan = 1, int rowspan = 1)
		{
			int col = index % this.cols;
			int row = index / this.cols;
			return this.GetCellRect(col, row, colspan, rowspan);
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x0013801C File Offset: 0x0013621C
		public Rect GetCellRect(int col, int row, int colspan = 1, int rowspan = 1)
		{
			return new Rect(Mathf.Floor(this.container.x + this.outerPadding + (float)col * this.colStride), Mathf.Floor(this.container.y + this.outerPadding + (float)row * this.rowStride), Mathf.Ceil(this.colWidth) * (float)colspan + this.innerPadding * (float)(colspan - 1), Mathf.Ceil(this.rowHeight) * (float)rowspan + this.innerPadding * (float)(rowspan - 1));
		}

		// Token: 0x04001F58 RID: 8024
		public Rect container;

		// Token: 0x04001F59 RID: 8025
		private int cols;

		// Token: 0x04001F5A RID: 8026
		private float outerPadding;

		// Token: 0x04001F5B RID: 8027
		private float innerPadding;

		// Token: 0x04001F5C RID: 8028
		private float colStride;

		// Token: 0x04001F5D RID: 8029
		private float rowStride;

		// Token: 0x04001F5E RID: 8030
		private float colWidth;

		// Token: 0x04001F5F RID: 8031
		private float rowHeight;
	}
}
