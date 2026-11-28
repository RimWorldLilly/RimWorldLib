using System;

namespace Verse
{
	// Token: 0x02000624 RID: 1572
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x06003139 RID: 12601 RVA: 0x0013AAB0 File Offset: 0x00138CB0
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		// Token: 0x04001F96 RID: 8086
		public float lineHeight = 20f;
	}
}
