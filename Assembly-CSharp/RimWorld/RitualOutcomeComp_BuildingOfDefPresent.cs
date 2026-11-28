using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C6 RID: 5830
	public class RitualOutcomeComp_BuildingOfDefPresent : RitualOutcomeComp_BuildingsPresent
	{
		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x060091F2 RID: 37362 RVA: 0x00355278 File Offset: 0x00353478
		protected override string LabelForDesc
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x060091F3 RID: 37363 RVA: 0x0035528A File Offset: 0x0035348A
		protected override Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual)
		{
			return cell.GetFirstThing(map, this.def);
		}

		// Token: 0x04005883 RID: 22659
		public ThingDef def;
	}
}
