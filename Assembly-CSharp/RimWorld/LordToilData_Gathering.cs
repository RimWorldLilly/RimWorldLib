using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D0F RID: 3343
	public class LordToilData_Gathering : LordToilData
	{
		// Token: 0x06005DFE RID: 24062 RVA: 0x002269E8 File Offset: 0x00224BE8
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.presentForTicks.RemoveAll((KeyValuePair<Pawn, int> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, int>(ref this.presentForTicks, "presentForTicks", LookMode.Reference, LookMode.Value, ref this.tmpPresentPawns, ref this.tmpPresentPawnsTicks, true, false, false);
		}

		// Token: 0x04003559 RID: 13657
		public Dictionary<Pawn, int> presentForTicks = new Dictionary<Pawn, int>();

		// Token: 0x0400355A RID: 13658
		private List<Pawn> tmpPresentPawns;

		// Token: 0x0400355B RID: 13659
		private List<int> tmpPresentPawnsTicks;
	}
}
