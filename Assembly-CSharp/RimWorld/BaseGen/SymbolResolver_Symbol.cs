using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002111 RID: 8465
	public class SymbolResolver_Symbol : SymbolResolver
	{
		// Token: 0x0600D47C RID: 54396 RVA: 0x004E9040 File Offset: 0x004E7240
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RuleDef ruleDef = allDefsListForReading[i];
				if (!(ruleDef.symbol != this.symbol))
				{
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						if (ruleDef.resolvers[j].CanResolve(rp))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D47D RID: 54397 RVA: 0x004E90B7 File Offset: 0x004E72B7
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp, null);
		}

		// Token: 0x040086D6 RID: 34518
		public string symbol;
	}
}
