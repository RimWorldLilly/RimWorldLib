using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001175 RID: 4469
	public class IncidentWorker_FarmAnimalsWanderIn : IncidentWorker
	{
		// Token: 0x0600702D RID: 28717 RVA: 0x00274656 File Offset: 0x00272856
		public override float ChanceFactorNow(IIncidentTarget target)
		{
			if (!ModsConfig.IdeologyActive || !IdeoUtility.AnyColonistWithRanchingIssue())
			{
				return base.ChanceFactorNow(target);
			}
			return IncidentWorker_FarmAnimalsWanderIn.BaseChanceFactorByAnimalBodySizePerCapitaCurve.Evaluate(PawnUtility.PlayerAnimalBodySizePerCapita()) * base.ChanceFactorNow(target);
		}

		// Token: 0x0600702E RID: 28718 RVA: 0x00274688 File Offset: 0x00272888
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			PawnKindDef pawnKindDef;
			return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null) && this.TryFindRandomPawnKind(map, out pawnKindDef);
		}

		// Token: 0x0600702F RID: 28719 RVA: 0x002746C8 File Offset: 0x002728C8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			PawnKindDef pawnKindDef;
			if (!this.TryFindRandomPawnKind(map, out pawnKindDef))
			{
				return false;
			}
			int num = Mathf.Clamp(GenMath.RoundRandom(((parms.totalBodySize > 0f) ? parms.totalBodySize : 2.5f) / pawnKindDef.RaceProps.baseBodySize), 2, 10);
			if (num >= 2)
			{
				this.SpawnAnimal(intVec, map, pawnKindDef, new Gender?(Gender.Female));
				this.SpawnAnimal(intVec, map, pawnKindDef, new Gender?(Gender.Male));
				num -= 2;
			}
			for (int i = 0; i < num; i++)
			{
				this.SpawnAnimal(intVec, map, pawnKindDef, null);
			}
			base.SendStandardLetter(parms.customLetterLabel ?? "LetterLabelFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)).CapitalizeFirst(), parms.customLetterText ?? "LetterFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)), LetterDefOf.PositiveEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06007030 RID: 28720 RVA: 0x002747FC File Offset: 0x002729FC
		private void SpawnAnimal(IntVec3 location, Map map, PawnKindDef pawnKind, Gender? gender = null)
		{
			IntVec3 loc = CellFinder.RandomClosewalkCellNear(location, map, 12, null);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, gender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false, false);
			pawn.SetFaction(Faction.OfPlayer, null);
		}

		// Token: 0x06007031 RID: 28721 RVA: 0x002748BC File Offset: 0x00272ABC
		private bool TryFindRandomPawnKind(Map map, out PawnKindDef kind)
		{
			return (from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && x.race.GetStatValueAbstract(StatDefOf.Wildness, null) < 0.35f && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race, 0f) && !x.race.tradeTags.NullOrEmpty<string>() && x.race.tradeTags.Contains("AnimalFarm") && !x.RaceProps.Dryad && !x.RaceProps.neverIncludeInQuests
			select x).TryRandomElementByWeight((PawnKindDef k) => this.SelectionChance(k), out kind);
		}

		// Token: 0x06007032 RID: 28722 RVA: 0x00274908 File Offset: 0x00272B08
		private float SelectionChance(PawnKindDef pawnKind)
		{
			float num = 0.42000002f - pawnKind.race.GetStatValueAbstract(StatDefOf.Wildness, null);
			if (PawnUtility.PlayerHasReproductivePair(pawnKind))
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x0400479E RID: 18334
		private const string FarmAnimalTradeTag = "AnimalFarm";

		// Token: 0x0400479F RID: 18335
		private const float MaxWildness = 0.35f;

		// Token: 0x040047A0 RID: 18336
		private const float TotalBodySizeToSpawn = 2.5f;

		// Token: 0x040047A1 RID: 18337
		private static SimpleCurve BaseChanceFactorByAnimalBodySizePerCapitaCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1.5f),
				true
			},
			{
				new CurvePoint(4f, 1f),
				true
			}
		};

		// Token: 0x040047A2 RID: 18338
		private const float SelectionChanceFactorIfExistingMatingPair = 0.5f;
	}
}
