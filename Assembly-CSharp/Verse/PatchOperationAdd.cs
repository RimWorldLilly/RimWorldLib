using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x0200034C RID: 844
	public class PatchOperationAdd : PatchOperationPathed
	{
		// Token: 0x06001A36 RID: 6710 RVA: 0x0009CE58 File Offset: 0x0009B058
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				result = true;
				XmlNode xmlNode = obj as XmlNode;
				if (this.order == PatchOperationAdd.Order.Append)
				{
					using (IEnumerator enumerator2 = node.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode node2 = (XmlNode)obj2;
							xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(node2, true));
						}
						continue;
					}
				}
				if (this.order == PatchOperationAdd.Order.Prepend)
				{
					for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
					{
						xmlNode.PrependChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[i], true));
					}
				}
			}
			return result;
		}

		// Token: 0x0400155D RID: 5469
		private XmlContainer value;

		// Token: 0x0400155E RID: 5470
		private PatchOperationAdd.Order order;

		// Token: 0x02002686 RID: 9862
		private enum Order
		{
			// Token: 0x04009DD7 RID: 40407
			Append,
			// Token: 0x04009DD8 RID: 40408
			Prepend
		}
	}
}
