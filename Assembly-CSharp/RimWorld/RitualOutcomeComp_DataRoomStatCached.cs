using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C1 RID: 5825
	public class RitualOutcomeComp_DataRoomStatCached : RitualOutcomeComp_Data
	{
		// Token: 0x060091D1 RID: 37329 RVA: 0x003549F5 File Offset: 0x00352BF5
		public override void Reset()
		{
			base.Reset();
			this.startingVal = null;
		}

		// Token: 0x060091D2 RID: 37330 RVA: 0x00354A0C File Offset: 0x00352C0C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float?>(ref this.startingVal, "startingVal", null, false);
		}

		// Token: 0x0400587E RID: 22654
		public float? startingVal;
	}
}
