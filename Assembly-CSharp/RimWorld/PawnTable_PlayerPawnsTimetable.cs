using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE1 RID: 7649
	public class PawnTable_PlayerPawnsTimetable : PawnTable_PlayerPawns
	{
		// Token: 0x0600C6ED RID: 50925 RVA: 0x0048DB1C File Offset: 0x0048BD1C
		public PawnTable_PlayerPawnsTimetable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
