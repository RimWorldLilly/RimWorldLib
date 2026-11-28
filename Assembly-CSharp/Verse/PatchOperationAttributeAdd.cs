using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000353 RID: 851
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		// Token: 0x06001A44 RID: 6724 RVA: 0x0009D390 File Offset: 0x0009B590
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] == null)
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04001565 RID: 5477
		protected string value;
	}
}
