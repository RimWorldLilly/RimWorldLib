using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020F0 RID: 8432
	public class SymbolResolver_BasePart_Outdoors_Leaf_Farm : SymbolResolver
	{
		// Token: 0x0600D403 RID: 54275 RVA: 0x004E4DC4 File Offset: 0x004E2FC4
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || rp.rect.Width < 9 || rp.rect.Height < 9) && BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes && BaseGen.globalSettings.basePart_farmsCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.55f && (BaseGen.globalSettings.maxFarms <= -1 || BaseGen.globalSettings.basePart_farmsCount < BaseGen.globalSettings.maxFarms) && (rp.rect.Width <= 15 && rp.rect.Height <= 15) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x0600D404 RID: 54276 RVA: 0x004E4ED4 File Offset: 0x004E30D4
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("farm", rp, null);
			BaseGen.globalSettings.basePart_farmsCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
			BaseGen.globalSettings.basePart_farmsCount++;
		}

		// Token: 0x040086BB RID: 34491
		private const float MaxCoverage = 0.55f;
	}
}
