using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200164A RID: 5706
	public class PreceptWorker_Relic : PreceptWorker
	{
		// Token: 0x06008F36 RID: 36662 RVA: 0x0034A5D2 File Offset: 0x003487D2
		public override float GetThingOrder(PreceptThingChance thingChance)
		{
			return -thingChance.chance;
		}

		// Token: 0x170018BD RID: 6333
		// (get) Token: 0x06008F37 RID: 36663 RVA: 0x0034A5DB File Offset: 0x003487DB
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				return new PreceptWorker_Relic.<get_ThingDefs>d__2(-2);
			}
		}
	}
}
