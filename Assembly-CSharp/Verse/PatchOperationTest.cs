using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000357 RID: 855
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x06001A4E RID: 6734 RVA: 0x0009D67E File Offset: 0x0009B87E
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
