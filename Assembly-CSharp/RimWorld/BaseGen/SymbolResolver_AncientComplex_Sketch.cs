using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020E0 RID: 8416
	public class SymbolResolver_AncientComplex_Sketch : SymbolResolver
	{
		// Token: 0x0600D3C8 RID: 54216 RVA: 0x004E34AC File Offset: 0x004E16AC
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.ancientLayoutStructureSketch != null;
		}

		// Token: 0x0600D3C9 RID: 54217 RVA: 0x004E34C4 File Offset: 0x004E16C4
		public override void Resolve(ResolveParams rp)
		{
			rp.ancientLayoutStructureSketch.layoutDef.Worker.Spawn(rp.ancientLayoutStructureSketch, BaseGen.globalSettings.map, rp.rect.Min, rp.threatPoints, null, true, false, null);
		}
	}
}
