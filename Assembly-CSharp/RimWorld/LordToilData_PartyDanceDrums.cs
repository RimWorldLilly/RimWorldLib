using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D09 RID: 3337
	public class LordToilData_PartyDanceDrums : LordToilData_Gathering
	{
		// Token: 0x06005DE9 RID: 24041 RVA: 0x00225F58 File Offset: 0x00224158
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn, Building>(ref this.playedInstruments, "playedInstruments", LookMode.Reference, LookMode.Reference, ref this.tmpPawns, ref this.tmpUsedInstruments, true, false, false);
		}

		// Token: 0x04003546 RID: 13638
		public Dictionary<Pawn, Building> playedInstruments = new Dictionary<Pawn, Building>();

		// Token: 0x04003547 RID: 13639
		private List<Pawn> tmpPawns;

		// Token: 0x04003548 RID: 13640
		private List<Building> tmpUsedInstruments;
	}
}
