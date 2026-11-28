using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002146 RID: 8518
	public class SymbolResolver_Interior_DiningRoom : SymbolResolver
	{
		// Token: 0x0600D518 RID: 54552 RVA: 0x004EFE6C File Offset: 0x004EE06C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp, null);
			BaseGen.symbolStack.Push("randomlyPlaceMealsOnTables", rp, null);
			BaseGen.symbolStack.Push("placeChairsNearTables", rp, null);
			int num = Mathf.Max(GenMath.RoundRandom((float)rp.rect.Area / 20f), 1);
			for (int i = 0; i < num; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.Table2x2c;
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
		}
	}
}
