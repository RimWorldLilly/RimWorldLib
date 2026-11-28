using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC2 RID: 3010
	public class WorkGiver_ConstructRemoveFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x060056AF RID: 22191 RVA: 0x00203296 File Offset: 0x00201496
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x0020329D File Offset: 0x0020149D
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveFloor, c);
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x002032AF File Offset: 0x002014AF
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, false) && pawn.Map.terrainGrid.CanRemoveTopLayerAt(c) && !WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, pawn.Map);
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x002032E4 File Offset: 0x002014E4
		public static bool AnyBuildingBlockingFloorRemoval(IntVec3 c, Map map)
		{
			if (!map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return false;
			}
			Building firstBuilding = c.GetFirstBuilding(map);
			if (firstBuilding == null || firstBuilding.def.terrainAffordanceNeeded == null)
			{
				return false;
			}
			if (map.terrainGrid.FoundationAt(c) == null)
			{
				return !map.terrainGrid.UnderTerrainAt(c).affordances.Contains(firstBuilding.def.terrainAffordanceNeeded);
			}
			return !map.terrainGrid.FoundationAt(c).affordances.Contains(firstBuilding.def.terrainAffordanceNeeded);
		}
	}
}
