using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A95 RID: 2709
	public class JobDriver_PlantHarvest_Designated : JobDriver_PlantHarvest
	{
		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06005193 RID: 20883 RVA: 0x001E764B File Offset: 0x001E584B
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.HarvestPlant;
			}
		}
	}
}
