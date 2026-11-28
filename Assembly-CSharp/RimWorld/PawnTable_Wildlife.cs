using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE2 RID: 7650
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x0600C6EE RID: 50926 RVA: 0x0048DB2C File Offset: 0x0048BD2C
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}

		// Token: 0x0600C6EF RID: 50927 RVA: 0x0048DB82 File Offset: 0x0048BD82
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
