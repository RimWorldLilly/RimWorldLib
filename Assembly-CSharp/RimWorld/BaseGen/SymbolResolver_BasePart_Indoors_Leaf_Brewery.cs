using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020E4 RID: 8420
	public class SymbolResolver_BasePart_Indoors_Leaf_Brewery : SymbolResolver
	{
		// Token: 0x0600D3D6 RID: 54230 RVA: 0x004E3950 File Offset: 0x004E1B50
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp)) && (BaseGen.globalSettings.basePart_gravcoresResolved >= BaseGen.globalSettings.requiredGravcoreRooms || !SymbolResolver_BasePart_Indoors_Leaf_Gravcore.CanResolve("basePart_indoors_leaf", rp)) && BaseGen.globalSettings.basePart_breweriesCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.08f && (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Medieval);
		}

		// Token: 0x0600D3D7 RID: 54231 RVA: 0x004E3A34 File Offset: 0x004E1C34
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("brewery", rp, null);
			BaseGen.globalSettings.basePart_breweriesCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}

		// Token: 0x040086AB RID: 34475
		private const float MaxCoverage = 0.08f;
	}
}
