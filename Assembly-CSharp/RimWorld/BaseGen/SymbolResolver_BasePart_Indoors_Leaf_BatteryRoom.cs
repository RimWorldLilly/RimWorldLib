using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020E3 RID: 8419
	public class SymbolResolver_BasePart_Indoors_Leaf_BatteryRoom : SymbolResolver
	{
		// Token: 0x0600D3D3 RID: 54227 RVA: 0x004E3814 File Offset: 0x004E1A14
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_throneRoomsResolved >= BaseGen.globalSettings.minThroneRooms && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp)) && (BaseGen.globalSettings.basePart_gravcoresResolved >= BaseGen.globalSettings.requiredGravcoreRooms || !SymbolResolver_BasePart_Indoors_Leaf_Gravcore.CanResolve("basePart_indoors_leaf", rp)) && BaseGen.globalSettings.basePart_batteriesCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.06f && (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial);
		}

		// Token: 0x0600D3D4 RID: 54228 RVA: 0x004E38F8 File Offset: 0x004E1AF8
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("batteryRoom", rp, null);
			BaseGen.globalSettings.basePart_batteriesCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}

		// Token: 0x040086AA RID: 34474
		private const float MaxCoverage = 0.06f;
	}
}
