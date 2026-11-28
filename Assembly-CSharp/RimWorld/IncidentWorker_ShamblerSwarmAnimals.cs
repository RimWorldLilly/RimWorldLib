using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001199 RID: 4505
	public class IncidentWorker_ShamblerSwarmAnimals : IncidentWorker_ShamblerSwarm
	{
		// Token: 0x060070BE RID: 28862 RVA: 0x0027748C File Offset: 0x0027568C
		protected override List<Pawn> GenerateEntities(IncidentParms parms, float points)
		{
			Map map = (Map)parms.target;
			bool polluted = Rand.Value < WildAnimalSpawner.PollutionAnimalSpawnChanceFromPollutionCurve.Evaluate(Find.WorldGrid[map.Tile].pollution);
			if (!map.Biome.AllWildAnimals.Where(delegate(PawnKindDef a)
			{
				if (!(map.mapTemperature.SeasonAcceptableFor(a.race, 0f) & polluted))
				{
					return map.Biome.CommonalityOfAnimal(a) > 0f;
				}
				return map.Biome.CommonalityOfPollutionAnimal(a) > 0f;
			}).TryRandomElement(out this.animalKind))
			{
				return null;
			}
			List<Pawn> list = new List<Pawn>();
			int num = Mathf.RoundToInt(points / this.animalKind.combatPower);
			for (int i = 0; i < num; i++)
			{
				PawnKindDef chimera = this.animalKind;
				if (Rand.Chance(0.02f))
				{
					chimera = PawnKindDefOf.Chimera;
				}
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(chimera, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				MutantUtility.SetFreshPawnAsMutant(pawn, MutantDefOf.Shambler);
				list.Add(pawn);
			}
			base.SetupShamblerHediffs(list, this.ShamblerLifespanTicksRange);
			return list;
		}

		// Token: 0x060070BF RID: 28863 RVA: 0x00277610 File Offset: 0x00275810
		protected override void SendLetter(IncidentParms parms, List<Pawn> entities)
		{
			TaggedString taggedString = "LetterShamblerAnimalsArrived".Translate(this.animalKind.Named("ANIMALKIND"));
			int num = entities.Count((Pawn e) => e.kindDef == PawnKindDefOf.Chimera);
			if (num == 1)
			{
				taggedString += "\n\n" + "LetterText_ShamblerChimera".Translate();
			}
			else if (num > 1)
			{
				taggedString += "\n\n" + "LetterText_ShamblerChimeraPlural".Translate();
			}
			base.SendStandardLetter("LetterLabelShamblerAnimalsArrived".Translate(this.animalKind.Named("ANIMALKIND")), taggedString, this.def.letterDef, parms, entities, Array.Empty<NamedArgument>());
		}

		// Token: 0x040047BA RID: 18362
		private const float ChimeraSpawnChance = 0.02f;

		// Token: 0x040047BB RID: 18363
		private PawnKindDef animalKind;
	}
}
