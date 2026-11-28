using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117A RID: 4474
	public class IncidentWorker_GauranlenPodSpawn : IncidentWorker
	{
		// Token: 0x06007045 RID: 28741 RVA: 0x00274EAB File Offset: 0x002730AB
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && ModsConfig.IdeologyActive;
		}

		// Token: 0x06007046 RID: 28742 RVA: 0x00274EC0 File Offset: 0x002730C0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 cell;
			if (!IncidentWorker_GauranlenPodSpawn.TryFindRootCell(map, out cell))
			{
				return false;
			}
			Thing thing;
			if (!this.TrySpawnAt(cell, map, out thing))
			{
				return false;
			}
			((Plant)thing).Growth = 1f;
			base.SendStandardLetter(parms, thing, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06007047 RID: 28743 RVA: 0x00274F18 File Offset: 0x00273118
		public static bool IsGoodBiome(BiomeDef biomeDef)
		{
			IncidentDef gauranlenPodSpawn = IncidentDefOf.GauranlenPodSpawn;
			return (gauranlenPodSpawn.disallowedBiomes == null || !gauranlenPodSpawn.disallowedBiomes.Contains(biomeDef)) && (gauranlenPodSpawn.allowedBiomes == null || gauranlenPodSpawn.allowedBiomes.Contains(biomeDef));
		}

		// Token: 0x06007048 RID: 28744 RVA: 0x00274F5C File Offset: 0x0027315C
		private static bool CanSpawnPodAt(IntVec3 c, Map map)
		{
			if (!c.Standable(map) || c.Fogged(map) || !c.GetRoom(map).PsychologicallyOutdoors || c.Roofed(map))
			{
				return false;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.growDays > 10f)
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == ThingDefOf.Plant_PodGauranlen)
				{
					return false;
				}
			}
			return map.reachability.CanReachFactionBase(c, map.ParentFaction) && !c.GetTerrain(map).avoidWander && c.GetFertility(map) >= ThingDefOf.Plant_PodGauranlen.plant.fertilityMin;
		}

		// Token: 0x06007049 RID: 28745 RVA: 0x00275028 File Offset: 0x00273228
		public static bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => IncidentWorker_GauranlenPodSpawn.CanSpawnPodAt(x, map), map, out cell);
		}

		// Token: 0x0600704A RID: 28746 RVA: 0x0027505C File Offset: 0x0027325C
		private bool TrySpawnAt(IntVec3 cell, Map map, out Thing plant)
		{
			Plant plant2 = cell.GetPlant(map);
			if (plant2 != null)
			{
				plant2.Destroy(DestroyMode.Vanish);
			}
			plant = GenSpawn.Spawn(ThingDefOf.Plant_PodGauranlen, cell, map, WipeMode.Vanish);
			return plant != null;
		}
	}
}
