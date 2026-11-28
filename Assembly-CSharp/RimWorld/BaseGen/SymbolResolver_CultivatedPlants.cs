using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020F8 RID: 8440
	public class SymbolResolver_CultivatedPlants : SymbolResolver
	{
		// Token: 0x0600D41A RID: 54298 RVA: 0x004E5788 File Offset: 0x004E3988
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x0600D41B RID: 54299 RVA: 0x004E57B0 File Offset: 0x004E39B0
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef = rp.cultivatedPlantDef ?? SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect);
			if (thingDef == null)
			{
				return;
			}
			float growth = rp.fixedCulativedPlantGrowth ?? Rand.Range(0.2f, 1f);
			int age = thingDef.plant.LimitedLifespan ? Rand.Range(0, Mathf.Max(thingDef.plant.LifespanTicks - 2500, 0)) : 0;
			List<Thing> list = new List<Thing>();
			using (CellRect.Enumerator enumerator = rp.rect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 c = enumerator.Current;
					if (c.InBounds(map) && !list.Any((Thing p) => p.def.plant.blockAdjacentSow && c.IsAdjacentToCardinalOrInside(p.OccupiedRect())))
					{
						float num = map.fertilityGrid.FertilityAt(c);
						if ((thingDef.plant.completelyIgnoreFertility || num >= thingDef.plant.fertilityMin) && this.TryDestroyBlockingThingsAt(c))
						{
							Plant plant = (Plant)GenSpawn.Spawn(thingDef, c, map, WipeMode.Vanish);
							list.Add(plant);
							plant.Growth = growth;
							if (plant.def.plant.LimitedLifespan)
							{
								plant.Age = age;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600D41C RID: 54300 RVA: 0x004E5944 File Offset: 0x004E3B44
		public static ThingDef DeterminePlantDef(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			float minFertility = float.MaxValue;
			bool flag = false;
			foreach (IntVec3 loc in rect)
			{
				float num = map.fertilityGrid.FertilityAt(loc);
				if (num > 0f)
				{
					flag = true;
					minFertility = Mathf.Min(minFertility, num);
				}
			}
			if (!flag)
			{
				return null;
			}
			ThingDef result;
			if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.Sowable && !x.plant.IsTree && !x.plant.cavePlant && !x.plant.diesToLight && (x.plant.completelyIgnoreFertility || x.plant.fertilityMin <= minFertility) && x.plant.minGrowthTemperature <= map.mapTemperature.OutdoorTemp && x.plant.maxGrowthTemperature >= map.mapTemperature.OutdoorTemp && x.plant.Harvestable
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600D41D RID: 54301 RVA: 0x004E5A0C File Offset: 0x004E3C0C
		private bool TryDestroyBlockingThingsAt(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			SymbolResolver_CultivatedPlants.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < SymbolResolver_CultivatedPlants.tmpThings.Count; i++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[i] is Pawn) && !SymbolResolver_CultivatedPlants.tmpThings[i].def.destroyable)
				{
					SymbolResolver_CultivatedPlants.tmpThings.Clear();
					return false;
				}
			}
			for (int j = 0; j < SymbolResolver_CultivatedPlants.tmpThings.Count; j++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[j] is Pawn))
				{
					SymbolResolver_CultivatedPlants.tmpThings[j].Destroy(DestroyMode.Vanish);
				}
			}
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			return true;
		}

		// Token: 0x040086C1 RID: 34497
		private const float MinPlantGrowth = 0.2f;

		// Token: 0x040086C2 RID: 34498
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
