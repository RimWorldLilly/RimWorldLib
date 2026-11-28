using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02002142 RID: 8514
	public class SymbolResolver_Interior_Barracks : SymbolResolver
	{
		// Token: 0x0600D50F RID: 54543 RVA: 0x004EFC62 File Offset: 0x004EDE62
		public override void Resolve(ResolveParams rp)
		{
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, true);
			BaseGen.symbolStack.Push("fillWithBeds", rp, null);
		}
	}
}
