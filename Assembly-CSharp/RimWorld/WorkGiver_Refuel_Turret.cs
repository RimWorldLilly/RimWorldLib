using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C37 RID: 3127
	public class WorkGiver_Refuel_Turret : WorkGiver_Refuel
	{
		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x06005900 RID: 22784 RVA: 0x0020DB64 File Offset: 0x0020BD64
		public override JobDef JobStandard
		{
			get
			{
				return JobDefOf.RearmTurret;
			}
		}

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x06005901 RID: 22785 RVA: 0x0020DB6B File Offset: 0x0020BD6B
		public override JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RearmTurretAtomic;
			}
		}

		// Token: 0x06005902 RID: 22786 RVA: 0x0020DB72 File Offset: 0x0020BD72
		public override bool CanRefuelThing(Thing t)
		{
			return t is Building_Turret;
		}
	}
}
