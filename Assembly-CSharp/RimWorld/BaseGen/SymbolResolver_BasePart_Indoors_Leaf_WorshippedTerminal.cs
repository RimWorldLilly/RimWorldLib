using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020E7 RID: 8423
	public class SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal : SymbolResolver
	{
		// Token: 0x0600D3DF RID: 54239 RVA: 0x004E3C54 File Offset: 0x004E1E54
		public static bool CanResolve(string symbol, ResolveParams rp)
		{
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RuleDef ruleDef = allDefsListForReading[i];
				if (!(ruleDef.symbol != symbol))
				{
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						if (ruleDef.resolvers[j] is SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal && ruleDef.resolvers[j].CanResolve(rp))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D3E0 RID: 54240 RVA: 0x004E3CCE File Offset: 0x004E1ECE
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_worshippedTerminalsResolved < BaseGen.globalSettings.requiredWorshippedTerminalRooms;
		}

		// Token: 0x0600D3E1 RID: 54241 RVA: 0x004E3CF4 File Offset: 0x004E1EF4
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("worshippedTerminal", rp, null);
			BaseGen.globalSettings.basePart_worshippedTerminalsResolved++;
		}
	}
}
