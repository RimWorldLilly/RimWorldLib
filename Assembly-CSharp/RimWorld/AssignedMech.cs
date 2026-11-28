using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155F RID: 5471
	public class AssignedMech : IExposable
	{
		// Token: 0x060085FE RID: 34302 RVA: 0x00306B94 File Offset: 0x00304D94
		public AssignedMech()
		{
		}

		// Token: 0x060085FF RID: 34303 RVA: 0x00306B9C File Offset: 0x00304D9C
		public AssignedMech(Pawn pawn)
		{
			this.pawn = pawn;
			this.tickAssigned = Find.TickManager.TicksGame;
		}

		// Token: 0x06008600 RID: 34304 RVA: 0x00306BBB File Offset: 0x00304DBB
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.tickAssigned, "tickAssigned", 0, false);
		}

		// Token: 0x0400528C RID: 21132
		public Pawn pawn;

		// Token: 0x0400528D RID: 21133
		public int tickAssigned;
	}
}
