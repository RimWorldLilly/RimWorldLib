using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EED RID: 3821
	public class CompProperties_Forbiddable : CompProperties
	{
		// Token: 0x060063CB RID: 25547 RVA: 0x0023C65A File Offset: 0x0023A85A
		public CompProperties_Forbiddable()
		{
			this.compClass = typeof(CompForbiddable);
		}

		// Token: 0x04003847 RID: 14407
		public bool allowNonPlayer;

		// Token: 0x04003848 RID: 14408
		public bool forbidOnMake;
	}
}
