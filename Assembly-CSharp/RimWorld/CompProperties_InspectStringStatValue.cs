using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CB RID: 6347
	public class CompProperties_InspectStringStatValue : CompProperties_InspectString
	{
		// Token: 0x06009F0E RID: 40718 RVA: 0x0039107C File Offset: 0x0038F27C
		public CompProperties_InspectStringStatValue()
		{
			this.compClass = typeof(CompInspectStringStatValue);
		}

		// Token: 0x04005E1A RID: 24090
		public StatDef stat;

		// Token: 0x04005E1B RID: 24091
		public ToStringNumberSense? numberSense;
	}
}
