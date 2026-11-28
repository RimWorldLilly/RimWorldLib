using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020EF RID: 8431
	public class SymbolResolver_BasePart_Outdoors_Leaf_Empty : SymbolResolver
	{
		// Token: 0x0600D400 RID: 54272 RVA: 0x004E4D3C File Offset: 0x004E2F3C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || rp.rect.Width < 9 || rp.rect.Height < 9);
		}

		// Token: 0x0600D401 RID: 54273 RVA: 0x004E4DA5 File Offset: 0x004E2FA5
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.globalSettings.basePart_emptyNodesResolved++;
		}
	}
}
