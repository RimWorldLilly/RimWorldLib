using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002076 RID: 8310
	public class PlaceWorker_InvalidOverSubstructure : PlaceWorker
	{
		// Token: 0x0600D172 RID: 53618 RVA: 0x004CFE5C File Offset: 0x004CE05C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return AcceptanceReport.WasAccepted;
			}
			foreach (IntVec3 c in GenAdj.OccupiedRect(loc, rot, checkingDef.Size))
			{
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
					if (terrainDef != null && terrainDef.IsSubstructure)
					{
						return "MessageCannotPlaceOverSubstructure".Translate();
					}
					foreach (Blueprint blueprint in map.thingGrid.ThingsListAt(c).OfType<Blueprint>())
					{
						TerrainDef terrainDef2 = blueprint.def.entityDefToBuild as TerrainDef;
						if (terrainDef2 != null && terrainDef2.IsSubstructure)
						{
							return "MessageCannotPlaceOverSubstructure".Translate();
						}
					}
				}
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
