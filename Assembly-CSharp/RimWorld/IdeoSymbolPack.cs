using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FAD RID: 4013
	public class IdeoSymbolPack
	{
		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x0600660A RID: 26122 RVA: 0x00244B92 File Offset: 0x00242D92
		public string PrimarySymbol
		{
			get
			{
				if (!this.ideoName.NullOrEmpty())
				{
					return this.ideoName;
				}
				return this.theme;
			}
		}

		// Token: 0x04003DAE RID: 15790
		[MustTranslate]
		public string ideoName;

		// Token: 0x04003DAF RID: 15791
		[MustTranslate]
		public string theme;

		// Token: 0x04003DB0 RID: 15792
		[MustTranslate]
		public string adjective;

		// Token: 0x04003DB1 RID: 15793
		[MustTranslate]
		public string member;

		// Token: 0x04003DB2 RID: 15794
		public bool prefix;
	}
}
