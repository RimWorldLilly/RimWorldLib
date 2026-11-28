using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118D RID: 4493
	public class IncidentWorker_PitGate : IncidentWorker
	{
		// Token: 0x0600708E RID: 28814 RVA: 0x002765B0 File Offset: 0x002747B0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			return base.CanFireNowSub(parms) && map.listerThings.ThingsOfDef(ThingDefOf.PitGate).Count == 0 && map.listerThings.ThingsOfDef(ThingDefOf.PitGateSpawner).Count == 0;
		}

		// Token: 0x0600708F RID: 28815 RVA: 0x0027660C File Offset: 0x0027480C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!LargeBuildingCellFinder.TryFindCell(out loc, map, IncidentWorker_PitGate.PitGateSpawnParms.ForThing(ThingDefOf.PitGate), null, null, false))
			{
				return false;
			}
			BuildingGroundSpawner buildingGroundSpawner = (BuildingGroundSpawner)ThingMaker.MakeThing(ThingDefOf.PitGateSpawner, null);
			PitGate pitGate = buildingGroundSpawner.ThingToSpawn as PitGate;
			pitGate.SetFaction(Faction.OfEntities, null);
			pitGate.pointsMultiplier = parms.pointMultiplier;
			GenSpawn.Spawn(buildingGroundSpawner, loc, map, WipeMode.Vanish);
			base.SendStandardLetter(parms, buildingGroundSpawner, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x040047B2 RID: 18354
		public static readonly LargeBuildingSpawnParms PitGateSpawnParms = new LargeBuildingSpawnParms
		{
			ignoreTerrainAffordance = true
		};
	}
}
