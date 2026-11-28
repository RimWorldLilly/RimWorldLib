using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200062D RID: 1581
	public class TreeNode
	{
		// Token: 0x06003195 RID: 12693 RVA: 0x0013D28A File Offset: 0x0013B48A
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x0013D297 File Offset: 0x0013B497
		public void SetOpen(int mask, bool val)
		{
			if (val)
			{
				this.openBits |= mask;
				return;
			}
			this.openBits &= ~mask;
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06003197 RID: 12695 RVA: 0x0013D2BA File Offset: 0x0013B4BA
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001FC1 RID: 8129
		public TreeNode parentNode;

		// Token: 0x04001FC2 RID: 8130
		public List<TreeNode> children;

		// Token: 0x04001FC3 RID: 8131
		public int nestDepth;

		// Token: 0x04001FC4 RID: 8132
		private int openBits;
	}
}
