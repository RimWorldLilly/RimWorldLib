using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020E2 RID: 8418
	public class SymbolResolver_BasePart_Indoors_Leaf_Barracks : SymbolResolver
	{
		// Token: 0x0600D3D0 RID: 54224 RVA: 0x004E376C File Offset: 0x004E196C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp)) && (BaseGen.globalSettings.basePart_gravcoresResolved >= BaseGen.globalSettings.requiredGravcoreRooms || !SymbolResolver_BasePart_Indoors_Leaf_Gravcore.CanResolve("basePart_indoors_leaf", rp));
		}

		// Token: 0x0600D3D1 RID: 54225 RVA: 0x004E37E7 File Offset: 0x004E19E7
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("barracks", rp, null);
			BaseGen.globalSettings.basePart_barracksResolved++;
		}
	}
}
