using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001317 RID: 4887
	public class TileMutatorWorker_ArcheanTrees : TileMutatorWorker
	{
		// Token: 0x0600793C RID: 31036 RVA: 0x002B14A8 File Offset: 0x002AF6A8
		public TileMutatorWorker_ArcheanTrees(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600793D RID: 31037 RVA: 0x002B14B4 File Offset: 0x002AF6B4
		public override void GenerateCriticalStructures(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.radius = ThingDefOf.Plant_TreeArchean.GetCompProperties<CompProperties_Terraformer>().radius;
			int randomInRange = TileMutatorWorker_ArcheanTrees.TreeRange.RandomInRange;
			int num = 0;
			foreach (IntVec3 intVec in map.AllCells.InRandomOrder(null))
			{
				if (this.ValidPosition(intVec, map))
				{
					this.SpawnTree(intVec, map);
					if (++num >= randomInRange)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600793E RID: 31038 RVA: 0x002B154C File Offset: 0x002AF74C
		private void SpawnTree(IntVec3 cell, Map map)
		{
			WildPlantSpawner.SpawnPlant(ThingDefOf.Plant_TreeArchean, map, cell, false).Growth = 1f;
			int num = GenRadial.NumCellsInRadius(this.radius + 1.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + GenRadial.RadialPattern[i];
				float magnitude = (intVec - cell).Magnitude;
				if (intVec.InBounds(map) && CompTerraformer.CanEverConvertCell(intVec, map, null) && (magnitude <= this.radius || Rand.Chance(0.33f)))
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.SoilRich);
				}
			}
			MapGenerator.UsedRects.Add(cell.RectAbout((int)this.radius / 2, (int)this.radius / 2));
		}

		// Token: 0x0600793F RID: 31039 RVA: 0x002B160C File Offset: 0x002AF80C
		private bool ValidPosition(IntVec3 center, Map map)
		{
			if (!CompTerraformer.CanEverConvertCell(center, map, null))
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(this.radius);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (CompTerraformer.CanEverConvertCell(center + GenRadial.RadialPattern[i], map, null))
				{
					num2++;
				}
				if (num2 >= 5)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004C0F RID: 19471
		private float radius;

		// Token: 0x04004C10 RID: 19472
		private const int MinCellsAround = 5;

		// Token: 0x04004C11 RID: 19473
		private const float ExtraRange = 1.9f;

		// Token: 0x04004C12 RID: 19474
		private const float ExtraChance = 0.33f;

		// Token: 0x04004C13 RID: 19475
		private static readonly IntRange TreeRange = new IntRange(10, 15);
	}
}
