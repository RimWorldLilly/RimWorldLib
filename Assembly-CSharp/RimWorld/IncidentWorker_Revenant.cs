using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001193 RID: 4499
	public class IncidentWorker_Revenant : IncidentWorker
	{
		// Token: 0x060070A5 RID: 28837 RVA: 0x00276C14 File Offset: 0x00274E14
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 spawnCenter = parms.spawnCenter;
			if (!spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out spawnCenter, map, CellFinder.EdgeRoadChance_Hostile, false, null))
			{
				return false;
			}
			Rot4 rot = Rot4.FromAngleFlat((map.Center - spawnCenter).AngleFlat);
			GenSpawn.Spawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Revenant, Faction.OfEntities, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), spawnCenter, map, rot, WipeMode.Vanish, false, false);
			return true;
		}
	}
}
