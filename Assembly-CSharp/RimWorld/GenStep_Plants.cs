using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B5 RID: 4789
	public class GenStep_Plants : GenStep
	{
		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x060076D4 RID: 30420 RVA: 0x0029DE2E File Offset: 0x0029C02E
		public override int SeedPart
		{
			get
			{
				return 578415222;
			}
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x0029DE38 File Offset: 0x0029C038
		public override void Generate(Map map, GenStepParams parms)
		{
			float currentPlantDensityFactor = map.wildPlantSpawner.CurrentPlantDensityFactor;
			float currentWholeMapNumDesiredPlants = map.wildPlantSpawner.CurrentWholeMapNumDesiredPlants;
			foreach (IntVec3 c in map.cellsInRandomOrder.GetAll())
			{
				if (!Rand.Chance(0.001f))
				{
					map.wildPlantSpawner.CheckSpawnWildPlantAt(c, currentPlantDensityFactor, currentWholeMapNumDesiredPlants, true);
				}
			}
		}

		// Token: 0x04004A98 RID: 19096
		private const float ChanceToSkip = 0.001f;
	}
}
