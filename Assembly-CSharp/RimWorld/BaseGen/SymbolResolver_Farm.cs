using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002125 RID: 8485
	public class SymbolResolver_Farm : SymbolResolver
	{
		// Token: 0x0600D4B4 RID: 54452 RVA: 0x004EBE6C File Offset: 0x004EA06C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x0600D4B5 RID: 54453 RVA: 0x004EBE94 File Offset: 0x004EA094
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.cultivatedPlantDef ?? SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect);
			if (rp.rect.Width >= 7 && rp.rect.Height >= 7 && (rp.rect.Width > 12 || rp.rect.Height > 12 || Rand.Bool) && thingDef.plant.Harvestable)
			{
				CellRect rect = new CellRect(rp.rect.maxX - 3, rp.rect.maxZ - 3, 4, 4);
				ThingDef harvestedThingDef = thingDef.plant.harvestedThingDef;
				int num = Rand.RangeInclusive(2, 3);
				for (int i = 0; i < num; i++)
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = rect.ContractedBy(1);
					resolveParams.singleThingDef = harvestedThingDef;
					resolveParams.singleThingStackCount = new int?(Rand.RangeInclusive(Mathf.Min(10, harvestedThingDef.stackLimit), Mathf.Min(50, harvestedThingDef.stackLimit)));
					BaseGen.symbolStack.Push("thing", resolveParams, null);
				}
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rect;
				BaseGen.symbolStack.Push("ensureCanReachMapEdge", resolveParams2, null);
				ResolveParams resolveParams3 = rp;
				resolveParams3.rect = rect;
				BaseGen.symbolStack.Push("emptyRoom", resolveParams3, null);
			}
			ResolveParams resolveParams4 = rp;
			resolveParams4.cultivatedPlantDef = thingDef;
			BaseGen.symbolStack.Push("cultivatedPlants", resolveParams4, null);
		}
	}
}
