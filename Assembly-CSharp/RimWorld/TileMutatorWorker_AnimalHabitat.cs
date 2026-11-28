using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001316 RID: 4886
	public class TileMutatorWorker_AnimalHabitat : TileMutatorWorker
	{
		// Token: 0x06007937 RID: 31031 RVA: 0x002B12CF File Offset: 0x002AF4CF
		public TileMutatorWorker_AnimalHabitat(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007938 RID: 31032 RVA: 0x002B12E4 File Offset: 0x002AF4E4
		public override string GetLabel(PlanetTile tile)
		{
			PawnKindDef animalKind = this.GetAnimalKind(tile);
			return "AnimalHabitat".Translate(animalKind.Named("ANIMALKIND"));
		}

		// Token: 0x06007939 RID: 31033 RVA: 0x002B1314 File Offset: 0x002AF514
		public override string GetDescription(PlanetTile tile)
		{
			PawnKindDef animalKind = this.GetAnimalKind(tile);
			return this.def.description.Formatted(animalKind.Named("ANIMALKIND"));
		}

		// Token: 0x0600793A RID: 31034 RVA: 0x002B1349 File Offset: 0x002AF549
		public override float AnimalCommonalityFactorFor(PawnKindDef animal, PlanetTile tile)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 1f;
			}
			if (animal != this.GetAnimalKind(tile))
			{
				return 1f;
			}
			return 10f;
		}

		// Token: 0x0600793B RID: 31035 RVA: 0x002B1370 File Offset: 0x002AF570
		public PawnKindDef GetAnimalKind(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			foreach (PawnKindDef pawnKindDef in primaryBiome.AllWildAnimals)
			{
				if (primaryBiome.CommonalityOfAnimal(pawnKindDef) > 0.1f)
				{
					this.tmpAnimals.Add(pawnKindDef);
				}
				else if (tile.Tile.IsCoastal && primaryBiome.CommonalityOfCoastalAnimal(pawnKindDef) > 0.1f)
				{
					this.tmpAnimals.Add(pawnKindDef);
				}
			}
			if (this.tmpAnimals.Empty<PawnKindDef>())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(190, 2);
				defaultInterpolatedStringHandler.AppendLiteral("TileMutatorWorker_AnimalHabitat: Could not find animal in biome with commonality > ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(0.1f);
				defaultInterpolatedStringHandler.AppendLiteral(", either increase some commonalities, or decrease animalDensity in the biomeDef below this mutators min of ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(this.def.animalDensityRange.min);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 123498);
				return null;
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			PawnKindDef result = this.tmpAnimals.RandomElement<PawnKindDef>();
			Rand.PopState();
			this.tmpAnimals.Clear();
			return result;
		}

		// Token: 0x04004C0C RID: 19468
		private const float AnimalCommonalityFactor = 10f;

		// Token: 0x04004C0D RID: 19469
		private const float MinAnimalCommonality = 0.1f;

		// Token: 0x04004C0E RID: 19470
		private readonly List<PawnKindDef> tmpAnimals = new List<PawnKindDef>();
	}
}
