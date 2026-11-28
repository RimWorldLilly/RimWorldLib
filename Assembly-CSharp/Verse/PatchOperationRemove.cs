using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x0200034F RID: 847
	public class PatchOperationRemove : PatchOperationPathed
	{
		// Token: 0x06001A3C RID: 6716 RVA: 0x0009D1D4 File Offset: 0x0009B3D4
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}
	}
}
