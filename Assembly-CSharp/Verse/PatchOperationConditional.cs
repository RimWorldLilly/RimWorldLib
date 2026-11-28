using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000358 RID: 856
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x06001A50 RID: 6736 RVA: 0x0009D698 File Offset: 0x0009B898
		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (xml.SelectSingleNode(this.xpath) != null)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return this.match != null || this.nomatch != null;
		}

		// Token: 0x04001569 RID: 5481
		private PatchOperation match;

		// Token: 0x0400156A RID: 5482
		private PatchOperation nomatch;
	}
}
