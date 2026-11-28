using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F92 RID: 3986
	public class RoomPart_CrateDef : RoomPartDef
	{
		// Token: 0x060065C4 RID: 26052 RVA: 0x002438A6 File Offset: 0x00241AA6
		public RoomPart_CrateDef()
		{
			this.workerClass = typeof(RoomPart_Crate);
		}

		// Token: 0x04003CD4 RID: 15572
		public ThingDef crateDef;

		// Token: 0x04003CD5 RID: 15573
		public ThingSetMakerDef thingSetMaker;

		// Token: 0x04003CD6 RID: 15574
		public RotEnum rotations = RotEnum.All;

		// Token: 0x04003CD7 RID: 15575
		public bool triggerThreatSignal = true;
	}
}
