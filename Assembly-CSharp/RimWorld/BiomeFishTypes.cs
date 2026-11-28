using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000F3E RID: 3902
	public class BiomeFishTypes
	{
		// Token: 0x040039EB RID: 14827
		public List<FishChance> freshwater_Common = new List<FishChance>();

		// Token: 0x040039EC RID: 14828
		public List<FishChance> saltwater_Common = new List<FishChance>();

		// Token: 0x040039ED RID: 14829
		public List<FishChance> freshwater_Uncommon = new List<FishChance>();

		// Token: 0x040039EE RID: 14830
		public List<FishChance> saltwater_Uncommon = new List<FishChance>();

		// Token: 0x040039EF RID: 14831
		public ThingSetMakerDef rareCatchesSetMaker;
	}
}
