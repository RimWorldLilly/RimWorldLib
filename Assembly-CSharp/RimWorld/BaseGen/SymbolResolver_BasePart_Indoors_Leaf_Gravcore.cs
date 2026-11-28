using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020E8 RID: 8424
	public class SymbolResolver_BasePart_Indoors_Leaf_Gravcore : SymbolResolver
	{
		// Token: 0x0600D3E3 RID: 54243 RVA: 0x004E3D24 File Offset: 0x004E1F24
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
						if (ruleDef.resolvers[j] is SymbolResolver_BasePart_Indoors_Leaf_Gravcore && ruleDef.resolvers[j].CanResolve(rp))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D3E4 RID: 54244 RVA: 0x004E3D9E File Offset: 0x004E1F9E
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_gravcoresResolved < BaseGen.globalSettings.requiredGravcoreRooms;
		}

		// Token: 0x0600D3E5 RID: 54245 RVA: 0x004E3DC4 File Offset: 0x004E1FC4
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("gravcore", rp, null);
			BaseGen.globalSettings.basePart_gravcoresResolved++;
		}
	}
}
