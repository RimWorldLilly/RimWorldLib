using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020016A5 RID: 5797
	public class RitualObligationTargetWorker_ConsumableBuilding : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x06009144 RID: 37188 RVA: 0x00353302 File Offset: 0x00351502
		public RitualObligationTargetWorker_ConsumableBuilding()
		{
		}

		// Token: 0x06009145 RID: 37189 RVA: 0x0035330A File Offset: 0x0035150A
		public RitualObligationTargetWorker_ConsumableBuilding(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009146 RID: 37190 RVA: 0x00353314 File Offset: 0x00351514
		public override List<string> MissingTargetBuilding(Ideo ideo)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.def.thingDefs.Count; i++)
			{
				if (ideo.HasPreceptForBuilding(this.def.thingDefs[i]))
				{
					return base.MissingTargetBuilding(ideo);
				}
				list.Add(this.def.thingDefs[i].label);
			}
			return list;
		}
	}
}
