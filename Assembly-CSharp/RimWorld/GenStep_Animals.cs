using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001293 RID: 4755
	public class GenStep_Animals : GenStep
	{
		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x060075F4 RID: 30196 RVA: 0x00297C92 File Offset: 0x00295E92
		public override int SeedPart
		{
			get
			{
				return 1298760307;
			}
		}

		// Token: 0x060075F5 RID: 30197 RVA: 0x00297C9C File Offset: 0x00295E9C
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			while (!map.wildAnimalSpawner.AnimalEcosystemFull)
			{
				num++;
				if (num >= 10000)
				{
					Log.Error("Too many iterations.");
					return;
				}
				IntVec3 loc = RCellFinder.RandomAnimalSpawnCell_MapGen(map);
				if (!map.wildAnimalSpawner.SpawnRandomWildAnimalAt(loc, false, null))
				{
					break;
				}
			}
		}
	}
}
