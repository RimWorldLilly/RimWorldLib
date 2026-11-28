using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F1 RID: 8945
	public class RoomTemperatureVacuum : IExposable
	{
		// Token: 0x0600DCF5 RID: 56565 RVA: 0x00524014 File Offset: 0x00522214
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.roomCell, "roomCell", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.temperature, "temperature", 0f, false);
			Scribe_Values.Look<float>(ref this.vacuum, "vacuum", 0f, false);
		}

		// Token: 0x04008D85 RID: 36229
		public IntVec3 roomCell;

		// Token: 0x04008D86 RID: 36230
		public float temperature;

		// Token: 0x04008D87 RID: 36231
		public float vacuum;

		// Token: 0x02003DE6 RID: 15846
		public struct Data
		{
			// Token: 0x0400F41D RID: 62493
			public IntVec3 local;

			// Token: 0x0400F41E RID: 62494
			public float temperature;

			// Token: 0x0400F41F RID: 62495
			public float vacuum;
		}
	}
}
