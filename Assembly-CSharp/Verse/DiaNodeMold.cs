using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000667 RID: 1639
	public class DiaNodeMold
	{
		// Token: 0x060033F0 RID: 13296 RVA: 0x0014E5A8 File Offset: 0x0014C7A8
		public void PostLoad()
		{
			int num = 0;
			foreach (string text in this.texts.ListFullCopy<string>())
			{
				this.texts[num] = text.Replace("\\n", Environment.NewLine);
				num++;
			}
			foreach (DiaOptionMold diaOptionMold in this.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.PostLoad();
				}
			}
		}

		// Token: 0x040021CB RID: 8651
		public string name = "Unnamed";

		// Token: 0x040021CC RID: 8652
		public bool unique;

		// Token: 0x040021CD RID: 8653
		public List<string> texts = new List<string>();

		// Token: 0x040021CE RID: 8654
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x040021CF RID: 8655
		[Unsaved(false)]
		public bool isRoot = true;

		// Token: 0x040021D0 RID: 8656
		[Unsaved(false)]
		public bool used;

		// Token: 0x040021D1 RID: 8657
		[Unsaved(false)]
		public DiaNodeType nodeType;
	}
}
