using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000350 RID: 848
	public class PatchOperationReplace : PatchOperationPathed
	{
		// Token: 0x06001A3E RID: 6718 RVA: 0x0009D224 File Offset: 0x0009B424
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode parentNode = xmlNode.ParentNode;
				foreach (object obj in node.ChildNodes)
				{
					XmlNode node2 = (XmlNode)obj;
					parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node2, true), xmlNode);
				}
				parentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04001562 RID: 5474
		private XmlContainer value;
	}
}
