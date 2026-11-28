using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBE RID: 4030
	public class PawnTableDef : Def
	{
		// Token: 0x04003EC8 RID: 16072
		public List<PawnColumnDef> columns;

		// Token: 0x04003EC9 RID: 16073
		public Type workerClass = typeof(PawnTable);

		// Token: 0x04003ECA RID: 16074
		public int minWidth = 998;
	}
}
