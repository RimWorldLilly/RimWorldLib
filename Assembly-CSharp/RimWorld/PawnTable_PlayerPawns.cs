using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE0 RID: 7648
	public class PawnTable_PlayerPawns : PawnTable
	{
		// Token: 0x0600C6EB RID: 50923 RVA: 0x0048DB07 File Offset: 0x0048BD07
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return PlayerPawnsDisplayOrderUtility.InOrder(input);
		}

		// Token: 0x0600C6EC RID: 50924 RVA: 0x0048DB0F File Offset: 0x0048BD0F
		public PawnTable_PlayerPawns(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
