using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002143 RID: 8515
	public class SymbolResolver_Interior_BatteryRoom : SymbolResolver
	{
		// Token: 0x0600D511 RID: 54545 RVA: 0x004EFC84 File Offset: 0x004EDE84
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp, null);
			BaseGen.symbolStack.Push("chargeBatteries", rp, null);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Battery;
			resolveParams.thingRot = new Rot4?(Rand.Bool ? Rot4.North : Rot4.East);
			resolveParams.fillWithThingsPadding = new int?(rp.fillWithThingsPadding.GetValueOrDefault(1));
			BaseGen.symbolStack.Push("fillWithThings", resolveParams, null);
		}
	}
}
