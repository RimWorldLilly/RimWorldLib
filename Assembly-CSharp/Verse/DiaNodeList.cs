using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000668 RID: 1640
	public class DiaNodeList
	{
		// Token: 0x060033F2 RID: 13298 RVA: 0x0014E6C4 File Offset: 0x0014C8C4
		public DiaNodeMold RandomNodeFromList()
		{
			List<DiaNodeMold> list = this.Nodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.NodeNames)
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
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x040021D2 RID: 8658
		public string Name = "NeedsName";

		// Token: 0x040021D3 RID: 8659
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x040021D4 RID: 8660
		public List<string> NodeNames = new List<string>();
	}
}
