using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020E6 RID: 8422
	public class SymbolResolver_BasePart_Indoors_Leaf_ThroneRoom : SymbolResolver
	{
		// Token: 0x0600D3DC RID: 54236 RVA: 0x004E3B3C File Offset: 0x004E1D3C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.faction != null && rp.faction == Faction.OfEmpire && (rp.allowGeneratingThronerooms == null || rp.allowGeneratingThronerooms.Value) && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp)) && (BaseGen.globalSettings.basePart_gravcoresResolved >= BaseGen.globalSettings.requiredGravcoreRooms || !SymbolResolver_BasePart_Indoors_Leaf_Gravcore.CanResolve("basePart_indoors_leaf", rp)) && (BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks || BaseGen.globalSettings.basePart_throneRoomsResolved < BaseGen.globalSettings.minThroneRooms) && (BaseGen.globalSettings.basePart_throneRoomsResolved == 0 || BaseGen.globalSettings.basePart_throneRoomsResolved < BaseGen.globalSettings.minThroneRooms);
		}

		// Token: 0x0600D3DD RID: 54237 RVA: 0x004E3C26 File Offset: 0x004E1E26
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("throneRoom", rp, null);
			BaseGen.globalSettings.basePart_throneRoomsResolved++;
		}
	}
}
