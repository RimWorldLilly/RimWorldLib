using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200131C RID: 4892
	public class TileMutatorWorker_Cavern : TileMutatorWorker_Caves
	{
		// Token: 0x170014B0 RID: 5296
		// (get) Token: 0x06007953 RID: 31059 RVA: 0x002B1FEE File Offset: 0x002B01EE
		protected override float BranchChance
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x06007954 RID: 31060 RVA: 0x002B1FF5 File Offset: 0x002B01F5
		protected override float WidthOffsetPerCell
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x06007955 RID: 31061 RVA: 0x002B1FFC File Offset: 0x002B01FC
		protected override float MinTunnelWidth
		{
			get
			{
				return 1.6f;
			}
		}

		// Token: 0x06007956 RID: 31062 RVA: 0x002B2003 File Offset: 0x002B0203
		public TileMutatorWorker_Cavern(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007957 RID: 31063 RVA: 0x002B200C File Offset: 0x002B020C
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.openingNoise = new DistFromPoint((float)map.Size.x * 0.4f);
			this.openingNoise = new Scale((double)TileMutatorWorker_Cavern.OpeningSquashRange.RandomInRange, 1.0, 1.0, this.openingNoise);
			this.openingNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.openingNoise);
			this.openingNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.openingNoise);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num2 = (num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			if (num2 != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num2.Value, 0.1f, map);
				this.openingNoise = new SmoothMin(this.openingNoise, rhs, 0.2);
			}
			this.openingNoise = MapNoiseUtility.AddDisplacementNoise(this.openingNoise, 0.015f, 40f, 4, -1);
			this.holesNoise = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			this.holesNoise = MapNoiseUtility.AddDisplacementNoise(this.holesNoise, 0.015f, 40f, 4, -1);
			base.Init(map);
		}

		// Token: 0x06007958 RID: 31064 RVA: 0x002B21B4 File Offset: 0x002B03B4
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.openingNoise.GetValue(intVec);
				if (value > 0.5f)
				{
					elevation[intVec] = value;
				}
			}
			this.CreateTunnels(map);
			foreach (IntVec3 intVec2 in map.AllCells)
			{
				if (this.holesNoise.GetValue(intVec2) > 0.7f)
				{
					caves[intVec2] = 1f;
				}
			}
			RoofCollapseCellsFinder.CheckAndRemoveCollpsingRoofs(map);
		}

		// Token: 0x06007959 RID: 31065 RVA: 0x002B228C File Offset: 0x002B048C
		private void CreateTunnels(Map map)
		{
			TileMutatorWorker_Cavern.<>c__DisplayClass22_0 CS$<>8__locals1 = new TileMutatorWorker_Cavern.<>c__DisplayClass22_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.elevation = MapGenerator.Elevation;
			List<IntVec3> group = this.FindLargestContiguousRockFormation(CS$<>8__locals1.map);
			int randomInRange = TileMutatorWorker_Cavern.NumTunnelsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 start = IntVec3.Invalid;
				float num = -1f;
				float dir = -1f;
				float num2 = -1f;
				for (int j = 0; j < 10; j++)
				{
					IntVec3 intVec = MapGenCavesUtility.FindRandomEdgeCellForTunnel(group, CS$<>8__locals1.map);
					float distToCave = MapGenCavesUtility.GetDistToCave(intVec, group, CS$<>8__locals1.map, 40f, false);
					float num4;
					float num3 = MapGenCavesUtility.FindBestInitialDir(intVec, group, out num4);
					if (!start.IsValid || distToCave > num || (Mathf.Approximately(distToCave, num) && num4 > num2))
					{
						start = intVec;
						num = distToCave;
						dir = num3;
						num2 = num4;
					}
				}
				float width = Rand.Range(5.6f, 7f);
				MapGenCavesUtility.CaveGenParms @default = MapGenCavesUtility.CaveGenParms.Default;
				@default.widthOffsetPerCell = this.WidthOffsetPerCell;
				@default.minTunnelWidth = this.MinTunnelWidth;
				@default.branchChance = this.BranchChance;
				@default.allowBranchingAfterThisManyCells = 10;
				@default.widthNoiseAmplitude = 2f;
				MapGenCavesUtility.Dig(start, dir, width, group, CS$<>8__locals1.map, false, this.directionNoise, @default, new Predicate<IntVec3>(CS$<>8__locals1.<CreateTunnels>g__Rock|0), null);
			}
		}

		// Token: 0x0600795A RID: 31066 RVA: 0x002B23EC File Offset: 0x002B05EC
		private List<IntVec3> FindLargestContiguousRockFormation(Map map)
		{
			HashSet<IntVec3> checkedCells = new HashSet<IntVec3>();
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			HashSet<IntVec3> current = new HashSet<IntVec3>();
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			Predicate<IntVec3> <>9__0;
			Action<IntVec3> <>9__1;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (TileMutatorWorker_Caves.IsRock(intVec, elevation, map) && !checkedCells.Contains(intVec))
				{
					current.Clear();
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => TileMutatorWorker_Caves.IsRock(x, elevation, map)));
					}
					Action<IntVec3> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(IntVec3 x)
						{
							current.Add(x);
							checkedCells.Add(x);
						});
					}
					floodFiller.FloodFill(root, passCheck, processor, int.MaxValue, false, null);
					if (current.Count > hashSet.Count)
					{
						hashSet.Clear();
						hashSet.AddRange(current);
					}
				}
			}
			return hashSet.ToList<IntVec3>();
		}

		// Token: 0x04004C2F RID: 19503
		private const float OpeningNoiseFreq = 0.015f;

		// Token: 0x04004C30 RID: 19504
		private const float OpeningNoiseStrength = 40f;

		// Token: 0x04004C31 RID: 19505
		private const float OpeningRadius = 0.4f;

		// Token: 0x04004C32 RID: 19506
		private const float OpeningThreshold = 0.5f;

		// Token: 0x04004C33 RID: 19507
		private static readonly FloatRange OpeningSquashRange = new FloatRange(1f, 1.3f);

		// Token: 0x04004C34 RID: 19508
		private const float CoastFalloff = 0.1f;

		// Token: 0x04004C35 RID: 19509
		private const float HolesFrequency = 0.02f;

		// Token: 0x04004C36 RID: 19510
		private const float HolesThreshold = 0.7f;

		// Token: 0x04004C37 RID: 19511
		private const float BaseWidth = 7f;

		// Token: 0x04004C38 RID: 19512
		private const int AllowBranchingAfterThisManyCells = 10;

		// Token: 0x04004C39 RID: 19513
		private static readonly IntRange NumTunnelsRange = new IntRange(4, 6);

		// Token: 0x04004C3A RID: 19514
		private ModuleBase openingNoise;

		// Token: 0x04004C3B RID: 19515
		private ModuleBase holesNoise;
	}
}
