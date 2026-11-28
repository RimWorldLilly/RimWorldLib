using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019CB RID: 6603
	public static class GauranlenUtility
	{
		// Token: 0x0600A60A RID: 42506 RVA: 0x003B3824 File Offset: 0x003B1A24
		public static bool CocoonAndPodCellValidator(IntVec3 c, Map map, ThingDef thingDefToSpawn = null)
		{
			Thing thing;
			if (((thingDefToSpawn != null) ? thingDefToSpawn.plant : null) != null && thingDefToSpawn.CanEverPlantAt(c, map, out thing, false, true, false).Accepted)
			{
				return false;
			}
			if (!c.InBounds(map) || !c.Standable(map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Building || thingList[i].def.IsFrame)
				{
					return false;
				}
				if (thingList[i].def == ThingDefOf.Plant_TreeGauranlen || thingList[i].def == ThingDefOf.GaumakerCocoon || thingList[i].def == ThingDefOf.Plant_PodGauranlen)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A60B RID: 42507 RVA: 0x003B38EA File Offset: 0x003B1AEA
		public static List<Thing> BuildingsAffectingConnectionStrengthAt(IntVec3 pos, Map map, CompProperties_TreeConnection props)
		{
			return map.listerArtificialBuildingsForMeditation.GetForCell(pos, props.radiusToBuildingForConnectionStrengthLoss);
		}

		// Token: 0x0600A60C RID: 42508 RVA: 0x003B38FE File Offset: 0x003B1AFE
		public static IEnumerable<Thing> GetConnectionsAffectedByBuilding(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			GauranlenUtility.<GetConnectionsAffectedByBuilding>d__2 <GetConnectionsAffectedByBuilding>d__ = new GauranlenUtility.<GetConnectionsAffectedByBuilding>d__2(-2);
			<GetConnectionsAffectedByBuilding>d__.<>3__map = map;
			<GetConnectionsAffectedByBuilding>d__.<>3__def = def;
			<GetConnectionsAffectedByBuilding>d__.<>3__faction = faction;
			<GetConnectionsAffectedByBuilding>d__.<>3__pos = pos;
			<GetConnectionsAffectedByBuilding>d__.<>3__rotation = rotation;
			return <GetConnectionsAffectedByBuilding>d__;
		}

		// Token: 0x0600A60D RID: 42509 RVA: 0x003B392C File Offset: 0x003B1B2C
		public static void DrawConnectionsAffectedByBuildingOverlay(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			CompProperties_TreeConnection compProperties_TreeConnection = (CompProperties_TreeConnection)def.CompDefFor<CompTreeConnection>();
			int num = 0;
			foreach (Thing t in GauranlenUtility.GetConnectionsAffectedByBuilding(map, def, faction, pos, rotation))
			{
				if (num++ > 10)
				{
					break;
				}
				GenAdj.OccupiedRect(pos, rotation, def.size);
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red, 0.2f);
			}
		}
	}
}
