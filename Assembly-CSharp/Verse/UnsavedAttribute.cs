using System;

namespace Verse
{
	// Token: 0x020005E3 RID: 1507
	[AttributeUsage(AttributeTargets.Field)]
	public class UnsavedAttribute : Attribute
	{
		// Token: 0x06002FA7 RID: 12199 RVA: 0x001319CF File Offset: 0x0012FBCF
		public UnsavedAttribute(bool allowLoading = false)
		{
			this.allowLoading = allowLoading;
		}

		// Token: 0x04001E52 RID: 7762
		public bool allowLoading;
	}
}
