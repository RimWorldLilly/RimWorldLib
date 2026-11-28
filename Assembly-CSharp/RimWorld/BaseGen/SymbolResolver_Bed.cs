using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020F6 RID: 8438
	public class SymbolResolver_Bed : SymbolResolver
	{
		// Token: 0x0600D415 RID: 54293 RVA: 0x004E552C File Offset: 0x004E372C
		public override void Resolve(ResolveParams rp)
		{
			ThingDef singleThingDef = rp.singleThingDef ?? Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = singleThingDef;
			resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit = new bool?(rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit.GetValueOrDefault(true));
			BaseGen.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
