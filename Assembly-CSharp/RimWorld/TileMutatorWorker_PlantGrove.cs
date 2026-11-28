using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001342 RID: 4930
	public class TileMutatorWorker_PlantGrove : TileMutatorWorker
	{
		// Token: 0x06007A0A RID: 31242 RVA: 0x002B7186 File Offset: 0x002B5386
		public TileMutatorWorker_PlantGrove(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A0B RID: 31243 RVA: 0x002B718F File Offset: 0x002B538F
		public override bool IsValidTile(PlanetTile tile, PlanetLayer layer)
		{
			return layer[tile].PrimaryBiome.AllWildPlants.Any(new Predicate<ThingDef>(this.def.plantKinds.Contains));
		}

		// Token: 0x06007A0C RID: 31244 RVA: 0x002B71BE File Offset: 0x002B53BE
		public override string GetLabel(PlanetTile tile)
		{
			return "PlantGrove".Translate(this.GetPlantKind(tile).label);
		}

		// Token: 0x06007A0D RID: 31245 RVA: 0x002B71E0 File Offset: 0x002B53E0
		public override string GetDescription(PlanetTile tile)
		{
			return this.def.description.Formatted(this.GetPlantKind(tile).label);
		}

		// Token: 0x06007A0E RID: 31246 RVA: 0x002B7208 File Offset: 0x002B5408
		public override float PlantCommonalityFactorFor(ThingDef plant, PlanetTile tile)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 1f;
			}
			if (plant != this.GetPlantKind(tile))
			{
				return 1f;
			}
			return 10f;
		}

		// Token: 0x06007A0F RID: 31247 RVA: 0x002B722C File Offset: 0x002B542C
		private ThingDef GetPlantKind(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			List<ThingDef> list;
			if (!TileMutatorWorker_PlantGrove.cachedPlantKinds.TryGetValue(primaryBiome, out list))
			{
				list = new List<ThingDef>();
				foreach (ThingDef item in primaryBiome.AllWildPlants)
				{
					if (this.def.plantKinds.Contains(item))
					{
						list.Add(item);
					}
				}
				TileMutatorWorker_PlantGrove.cachedPlantKinds.Add(primaryBiome, list);
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			ThingDef result = list.RandomElement<ThingDef>();
			Rand.PopState();
			return result;
		}

		// Token: 0x04004D03 RID: 19715
		private static Dictionary<BiomeDef, List<ThingDef>> cachedPlantKinds = new Dictionary<BiomeDef, List<ThingDef>>();

		// Token: 0x04004D04 RID: 19716
		private const float PlantCommonalityFactor = 10f;
	}
}
