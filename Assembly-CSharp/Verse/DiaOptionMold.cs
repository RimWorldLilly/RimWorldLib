using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Verse
{
	// Token: 0x0200066A RID: 1642
	public class DiaOptionMold
	{
		// Token: 0x060033FE RID: 13310 RVA: 0x0014EA7C File Offset: 0x0014CC7C
		public DiaNodeMold RandomLinkNode()
		{
			List<DiaNodeMold> list = this.ChildNodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.ChildNodeNames)
			{
				list.Add(DialogDatabase.GetNodeNamed(nodeName));
			}
			foreach (DiaNodeMold diaNodeMold in list)
			{
				if (diaNodeMold.unique && diaNodeMold.used)
				{
					list.Remove(diaNodeMold);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x040021E0 RID: 8672
		public string Text = "OK".Translate();

		// Token: 0x040021E1 RID: 8673
		[XmlElement("Node")]
		public List<DiaNodeMold> ChildNodes = new List<DiaNodeMold>();

		// Token: 0x040021E2 RID: 8674
		[XmlElement("NodeName")]
		[DefaultValue("")]
		public List<string> ChildNodeNames = new List<string>();
	}
}
