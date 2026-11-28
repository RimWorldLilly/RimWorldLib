using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001180 RID: 4480
	public class IncidentWorker_HerdMigration : IncidentWorker
	{
		// Token: 0x0600705B RID: 28763 RVA: 0x00275428 File Offset: 0x00273628
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef pawnKindDef;
			IntVec3 intVec;
			IntVec3 intVec2;
			return this.TryFindAnimalKind(map, out pawnKindDef) && this.TryFindStartAndEndCells(map, out intVec, out intVec2);
		}

		// Token: 0x0600705C RID: 28764 RVA: 0x0027545C File Offset: 0x0027365C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef pawnKindDef;
			if (!this.TryFindAnimalKind(map, out pawnKindDef))
			{
				return false;
			}
			IntVec3 intVec;
			IntVec3 near;
			if (!this.TryFindStartAndEndCells(map, out intVec, out near))
			{
				return false;
			}
			Rot4 rot = Rot4.FromAngleFlat((map.Center - intVec).AngleFlat);
			List<Pawn> list = this.GenerateAnimals(pawnKindDef, map.Tile);
			for (int i = 0; i < list.Count; i++)
			{
				Thing newThing = list[i];
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
				GenSpawn.Spawn(newThing, loc, map, rot, WipeMode.Vanish, false, false);
			}
			LordMaker.MakeNewLord(null, new LordJob_ExitMapNear(near, LocomotionUrgency.Walk, 12f, false, false), map, list);
			TaggedString baseLetterText = this.def.letterText.Formatted(pawnKindDef.GetLabelPlural(-1)).CapitalizeFirst();
			string str = string.Format(this.def.letterLabel, pawnKindDef.GetLabelPlural(-1).CapitalizeFirst());
			base.SendStandardLetter(str, baseLetterText, this.def.letterDef, parms, list[0], Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600705D RID: 28765 RVA: 0x00275580 File Offset: 0x00273780
		private bool TryFindAnimalKind(Map map, out PawnKindDef animalKind)
		{
			bool polluted = ModsConfig.BiotechActive && Rand.Value < WildAnimalSpawner.PollutionAnimalSpawnChanceFromPollutionCurve.Evaluate(Find.WorldGrid[map.Tile].pollution);
			if ((from k in map.Biomes.SelectMany((BiomeDef b) => b.AllWildAnimals)
			where this.IsValidBiomeAnimal(polluted, map, k)
			select k).TryRandomElementByWeight((PawnKindDef x) => Mathf.Lerp(0.2f, 1f, x.race.GetStatValueAbstract(StatDefOf.Wildness, null)), out animalKind))
			{
				return true;
			}
			return (from k in DefDatabase<PawnKindDef>.AllDefs
			where this.IsValidAnimal(polluted, map, k)
			select k).TryRandomElementByWeight((PawnKindDef x) => Mathf.Lerp(0.2f, 1f, x.race.GetStatValueAbstract(StatDefOf.Wildness, null)), out animalKind);
		}

		// Token: 0x0600705E RID: 28766 RVA: 0x00275680 File Offset: 0x00273880
		private bool IsValidBiomeAnimal(bool polluted, Map map, PawnKindDef k)
		{
			if (!k.RaceProps.CanDoHerdMigration)
			{
				return false;
			}
			bool flag = polluted ? map.Biomes.Any((BiomeDef b) => b.CommonalityOfPollutionAnimal(k) > 0f) : map.Biomes.Any((BiomeDef b) => b.CommonalityOfAnimal(k) > 0f);
			if (map.TileInfo.IsCoastal && !polluted)
			{
				flag |= map.Biomes.Any((BiomeDef b) => b.CommonalityOfCoastalAnimal(k) > 0f);
			}
			return flag;
		}

		// Token: 0x0600705F RID: 28767 RVA: 0x0027570D File Offset: 0x0027390D
		private bool IsValidAnimal(bool polluted, Map map, PawnKindDef k)
		{
			return map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(k.race, 0f) && this.IsValidBiomeAnimal(polluted, map, k);
		}

		// Token: 0x06007060 RID: 28768 RVA: 0x00275734 File Offset: 0x00273934
		private bool TryFindStartAndEndCells(Map map, out IntVec3 start, out IntVec3 end)
		{
			if (!RCellFinder.TryFindRandomPawnEntryCell(out start, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				end = IntVec3.Invalid;
				return false;
			}
			end = IntVec3.Invalid;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 startLocal = start;
				IntVec3 intVec;
				if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => map.reachability.CanReach(startLocal, x, PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true)), map, CellFinder.EdgeRoadChance_Ignore, out intVec))
				{
					break;
				}
				if (!end.IsValid || intVec.DistanceToSquared(start) > end.DistanceToSquared(start))
				{
					end = intVec;
				}
			}
			return end.IsValid;
		}

		// Token: 0x06007061 RID: 28769 RVA: 0x002757FC File Offset: 0x002739FC
		private List<Pawn> GenerateAnimals(PawnKindDef animalKind, PlanetTile tile)
		{
			int num = IncidentWorker_HerdMigration.AnimalsCount.RandomInRange;
			num = Mathf.Max(num, Mathf.CeilToInt(4f / animalKind.RaceProps.baseBodySize));
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < num; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, new PlanetTile?(tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			return list;
		}

		// Token: 0x040047AA RID: 18346
		private static readonly IntRange AnimalsCount = new IntRange(3, 5);

		// Token: 0x040047AB RID: 18347
		private const float MinTotalBodySize = 4f;
	}
}
