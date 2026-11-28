using System;

namespace Verse
{
	// Token: 0x0200034B RID: 843
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x06001A34 RID: 6708 RVA: 0x0009CE35 File Offset: 0x0009B035
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}

		// Token: 0x0400155C RID: 5468
		protected string xpath;
	}
}
