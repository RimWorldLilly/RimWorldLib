using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCA RID: 3018
	public class WorkGiver_Deconstruct : WorkGiver_RemoveBuilding
	{
		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x060056E4 RID: 22244 RVA: 0x00204737 File Offset: 0x00202937
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x060056E5 RID: 22245 RVA: 0x0020473E File Offset: 0x0020293E
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Deconstruct;
			}
		}

		// Token: 0x060056E6 RID: 22246 RVA: 0x00204748 File Offset: 0x00202948
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t.GetInnerIfMinified() as Building;
			return building != null && building.DeconstructibleBy(pawn.Faction) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
