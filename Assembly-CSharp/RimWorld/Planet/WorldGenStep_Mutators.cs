using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200234F RID: 9039
	public class WorldGenStep_Mutators : WorldGenStep
	{
		// Token: 0x1700274E RID: 10062
		// (get) Token: 0x0600DFC3 RID: 57283 RVA: 0x0053373E File Offset: 0x0053193E
		public override int SeedPart
		{
			get
			{
				return 897213645;
			}
		}

		// Token: 0x0600DFC4 RID: 57284 RVA: 0x00533748 File Offset: 0x00531948
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			using (ProfilerBlock.Scope("Generate Mutators"))
			{
				WorldGenStep_Mutators.AddMutatorsFromTile(layer);
			}
		}

		// Token: 0x0600DFC5 RID: 57285 RVA: 0x00533788 File Offset: 0x00531988
		public static void AddMutatorsFromTile(PlanetLayer layer)
		{
			List<TileMutatorDef> list = (from m in DefDatabase<TileMutatorDef>.AllDefsListForReading
			where m.chanceOnNonLandmarkTile > 0f
			select m).ToList<TileMutatorDef>();
			foreach (Tile tile in layer.Tiles)
			{
				PlanetTile tile2 = tile.tile;
				if (tile.hilliness == Hilliness.Mountainous || tile.hilliness == Hilliness.Impassable)
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Mountain);
				}
				if (ModsConfig.OdysseyActive && Find.World.LakeDirectionAt(tile2).IsValid)
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Lakeshore);
				}
				else if (Find.World.CoastDirectionAt(tile2).IsValid)
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Coast);
				}
				SurfaceTile surfaceTile = tile as SurfaceTile;
				if (surfaceTile != null && !surfaceTile.Rivers.NullOrEmpty<SurfaceTile.RiverLink>())
				{
					if (!ModsConfig.OdysseyActive)
					{
						WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.River);
					}
					else if ((!surfaceTile.IsCoastal || surfaceTile.Rivers.Count <= 2 || !WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.RiverDelta)) && (surfaceTile.Rivers.Count != 1 || !WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Headwater)) && (surfaceTile.Rivers.Count <= 2 || !WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.RiverConfluence)) && (!Rand.Chance(0.05f) || !WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.RiverIsland)))
					{
						WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.River);
					}
				}
				if ((tile.hilliness == Hilliness.Mountainous && Rand.Chance(0.5f)) || (tile.hilliness == Hilliness.LargeHills && Rand.Chance(0.25f)))
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Caves);
				}
				if (tile.Landmark == null)
				{
					foreach (TileMutatorDef tileMutatorDef in list)
					{
						if (Rand.Chance(tileMutatorDef.chanceOnNonLandmarkTile))
						{
							WorldGenStep_Mutators.TryAddMutator(tile, layer, tileMutatorDef);
						}
					}
				}
				if (ModsConfig.OdysseyActive && Rand.Chance(0.2f))
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.MixedBiome);
				}
			}
		}

		// Token: 0x0600DFC6 RID: 57286 RVA: 0x00533A08 File Offset: 0x00531C08
		private static bool TryAddMutator(Tile tile, PlanetLayer layer, TileMutatorDef mutator)
		{
			if (tile.Mutators.Contains(mutator))
			{
				return false;
			}
			if (!mutator.IsValidTile(tile.tile, layer))
			{
				return false;
			}
			try
			{
				tile.AddMutator(mutator);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Error adding mutator ";
				array[1] = ((mutator != null) ? mutator.ToString() : null);
				array[2] = " to tile ";
				array[3] = ((tile != null) ? tile.ToString() : null);
				array[4] = ": ";
				int num = 5;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
				return false;
			}
			return true;
		}

		// Token: 0x04008F95 RID: 36757
		private const float MixedBiomeChance = 0.2f;

		// Token: 0x04008F96 RID: 36758
		private const float CaveChanceMountainous = 0.5f;

		// Token: 0x04008F97 RID: 36759
		private const float CaveChanceLargeHills = 0.25f;

		// Token: 0x04008F98 RID: 36760
		private const float RiverIslandChance = 0.05f;
	}
}
