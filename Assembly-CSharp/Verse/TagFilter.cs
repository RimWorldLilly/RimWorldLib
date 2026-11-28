using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000BD RID: 189
	public class TagFilter
	{
		// Token: 0x06000773 RID: 1907 RVA: 0x0002BA00 File Offset: 0x00029C00
		public bool Allows(List<string> otherTags)
		{
			if (otherTags != null)
			{
				for (int i = 0; i < otherTags.Count; i++)
				{
					if (this.tags.Contains(otherTags[i]))
					{
						return this.whitelist;
					}
				}
			}
			return !this.whitelist;
		}

		// Token: 0x0400033D RID: 829
		public List<string> tags = new List<string>();

		// Token: 0x0400033E RID: 830
		public bool whitelist = true;
	}
}
