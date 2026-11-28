using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x0200034E RID: 846
	public class PatchOperationInsert : PatchOperationPathed
	{
		// Token: 0x06001A3A RID: 6714 RVA: 0x0009D088 File Offset: 0x0009B288
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				result = true;
				XmlNode xmlNode = obj as XmlNode;
				XmlNode parentNode = xmlNode.ParentNode;
				if (this.order == PatchOperationInsert.Order.Append)
				{
					using (IEnumerator enumerator2 = node.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode node2 = (XmlNode)obj2;
							parentNode.InsertAfter(parentNode.OwnerDocument.ImportNode(node2, true), xmlNode);
						}
						goto IL_E0;
					}
					goto IL_98;
				}
				goto IL_98;
				IL_E0:
				if (xmlNode.NodeType == XmlNodeType.Text)
				{
					parentNode.Normalize();
					continue;
				}
				continue;
				IL_98:
				if (this.order == PatchOperationInsert.Order.Prepend)
				{
					for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
					{
						parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node.ChildNodes[i], true), xmlNode);
					}
					goto IL_E0;
				}
				goto IL_E0;
			}
			return result;
		}

		// Token: 0x04001560 RID: 5472
		private XmlContainer value;

		// Token: 0x04001561 RID: 5473
		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		// Token: 0x02002687 RID: 9863
		private enum Order
		{
			// Token: 0x04009DDA RID: 40410
			Append,
			// Token: 0x04009DDB RID: 40411
			Prepend
		}
	}
}
