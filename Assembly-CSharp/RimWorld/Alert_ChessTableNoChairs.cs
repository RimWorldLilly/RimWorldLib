using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF4 RID: 7156
	public class Alert_ChessTableNoChairs : Alert_JoyBuildingNoChairs
	{
		// Token: 0x17002113 RID: 8467
		// (get) Token: 0x0600B6FB RID: 46843 RVA: 0x00407AC4 File Offset: 0x00405CC4
		protected override JoyGiverDef JoyGiver
		{
			get
			{
				return JoyGiverDefOf.Play_Chess;
			}
		}

		// Token: 0x0600B6FC RID: 46844 RVA: 0x00407ACB File Offset: 0x00405CCB
		public Alert_ChessTableNoChairs()
		{
			this.defaultLabel = "ChessTablesNeedChairs".Translate();
			this.defaultExplanation = "ChessTablesNeedChairsDesc".Translate();
		}
	}
}
