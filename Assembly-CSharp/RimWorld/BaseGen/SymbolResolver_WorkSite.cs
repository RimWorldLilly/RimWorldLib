using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200213F RID: 8511
	public class SymbolResolver_WorkSite : SymbolResolver
	{
		// Token: 0x0600D509 RID: 54537 RVA: 0x004EF885 File Offset: 0x004EDA85
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("storage", rp, null);
		}
	}
}
