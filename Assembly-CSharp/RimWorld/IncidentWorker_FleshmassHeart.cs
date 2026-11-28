using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001178 RID: 4472
	public class IncidentWorker_FleshmassHeart : IncidentWorker
	{
		// Token: 0x0600703C RID: 28732 RVA: 0x00274BE8 File Offset: 0x00272DE8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ModsConfig.AnomalyActive && base.CanFireNowSub(parms);
		}

		// Token: 0x0600703D RID: 28733 RVA: 0x00274BFC File Offset: 0x00272DFC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!LargeBuildingCellFinder.TryFindCell(out loc, map, IncidentWorker_FleshmassHeart.HeartSpawnParms.ForThing(ThingDefOf.FleshmassHeart), null, null, false))
			{
				return false;
			}
			BuildingGroundSpawner buildingGroundSpawner = (BuildingGroundSpawner)ThingMaker.MakeThing(ThingDefOf.FleshmassHeartSpawner, null);
			Building_FleshmassHeart building_FleshmassHeart = buildingGroundSpawner.ThingToSpawn as Building_FleshmassHeart;
			building_FleshmassHeart.Comp.threatPoints = parms.points;
			building_FleshmassHeart.SetFaction(Faction.OfEntities, null);
			GenSpawn.Spawn(buildingGroundSpawner, loc, map, WipeMode.Vanish);
			base.SendStandardLetter(parms, buildingGroundSpawner, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x040047A8 RID: 18344
		public static readonly LargeBuildingSpawnParms HeartSpawnParms = new LargeBuildingSpawnParms
		{
			minDistToEdge = 10,
			minDistanceToColonyBuilding = 20f,
			preferFarFromColony = true
		};
	}
}
