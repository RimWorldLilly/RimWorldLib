using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF5 RID: 7157
	public class Alert_PokerTableNoChairs : Alert_JoyBuildingNoChairs
	{
		// Token: 0x17002114 RID: 8468
		// (get) Token: 0x0600B6FD RID: 46845 RVA: 0x00407AFD File Offset: 0x00405CFD
		protected override JoyGiverDef JoyGiver
		{
			get
			{
				return JoyGiverDefOf.Play_Poker;
			}
		}

		// Token: 0x0600B6FE RID: 46846 RVA: 0x00407B04 File Offset: 0x00405D04
		public Alert_PokerTableNoChairs()
		{
			this.defaultLabel = "PokerTablesNeedChairs".Translate();
			this.defaultExplanation = "PokerTablesNeedChairsDesc".Translate();
		}
	}
}
