using System;

namespace Verse
{
	// Token: 0x02000352 RID: 850
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x06001A42 RID: 6722 RVA: 0x0009D36F File Offset: 0x0009B56F
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}

		// Token: 0x04001564 RID: 5476
		protected string attribute;
	}
}
