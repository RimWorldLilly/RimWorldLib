using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D14 RID: 3348
	public class LordToilData_Speech : LordToilData_Gathering
	{
		// Token: 0x06005E14 RID: 24084 RVA: 0x00226FF8 File Offset: 0x002251F8
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectPreferredSide, "spectateRectPreferredSide", SpectateRectSide.None, false);
		}

		// Token: 0x04003564 RID: 13668
		public CellRect spectateRect;

		// Token: 0x04003565 RID: 13669
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x04003566 RID: 13670
		public SpectateRectSide spectateRectPreferredSide;
	}
}
