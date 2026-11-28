using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020DF RID: 8415
	public class SymbolResolver_AncientComplex_Mechanitor : SymbolResolver_AncientComplex_Base
	{
		// Token: 0x170025EE RID: 9710
		// (get) Token: 0x0600D3C5 RID: 54213 RVA: 0x004E3458 File Offset: 0x004E1658
		protected override LayoutDef DefaultLayoutDef
		{
			get
			{
				return LayoutDefOf.AncientComplex_Mechanitor_Loot;
			}
		}

		// Token: 0x0600D3C6 RID: 54214 RVA: 0x004E3460 File Offset: 0x004E1660
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.floorDef = TerrainDefOf.PackedDirt;
			BaseGen.symbolStack.Push("outdoorsPath", resolveParams, null);
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
			base.ResolveComplex(rp);
		}
	}
}
