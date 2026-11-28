using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCF RID: 3023
	public class WorkGiver_Uninstall : WorkGiver_RemoveBuilding
	{
		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06005701 RID: 22273 RVA: 0x00204CFF File Offset: 0x00202EFF
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06005702 RID: 22274 RVA: 0x00204D06 File Offset: 0x00202F06
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Uninstall;
			}
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x00204D0D File Offset: 0x00202F0D
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.Claimable)
			{
				if (t.Faction != pawn.Faction)
				{
					return false;
				}
			}
			else if (pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}
	}
}
