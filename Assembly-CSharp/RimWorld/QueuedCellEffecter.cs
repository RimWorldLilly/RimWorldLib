using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001288 RID: 4744
	internal struct QueuedCellEffecter
	{
		// Token: 0x0600759C RID: 30108 RVA: 0x00294842 File Offset: 0x00292A42
		public QueuedCellEffecter(EffecterDef edef, IntVec3 cell, int tick)
		{
			this.effecterDef = edef;
			this.tick = tick;
			this.cell = cell;
		}

		// Token: 0x040049AB RID: 18859
		public EffecterDef effecterDef;

		// Token: 0x040049AC RID: 18860
		public int tick;

		// Token: 0x040049AD RID: 18861
		public IntVec3 cell;
	}
}
