using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001213 RID: 4627
	public struct ComplexResolveParams
	{
		// Token: 0x040048AB RID: 18603
		public List<Thing> spawnedThings;

		// Token: 0x040048AC RID: 18604
		public LayoutRoom room;

		// Token: 0x040048AD RID: 18605
		public List<LayoutRoom> allRooms;

		// Token: 0x040048AE RID: 18606
		public float points;

		// Token: 0x040048AF RID: 18607
		public Map map;

		// Token: 0x040048B0 RID: 18608
		public CellRect complexRect;

		// Token: 0x040048B1 RID: 18609
		public Faction hostileFaction;

		// Token: 0x040048B2 RID: 18610
		public string triggerSignal;

		// Token: 0x040048B3 RID: 18611
		public int? delayTicks;

		// Token: 0x040048B4 RID: 18612
		public bool passive;
	}
}
