using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A96 RID: 2710
	public class JobDriver_PlantCut_Designated : JobDriver_PlantCut
	{
		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x001E765A File Offset: 0x001E585A
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.CutPlant;
			}
		}
	}
}
