using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BB RID: 4283
	public class QuestPart_DisableRandomMoodCausedMentalBreaks : QuestPartActivable
	{
		// Token: 0x06006B1F RID: 27423 RVA: 0x00259A64 File Offset: 0x00257C64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006B20 RID: 27424 RVA: 0x00259AC0 File Offset: 0x00257CC0
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0400441A RID: 17434
		public List<Pawn> pawns = new List<Pawn>();
	}
}
