using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C7 RID: 5831
	public class RitualOutcomeComp_BuildingOfAnyDefPresent : RitualOutcomeComp_BuildingsPresent
	{
		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x060091F5 RID: 37365 RVA: 0x003552A4 File Offset: 0x003534A4
		protected override string LabelForDesc
		{
			get
			{
				return "RitualOutcomeLabelAnyOfThese".Translate() + ": " + (from d in this.defs
				select d.LabelCap.Resolve()).ToCommaList(false, false);
			}
		}

		// Token: 0x060091F6 RID: 37366 RVA: 0x00355300 File Offset: 0x00353500
		protected override Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual)
		{
			foreach (ThingDef def in this.defs)
			{
				Thing firstThing = cell.GetFirstThing(map, def);
				if (firstThing != null)
				{
					return firstThing;
				}
			}
			return null;
		}

		// Token: 0x04005884 RID: 22660
		public List<ThingDef> defs;
	}
}
