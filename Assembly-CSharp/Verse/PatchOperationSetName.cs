using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000351 RID: 849
	public class PatchOperationSetName : PatchOperationPathed
	{
		// Token: 0x06001A40 RID: 6720 RVA: 0x0009D2F0 File Offset: 0x0009B4F0
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode xmlNode2 = xmlNode.OwnerDocument.CreateElement(this.name);
				xmlNode2.InnerXml = xmlNode.InnerXml;
				xmlNode.ParentNode.InsertBefore(xmlNode2, xmlNode);
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04001563 RID: 5475
		protected string name;
	}
}
