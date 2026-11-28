using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D97 RID: 7575
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		// Token: 0x17002482 RID: 9346
		// (get) Token: 0x0600C48C RID: 50316 RVA: 0x0047EE54 File Offset: 0x0047D054
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		// Token: 0x17002483 RID: 9347
		// (get) Token: 0x0600C48D RID: 50317 RVA: 0x0047EE5B File Offset: 0x0047D05B
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists
				where !pawn.DevelopmentalStage.Baby()
				select pawn;
			}
		}
	}
}
