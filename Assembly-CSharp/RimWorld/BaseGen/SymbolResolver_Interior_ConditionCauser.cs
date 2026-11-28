using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02002145 RID: 8517
	public class SymbolResolver_Interior_ConditionCauser : SymbolResolver
	{
		// Token: 0x0600D516 RID: 54550 RVA: 0x004EFE34 File Offset: 0x004EE034
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.singleThingToSpawn = rp.conditionCauser;
			BaseGen.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
