using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000B2 RID: 178
	public class PawnTags : IExposable
	{
		// Token: 0x0600074F RID: 1871 RVA: 0x00029F3A File Offset: 0x0002813A
		public bool Contains(string tag)
		{
			return this.tags.Contains(tag);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00029F48 File Offset: 0x00028148
		public void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x040002BF RID: 703
		public List<string> tags;
	}
}
