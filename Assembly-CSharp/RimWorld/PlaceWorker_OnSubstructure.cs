using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002073 RID: 8307
	[Obsolete("Use Substructure affordance instead.")]
	public class PlaceWorker_OnSubstructure : PlaceWorker
	{
		// Token: 0x0600D169 RID: 53609 RVA: 0x004CFB78 File Offset: 0x004CDD78
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(loc, rot, checkingDef.Size))
			{
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
					if (terrainDef == null || !terrainDef.IsSubstructure)
					{
						return "MessageMustBePlacedOnSubstructure".Translate();
					}
				}
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
