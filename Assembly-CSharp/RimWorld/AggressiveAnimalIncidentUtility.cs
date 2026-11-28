using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001165 RID: 4453
	public static class AggressiveAnimalIncidentUtility
	{
		// Token: 0x06006FDF RID: 28639 RVA: 0x0027238C File Offset: 0x0027058C
		public static float AnimalWeight(PawnKindDef animal, float points)
		{
			points = Mathf.Max(points, 70f);
			if (animal.combatPower * 2f > points)
			{
				return 0f;
			}
			int num = Mathf.Min(Mathf.RoundToInt(points / animal.combatPower), 100);
			return Mathf.Clamp01(Mathf.InverseLerp(100f, 10f, (float)num));
		}

		// Token: 0x06006FE0 RID: 28640 RVA: 0x002723E8 File Offset: 0x002705E8
		public static bool TryFindAggressiveAnimalKind(float points, PlanetTile tile, out PawnKindDef animalKind)
		{
			List<PawnKindDef> animals = (from k in DefDatabase<PawnKindDef>.AllDefs
			where AggressiveAnimalIncidentUtility.CanArriveManhunter(k) && (!tile.Valid || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race))
			select k).ToList<PawnKindDef>();
			return AggressiveAnimalIncidentUtility.TryGetHabitatAnimal(tile, out animalKind) || AggressiveAnimalIncidentUtility.TryGetAnimalFromList(points, animals, out animalKind);
		}

		// Token: 0x06006FE1 RID: 28641 RVA: 0x00272438 File Offset: 0x00270638
		public static bool TryFindAggressiveAnimalKind(float points, Map map, out PawnKindDef animalKind)
		{
			if (AggressiveAnimalIncidentUtility.TryGetHabitatAnimal(map.Tile, out animalKind))
			{
				return true;
			}
			bool polluted = ModsConfig.BiotechActive && map.Tile.Valid && Rand.Value < WildAnimalSpawner.PollutionAnimalSpawnChanceFromPollutionCurve.Evaluate(Find.WorldGrid[map.Tile].pollution);
			List<PawnKindDef> list = (from k in map.Biomes.SelectMany((BiomeDef b) => b.AllWildAnimals)
			where AggressiveAnimalIncidentUtility.CanArriveWithPollution(k, map, polluted) && AggressiveAnimalIncidentUtility.CanArriveManhunter(k)
			select k).ToList<PawnKindDef>();
			if (polluted && list.Count == 0)
			{
				polluted = false;
				list = (from k in map.Biomes.SelectMany((BiomeDef b) => b.AllWildAnimals)
				where AggressiveAnimalIncidentUtility.CanArriveWithPollution(k, map, polluted) && AggressiveAnimalIncidentUtility.CanArriveManhunter(k)
				select k).ToList<PawnKindDef>();
			}
			if (AggressiveAnimalIncidentUtility.TryGetAnimalFromList(points, list, out animalKind))
			{
				return true;
			}
			list = (from k in DefDatabase<PawnKindDef>.AllDefs
			where AggressiveAnimalIncidentUtility.CanArriveManhunter(k) && AggressiveAnimalIncidentUtility.CanArriveWithPollution(k, map, polluted) && (!map.Tile.Valid || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(map.Tile, k.race))
			select k).ToList<PawnKindDef>();
			return AggressiveAnimalIncidentUtility.TryGetAnimalFromList(points, list, out animalKind);
		}

		// Token: 0x06006FE2 RID: 28642 RVA: 0x0027258C File Offset: 0x0027078C
		private static bool TryGetAnimalFromList(float points, List<PawnKindDef> animals, out PawnKindDef animalKind)
		{
			if (animals.Any<PawnKindDef>())
			{
				if (animals.TryRandomElementByWeight((PawnKindDef a) => AggressiveAnimalIncidentUtility.AnimalWeight(a, points), out animalKind))
				{
					return true;
				}
				if (points > animals.Min((PawnKindDef a) => a.combatPower) * 2f)
				{
					animalKind = animals.MaxBy((PawnKindDef a) => a.combatPower);
					return true;
				}
			}
			animalKind = null;
			return false;
		}

		// Token: 0x06006FE3 RID: 28643 RVA: 0x00272628 File Offset: 0x00270828
		private static bool TryGetHabitatAnimal(PlanetTile tile, out PawnKindDef animalKind)
		{
			animalKind = null;
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (!tile.Valid)
			{
				return false;
			}
			if (Find.WorldGrid[tile].Mutators.Contains(TileMutatorDefOf.AnimalHabitat))
			{
				TileMutatorWorker_AnimalHabitat tileMutatorWorker_AnimalHabitat = (TileMutatorWorker_AnimalHabitat)TileMutatorDefOf.AnimalHabitat.Worker;
				animalKind = tileMutatorWorker_AnimalHabitat.GetAnimalKind(tile);
				if (AggressiveAnimalIncidentUtility.CanArriveManhunter(animalKind) && Rand.Chance(0.5f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006FE4 RID: 28644 RVA: 0x00272699 File Offset: 0x00270899
		public static int GetAnimalsCount(PawnKindDef animalKind, float points)
		{
			return Mathf.Clamp(Mathf.RoundToInt(points / animalKind.combatPower), 2, 100);
		}

		// Token: 0x06006FE5 RID: 28645 RVA: 0x002726B0 File Offset: 0x002708B0
		public static List<Pawn> GenerateAnimals(PawnKindDef animalKind, PlanetTile tile, float points, int animalCount = 0)
		{
			List<Pawn> list = new List<Pawn>();
			int num = (animalCount > 0) ? animalCount : AggressiveAnimalIncidentUtility.GetAnimalsCount(animalKind, points);
			for (int i = 0; i < num; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, new PlanetTile?(tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06006FE6 RID: 28646 RVA: 0x00272780 File Offset: 0x00270980
		public static List<Pawn> GenerateAnimals(List<PawnKindDef> animalKinds, PlanetTile tile)
		{
			List<Pawn> list = new List<Pawn>();
			foreach (PawnKindDef kind in animalKinds)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, new PlanetTile?(tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06006FE7 RID: 28647 RVA: 0x0027286C File Offset: 0x00270A6C
		[DebugOutput]
		public static void ManhunterResults()
		{
			List<PawnKindDef> candidates = (from k in DefDatabase<PawnKindDef>.AllDefs.Where(new Func<PawnKindDef, bool>(AggressiveAnimalIncidentUtility.CanArriveManhunter))
			orderby -k.combatPower
			select k).ToList<PawnKindDef>();
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add(20f * Mathf.Pow(1.25f, (float)i));
			}
			DebugTables.MakeTablesDialog<float, PawnKindDef>(list, (float points) => points.ToString("F0") + " pts", candidates, (PawnKindDef candidate) => candidate.defName + " (" + candidate.combatPower.ToString("F0") + ")", delegate(float points, PawnKindDef candidate)
			{
				float num = candidates.Sum((PawnKindDef k) => AggressiveAnimalIncidentUtility.AnimalWeight(k, points));
				float num2 = AggressiveAnimalIncidentUtility.AnimalWeight(candidate, points);
				if (num2 != 0f)
				{
					string str = (num2 * 100f / num).ToString("F0");
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler.AppendLiteral("%, ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(Mathf.Max(Mathf.RoundToInt(points / candidate.combatPower), 1));
					return str + defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return "0%";
			}, "");
		}

		// Token: 0x06006FE8 RID: 28648 RVA: 0x0027294A File Offset: 0x00270B4A
		private static bool CanArriveManhunter(PawnKindDef kind)
		{
			return kind.RaceProps.Animal && kind.canArriveManhunter && kind.RaceProps.CanPassFences;
		}

		// Token: 0x06006FE9 RID: 28649 RVA: 0x00272970 File Offset: 0x00270B70
		private static bool CanArriveWithPollution(PawnKindDef k, Map map, bool polluted)
		{
			if (polluted)
			{
				return map.Biomes.Any((BiomeDef b) => b.CommonalityOfPollutionAnimal(k) > 0f);
			}
			if (map.TileInfo.IsCoastal)
			{
				return map.Biomes.Any((BiomeDef b) => b.CommonalityOfAnimal(k) > 0f || b.CommonalityOfCoastalAnimal(k) > 0f);
			}
			return map.Biomes.Any((BiomeDef b) => b.CommonalityOfAnimal(k) > 0f);
		}

		// Token: 0x04004785 RID: 18309
		public const int MinAnimalCount = 2;

		// Token: 0x04004786 RID: 18310
		public const int MaxAnimalCount = 100;

		// Token: 0x04004787 RID: 18311
		public const float MinPoints = 70f;
	}
}
