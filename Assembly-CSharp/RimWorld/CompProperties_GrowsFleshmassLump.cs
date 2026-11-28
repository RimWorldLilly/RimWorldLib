using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AF RID: 6319
	public class CompProperties_GrowsFleshmassLump : CompProperties
	{
		// Token: 0x06009E53 RID: 40531 RVA: 0x0038D2BB File Offset: 0x0038B4BB
		public CompProperties_GrowsFleshmassLump()
		{
			this.compClass = typeof(CompGrowsFleshmassLump);
		}

		// Token: 0x04005DA9 RID: 23977
		public IntRange maxFleshmassRange;

		// Token: 0x04005DAA RID: 23978
		public IntRange ticksBetweenFleshmassRange;
	}
}
