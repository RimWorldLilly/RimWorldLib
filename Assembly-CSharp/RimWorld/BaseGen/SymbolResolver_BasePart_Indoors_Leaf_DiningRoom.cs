using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020E5 RID: 8421
	public class SymbolResolver_BasePart_Indoors_Leaf_DiningRoom : SymbolResolver
	{
		// Token: 0x0600D3D9 RID: 54233 RVA: 0x004E3A8C File Offset: 0x004E1C8C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp)) && (BaseGen.globalSettings.basePart_gravcoresResolved >= BaseGen.globalSettings.requiredGravcoreRooms || !SymbolResolver_BasePart_Indoors_Leaf_Gravcore.CanResolve("basePart_indoors_leaf", rp));
		}

		// Token: 0x0600D3DA RID: 54234 RVA: 0x004E3B1F File Offset: 0x004E1D1F
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("diningRoom", rp, null);
		}
	}
}
