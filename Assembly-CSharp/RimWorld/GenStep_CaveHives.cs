using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129D RID: 4765
	public class GenStep_CaveHives : GenStep
	{
		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x06007636 RID: 30262 RVA: 0x002994F3 File Offset: 0x002976F3
		public override int SeedPart
		{
			get
			{
				return 349641510;
			}
		}

		// Token: 0x06007637 RID: 30263 RVA: 0x002994FC File Offset: 0x002976FC
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!Find.Storyteller.difficulty.allowCaveHives)
			{
				return;
			}
			if (Faction.OfInsects == null)
			{
				return;
			}
			MapGenFloatGrid caves = MapGenerator.Caves;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			float num = 0.7f;
			int num2 = 0;
			this.rockCells.Clear();
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (elevation[intVec] > num)
				{
					this.rockCells.Add(intVec);
				}
				if (caves[intVec] > 0f)
				{
					num2++;
				}
			}
			List<IntVec3> list = (from c in map.AllCells
			where map.thingGrid.ThingsAt(c).Any((Thing thing) => thing.Faction != null)
			select c).ToList<IntVec3>();
			GenMorphology.Dilate(list, 50, map, null);
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>(list);
			int num3 = GenMath.RoundRandom((float)num2 / 1000f);
			GenMorphology.Erode(this.rockCells, 10, map, null);
			this.possibleSpawnCells.Clear();
			for (int i = 0; i < this.rockCells.Count; i++)
			{
				if (caves[this.rockCells[i]] > 0f && !hashSet.Contains(this.rockCells[i]))
				{
					this.possibleSpawnCells.Add(this.rockCells[i]);
				}
			}
			this.spawnedHives.Clear();
			for (int j = 0; j < num3; j++)
			{
				this.TrySpawnHive(map);
			}
			this.spawnedHives.Clear();
		}

		// Token: 0x06007638 RID: 30264 RVA: 0x002996BC File Offset: 0x002978BC
		private void TrySpawnHive(Map map)
		{
			IntVec3 intVec;
			if (!this.TryFindHiveSpawnCell(map, out intVec))
			{
				return;
			}
			this.possibleSpawnCells.Remove(intVec);
			Hive item = HiveUtility.SpawnHive(intVec, map, WipeMode.VanishOrMoveAside, true, false, false, false, false, false, true);
			this.spawnedHives.Add(item);
		}

		// Token: 0x06007639 RID: 30265 RVA: 0x00299700 File Offset: 0x00297900
		private bool TryFindHiveSpawnCell(Map map, out IntVec3 spawnCell)
		{
			float num = -1f;
			IntVec3 intVec = IntVec3.Invalid;
			Func<IntVec3, bool> <>9__0;
			for (int i = 0; i < 3; i++)
			{
				IEnumerable<IntVec3> source = this.possibleSpawnCells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((IntVec3 x) => x.Standable(map) && x.GetFirstItem(map) == null && x.GetFirstBuilding(map) == null && x.GetFirstPawn(map) == null));
				}
				IntVec3 intVec2;
				if (!source.Where(predicate).TryRandomElement(out intVec2))
				{
					break;
				}
				float num2 = -1f;
				for (int j = 0; j < this.spawnedHives.Count; j++)
				{
					float num3 = (float)intVec2.DistanceToSquared(this.spawnedHives[j].Position);
					if (num2 < 0f || num3 < num2)
					{
						num2 = num3;
					}
				}
				if (!intVec.IsValid || num2 > num)
				{
					intVec = intVec2;
					num = num2;
				}
			}
			spawnCell = intVec;
			return spawnCell.IsValid;
		}

		// Token: 0x04004A0E RID: 18958
		private List<IntVec3> rockCells = new List<IntVec3>();

		// Token: 0x04004A0F RID: 18959
		private List<IntVec3> possibleSpawnCells = new List<IntVec3>();

		// Token: 0x04004A10 RID: 18960
		private List<Hive> spawnedHives = new List<Hive>();

		// Token: 0x04004A11 RID: 18961
		private const int MinDistToOpenSpace = 10;

		// Token: 0x04004A12 RID: 18962
		private const int MinDistFromFactionBase = 50;

		// Token: 0x04004A13 RID: 18963
		private const float CaveCellsPerHive = 1000f;
	}
}
