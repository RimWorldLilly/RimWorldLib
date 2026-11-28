using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002144 RID: 8516
	public class SymbolResolver_Interior_Brewery : SymbolResolver
	{
		// Token: 0x170025F1 RID: 9713
		// (get) Token: 0x0600D513 RID: 54547 RVA: 0x004EFD12 File Offset: 0x004EDF12
		private float SpawnPassiveCoolerIfTemperatureAbove
		{
			get
			{
				return ThingDefOf.FermentingBarrel.GetCompProperties<CompProperties_TemperatureRuinable>().maxSafeTemperature;
			}
		}

		// Token: 0x0600D514 RID: 54548 RVA: 0x004EFD24 File Offset: 0x004EDF24
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp > this.SpawnPassiveCoolerIfTemperatureAbove)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
			if (map.mapTemperature.OutdoorTemp < 7f)
			{
				ThingDef singleThingDef;
				if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
				{
					singleThingDef = ThingDefOf.Heater;
				}
				else
				{
					singleThingDef = ThingDefOf.Campfire;
				}
				ResolveParams resolveParams2 = rp;
				resolveParams2.singleThingDef = singleThingDef;
				BaseGen.symbolStack.Push("edgeThing", resolveParams2, null);
			}
			BaseGen.symbolStack.Push("addWortToFermentingBarrels", rp, null);
			ResolveParams resolveParams3 = rp;
			resolveParams3.singleThingDef = ThingDefOf.FermentingBarrel;
			resolveParams3.thingRot = new Rot4?(Rand.Bool ? Rot4.North : Rot4.East);
			resolveParams3.fillWithThingsPadding = new int?(rp.fillWithThingsPadding.GetValueOrDefault(1));
			BaseGen.symbolStack.Push("fillWithThings", resolveParams3, null);
		}

		// Token: 0x04008720 RID: 34592
		private const float SpawnHeaterIfTemperatureBelow = 7f;
	}
}
