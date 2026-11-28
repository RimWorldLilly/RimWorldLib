using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020F1 RID: 8433
	public class SymbolResolver_BasePart_Outdoors_Leaf_LandingPad : SymbolResolver
	{
		// Token: 0x0600D406 RID: 54278 RVA: 0x004E4F3C File Offset: 0x004E313C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_landingPadsResolved < BaseGen.globalSettings.minLandingPads) && (BaseGen.globalSettings.basePart_landingPadsResolved == 0 || BaseGen.globalSettings.basePart_landingPadsResolved < BaseGen.globalSettings.minLandingPads) && rp.faction != null && rp.faction == Faction.OfEmpire;
		}

		// Token: 0x0600D407 RID: 54279 RVA: 0x004E4FD8 File Offset: 0x004E31D8
		public override void Resolve(ResolveParams rp)
		{
			CellRect rect;
			if (!rp.rect.TryFindRandomInnerRect(new IntVec2(9, 9), out rect, null))
			{
				return;
			}
			ResolveParams resolveParams = rp;
			resolveParams.rect = rect;
			BaseGen.symbolStack.Push("landingPad", resolveParams, null);
			BaseGen.globalSettings.basePart_landingPadsResolved++;
		}

		// Token: 0x040086BC RID: 34492
		private static List<ThingDef> availablePowerPlants = new List<ThingDef>();

		// Token: 0x040086BD RID: 34493
		public const int Size = 9;
	}
}
