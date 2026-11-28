using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001339 RID: 4921
	public class TileMutatorWorker_LavaLake : TileMutatorWorker_Lake
	{
		// Token: 0x170014CD RID: 5325
		// (get) Token: 0x060079E6 RID: 31206 RVA: 0x002B601A File Offset: 0x002B421A
		protected override float LakeRadius
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x060079E7 RID: 31207 RVA: 0x002B6021 File Offset: 0x002B4221
		public TileMutatorWorker_LavaLake(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079E8 RID: 31208 RVA: 0x002B602C File Offset: 0x002B422C
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.GetValAt(intVec, map) > 0.25f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x060079E9 RID: 31209 RVA: 0x002B6094 File Offset: 0x002B4294
		public override void GeneratePostTerrain(Map map)
		{
			base.GeneratePostTerrain(map);
			CellRect mapRect = CellRect.WholeMap(map);
			HashSet<IntVec3> visited = new HashSet<IntVec3>();
			HashSet<IntVec3> island = new HashSet<IntVec3>();
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			Predicate<IntVec3> <>9__0;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (!visited.Contains(intVec) && intVec.GetTerrain(map) != TerrainDefOf.LavaDeep)
				{
					island.Clear();
					bool touchesMapEdge = false;
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => x.GetTerrain(map) != TerrainDefOf.LavaDeep));
					}
					floodFiller.FloodFill(root, passCheck, delegate(IntVec3 x)
					{
						visited.Add(x);
						island.Add(x);
						if (mapRect.IsOnEdge(x))
						{
							touchesMapEdge = true;
						}
					}, int.MaxValue, false, null);
					if (!touchesMapEdge || island.Count < hashSet.Count)
					{
						using (HashSet<IntVec3>.Enumerator enumerator2 = island.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								IntVec3 c = enumerator2.Current;
								map.terrainGrid.SetTerrain(c, TerrainDefOf.LavaDeep);
							}
							continue;
						}
					}
					if (island.Count > hashSet.Count)
					{
						foreach (IntVec3 c2 in hashSet)
						{
							map.terrainGrid.SetTerrain(c2, TerrainDefOf.LavaDeep);
						}
						hashSet.Clear();
						hashSet.AddRange(island);
					}
				}
			}
		}

		// Token: 0x060079EA RID: 31210 RVA: 0x002B6314 File Offset: 0x002B4514
		protected override void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			if (valAt > 0.4f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.LavaDeep);
				return;
			}
			if (valAt > 0.25f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.VolcanicRock);
			}
		}

		// Token: 0x04004CE9 RID: 19689
		private const float LavaThreshold = 0.4f;

		// Token: 0x04004CEA RID: 19690
		private const float VolcanicRockThreshold = 0.25f;
	}
}
