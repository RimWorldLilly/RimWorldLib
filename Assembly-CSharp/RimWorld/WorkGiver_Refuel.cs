using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C35 RID: 3125
	public class WorkGiver_Refuel : WorkGiver_Scanner
	{
		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x060058F3 RID: 22771 RVA: 0x0020D792 File Offset: 0x0020B992
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Refuelable);
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x060058F4 RID: 22772 RVA: 0x0020D79B File Offset: 0x0020B99B
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x060058F5 RID: 22773 RVA: 0x0020D79E File Offset: 0x0020B99E
		public virtual JobDef JobStandard
		{
			get
			{
				return JobDefOf.Refuel;
			}
		}

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060058F6 RID: 22774 RVA: 0x0020D7A5 File Offset: 0x0020B9A5
		public virtual JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RefuelAtomic;
			}
		}

		// Token: 0x060058F7 RID: 22775 RVA: 0x0020D7AC File Offset: 0x0020B9AC
		public virtual bool CanRefuelThing(Thing t)
		{
			return !(t is Building_Turret);
		}

		// Token: 0x060058F8 RID: 22776 RVA: 0x0020D7BA File Offset: 0x0020B9BA
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.CanRefuelThing(t) && RefuelWorkGiverUtility.CanRefuel(pawn, t, forced);
		}

		// Token: 0x060058F9 RID: 22777 RVA: 0x0020D7CF File Offset: 0x0020B9CF
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return RefuelWorkGiverUtility.RefuelJob(pawn, t, forced, this.JobStandard, this.JobAtomic);
		}
	}
}
