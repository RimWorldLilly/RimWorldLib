using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002348 RID: 9032
	public static class WorldGenerator
	{
		// Token: 0x17002747 RID: 10055
		// (get) Token: 0x0600DFA4 RID: 57252 RVA: 0x005325E4 File Offset: 0x005307E4
		private static List<GameSetupStepDef> GameSetupStepsInOrder
		{
			get
			{
				List<GameSetupStepDef> result;
				if ((result = WorldGenerator.cachedGenSteps) == null)
				{
					result = (WorldGenerator.cachedGenSteps = (from x in DefDatabase<GameSetupStepDef>.AllDefs
					orderby x.order, x.index
					select x).ToList<GameSetupStepDef>());
				}
				return result;
			}
		}

		// Token: 0x0600DFA5 RID: 57253 RVA: 0x00532654 File Offset: 0x00530854
		public static World GenerateWorld(float planetCoverage, string seedString, OverallRainfall overallRainfall, OverallTemperature overallTemperature, OverallPopulation population, LandmarkDensity landmarkDensity, List<FactionDef> factions = null, float pollution = 0f)
		{
			DeepProfiler.Start("GenerateWorld");
			Rand.PushState();
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			Rand.Seed = seedFromSeedString;
			World creatingWorld;
			try
			{
				Current.CreatingWorld = new World
				{
					info = 
					{
						seedString = seedString,
						planetCoverage = planetCoverage,
						overallRainfall = overallRainfall,
						overallTemperature = overallTemperature,
						overallPopulation = population,
						landmarkDensity = landmarkDensity,
						name = NameGenerator.GenerateName(RulePackDefOf.NamerWorld, null, false, null, null, null),
						factions = factions,
						pollution = pollution
					}
				};
				foreach (GameSetupStepDef gameSetupStepDef in WorldGenerator.GameSetupStepsInOrder)
				{
					Rand.Seed = Gen.HashCombineInt(seedFromSeedString, gameSetupStepDef.setupStep.SeedPart);
					gameSetupStepDef.setupStep.GenerateFresh();
				}
				foreach (KeyValuePair<int, PlanetLayer> keyValuePair in from x in Find.WorldGrid.PlanetLayers
				orderby x.Key
				select x)
				{
					int num;
					PlanetLayer planetLayer;
					keyValuePair.Deconstruct(out num, out planetLayer);
					PlanetLayer planetLayer2 = planetLayer;
					WorldGenerator.GeneratePlanetLayer(planetLayer2, seedString, seedFromSeedString);
					if (planetLayer2.Tiles.Count == 0)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(112, 1);
						defaultInterpolatedStringHandler.AppendLiteral("No tiles on layer ");
						defaultInterpolatedStringHandler.AppendFormatted<PlanetLayer>(planetLayer2);
						defaultInterpolatedStringHandler.AppendLiteral(", layer should have a world gen step worker which initializes tiles such as WorldGenStep_Tiles");
						Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				Rand.Seed = seedFromSeedString;
				Current.CreatingWorld.grid.StandardizeTileData();
				Current.CreatingWorld.FinalizeInit(false);
				Find.Scenario.PostWorldGenerate();
				if (!ModsConfig.IdeologyActive)
				{
					Find.Scenario.PostIdeoChosen();
				}
				creatingWorld = Current.CreatingWorld;
			}
			finally
			{
				Rand.PopState();
				DeepProfiler.End();
				Current.CreatingWorld = null;
			}
			return creatingWorld;
		}

		// Token: 0x0600DFA6 RID: 57254 RVA: 0x005328A8 File Offset: 0x00530AA8
		public static void GeneratePlanetLayer(PlanetLayer layer, string seedString, int seed)
		{
			List<WorldGenStepDef> genStepsInOrder = layer.Def.GenStepsInOrder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
			defaultInterpolatedStringHandler.AppendLiteral("WorldGen - ");
			defaultInterpolatedStringHandler.AppendFormatted<PlanetLayer>(layer);
			DeepProfiler.Start(defaultInterpolatedStringHandler.ToStringAndClear());
			for (int i = 0; i < genStepsInOrder.Count; i++)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("WorldGenStep - ");
				defaultInterpolatedStringHandler2.AppendFormatted<WorldGenStepDef>(genStepsInOrder[i]);
				DeepProfiler.Start(defaultInterpolatedStringHandler2.ToStringAndClear());
				try
				{
					Rand.Seed = Gen.HashCombineInt(seed, WorldGenerator.GetSeedPart(genStepsInOrder, i));
					genStepsInOrder[i].worldGenStep.GenerateFresh(seedString, layer);
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(23, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("Error in WorldGenStep: ");
					defaultInterpolatedStringHandler3.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				finally
				{
					DeepProfiler.End();
				}
			}
		}

		// Token: 0x0600DFA7 RID: 57255 RVA: 0x005329AC File Offset: 0x00530BAC
		public static void GenerateWithoutWorldData(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			Rand.PushState();
			foreach (GameSetupStepDef gameSetupStepDef in WorldGenerator.GameSetupStepsInOrder)
			{
				Rand.Seed = Gen.HashCombineInt(seedFromSeedString, gameSetupStepDef.setupStep.SeedPart);
				gameSetupStepDef.setupStep.GenerateWithoutWorldData();
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in from x in Find.WorldGrid.PlanetLayers
			orderby x.Key
			select x)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				List<WorldGenStepDef> genStepsInOrder = planetLayer2.Def.GenStepsInOrder;
				for (int i = 0; i < genStepsInOrder.Count; i++)
				{
					try
					{
						Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(genStepsInOrder, i));
						genStepsInOrder[i].worldGenStep.GenerateWithoutWorldData(seedString, planetLayer2);
					}
					catch (Exception value)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Error in WorldGenStep: ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
			Rand.PopState();
		}

		// Token: 0x0600DFA8 RID: 57256 RVA: 0x00532B28 File Offset: 0x00530D28
		public static void GenerateFromScribe(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			Rand.PushState();
			foreach (GameSetupStepDef gameSetupStepDef in WorldGenerator.GameSetupStepsInOrder)
			{
				Rand.Seed = Gen.HashCombineInt(seedFromSeedString, gameSetupStepDef.setupStep.SeedPart);
				gameSetupStepDef.setupStep.GenerateFromScribe();
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in from x in Find.WorldGrid.PlanetLayers
			orderby x.Key
			select x)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				List<WorldGenStepDef> genStepsInOrder = planetLayer2.Def.GenStepsInOrder;
				for (int i = 0; i < genStepsInOrder.Count; i++)
				{
					try
					{
						Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(genStepsInOrder, i));
						genStepsInOrder[i].worldGenStep.GenerateFromScribe(seedString, planetLayer2);
					}
					catch (Exception value)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Error in WorldGenStep: ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
			Rand.PopState();
		}

		// Token: 0x0600DFA9 RID: 57257 RVA: 0x00532CA4 File Offset: 0x00530EA4
		private static int GetSeedPart(List<WorldGenStepDef> genSteps, int index)
		{
			int seedPart = genSteps[index].worldGenStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (genSteps[i].worldGenStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x0600DFAA RID: 57258 RVA: 0x00532CEC File Offset: 0x00530EEC
		private static int GetSeedFromSeedString(string seedString)
		{
			return GenText.StableStringHash(seedString);
		}

		// Token: 0x04008F86 RID: 36742
		public const float DefaultPlanetCoverage = 0.3f;

		// Token: 0x04008F87 RID: 36743
		public const float DefaultPlanetCoverageOdyssey = 0.5f;

		// Token: 0x04008F88 RID: 36744
		public const OverallRainfall DefaultOverallRainfall = OverallRainfall.Normal;

		// Token: 0x04008F89 RID: 36745
		public const OverallPopulation DefaultOverallPopulation = OverallPopulation.Normal;

		// Token: 0x04008F8A RID: 36746
		public const OverallTemperature DefaultOverallTemperature = OverallTemperature.Normal;

		// Token: 0x04008F8B RID: 36747
		public const LandmarkDensity DefaultLandmarkDensity = LandmarkDensity.Normal;

		// Token: 0x04008F8C RID: 36748
		public const float DefaultPollutionCoverage = 0.05f;

		// Token: 0x04008F8D RID: 36749
		private static List<GameSetupStepDef> cachedGenSteps;
	}
}
