using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000354 RID: 852
	public class PatchOperationAttributeRemove : PatchOperationAttribute
	{
		// Token: 0x06001A46 RID: 6726 RVA: 0x0009D438 File Offset: 0x0009B638
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes.Remove(xmlNode.Attributes[this.attribute]);
					result = true;
				}
			}
			return result;
		}
	}
}
