using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000349 RID: 841
	public class XmlContainer
	{
		// Token: 0x06001A2D RID: 6701 RVA: 0x0009CD3F File Offset: 0x0009AF3F
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		// Token: 0x04001558 RID: 5464
		public XmlNode node;
	}
}
