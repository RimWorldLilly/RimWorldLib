using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F71 RID: 3953
	public class IdeoSymbolPartDef : Def
	{
		// Token: 0x06006562 RID: 25954 RVA: 0x00241C08 File Offset: 0x0023FE08
		public bool CanBeChosenForIdeo(Ideo ideo)
		{
			if (this.memes == null && this.cultures == null)
			{
				return true;
			}
			if (this.cultures != null && this.cultures.Contains(ideo.culture))
			{
				return true;
			}
			if (this.memes != null)
			{
				for (int i = 0; i < ideo.memes.Count; i++)
				{
					if (this.memes.Contains(ideo.memes[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04003BC1 RID: 15297
		public List<MemeDef> memes;

		// Token: 0x04003BC2 RID: 15298
		public List<CultureDef> cultures;
	}
}
