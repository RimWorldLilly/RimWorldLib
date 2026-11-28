using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D11 RID: 3345
	public class LordToilData_MarriageCeremony : LordToilData
	{
		// Token: 0x06005E0B RID: 24075 RVA: 0x00226D70 File Offset: 0x00224F70
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
		}

		// Token: 0x04003560 RID: 13664
		public CellRect spectateRect;

		// Token: 0x04003561 RID: 13665
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;
	}
}
