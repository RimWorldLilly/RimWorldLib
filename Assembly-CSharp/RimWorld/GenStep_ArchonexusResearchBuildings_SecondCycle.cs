using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001296 RID: 4758
	public class GenStep_ArchonexusResearchBuildings_SecondCycle : GenStep_ArchonexusResearchBuildings
	{
		// Token: 0x06007601 RID: 30209 RVA: 0x00297E18 File Offset: 0x00296018
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = CellRect.CenteredOn(loc, this.Size.x, this.Size.z);
			resolveParams.minorBuildingCount = new int?(4);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("archonexusResearchBuildings", resolveParams, null);
			BaseGen.Generate();
		}
	}
}
