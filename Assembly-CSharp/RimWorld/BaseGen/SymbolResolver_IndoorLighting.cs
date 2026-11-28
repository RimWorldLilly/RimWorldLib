using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200212A RID: 8490
	public class SymbolResolver_IndoorLighting : SymbolResolver
	{
		// Token: 0x0600D4C3 RID: 54467 RVA: 0x004EC5D4 File Offset: 0x004EA7D4
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
			{
				thingDef = ThingDefOf.StandingLamp;
			}
			else if (map.mapTemperature.OutdoorTemp > 18f)
			{
				thingDef = null;
			}
			else
			{
				thingDef = ThingDefOf.TorchLamp;
			}
			if (thingDef != null)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = thingDef;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
		}

		// Token: 0x040086F8 RID: 34552
		private const float NeverSpawnTorchesIfTemperatureAbove = 18f;
	}
}
