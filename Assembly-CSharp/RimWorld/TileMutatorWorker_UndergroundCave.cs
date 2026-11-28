using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200134C RID: 4940
	public class TileMutatorWorker_UndergroundCave : TileMutatorWorker_Caves
	{
		// Token: 0x170014DA RID: 5338
		// (get) Token: 0x06007A52 RID: 31314 RVA: 0x002B98C4 File Offset: 0x002B7AC4
		protected override float BranchChance
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x06007A53 RID: 31315 RVA: 0x002B98CB File Offset: 0x002B7ACB
		protected override float WidthOffsetPerCell
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x170014DC RID: 5340
		// (get) Token: 0x06007A54 RID: 31316 RVA: 0x002B98D2 File Offset: 0x002B7AD2
		protected override float MinTunnelWidth
		{
			get
			{
				return 1.6f;
			}
		}

		// Token: 0x06007A55 RID: 31317 RVA: 0x002B98D9 File Offset: 0x002B7AD9
		public TileMutatorWorker_UndergroundCave(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A56 RID: 31318 RVA: 0x002B98E4 File Offset: 0x002B7AE4
		public override void GeneratePostElevationFertility(Map map)
		{
			TileMutatorWorker_UndergroundCave.<>c__DisplayClass11_0 CS$<>8__locals1 = new TileMutatorWorker_UndergroundCave.<>c__DisplayClass11_0();
			CS$<>8__locals1.map = map;
			base.GeneratePostElevationFertility(CS$<>8__locals1.map);
			this.directionNoise = new Perlin(0.002050000010058284, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			List<IntVec3> list = CS$<>8__locals1.map.AllCells.ToList<IntVec3>();
			int num = 0;
			while (hashSet.Count < 2000)
			{
				num++;
				hashSet.Clear();
				MapGenerator.Caves.Clear();
				IntVec3 start = list.RandomElement<IntVec3>();
				float width = Rand.Range(3.2f, 4f);
				MapGenCavesUtility.CaveGenParms @default = MapGenCavesUtility.CaveGenParms.Default;
				@default.widthOffsetPerCell = this.WidthOffsetPerCell;
				@default.minTunnelWidth = this.MinTunnelWidth;
				@default.branchChance = this.BranchChance;
				@default.allowBranchingAfterThisManyCells = 10;
				MapGenCavesUtility.Dig(start, Rand.Range(0f, 360f), width, list, CS$<>8__locals1.map, true, this.directionNoise, @default, new Predicate<IntVec3>(CS$<>8__locals1.<GeneratePostElevationFertility>g__Rock|0), hashSet);
				if (num > 100)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Underground cave generation exceeded ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(100);
					defaultInterpolatedStringHandler.AppendLiteral(" tries");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					return;
				}
			}
		}

		// Token: 0x04004D44 RID: 19780
		private const int MinCaveSize = 2000;

		// Token: 0x04004D45 RID: 19781
		private const int MaxTries = 100;

		// Token: 0x04004D46 RID: 19782
		private const float BaseWidth = 4f;

		// Token: 0x04004D47 RID: 19783
		private const int AllowBranchingAfterThisManyCells = 10;
	}
}
