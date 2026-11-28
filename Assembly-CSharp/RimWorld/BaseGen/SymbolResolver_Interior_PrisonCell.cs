using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02002147 RID: 8519
	public class SymbolResolver_Interior_PrisonCell : SymbolResolver
	{
		// Token: 0x0600D51A RID: 54554 RVA: 0x004EFF00 File Offset: 0x004EE100
		public override void Resolve(ResolveParams rp)
		{
			ThingSetMakerParams value = default(ThingSetMakerParams);
			value.techLevel = new TechLevel?((rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Spacer);
			ResolveParams resolveParams = rp;
			resolveParams.thingSetMakerDef = ThingSetMakerDefOf.MapGen_PrisonCellStockpile;
			resolveParams.thingSetMakerParams = new ThingSetMakerParams?(value);
			resolveParams.innerStockpileSize = new int?(3);
			BaseGen.symbolStack.Push("innerStockpile", resolveParams, null);
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, false);
			BaseGen.symbolStack.Push("prisonerBed", rp, null);
		}

		// Token: 0x04008721 RID: 34593
		private const int FoodStockpileSize = 3;
	}
}
