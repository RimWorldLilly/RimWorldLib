using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020020EC RID: 8428
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_EdgeStreet : SymbolResolver
	{
		// Token: 0x0600D3F8 RID: 54264 RVA: 0x004E4AC4 File Offset: 0x004E2CC4
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.floorDef = (rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false));
			BaseGen.symbolStack.Push("edgeStreet", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.rect = rp.rect.ContractedBy(1);
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", resolveParams2, null);
		}
	}
}
