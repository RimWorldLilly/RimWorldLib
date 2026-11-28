using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F93 RID: 3987
	public class RoomPart_BarricadeDef : RoomPartDef
	{
		// Token: 0x060065C5 RID: 26053 RVA: 0x002438CD File Offset: 0x00241ACD
		public RoomPart_BarricadeDef()
		{
			this.workerClass = typeof(RoomPart_Barricades);
		}

		// Token: 0x04003CD8 RID: 15576
		public ThingDef wallDef;

		// Token: 0x04003CD9 RID: 15577
		public ThingDef stuffDef;

		// Token: 0x04003CDA RID: 15578
		public int steps = 2;

		// Token: 0x04003CDB RID: 15579
		public int offset = 2;

		// Token: 0x04003CDC RID: 15580
		public float chancePerDoor = 0.75f;
	}
}
