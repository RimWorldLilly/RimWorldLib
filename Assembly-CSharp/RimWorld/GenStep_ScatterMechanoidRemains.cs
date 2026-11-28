using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C6 RID: 4806
	[Obsolete]
	public class GenStep_ScatterMechanoidRemains : GenStep_Scatterer
	{
		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x06007741 RID: 30529 RVA: 0x002A10C5 File Offset: 0x0029F2C5
		public override int SeedPart
		{
			get
			{
				return 344678634;
			}
		}

		// Token: 0x06007742 RID: 30530 RVA: 0x002A10CC File Offset: 0x0029F2CC
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter mechanoid remains"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			base.Generate(map, parms);
		}

		// Token: 0x06007743 RID: 30531 RVA: 0x002A10F1 File Offset: 0x0029F2F1
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			return base.CanScatterAt(loc, map) && this.CanPlaceDebrisAt(loc, map);
		}

		// Token: 0x06007744 RID: 30532 RVA: 0x002A110C File Offset: 0x0029F30C
		private bool CanPlaceDebrisAt(IntVec3 loc, Map map)
		{
			return loc.InBounds(map) && !loc.Roofed(map) && loc.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy) && !map.terrainGrid.TerrainAt(loc).IsWater;
		}

		// Token: 0x06007745 RID: 30533 RVA: 0x002A114C File Offset: 0x0029F34C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			foreach (IntVec3 c in GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientMechDropBeacon, null), loc, map, Rot4.North, WipeMode.Vanish, false, false).OccupiedRect())
			{
				if (Rand.Bool && c.InBounds(map) && !c.Impassable(map))
				{
					FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_MachineBits, 1, FilthSourceFlags.None, true);
				}
			}
			FilthMaker.TryMakeFilth(loc, map, ThingDefOf.Filth_RubbleBuilding, 1, FilthSourceFlags.None, true);
			CellRect cellRect = CellRect.CenteredOn(loc, 10);
			int randomInRange = GenStep_ScatterMechanoidRemains.MaxShellsRange.RandomInRange;
			int num = 0;
			foreach (IntVec3 intVec in cellRect.InRandomOrder(null))
			{
				if (GenSight.LineOfSight(intVec, loc, map, true, null, 0, 0))
				{
					ThingDef chunkMechanoidSlag = ThingDefOf.ChunkMechanoidSlag;
					Rot4 rot = chunkMechanoidSlag.rotatable ? Rot4.Random : Rot4.North;
					bool flag = true;
					foreach (IntVec3 loc2 in GenAdj.OccupiedRect(intVec, rot, chunkMechanoidSlag.size))
					{
						if (!this.CanPlaceDebrisAt(loc2, map))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(chunkMechanoidSlag, null), intVec, map, rot, WipeMode.Vanish, false, false);
						FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Ash, 1, FilthSourceFlags.None, true);
						num++;
						if (num >= randomInRange)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x04004ACF RID: 19151
		private const int ShellScatterRadius = 10;

		// Token: 0x04004AD0 RID: 19152
		private static readonly IntRange MaxShellsRange = new IntRange(1, 2);
	}
}
