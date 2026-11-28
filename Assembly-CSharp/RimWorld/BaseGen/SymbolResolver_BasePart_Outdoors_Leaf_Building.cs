using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020EE RID: 8430
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		// Token: 0x0600D3FD RID: 54269 RVA: 0x004E4C30 File Offset: 0x004E2E30
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings || rp.rect.Width < 9 || rp.rect.Height < 9) && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings);
		}

		// Token: 0x0600D3FE RID: 54270 RVA: 0x004E4CC8 File Offset: 0x004E2EC8
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
			resolveParams.floorDef = (rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, true));
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams, null);
			BaseGen.globalSettings.basePart_buildingsResolved++;
		}
	}
}
