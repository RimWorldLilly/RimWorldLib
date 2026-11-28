using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FFB RID: 8187
	public struct RoyalTitleInheritanceOutcome
	{
		// Token: 0x1700255C RID: 9564
		// (get) Token: 0x0600CF69 RID: 53097 RVA: 0x004C60D8 File Offset: 0x004C42D8
		public bool FoundHeir
		{
			get
			{
				return this.heir != null;
			}
		}

		// Token: 0x1700255D RID: 9565
		// (get) Token: 0x0600CF6A RID: 53098 RVA: 0x004C60E3 File Offset: 0x004C42E3
		public bool HeirHasTitle
		{
			get
			{
				return this.heirCurrentTitle != null;
			}
		}

		// Token: 0x040083EC RID: 33772
		public Pawn heir;

		// Token: 0x040083ED RID: 33773
		public RoyalTitleDef heirCurrentTitle;

		// Token: 0x040083EE RID: 33774
		public bool heirTitleHigher;
	}
}
