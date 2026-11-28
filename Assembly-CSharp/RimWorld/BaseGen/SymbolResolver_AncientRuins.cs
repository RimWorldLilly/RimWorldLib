using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200211A RID: 8474
	public class SymbolResolver_AncientRuins : SymbolResolver
	{
		// Token: 0x0600D494 RID: 54420 RVA: 0x004E9F34 File Offset: 0x004E8134
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, true));
			resolveParams.chanceToSkipWallBlock = new float?(rp.chanceToSkipWallBlock.GetValueOrDefault(0.1f));
			resolveParams.clearEdificeOnly = new bool?(rp.clearEdificeOnly.GetValueOrDefault(true));
			resolveParams.noRoof = new bool?(rp.noRoof.GetValueOrDefault(true));
			BaseGen.symbolStack.Push("emptyRoom", resolveParams, null);
		}
	}
}
