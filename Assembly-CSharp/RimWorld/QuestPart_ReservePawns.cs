using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F1 RID: 4337
	public class QuestPart_ReservePawns : QuestPart
	{
		// Token: 0x06006C49 RID: 27721 RVA: 0x0025FF56 File Offset: 0x0025E156
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06006C4A RID: 27722 RVA: 0x0025FF64 File Offset: 0x0025E164
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040044EE RID: 17646
		public List<Pawn> pawns = new List<Pawn>();
	}
}
