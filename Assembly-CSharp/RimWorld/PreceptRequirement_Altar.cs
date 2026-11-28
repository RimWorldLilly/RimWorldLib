using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001652 RID: 5714
	public class PreceptRequirement_Altar : PreceptRequirement
	{
		// Token: 0x06008F8E RID: 36750 RVA: 0x0034B99C File Offset: 0x00349B9C
		public override bool Met(List<Precept> precepts)
		{
			for (int i = 0; i < precepts.Count; i++)
			{
				Precept_Building precept_Building = precepts[i] as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef.isAltar)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008F8F RID: 36751 RVA: 0x0034B9DC File Offset: 0x00349BDC
		public override Precept MakePrecept(Ideo ideo)
		{
			PreceptDef ideoBuilding = PreceptDefOf.IdeoBuilding;
			Precept_Building precept_Building = (Precept_Building)PreceptMaker.MakePrecept(ideoBuilding);
			precept_Building.ideo = ideo;
			precept_Building.ThingDef = (from b in ideoBuilding.Worker.ThingDefsForIdeo(ideo, null)
			where b.def.isAltar
			select b).RandomElement<PreceptThingChance>().def;
			return precept_Building;
		}
	}
}
