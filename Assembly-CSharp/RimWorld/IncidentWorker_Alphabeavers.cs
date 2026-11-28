using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001166 RID: 4454
	public class IncidentWorker_Alphabeavers : IncidentWorker
	{
		// Token: 0x06006FEA RID: 28650 RVA: 0x002729E4 File Offset: 0x00270BE4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(PawnKindDefOf.Alphabeaver.race, 0f) && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
		}

		// Token: 0x06006FEB RID: 28651 RVA: 0x00272A38 File Offset: 0x00270C38
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef alphabeaver = PawnKindDefOf.Alphabeaver;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			float freeColonistsCount = (float)map.mapPawns.FreeColonistsCount;
			float randomInRange = IncidentWorker_Alphabeavers.CountPerColonistRange.RandomInRange;
			int num = Mathf.Clamp(GenMath.RoundRandom(freeColonistsCount * randomInRange), 1, 10);
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
				((Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(alphabeaver, null, null), loc, map, WipeMode.Vanish)).needs.food.CurLevelPercentage = 1f;
			}
			base.SendStandardLetter("LetterLabelBeaversArrived".Translate(), "BeaversArrived".Translate(), LetterDefOf.ThreatSmall, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x04004788 RID: 18312
		private static readonly FloatRange CountPerColonistRange = new FloatRange(1f, 1.5f);

		// Token: 0x04004789 RID: 18313
		private const int MinCount = 1;

		// Token: 0x0400478A RID: 18314
		private const int MaxCount = 10;
	}
}
