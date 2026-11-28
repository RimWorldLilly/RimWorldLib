using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000665 RID: 1637
	public class DiaNode
	{
		// Token: 0x060033ED RID: 13293 RVA: 0x0014E4B4 File Offset: 0x0014C6B4
		public DiaNode(TaggedString text)
		{
			this.text = text;
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x0014E4D0 File Offset: 0x0014C6D0
		public DiaNode(DiaNodeMold newDef)
		{
			this.def = newDef;
			this.def.used = true;
			this.text = this.def.texts.RandomElement<string>();
			if (this.def.optionList.Count > 0)
			{
				using (List<DiaOptionMold>.Enumerator enumerator = this.def.optionList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DiaOptionMold diaOptionMold = enumerator.Current;
						this.options.Add(new DiaOption(diaOptionMold));
					}
					return;
				}
			}
			this.options.Add(new DiaOption("OK".Translate()));
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x0014E5A4 File Offset: 0x0014C7A4
		public void PreClose()
		{
		}

		// Token: 0x040021C3 RID: 8643
		public TaggedString text;

		// Token: 0x040021C4 RID: 8644
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x040021C5 RID: 8645
		protected DiaNodeMold def;
	}
}
