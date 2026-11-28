using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E4 RID: 5348
	public class PawnsArrivalModeWorker_EdgeWalkInDarkness : PawnsArrivalModeWorker
	{
		// Token: 0x060082BC RID: 33468 RVA: 0x002EEC0C File Offset: 0x002ECE0C
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 spawnCenter = parms.spawnCenter;
				Map map2 = map;
				int radius = 8;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 x) => map.glowGrid.PsychGlowAt(x) == PsychGlow.Dark));
				}
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spawnCenter, map2, radius, extraValidator);
				if (!loc.IsValid)
				{
					loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
				}
				GenSpawn.Spawn(pawns[i], loc, map, parms.spawnRotation, WipeMode.Vanish, false, false);
			}
		}

		// Token: 0x060082BD RID: 33469 RVA: 0x002EECB0 File Offset: 0x002ECEB0
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Ignore, false, (IntVec3 x) => map.glowGrid.PsychGlowAt(x) == PsychGlow.Dark) && !RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Ignore, false, null))
			{
				return false;
			}
			parms.spawnRotation = Rot4.FromAngleFlat((map.Center - parms.spawnCenter).AngleFlat);
			return true;
		}
	}
}
