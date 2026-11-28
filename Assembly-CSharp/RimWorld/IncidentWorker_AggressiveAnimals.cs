using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001164 RID: 4452
	public class IncidentWorker_AggressiveAnimals : IncidentWorker
	{
		// Token: 0x06006FDC RID: 28636 RVA: 0x002720FC File Offset: 0x002702FC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			PawnKindDef pawnKindDef;
			IntVec3 intVec;
			return AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(parms.points, map, out pawnKindDef) && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
		}

		// Token: 0x06006FDD RID: 28637 RVA: 0x00272144 File Offset: 0x00270344
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef pawnKind = parms.pawnKind;
			if ((pawnKind == null && !AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(parms.points, map, out pawnKind)) || AggressiveAnimalIncidentUtility.GetAnimalsCount(pawnKind, parms.points) == 0)
			{
				return false;
			}
			IntVec3 spawnCenter = parms.spawnCenter;
			if (!spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out spawnCenter, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			List<Pawn> list = AggressiveAnimalIncidentUtility.GenerateAnimals(pawnKind, map.Tile, parms.points * 1f, parms.pawnCount);
			Rot4 rot = Rot4.FromAngleFlat((map.Center - spawnCenter).AngleFlat);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spawnCenter, map, 10, null);
				QuestUtility.AddQuestTag(GenSpawn.Spawn(pawn, loc, map, rot, WipeMode.Vanish, false, false), parms.questTag);
				pawn.health.AddHediff(HediffDefOf.Scaria, null, null, null);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, false, null, false, false, false);
				pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 120000);
			}
			if (ModsConfig.AnomalyActive)
			{
				if (this.def == IncidentDefOf.FrenziedAnimals)
				{
					base.SendStandardLetter("FrenziedAnimalsLabel".Translate(), "FrenziedAnimalsText".Translate(pawnKind.GetLabelPlural(-1)), LetterDefOf.ThreatBig, parms, list[0], Array.Empty<NamedArgument>());
				}
				else
				{
					base.SendStandardLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate(pawnKind.GetLabelPlural(-1)), LetterDefOf.ThreatBig, parms, list[0], Array.Empty<NamedArgument>());
				}
			}
			else
			{
				base.SendStandardLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate(pawnKind.GetLabelPlural(-1)), LetterDefOf.ThreatBig, parms, list[0], Array.Empty<NamedArgument>());
			}
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
			return true;
		}

		// Token: 0x04004782 RID: 18306
		private const float PointsFactor = 1f;

		// Token: 0x04004783 RID: 18307
		private const int AnimalsStayDurationMin = 60000;

		// Token: 0x04004784 RID: 18308
		private const int AnimalsStayDurationMax = 120000;
	}
}
