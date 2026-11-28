using System;

namespace Verse
{
	// Token: 0x020005E1 RID: 1505
	[AttributeUsage(AttributeTargets.Field)]
	public class TranslationHandleAttribute : Attribute
	{
		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x00131995 File Offset: 0x0012FB95
		// (set) Token: 0x06002FA4 RID: 12196 RVA: 0x0013199D File Offset: 0x0012FB9D
		public int Priority { get; set; }
	}
}
