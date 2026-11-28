using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000355 RID: 853
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		// Token: 0x06001A48 RID: 6728 RVA: 0x0009D4D0 File Offset: 0x0009B6D0
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes[this.attribute].Value = this.value;
				}
				else
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04001566 RID: 5478
		protected string value;
	}
}
