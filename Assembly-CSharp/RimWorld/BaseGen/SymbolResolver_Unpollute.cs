using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002116 RID: 8470
	public class SymbolResolver_Unpollute : SymbolResolver
	{
		// Token: 0x0600D488 RID: 54408 RVA: 0x004E92F4 File Offset: 0x004E74F4
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			foreach (IntVec3 c in rp.rect)
			{
				if (c.InBounds(map) && c.CanUnpollute(map) && (!rp.rect.IsOnEdge(c) || Rand.Chance(rp.edgeUnpolluteChance)))
				{
					c.Unpollute(map);
				}
			}
		}
	}
}
